using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AmisduMalade.Services
{
    // هنا يوجد المنطق الفعلي
    public class VolunteerService : IVolunteerService
    {
        private readonly AppDbContext _db;

        public VolunteerService(AppDbContext db)
        {
            _db = db;
        }

        // تسجيل متطوع جديد
        public async Task<Volunteer> RegisterAsync(VolunteerRegisterVM vm)
        {
            var volunteer = new Volunteer
            {
                FullName = vm.FullName,
                Phone = vm.Phone,
                BirthDate = vm.BirthDate,
                Municipality = vm.Municipality,
                Profession = vm.Profession,
                MaritalStatus = vm.MaritalStatus,
                // نحول القائمة لـ JSON لحفظها في DB
                AvailableDays = JsonSerializer.Serialize(vm.AvailableDays),
                AvailableHours = JsonSerializer.Serialize(vm.AvailableHours),
                Skills = JsonSerializer.Serialize(vm.Skills),
                TrainingLevel = vm.TrainingLevel,
                Status = "Pending"
            };

            _db.Volunteers.Add(volunteer);
            await _db.SaveChangesAsync();
            return volunteer;
        }

        // جيب كل المتطوعين
        public async Task<List<Volunteer>> GetAllAsync()
        {
            return await _db.Volunteers.ToListAsync();
        }

        // جيب متطوع بالـ ID
        public async Task<Volunteer?> GetByIdAsync(int id)
        {
            return await _db.Volunteers.FindAsync(id);
        }

        // غيّر حالة المتطوع (قبول/رفض)
        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var volunteer = await _db.Volunteers.FindAsync(id);
            if (volunteer == null) return false;

            volunteer.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}