namespace AmisduMalade.ViewModels
{
    public class CreateCareRequestVM
    {
        public Guid PatientId { get; set; }
        public Guid? RequestedByContactId { get; set; }
        public Guid? BranchId { get; set; }
        public string? CareLocationType { get; set; }
        public Guid? HospitalId { get; set; }
        public string? LocationDetails { get; set; }
        public string? Municipality { get; set; }
        public DateTime RequestedStartDate { get; set; }
        public DateTime? RequestedEndDate { get; set; }
        public string? DurationUnit { get; set; }
        public string PriorityLevel { get; set; } = "Normal";
        public bool NeedsNightPresence { get; set; } = false;
        public bool NeedsTransportSupport { get; set; } = false;
        public string? MedicalSummary { get; set; }
        public string? SupportSummary { get; set; }

        // المهارات المطلوبة
        public List<RequiredSkillVM> RequiredSkills { get; set; } = new();
    }

    public class RequiredSkillVM
    {
        public Guid SkillId { get; set; }
        public string? RequiredLevel { get; set; }
        public bool Mandatory { get; set; } = true;
    }
}