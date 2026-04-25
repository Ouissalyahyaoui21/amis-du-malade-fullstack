namespace AmisduMalade.ViewModels
{
    // تسجيل متطوع جديد - الخطوة 1
    public class VolunteerRegisterVM
    {
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string? Email { get; set; }
        public string? Municipality { get; set; }
        public string? VolunteerCategory { get; set; }
        public bool CanHomeVisit { get; set; } = true;
        public bool CanHospitalVisit { get; set; } = false;
        public bool CanNightPresence { get; set; } = false;
        public bool HasTransportation { get; set; } = false;
        public Guid? BranchId { get; set; }

        // المهارات - قائمة IDs
        public List<VolunteerSkillVM> Skills { get; set; } = new();

        // التوفر - قائمة أوقات
        public List<VolunteerAvailabilityVM> Availabilities { get; set; } = new();

        // مناطق التغطية
        public List<string> CoverageAreas { get; set; } = new();
    }

    public class VolunteerSkillVM
    {
        public Guid SkillId { get; set; }
        public string? Level { get; set; }
    }

    public class VolunteerAvailabilityVM
    {
        public string DayOfWeek { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }

    public class UpdateVolunteerStatusVM
    {
        public string Status { get; set; } = "";
        // Pending/Interview/Approved/Rejected/Suspended
    }
}