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

    public class UpdateVisitSessionVM
    {
        public string Status { get; set; } = "";
        // Planned/InProgress/Completed/Cancelled/Missed
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public string? SessionSummary { get; set; }
    }

    public class AddVisitNoteVM
    {
        public Guid VolunteerId { get; set; }
        public string NoteType { get; set; } = "General";
        // General/Medical/Behavioral/Alert
        public string Content { get; set; } = "";
    }

    public class AddVisitRatingVM
    {
        public int Score { get; set; } // 1-5
        public string? Comment { get; set; }
    }
}