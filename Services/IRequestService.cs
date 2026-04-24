using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IRequestService
    {
        Task<PatientRequest> CreateAsync(RequestCreateVM vm);
        Task<List<PatientRequest>> GetAllAsync();
        Task<PatientRequest?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}