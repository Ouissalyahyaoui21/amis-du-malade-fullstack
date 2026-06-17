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

        // ── Endpoint عام — الموبايل يرسل بيانات المريض + الطلب معاً ────────────
        public async Task<CareRequestResponseVM> CreatePublicAsync(CreateCareRequestPublicVM vm)
        {
            // 1. تحويل مفتاح الموقع (يُستخدم في المريض والطلب معاً)
            var locationType = vm.CareLocationType switch
            {
                "home"         => "Home",
                "hospital"     => "Hospital",
                "clinic"       => "Hospital",
                "elderly_home" => "NursingHome",
                _              => "Other"
            };

            // 2. إنشاء المريض تلقائياً
            var patient = new Patient
            {
                FullName             = vm.PatientName,
                Gender               = vm.PatientGender,
                Municipality         = vm.PatientMunicipality ?? vm.Municipality,
                Address              = vm.PatientAddress,
                CurrentResidenceType = locationType,
                Notes                = vm.MedicalSummary
            };
            if (vm.PatientAge.HasValue)
                patient.BirthDate = DateTime.UtcNow.AddYears(-vm.PatientAge.Value);

            _db.Patients.Add(patient);

            // 3. إضافة جهة الاتصال (الطالب)
            if (!string.IsNullOrWhiteSpace(vm.RequesterName))
            {
                patient.Contacts.Add(new PatientContact
                {
                    FullName          = vm.RequesterName,
                    Phone             = vm.RequesterPhone,
                    RelationToPatient = vm.RequesterRelation,
                    IsPrimaryContact  = true
                });
            }

            await _db.SaveChangesAsync();

            // 4. إنشاء طلب المرافقة
            var request = new CareRequest
            {
                PatientId        = patient.Id,
                CareLocationType = locationType,
                Municipality     = vm.Municipality ?? vm.PatientMunicipality,
                RequestedStartDate = vm.RequestedStartDate,
                NeedsNightPresence  = vm.NeedsNightPresence,
                NeedsTransportSupport = vm.NeedsTransportSupport,
                PriorityLevel    = vm.PriorityLevel,
                MedicalSummary   = vm.MedicalSummary,
                SupportSummary   = vm.SupportSummary,
                Status           = "New"
            };

            // 5. ربط المهارات المطلوبة بالاسم
            foreach (var skillName in vm.RequiredSkillNames.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                var skill = await _db.Skills.FirstOrDefaultAsync(s => s.Name == skillName);
                if (skill == null)
                {
                    skill = new Skill { Name = skillName };
                    _db.Skills.Add(skill);
                    await _db.SaveChangesAsync();
                }
                request.RequiredSkills.Add(new CareRequestRequiredSkill
                {
                    SkillId   = skill.Id,
                    Mandatory = true
                });
            }

            _db.CareRequests.Add(request);
            await _db.SaveChangesAsync();

            return new CareRequestResponseVM
            {
                Id              = request.Id,
                ReferenceNumber = $"ADM-{DateTime.UtcNow.Year}-{request.Id.ToString()[..6].ToUpper()}",
                Status          = request.Status,
                PatientName     = patient.FullName,
                CreatedAt       = request.CreatedAt
            };
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
                .Include(r => r.Patient).ThenInclude(p => p.Contacts)
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

        // ⭐ الخوارزمية الذكية المحسّنة
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

            var requestDay = request.RequestedStartDate.DayOfWeek.ToString();
            var requestHour = request.RequestedStartDate.Hour;

            var suggestions = new List<object>();

            foreach (var v in volunteers)
            {
                double score = 0;
                var reasons = new List<string>();

                // ── نقاط المهارات (40) ───────────────────────────────────────
                var requiredSkillIds = request.RequiredSkills.Select(s => s.SkillId).ToList();
                var volunteerSkillIds = v.Skills.Select(s => s.SkillId).ToList();
                int matched = requiredSkillIds.Any()
                    ? requiredSkillIds.Intersect(volunteerSkillIds).Count()
                    : 0;

                if (requiredSkillIds.Any())
                {
                    score += (matched * 40.0) / requiredSkillIds.Count;
                    if (matched > 0)
                        reasons.Add($"يمتلك {matched}/{requiredSkillIds.Count} مهارات مطلوبة");
                }
                else
                {
                    score += 40;
                    reasons.Add("لا توجد مهارات محددة");
                }

                // ── نوع الرعاية — توافق القدرات (10 إضافية) ────────────────
                if (request.CareLocationType == "Home"       && v.CanHomeVisit)     { score += 10; reasons.Add("يستطيع الزيارة المنزلية"); }
                if (request.CareLocationType == "Hospital"   && v.CanHospitalVisit) { score += 10; reasons.Add("يستطيع المرافقة في المستشفى"); }
                if (request.NeedsNightPresence               && v.CanNightPresence) { score += 10; reasons.Add("يستطيع الحضور الليلي"); }

                // ── نقاط المنطقة (30) ────────────────────────────────────────
                bool coversArea = v.CoverageAreas.Any(a =>
                    string.Equals(a.Municipality, request.Municipality, StringComparison.OrdinalIgnoreCase))
                    || string.Equals(v.Municipality, request.Municipality, StringComparison.OrdinalIgnoreCase);

                if (coversArea) { score += 30; reasons.Add("يغطي نفس المنطقة"); }
                else if (!string.IsNullOrWhiteSpace(v.Municipality) && !string.IsNullOrWhiteSpace(request.Municipality))
                {
                    // تطابق جزئي — نفس بداية اسم البلدية
                    bool partial = v.Municipality!.StartsWith(request.Municipality![..Math.Min(4, request.Municipality.Length)],
                        StringComparison.OrdinalIgnoreCase);
                    if (partial) { score += 15; reasons.Add("منطقة مجاورة"); }
                }

                // ── نقاط التوفر الحقيقي (30) ─────────────────────────────────
                if (v.Availabilities.Any())
                {
                    bool availableOnDay = v.Availabilities.Any(a =>
                        string.Equals(a.DayOfWeek, requestDay, StringComparison.OrdinalIgnoreCase));

                    bool availableAtTime = v.Availabilities.Any(a =>
                        string.Equals(a.DayOfWeek, requestDay, StringComparison.OrdinalIgnoreCase)
                        && a.StartTime.Hours <= requestHour
                        && a.EndTime.Hours   >= requestHour);

                    if (availableAtTime)    { score += 30; reasons.Add("متاح في اليوم والوقت المطلوب"); }
                    else if (availableOnDay){ score += 15; reasons.Add("متاح في اليوم المطلوب"); }
                    else                    { score += 5;  reasons.Add("توفر جزئي"); }
                }

                // ── النقل (10 إضافية) ─────────────────────────────────────────
                if (request.NeedsTransportSupport && v.HasTransportation)
                {
                    score += 10;
                    reasons.Add("يملك وسيلة نقل");
                }

                if (score > 0)
                {
                    suggestions.Add(new
                    {
                        volunteerId  = v.Id,
                        name         = v.FullName,
                        phone        = v.Phone,
                        municipality = v.Municipality,
                        matchScore   = (int)Math.Min(100, Math.Round(score / 140.0 * 100, 0)),
                        reasons
                    });
                }
            }

            return suggestions
                .OrderByDescending(s => ((dynamic)s).matchScore)
                .Take(5)
                .ToList<object>();
        }
    }
}