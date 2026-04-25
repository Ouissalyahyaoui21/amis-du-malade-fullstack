using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IAssignmentService
    {
        Task<Assignment> CreateAsync(CreateAssignmentVM vm);
        Task<List<Assignment>> GetAllAsync();
        Task<bool> UpdateStatusAsync(Guid id, string status);
    }
}