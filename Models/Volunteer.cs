namespace AmisduMalade.Models
{
    public class Volunteer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? BranchId { get; set; }
        public AssociationBranch? Branch { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
        public string? Municipality { get; set; }
        public string? VolunteerCategory { get; set; }
        public string? TrainingLevel { get; set; }
        public bool CanHomeVisit { get; set; } = true;
        public bool CanHospitalVisit { get; set; } = false;
        public bool CanNightPresence { get; set; } = false;
        public bool HasTransportation { get; set; } = false;
        public string Status { get; set; } = "Pending";
        // Pending/Interview/Approved/Rejected/Suspended
        public bool VerificationBadge { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // العلاقات
        public List<VolunteerSkill> Skills { get; set; } = new();
        public List<VolunteerAvailability> Availabilities { get; set; } = new();
        public List<VolunteerCoverageArea> CoverageAreas { get; set; } = new();
        public List<VolunteerDocument> Documents { get; set; } = new();
        public List<VolunteerInterview> Interviews { get; set; } = new();
    }
}