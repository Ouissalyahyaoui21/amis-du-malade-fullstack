namespace AmisduMalade.ViewModels
{
    // هذا الشكل اللي يرجع للوحة التحكم
    public class DashboardVM
    {
        // الأرقام الكبيرة في الأعلى
        public int TotalVolunteers { get; set; }
        public int ActiveVolunteers { get; set; }
        public int PendingRequests { get; set; }
        public int PatientsServedThisMonth { get; set; }
        
        // مقارنة بالشهر الماضي
        public int NewVolunteersThisMonth { get; set; }
        public int NewVolunteersLastMonth { get; set; }
        
        // توزيع المتطوعين حسب الحالة
        public Dictionary<string, int> VolunteersByStatus { get; set; } = new();
        
        // توزيع حسب البلدية
        public Dictionary<string, int> VolunteersByMunicipality { get; set; } = new();
        
        // التسجيلات الشهرية (آخر 6 أشهر)
        public List<MonthlyStatVM> MonthlyRegistrations { get; set; } = new();
        
        // آخر الأنشطة
        public List<ActivityVM> RecentActivities { get; set; } = new();
    }
    
    // إحصائية شهرية
    public class MonthlyStatVM
    {
        public string Month { get; set; } = "";  // "يناير", "فبراير"...
        public int Count { get; set; }
    }
    
    // نشاط في القائمة
    public class ActivityVM
    {
        public string Description { get; set; } = ""; // "تسجيل جديد: محمد..."
        public string TimeAgo { get; set; } = "";     // "منذ 5 دقائق"
        public string Type { get; set; } = "";        // volunteer/request/training
    }
}