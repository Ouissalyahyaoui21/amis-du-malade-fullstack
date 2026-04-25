namespace AmisduMalade.Models
{
    public class Alert
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Guid? VolunteerId { get; set; }
        public Guid? CareRequestId { get; set; }
        public Guid? VisitSessionId { get; set; }
        public string Severity { get; set; } = "Medium";
        // Low/Medium/High/Critical
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string Status { get; set; } = "Open";
        // Open/InProgress/Resolved/Closed
        public Guid? ResolvedByUserId { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}