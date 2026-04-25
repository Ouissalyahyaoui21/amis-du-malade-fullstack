namespace AmisduMalade.ViewModels
{
    public class CreateAlertVM
    {
        public Guid PatientId { get; set; }
        public Guid? VolunteerId { get; set; }
        public Guid? CareRequestId { get; set; }
        public Guid? VisitSessionId { get; set; }
        public string Severity { get; set; } = "Medium";
        // Low/Medium/High/Critical
        public string Title { get; set; } = "";
        public string? Description { get; set; }
    }

    public class ResolveAlertVM
    {
        public Guid ResolvedByUserId { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}