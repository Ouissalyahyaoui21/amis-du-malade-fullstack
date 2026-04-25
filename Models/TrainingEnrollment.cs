namespace AmisduMalade.Models
{
    public class TrainingEnrollment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TrainingId { get; set; }
        public Training Training { get; set; } = null!;
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string Status { get; set; } = "Enrolled";
        // Enrolled/Completed/Failed
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}