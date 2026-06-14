using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface IInterviewService
    {
        Task<InterviewResponseVM> ScheduleAsync(ScheduleInterviewVM vm);
        Task<List<InterviewResponseVM>> GetAllAsync();
        Task<InterviewResponseVM?> GetByIdAsync(Guid id);
        Task<bool> RecordResultAsync(Guid id, RecordInterviewResultVM vm);
        Task<bool> CancelAsync(Guid id);
    }
}
