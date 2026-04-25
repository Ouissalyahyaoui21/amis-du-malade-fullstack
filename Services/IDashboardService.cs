using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IDashboardService
    {
        Task<DashboardVM> GetDashboardAsync();
    }
}