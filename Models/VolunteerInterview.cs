namespace AmisduMalade.Models
{
    public class VolunteerInterview
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public Guid? InterviewedByUserId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Location { get; set; } = "";
        public string Status { get; set; } = "Scheduled";
        // Scheduled/Completed/Cancelled
        public int? Score { get; set; }
        public string? Result { get; set; }
        // Accepted / Rejected
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}