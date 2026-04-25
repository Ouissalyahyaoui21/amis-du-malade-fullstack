namespace AmisduMalade.Models
{
    public class VisitSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        public string SessionType { get; set; } = "Regular";
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public string Status { get; set; } = "Planned";
        // Planned/InProgress/Completed/Cancelled/Missed
        public string? LocationNotes { get; set; }
        public string? SessionSummary { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<VisitNote> Notes { get; set; } = new();
        public VisitRating? Rating { get; set; }
    }
}