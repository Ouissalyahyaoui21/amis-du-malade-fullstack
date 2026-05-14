using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IContributionService
    {
        Task<ContributionResponseVM> CreateAsync(CreateContributionVM vm);
        Task<List<ContributionResponseVM>> GetAllAsync();
        Task<ContributionResponseVM?> GetByIdAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
    }
}
