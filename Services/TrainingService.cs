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

        public async Task<TrainingResponseVM> CreateAsync(CreateTrainingVM vm)
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
            return MapToVM(training, 0);
        }

        public async Task<List<TrainingResponseVM>> GetAllAsync()
        {
            var trainings = await _db.Trainings
                .Include(t => t.Enrollments)
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();

            return trainings.Select(t => MapToVM(t, t.Enrollments.Count)).ToList();
        }

        public async Task<TrainingResponseVM?> GetByIdAsync(Guid id)
        {
            var training = await _db.Trainings
                .Include(t => t.Enrollments)
                .FirstOrDefaultAsync(t => t.Id == id);

            return training == null ? null : MapToVM(training, training.Enrollments.Count);
        }

        public async Task<EnrollmentResponseVM> EnrollVolunteerAsync(Guid trainingId, Guid volunteerId)
        {
            var enrollment = new TrainingEnrollment
            {
                TrainingId = trainingId,
                VolunteerId = volunteerId,
                Status = "Enrolled"
            };

            _db.TrainingEnrollments.Add(enrollment);
            await _db.SaveChangesAsync();

            await _db.Entry(enrollment).Reference(e => e.Training).LoadAsync();
            await _db.Entry(enrollment).Reference(e => e.Volunteer).LoadAsync();
            return MapEnrollmentToVM(enrollment);
        }

        public async Task<List<EnrollmentResponseVM>> GetEnrollmentsAsync(Guid trainingId)
        {
            var enrollments = await _db.TrainingEnrollments
                .Include(e => e.Volunteer)
                .Include(e => e.Training)
                .Where(e => e.TrainingId == trainingId)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();

            return enrollments.Select(MapEnrollmentToVM).ToList();
        }

        public async Task<bool> CompleteEnrollmentAsync(Guid enrollmentId, string status)
        {
            var enrollment = await _db.TrainingEnrollments.FindAsync(enrollmentId);
            if (enrollment == null) return false;

            enrollment.Status = status; // Completed / Failed
            await _db.SaveChangesAsync();
            return true;
        }

        private static TrainingResponseVM MapToVM(Training t, int enrolledCount) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            StartDate = t.StartDate,
            Location = t.Location,
            Capacity = t.Capacity,
            Status = t.Status,
            EnrolledCount = enrolledCount,
            CreatedAt = t.CreatedAt
        };

        private static EnrollmentResponseVM MapEnrollmentToVM(TrainingEnrollment e) => new()
        {
            Id = e.Id,
            TrainingId = e.TrainingId,
            TrainingTitle = e.Training?.Title ?? "",
            VolunteerId = e.VolunteerId,
            VolunteerName = e.Volunteer?.FullName ?? "",
            Status = e.Status,
            EnrolledAt = e.EnrolledAt
        };
    }
}
