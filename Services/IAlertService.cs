using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IAlertService
    {
        Task<Alert> CreateAsync(CreateAlertVM vm);
        Task<List<Alert>> GetAllAsync();
        Task<List<Alert>> GetOpenAsync();
        Task<bool> ResolveAsync(Guid id, ResolveAlertVM vm);
    }
}