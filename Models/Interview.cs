namespace AmisduMalade.Models
{
    // المقابلة الشخصية للمتطوع
    public class Interview
    {
        public int Id { get; set; }
        
        // ربط بالمتطوع
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        
        // تفاصيل المقابلة
        public DateTime ScheduledAt { get; set; }    // موعد المقابلة
        public string CenterLocation { get; set; } = ""; // المركز
        public string Status { get; set; } = "Scheduled"; 
        // Scheduled = محددة
        // Completed = تمت
        // Cancelled = ملغاة
        
        public string? Notes { get; set; }           // ملاحظات الإدارة
        public int? Score { get; set; }              // تقييم من 5
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}