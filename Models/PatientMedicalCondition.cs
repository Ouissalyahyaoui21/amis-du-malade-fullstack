namespace AmisduMalade.Models
{
    public class PatientMedicalCondition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Guid MedicalConditionId { get; set; }
        public MedicalCondition MedicalCondition { get; set; } = null!;
        public string? Severity { get; set; }
        public string? Notes { get; set; }
        public DateTime DiagnosedAt { get; set; } = DateTime.UtcNow;
    }
}