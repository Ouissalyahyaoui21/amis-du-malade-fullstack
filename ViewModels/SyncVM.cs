namespace AmisduMalade.ViewModels
{
    // الموبايل يرسل هذا عند المزامنة
    public class SyncPullRequestVM
    {
        public string EntityName { get; set; } = "";
        public int LastServerVersion { get; set; } = 0;
    }

    // الموبايل يرسل عملية معلقة
    public class PendingOperationVM
    {
        public string EntityName { get; set; } = "";
        public string EntityId { get; set; } = "";
        public string OperationType { get; set; } = "";
        // Create/Update/Delete
        public string PayloadJson { get; set; } = "";
    }
}