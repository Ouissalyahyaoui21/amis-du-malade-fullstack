namespace AmisduMalade.ViewModels
{
    public class CreateVisitSessionVM
    {
        public Guid AssignmentId { get; set; }
        public string SessionType { get; set; } = "Regular";
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string? LocationNotes { get; set; }
    }

    public class AddVisitNoteVM
    {
        public Guid VolunteerId { get; set; }
        public string NoteType { get; set; } = "General";
        public string Content { get; set; } = "";
    }

    public class AddVisitRatingVM
    {
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}