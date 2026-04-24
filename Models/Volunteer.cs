namespace AmisduMalade.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Municipality { get; set; } = "";
        public string Profession { get; set; } = "";
        public string MaritalStatus { get; set; } = "";

        // التوفر
        public string AvailableDays { get; set; } = "";
        public string AvailableHours { get; set; } = "";

        // المهارات
        public string Skills { get; set; } = "";
        public string TrainingLevel { get; set; } = "";

        // الحالة
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}