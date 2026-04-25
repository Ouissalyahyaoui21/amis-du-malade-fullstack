namespace AmisduMalade.Models
{
    public class VolunteerDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string DocumentType { get; set; } = "";
        // NationalId / MedicalCertificate / Photo / Other
        public string FilePath { get; set; } = "";
        public string Status { get; set; } = "Pending";
        // Pending / Verified / Rejected
        public Guid? VerifiedByUserId { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}