using AmisduMalade.Data;
using AmisduMalade.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AmisduMalade.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _db;

        public AssignmentService(AppDbContext db)
        {
            _db = db;
        }

        // ⭐ الخوارزمية الذكية - اقتراح أفضل المتطوعين
        public async Task<List<object>> GetSuggestionsAsync(int requestId)
        {
            var request = await _db.PatientRequests.FindAsync(requestId);
            if (request == null)
                throw new Exception("الطلب غير موجود");

            // جيب كل المتطوعين النشطين
            var volunteers = await _db.Volunteers
                .Where(v => v.Status == "Active")
                .ToListAsync();

            var suggestions = new List<object>();

            foreach (var volunteer in volunteers)
            {
                double score = 0;
                var reasons = new List<string>();

                // ── 1. نقاط المهارات (40 نقطة) ──────────────
                var volunteerSkills = JsonSerializer
                    .Deserialize<List<string>>(volunteer.Skills) ?? new();
                var requiredSkills = JsonSerializer
                    .Deserialize<List<string>>(request.RequiredSkills) ?? new();

                if (requiredSkills.Any())
                {
                    var matchedSkills = volunteerSkills
                        .Intersect(requiredSkills).Count();
                    var skillScore = (matchedSkills * 40.0) / requiredSkills.Count;
                    score += skillScore;

                    if (matchedSkills > 0)
                        reasons.Add($"يمتلك {matchedSkills} من {requiredSkills.Count} مهارات مطلوبة");
                }
                else
                {
                    score += 40; // لا توجد مهارات مطلوبة = نقاط كاملة
                }

                // ── 2. نقاط البلدية (30 نقطة) ────────────────
                if (volunteer.Municipality == request.RequesterCity)
                {
                    score += 30;
                    reasons.Add("نفس البلدية");
                }
                else if (IsSameRegion(volunteer.Municipality, request.RequesterCity))
                {
                    score += 15;
                    reasons.Add("منطقة قريبة");
                }

                // ── 3. نقاط التوفر (30 نقطة) ─────────────────
                var availableDays = JsonSerializer
                    .Deserialize<List<string>>(volunteer.AvailableDays) ?? new();

                if (availableDays.Any())
                {
                    score += 30;
                    reasons.Add("متاح للعمل");
                }

                // ── 4. نقطة إضافية للمستوى التدريبي ──────────
                if (volunteer.TrainingLevel == "مؤهل")
                {
                    score += 10;
                    reasons.Add("مؤهل تدريبياً");
                }

                suggestions.Add(new
                {
                    volunteerId = volunteer.Id,
                    volunteerName = volunteer.FullName,
                    phone = volunteer.Phone,
                    municipality = volunteer.Municipality,
                    skills = volunteerSkills,
                    matchScore = Math.Round(score, 1),
                    matchReasons = reasons
                });
            }

            // رتّب من الأعلى للأقل
            return suggestions
                .OrderByDescending(s => ((dynamic)s).matchScore)
                .Take(5) // أفضل 5 متطوعين
                .ToList<object>();
        }

        // تكليف متطوع بطلب
        public async Task<Assignment> AssignAsync(
            int requestId, int volunteerId, bool isAutomatic)
        {
            // تحقق أن الطلب والمتطوع موجودان
            var request = await _db.PatientRequests.FindAsync(requestId);
            var volunteer = await _db.Volunteers.FindAsync(volunteerId);

            if (request == null) throw new Exception("الطلب غير موجود");
            if (volunteer == null) throw new Exception("المتطوع غير موجود");

            // احسب نقطة التوافق
            var suggestions = await GetSuggestionsAsync(requestId);
            var matchScore = 0.0;
            foreach (dynamic s in suggestions)
            {
                if (s.volunteerId == volunteerId)
                {
                    matchScore = s.matchScore;
                    break;
                }
            }

            var assignment = new Assignment
            {
                RequestId = requestId,
                VolunteerId = volunteerId,
                IsAutomatic = isAutomatic,
                MatchScore = matchScore,
                Status = "Active"
            };

            // حدّث حالة الطلب
            request.Status = "Assigned";

            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }

        // جيب كل التكليفات
        public async Task<List<Assignment>> GetAllAsync()
        {
            return await _db.Assignments
                .Include(a => a.Volunteer)
                .Include(a => a.Request)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        // تحديث حالة تكليف
        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var assignment = await _db.Assignments
                .Include(a => a.Request)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null) return false;

            assignment.Status = status;

            // إذا اكتمل التكليف - حدّث الطلب
            if (status == "Completed" && assignment.Request != null)
                assignment.Request.Status = "Completed";

            await _db.SaveChangesAsync();
            return true;
        }

        // دالة مساعدة - تحقق من المنطقة الجغرافية
        private bool IsSameRegion(string municipality1, string municipality2)
        {
            // مناطق ولاية سكيكدة
            var regions = new List<List<string>>
            {
                new() { "سكيكدة", "عزابة", "الحروش" },
                new() { "القل", "الميلية", "بوقوس" },
                new() { "زرداز", "تمالوس" }
            };

            foreach (var region in regions)
            {
                if (region.Contains(municipality1) &&
                    region.Contains(municipality2))
                    return true;
            }
            return false;
        }
    }
}