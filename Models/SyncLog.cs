namespace AmisduMalade.Models
{
    public class SyncLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EntityName { get; set; } = "";
        public string Direction { get; set; } = "";
        // Pull / Push
        public string? OperationType { get; set; }
        public string? EntityId { get; set; }
        public int RecordCount { get; set; } = 0;
        public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
    }
}