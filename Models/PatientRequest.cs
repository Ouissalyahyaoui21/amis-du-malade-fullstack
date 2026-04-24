namespace AmisduMalade.Models
{
    public class PatientRequest
    {
        public int Id { get; set; }

        // معلومات الراغب (العائلة)
        public string RequesterName { get; set; } = "";
        public string RequesterPhone { get; set; } = "";
        public string RequesterCity { get; set; } = "";
        public string RelationToPatient { get; set; } = "";

        // معلومات المريض
        public string PatientName { get; set; } = "";
        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = "";
        public string PatientAddress { get; set; } = "";
        public string HealthConditions { get; set; } = "";
        public string PatientDescription { get; set; } = "";

        // تفاصيل الطلب
        public string PatientLocation { get; set; } = "";
        public DateTime StartDate { get; set; }
        public string Duration { get; set; } = "";
        public bool NeedsNightCare { get; set; }

        // مؤهلات المرافق المطلوبة
        public string RequiredSkills { get; set; } = "";
        public string Notes { get; set; } = "";

        // الحالة
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}