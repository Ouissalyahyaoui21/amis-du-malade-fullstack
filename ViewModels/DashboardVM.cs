namespace AmisduMalade.ViewModels
{
    public class DashboardVM
    {
        // الأرقام الكبيرة
        public int TotalVolunteers { get; set; }
        public int ActiveVolunteers { get; set; }
        public int PendingVolunteers { get; set; }
        public int TotalPatients { get; set; }
        public int TotalCareRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ActiveAssignments { get; set; }
        public int OpenAlerts { get; set; }

        // المقارنة الشهرية
        public int NewVolunteersThisMonth { get; set; }
        public int NewRequestsThisMonth { get; set; }

        // توزيع المتطوعين حسب الحالة
        public Dictionary<string, int> VolunteersByStatus { get; set; } = new();

        // توزيع الطلبات حسب الأولوية
        public Dictionary<string, int> RequestsByPriority { get; set; } = new();

        // توزيع حسب البلدية
        public Dictionary<string, int> VolunteersByMunicipality { get; set; } = new();

        // المساهمات
        public int TotalContributions   { get; set; }
        public int PendingContributions { get; set; }

        // آخر الأنشطة
        public List<ActivityVM> RecentActivities { get; set; } = new();
    }

    public class ActivityVM
    {
        public string Description { get; set; } = "";
        public string TimeAgo { get; set; } = "";
        public string Type { get; set; } = "";
        // volunteer/request/assignment/alert
    }
}