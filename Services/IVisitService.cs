using AmisduMalade.Models;
using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IVisitService
    {
        Task<VisitSession> CreateSessionAsync(CreateVisitSessionVM vm);
        Task<List<VisitSession>> GetByAssignmentAsync(Guid assignmentId);
        Task<VisitSession?> GetSessionByIdAsync(Guid id);
        Task<bool> UpdateSessionAsync(Guid id, UpdateVisitSessionVM vm);
        Task<VisitNote> AddNoteAsync(Guid sessionId, AddVisitNoteVM vm);
        Task<VisitRating> AddRatingAsync(Guid sessionId, AddVisitRatingVM vm);
    }
}