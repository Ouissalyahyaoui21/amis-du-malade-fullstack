namespace AmisduMalade.Models
{
    public class PatientContact
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
        public string? RelationToPatient { get; set; }
        public bool IsPrimaryContact { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}