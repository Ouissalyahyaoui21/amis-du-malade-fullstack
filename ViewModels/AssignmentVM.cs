namespace AmisduMalade.ViewModels
{
    public class CreateAssignmentVM
    {
        public Guid CareRequestId { get; set; }
        public Guid VolunteerId { get; set; }
        public string AssignmentType { get; set; } = "Primary";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}