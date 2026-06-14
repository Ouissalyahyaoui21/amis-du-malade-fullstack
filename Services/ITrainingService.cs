using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface ITrainingService
    {
        Task<TrainingResponseVM> CreateAsync(CreateTrainingVM vm);
        Task<List<TrainingResponseVM>> GetAllAsync();
        Task<TrainingResponseVM?> GetByIdAsync(Guid id);
        Task<EnrollmentResponseVM> EnrollVolunteerAsync(Guid trainingId, Guid volunteerId);
        Task<List<EnrollmentResponseVM>> GetEnrollmentsAsync(Guid trainingId);
        Task<bool> CompleteEnrollmentAsync(Guid enrollmentId, string status);
    }
}
