using AmisduMalade.Models;

namespace AmisduMalade.Services
{
    public interface IAssignmentService
    {
        Task<List<object>> GetSuggestionsAsync(int requestId);
        Task<Assignment> AssignAsync(int requestId, int volunteerId, bool isAutomatic);
        Task<List<Assignment>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int id, string status);
    }
}