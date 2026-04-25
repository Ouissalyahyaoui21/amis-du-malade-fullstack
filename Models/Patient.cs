namespace AmisduMalade.Models
{
    public class Patient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Municipality { get; set; }
        public string? CurrentResidenceType { get; set; }
        // Home / Hospital / NursingHome / Other
        public Guid? CurrentHospitalId { get; set; }
        public Hospital? CurrentHospital { get; set; }
        public string? MobilityStatus { get; set; }
        public string? DependencyLevel { get; set; }
        // Low / Medium / High / Total
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // العلاقات
        public List<PatientContact> Contacts { get; set; } = new();
        public List<PatientMedicalCondition> MedicalConditions { get; set; } = new();
        public List<CareRequest> CareRequests { get; set; } = new();
    }
}