using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    // Interface = عقد - يحدد ماذا يفعل الـ Service
    public interface IVolunteerService
    {
        Task<Volunteer> RegisterAsync(VolunteerRegisterVM vm);
        Task<List<Volunteer>> GetAllAsync();
        Task<Volunteer?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}