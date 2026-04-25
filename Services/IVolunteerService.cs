using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IVolunteerService
    {
        Task<Volunteer> RegisterAsync(VolunteerRegisterVM vm);
        Task<List<Volunteer>> GetAllAsync();
        Task<Volunteer?> GetByIdAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
    }
}