using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IInterviewService
    {
        Task<Interview> CreateAsync(CreateInterviewVM vm);
        Task<List<Interview>> GetAllAsync();
        Task<List<Interview>> GetByVolunteerAsync(int volunteerId);
        Task<bool> CompleteAsync(int id, CompleteInterviewVM vm);
        Task<bool> CancelAsync(int id);
    }
}