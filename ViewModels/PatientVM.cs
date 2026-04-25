namespace AmisduMalade.ViewModels
{
    public class CreatePatientVM
    {
        public string FullName { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Municipality { get; set; }
        public string? CurrentResidenceType { get; set; }
        public Guid? CurrentHospitalId { get; set; }
        public string? MobilityStatus { get; set; }
        public string? DependencyLevel { get; set; }
        public string? Notes { get; set; }

        // جهات الاتصال
        public List<PatientContactVM> Contacts { get; set; } = new();

        // الأمراض
        public List<PatientMedicalConditionVM> MedicalConditions { get; set; } = new();
    }

    public class PatientContactVM
    {
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
        public string? RelationToPatient { get; set; }
        public bool IsPrimaryContact { get; set; } = false;
    }

    public class PatientMedicalConditionVM
    {
        public Guid MedicalConditionId { get; set; }
        public string? Severity { get; set; }
        public string? Notes { get; set; }
    }
}