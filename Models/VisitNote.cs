namespace AmisduMalade.Models
{
    public class VisitNote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VisitSessionId { get; set; }
        public VisitSession VisitSession { get; set; } = null!;
        public Guid VolunteerId { get; set; }
        public string NoteType { get; set; } = "General";
        // General/Medical/Behavioral/Alert
        public string Content { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}