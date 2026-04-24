using AmisduMalade.Models;

namespace AmisduMalade.Services
{
    public interface ICenterService
    {
        Task<List<Center>> GetAllAsync();
        Task<Center?> GetByIdAsync(int id);
        Task<object> GetStatsAsync(int id);
    }
}