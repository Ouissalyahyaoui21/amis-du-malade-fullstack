namespace AmisduMalade.ViewModels
{
    // لإنشاء دورة تدريبية جديدة
    public class CreateTrainingVM
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public string Location { get; set; } = "";
        public int Capacity { get; set; }
    }

    // لتسجيل متطوع في دورة
    public class EnrollVolunteerVM
    {
        public int VolunteerId { get; set; }
    }

    // لتحديث نتيجة متطوع بعد الدورة
    public class UpdateEnrollmentVM
    {
        public string Status { get; set; } = "";
        // "Completed" = أتم الدورة بنجاح
        // "Failed"    = لم يجتز الدورة
    }
}