namespace AmisduMalade.ViewModels
{
    public class CreateTrainingVM
    {
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; } = 20;
    }

    public class EnrollVolunteerVM
    {
        public Guid VolunteerId { get; set; }
    }

    public class TrainingResponseVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "";
        public int EnrolledCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class EnrollmentResponseVM
    {
        public Guid Id { get; set; }
        public Guid TrainingId { get; set; }
        public string TrainingTitle { get; set; } = "";
        public Guid VolunteerId { get; set; }
        public string VolunteerName { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime EnrolledAt { get; set; }
    }
}
