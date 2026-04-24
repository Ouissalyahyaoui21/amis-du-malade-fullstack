namespace AmisduMalade.ViewModels
{
    // لتحديد موعد مقابلة جديدة
    public class CreateInterviewVM
    {
        public int VolunteerId { get; set; }        // رقم المتطوع
        public DateTime ScheduledAt { get; set; }   // موعد المقابلة
        public string CenterLocation { get; set; } = ""; // المركز
    }

    // لتسجيل نتيجة المقابلة بعد إجرائها
    public class CompleteInterviewVM
    {
        public string Result { get; set; } = "";
        // "Accepted" = قبول المتطوع
        // "Rejected" = رفض المتطوع
        public int Score { get; set; }          // تقييم من 5
        public string? Notes { get; set; }      // ملاحظات الإدارة
    }
}