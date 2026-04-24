using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly AppDbContext _db;

        public TrainingService(AppDbContext db)
        {
            _db = db;
        }

        // إنشاء دورة تدريبية جديدة
        public async Task<Training> CreateAsync(CreateTrainingVM vm)
        {
            var training = new Training
            {
                Title = vm.Title,
                Description = vm.Description,
                StartDate = vm.StartDate,
                Location = vm.Location,
                Capacity = vm.Capacity,
                Status = "Active"
            };

            _db.Trainings.Add(training);
            await _db.SaveChangesAsync();
            return training;
        }

        // جيب كل الدورات مع عدد المسجلين
        public async Task<List<Training>> GetAllAsync()
        {
            return await _db.Trainings
                .Include(t => t.Enrollments)
                .ThenInclude(e => e.Volunteer)
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();
        }

        // جيب دورة محددة
        public async Task<Training?> GetByIdAsync(int id)
        {
            return await _db.Trainings
                .Include(t => t.Enrollments)
                .ThenInclude(e => e.Volunteer)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // تسجيل متطوع في دورة
        public async Task<bool> EnrollAsync(int trainingId, EnrollVolunteerVM vm)
        {
            var training = await _db.Trainings
                .Include(t => t.Enrollments)
                .FirstOrDefaultAsync(t => t.Id == trainingId);

            if (training == null) return false;

            // تحقق من الطاقة الاستيعابية
            if (training.Enrollments.Count >= training.Capacity)
                throw new Exception("الدورة مكتملة - لا توجد أماكن شاغرة");

            // تحقق أن المتطوع مش مسجل مسبقاً
            var exists = training.Enrollments
                .Any(e => e.VolunteerId == vm.VolunteerId);
            if (exists)
                throw new Exception("المتطوع مسجل في هذه الدورة مسبقاً");

            var enrollment = new TrainingEnrollment
            {
                TrainingId = trainingId,
                VolunteerId = vm.VolunteerId,
                Status = "Enrolled"
            };

            _db.TrainingEnrollments.Add(enrollment);
            await _db.SaveChangesAsync();
            return true;
        }

        // تحديث نتيجة متطوع (نجح/رسب)
        public async Task<bool> UpdateEnrollmentAsync(
            int trainingId, int volunteerId, UpdateEnrollmentVM vm)
        {
            var enrollment = await _db.TrainingEnrollments
                .FirstOrDefaultAsync(e =>
                    e.TrainingId == trainingId &&
                    e.VolunteerId == volunteerId);

            if (enrollment == null) return false;

            enrollment.Status = vm.Status;

            // إذا أتم الدورة بنجاح - حدّث مستوى تدريب المتطوع
            if (vm.Status == "Completed")
            {
                var volunteer = await _db.Volunteers
                    .FindAsync(volunteerId);
                if (volunteer != null)
                    volunteer.TrainingLevel = "مؤهل";
            }

            await _db.SaveChangesAsync();
            return true;
        }

        // إنهاء الدورة
        public async Task<bool> CompleteTrainingAsync(int id)
        {
            var training = await _db.Trainings.FindAsync(id);
            if (training == null) return false;

            training.Status = "Completed";
            await _db.SaveChangesAsync();
            return true;
        }
    }
}