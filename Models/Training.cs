namespace AmisduMalade.Models
{
    public class Training
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<TrainingEnrollment> Enrollments { get; set; } = new();
    }
}