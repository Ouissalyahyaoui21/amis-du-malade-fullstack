using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface ICareRequestService
    {
        Task<CareRequest> CreateAsync(CreateCareRequestVM vm);
        Task<CareRequestResponseVM> CreatePublicAsync(CreateCareRequestPublicVM vm);
        Task<List<CareRequest>> GetAllAsync();
        Task<CareRequest?> GetByIdAsync(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, string status);
        Task<List<object>> GetSuggestionsAsync(Guid requestId);
    }
}