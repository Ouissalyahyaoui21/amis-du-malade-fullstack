namespace AmisduMalade.Models
{
    public class Assignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CareRequestId { get; set; }
        public CareRequest CareRequest { get; set; } = null!;
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string AssignmentType { get; set; } = "Primary";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Assigned";
        // Assigned/Active/Completed/Cancelled
        public string? CancellationReason { get; set; }
        public string? Notes { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        public List<VisitSession> VisitSessions { get; set; } = new();
    }
}