using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface ITrainingService
    {
        Task<Training> CreateAsync(CreateTrainingVM vm);
        Task<List<Training>> GetAllAsync();
        Task<Training?> GetByIdAsync(int id);
        Task<bool> EnrollAsync(int trainingId, EnrollVolunteerVM vm);
        Task<bool> UpdateEnrollmentAsync(int trainingId, int volunteerId, UpdateEnrollmentVM vm);
        Task<bool> CompleteTrainingAsync(int id);
    }
}