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

    // نموذج مخصص للموبايل — يقبل بيانات المريض والطالب مباشرة
    public class CreateCareRequestPublicVM
    {
        // معلومات المريض (يُنشأ تلقائياً)
        public string PatientName { get; set; } = "";
        public int? PatientAge { get; set; }
        public string? PatientGender { get; set; }
        public string? PatientMunicipality { get; set; }

        // معلومات الطالب (جهة الاتصال)
        public string RequesterName { get; set; } = "";
        public string RequesterPhone { get; set; } = "";
        public string? RequesterRelation { get; set; }

        // تفاصيل الطلب
        public string? CareLocationType { get; set; }   // Home/Hospital/NursingHome/Other
        public string? Municipality { get; set; }
        public DateTime RequestedStartDate { get; set; } = DateTime.UtcNow;
        public bool NeedsNightPresence { get; set; } = false;
        public bool NeedsTransportSupport { get; set; } = false;
        public string PriorityLevel { get; set; } = "Normal";
        public string? MedicalSummary { get; set; }     // الحالات الصحية
        public string? SupportSummary { get; set; }     // ملاحظات المرافقة

        // أسماء المهارات المطلوبة (مثل "nursing", "transport")
        public List<string> RequiredSkillNames { get; set; } = new();
    }

    // استجابة موحدة لطلب المرافقة
    public class CareRequestResponseVM
    {
        public Guid Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public string Status { get; set; } = "";
        public string PatientName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}