using AmisduMalade.ViewModels;

namespace AmisduMalade.Services
{
    public interface ISyncService
    {
        Task<object> PullAsync(SyncPullRequestVM vm);
        Task<object> PushAsync(PendingOperationVM vm);
    }
}