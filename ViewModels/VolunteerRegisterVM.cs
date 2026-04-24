namespace AmisduMalade.ViewModels
{
    // هذا هو الشكل اللي يرسله الموبايل لتسجيل متطوع
    public class VolunteerRegisterVM
    {
        // الخطوة 1 - المعلومات الشخصية
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Municipality { get; set; } = "";
        public string Profession { get; set; } = "";
        public string MaritalStatus { get; set; } = "";

        // الخطوة 2 - التوفر
        public List<string> AvailableDays { get; set; } = new();
        public List<string> AvailableHours { get; set; } = new();

        // الخطوة 3 - المهارات
        public List<string> Skills { get; set; } = new();
        public string TrainingLevel { get; set; } = "";
    }
}