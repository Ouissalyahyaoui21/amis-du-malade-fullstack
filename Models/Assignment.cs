namespace AmisduMalade.Models
{
    // التكليف = ربط متطوع بطلب مريض
    public class Assignment
    {
        public int Id { get; set; }
        
        // الطرفان
        public int VolunteerId { get; set; }
        public Volunteer Volunteer { get; set; } = null!;
        
        public int RequestId { get; set; }
        public PatientRequest Request { get; set; } = null!;
        
        // نوع التكليف
        public bool IsAutomatic { get; set; } = false; 
        // true = تلقائي بالخوارزمية
        // false = يدوي من الإدارة
        
        public double MatchScore { get; set; } = 0;
        // نقطة التوافق من 100
        // مثلاً: 87.5 = توافق ممتاز
        
        public string Status { get; set; } = "Pending";
        // Pending/Active/Completed/Cancelled
        
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }
}