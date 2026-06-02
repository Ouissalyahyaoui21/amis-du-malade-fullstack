using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly AppDbContext _db;

        public VolunteerService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Volunteer> RegisterAsync(VolunteerRegisterVM vm)
        {
            var volunteer = new Volunteer
            {
                FullName = vm.FullName,
                Phone = vm.Phone,
                Email = vm.Email,
                Municipality = vm.Municipality,
                VolunteerCategory = vm.VolunteerCategory,
                CanHomeVisit = vm.CanHomeVisit,
                CanHospitalVisit = vm.CanHospitalVisit,
                CanNightPresence = vm.CanNightPresence,
                HasTransportation = vm.HasTransportation,
                BranchId = vm.BranchId,
                Status = "Pending"
            };

            // إضافة المهارات — إيجاد أو إنشاء Skill بالاسم
            foreach (var s in vm.Skills.Where(x => !string.IsNullOrWhiteSpace(x.SkillName)))
            {
                var skill = await _db.Skills.FirstOrDefaultAsync(sk => sk.Name == s.SkillName);
                if (skill == null)
                {
                    skill = new Skill { Name = s.SkillName };
                    _db.Skills.Add(skill);
                    await _db.SaveChangesAsync();
                }
                volunteer.Skills.Add(new VolunteerSkill
                {
                    SkillId = skill.Id,
                    Level = s.Level
                });
            }

            // إضافة التوفر
            foreach (var a in vm.Availabilities)
            {
                volunteer.Availabilities.Add(new VolunteerAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = TimeSpan.Parse(a.StartTime),
                    EndTime = TimeSpan.Parse(a.EndTime)
                });
            }

            // إضافة مناطق التغطية
            foreach (var area in vm.CoverageAreas)
            {
                volunteer.CoverageAreas.Add(new VolunteerCoverageArea
                {
                    Municipality = area
                });
            }

            _db.Volunteers.Add(volunteer);
            await _db.SaveChangesAsync();
            return volunteer;
        }

        public async Task<List<Volunteer>> GetAllAsync()
        {
            return await _db.Volunteers
                .Include(v => v.Skills).ThenInclude(s => s.Skill)
                .Include(v => v.Availabilities)
                .Include(v => v.CoverageAreas)
                .Include(v => v.Branch)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<Volunteer?> GetByIdAsync(Guid id)
        {
            return await _db.Volunteers
                .Include(v => v.Skills).ThenInclude(s => s.Skill)
                .Include(v => v.Availabilities)
                .Include(v => v.CoverageAreas)
                .Include(v => v.Documents)
                .Include(v => v.Interviews)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var volunteer = await _db.Volunteers.FindAsync(id);
            if (volunteer == null) return false;
            volunteer.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}