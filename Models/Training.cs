namespace AmisduMalade.Models
{
    // الدورة التدريبية
    public class Training
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";        // عنوان الدورة
        public string Description { get; set; } = "";  // وصف
        public DateTime StartDate { get; set; }        // تاريخ البداية
        public string Location { get; set; } = "";     // المكان
        public int Capacity { get; set; }              // عدد المقاعد
        public string Status { get; set; } = "Active"; // Active/Completed
        
        // المتطوعون المسجلون في هذه الدورة
        public List<TrainingEnrollment> Enrollments { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    
    // تسجيل متطوع في دورة
    public class TrainingEnrollment
    {
        public int Id { get; set; }
        public int TrainingId { get; set; }
        public Training Training { get; set; } = null!;
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        public string Status { get; set; } = "Enrolled"; 
        // Enrolled/Completed/Failed
        public DateTime EnrolledAt { get; set; } = DateTime.Now;
    }
}