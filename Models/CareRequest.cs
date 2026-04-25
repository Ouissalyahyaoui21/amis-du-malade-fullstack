namespace AmisduMalade.Models
{
    public class CareRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Guid? RequestedByContactId { get; set; }
        public Guid? BranchId { get; set; }
        public string? CareLocationType { get; set; }
        // Home/Hospital/NursingHome/Other
        public Guid? HospitalId { get; set; }
        public string? LocationDetails { get; set; }
        public string? Municipality { get; set; }
        public DateTime RequestedStartDate { get; set; }
        public DateTime? RequestedEndDate { get; set; }
        public string? DurationUnit { get; set; }
        // Day/Week/Month
        public string PriorityLevel { get; set; } = "Normal";
        // Low/Normal/High/Urgent
        public bool NeedsNightPresence { get; set; } = false;
        public bool NeedsTransportSupport { get; set; } = false;
        public string? MedicalSummary { get; set; }
        public string? SupportSummary { get; set; }
        public string Status { get; set; } = "New";
        // New/Reviewing/Assigned/Active/Completed/Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CareRequestRequiredSkill> RequiredSkills { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }
}