namespace AmisduMalade.ViewModels
{
    // هذا هو الشكل اللي يرسله الموبايل لطلب مرافق
    public class RequestCreateVM
    {
        // الخطوة 1 - معلومات الراغب
        public string RequesterName { get; set; } = "";
        public string RequesterPhone { get; set; } = "";
        public string RequesterCity { get; set; } = "";
        public string RelationToPatient { get; set; } = "";

        // الخطوة 2 - معلومات المريض
        public string PatientName { get; set; } = "";
        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = "";
        public string PatientAddress { get; set; } = "";
        public List<string> HealthConditions { get; set; } = new();
        public string PatientDescription { get; set; } = "";

        // الخطوة 3 - تفاصيل الطلب
        public string PatientLocation { get; set; } = "";
        public DateTime StartDate { get; set; }
        public string Duration { get; set; } = "";
        public bool NeedsNightCare { get; set; }

        // الخطوة 4 - مؤهلات المرافق
        public List<string> RequiredSkills { get; set; } = new();
        public string Notes { get; set; } = "";
    }
}