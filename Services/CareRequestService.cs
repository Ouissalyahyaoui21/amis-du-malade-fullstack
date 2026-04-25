using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class CareRequestService : ICareRequestService
    {
        private readonly AppDbContext _db;

        public CareRequestService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CareRequest> CreateAsync(CreateCareRequestVM vm)
        {
            var request = new CareRequest
            {
                PatientId = vm.PatientId,
                RequestedByContactId = vm.RequestedByContactId,
                BranchId = vm.BranchId,
                CareLocationType = vm.CareLocationType,
                HospitalId = vm.HospitalId,
                LocationDetails = vm.LocationDetails,
                Municipality = vm.Municipality,
                RequestedStartDate = vm.RequestedStartDate,
                RequestedEndDate = vm.RequestedEndDate,
                DurationUnit = vm.DurationUnit,
                PriorityLevel = vm.PriorityLevel,
                NeedsNightPresence = vm.NeedsNightPresence,
                NeedsTransportSupport = vm.NeedsTransportSupport,
                MedicalSummary = vm.MedicalSummary,
                SupportSummary = vm.SupportSummary,
                Status = "New"
            };

            foreach (var s in vm.RequiredSkills)
            {
                request.RequiredSkills.Add(new CareRequestRequiredSkill
                {
                    SkillId = s.SkillId,
                    RequiredLevel = s.RequiredLevel,
                    Mandatory = s.Mandatory
                });
            }

            _db.CareRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<List<CareRequest>> GetAllAsync()
        {
            return await _db.CareRequests
                .Include(r => r.Patient)
                .Include(r => r.RequiredSkills).ThenInclude(s => s.Skill)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<CareRequest?> GetByIdAsync(Guid id)
        {
            return await _db.CareRequests
                .Include(r => r.Patient).ThenInclude(p => p.Contacts)
                .Include(r => r.RequiredSkills).ThenInclude(s => s.Skill)
                .Include(r => r.Assignments)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var request = await _db.CareRequests.FindAsync(id);
            if (request == null) return false;
            request.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }

        // ⭐ الخوارزمية الذكية
        public async Task<List<object>> GetSuggestionsAsync(Guid requestId)
        {
            var request = await _db.CareRequests
                .Include(r => r.RequiredSkills)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new Exception("الطلب غير موجود");

            var volunteers = await _db.Volunteers
                .Include(v => v.Skills)
                .Include(v => v.Availabilities)
                .Include(v => v.CoverageAreas)
                .Where(v => v.Status == "Approved")
                .ToListAsync();

            var suggestions = new List<object>();

            foreach (var v in volunteers)
            {
                double score = 0;
                var reasons = new List<string>();

                // نقاط المهارات (40)
                var requiredSkillIds = request.RequiredSkills
                    .Select(s => s.SkillId).ToList();
                var volunteerSkillIds = v.Skills
                    .Select(s => s.SkillId).ToList();
                var matched = requiredSkillIds.Intersect(volunteerSkillIds).Count();

                if (requiredSkillIds.Any())
                {
                    score += (matched * 40.0) / requiredSkillIds.Count;
                    if (matched > 0)
                        reasons.Add($"يمتلك {matched}/{requiredSkillIds.Count} مهارات");
                }
                else score += 40;

                // نقاط المنطقة (30)
                var covers = v.CoverageAreas
                    .Any(a => a.Municipality == request.Municipality);
                if (covers) { score += 30; reasons.Add("يغطي المنطقة"); }

                // نقاط التوفر (30)
                if (v.Availabilities.Any())
                {
                    score += 30;
                    reasons.Add("متاح للعمل");
                }

                // النقل (10 إضافية)
                if (request.NeedsTransportSupport && v.HasTransportation)
                {
                    score += 10;
                    reasons.Add("عنده وسيلة نقل");
                }

                suggestions.Add(new
                {
                    volunteerId = v.Id,
                    name = v.FullName,
                    phone = v.Phone,
                    municipality = v.Municipality,
                    matchScore = Math.Round(score, 1),
                    reasons
                });
            }

            return suggestions
                .OrderByDescending(s => ((dynamic)s).matchScore)
                .Take(5).ToList<object>();
        }
    }
}