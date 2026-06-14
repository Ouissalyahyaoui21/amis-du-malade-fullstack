namespace AmisduMalade.ViewModels
{
    public class ScheduleInterviewVM
    {
        public Guid VolunteerId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Location { get; set; } = "";
    }

    public class RecordInterviewResultVM
    {
        public string Result { get; set; } = ""; // Accepted / Rejected
        public int? Score { get; set; }
        public string? Notes { get; set; }
    }

    public class InterviewResponseVM
    {
        public Guid Id { get; set; }
        public Guid VolunteerId { get; set; }
        public string VolunteerName { get; set; } = "";
        public string VolunteerPhone { get; set; } = "";
        public DateTime ScheduledAt { get; set; }
        public string Location { get; set; } = "";
        public string Status { get; set; } = "";
        public int? Score { get; set; }
        public string? Result { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
