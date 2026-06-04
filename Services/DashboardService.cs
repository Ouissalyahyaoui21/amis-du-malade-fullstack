using AmisduMalade.Data;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db) { _db = db; }

        public async Task<DashboardVM> GetDashboardAsync()
        {
            var now = DateTime.UtcNow;
            var thisMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            // الأرقام الأساسية
            var totalVolunteers = await _db.Volunteers.CountAsync();
            var activeVolunteers = await _db.Volunteers
                .CountAsync(v => v.Status == "Approved");
            var pendingVolunteers = await _db.Volunteers
                .CountAsync(v => v.Status == "Pending");
            var totalPatients = await _db.Patients
                .CountAsync(p => p.IsActive);
            var totalRequests = await _db.CareRequests.CountAsync();
            var pendingRequests = await _db.CareRequests
                .CountAsync(r => r.Status == "New" || r.Status == "Reviewing");
            var activeAssignments = await _db.Assignments
                .CountAsync(a => a.Status == "Active" || a.Status == "Assigned");
            var openAlerts = await _db.Alerts
                .CountAsync(a => a.Status == "Open" || a.Status == "InProgress");
            var totalContributions   = await _db.Contributions.CountAsync();
            var pendingContributions = await _db.Contributions
                .CountAsync(c => c.Status == "Pending");

            // المقارنة الشهرية
            var newVolunteersThisMonth = await _db.Volunteers
                .CountAsync(v => v.CreatedAt >= thisMonth);
            var newRequestsThisMonth = await _db.CareRequests
                .CountAsync(r => r.CreatedAt >= thisMonth);

            // توزيع المتطوعين حسب الحالة
            var byStatus = await _db.Volunteers
                .GroupBy(v => v.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            // توزيع الطلبات حسب الأولوية
            var byPriority = await _db.CareRequests
                .GroupBy(r => r.PriorityLevel)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToListAsync();

            // توزيع المتطوعين حسب البلدية
            var byMunicipality = await _db.Volunteers
                .Where(v => v.Municipality != null)
                .GroupBy(v => v.Municipality!)
                .Select(g => new { Municipality = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(6)
                .ToListAsync();

            // آخر الأنشطة
            var activities = new List<ActivityVM>();

            var lastVolunteers = await _db.Volunteers
                .OrderByDescending(v => v.CreatedAt).Take(2).ToListAsync();
            foreach (var v in lastVolunteers)
                activities.Add(new ActivityVM
                {
                    Description = $"تسجيل متطوع جديد: {v.FullName}",
                    TimeAgo = GetTimeAgo(v.CreatedAt),
                    Type = "volunteer"
                });

            var lastRequests = await _db.CareRequests
                .Include(r => r.Patient)
                .OrderByDescending(r => r.CreatedAt).Take(2).ToListAsync();
            foreach (var r in lastRequests)
                activities.Add(new ActivityVM
                {
                    Description = $"طلب جديد للمريض: {r.Patient.FullName}",
                    TimeAgo = GetTimeAgo(r.CreatedAt),
                    Type = "request"
                });

            var lastAlerts = await _db.Alerts
                .OrderByDescending(a => a.CreatedAt).Take(1).ToListAsync();
            foreach (var a in lastAlerts)
                activities.Add(new ActivityVM
                {
                    Description = $"تنبيه: {a.Title}",
                    TimeAgo = GetTimeAgo(a.CreatedAt),
                    Type = "alert"
                });

            return new DashboardVM
            {
                TotalVolunteers = totalVolunteers,
                ActiveVolunteers = activeVolunteers,
                PendingVolunteers = pendingVolunteers,
                TotalPatients = totalPatients,
                TotalCareRequests = totalRequests,
                PendingRequests = pendingRequests,
                ActiveAssignments = activeAssignments,
                OpenAlerts = openAlerts,
                NewVolunteersThisMonth = newVolunteersThisMonth,
                NewRequestsThisMonth = newRequestsThisMonth,
                VolunteersByStatus = byStatus
                    .ToDictionary(x => x.Status ?? "Unknown", x => x.Count),
                RequestsByPriority = byPriority
                    .ToDictionary(x => x.Priority ?? "Normal", x => x.Count),
                VolunteersByMunicipality = byMunicipality
                    .ToDictionary(x => x.Municipality, x => x.Count),
                TotalContributions   = totalContributions,
                PendingContributions = pendingContributions,
                RecentActivities = activities
                    .OrderByDescending(a => a.TimeAgo).Take(5).ToList()
            };
        }

        private string GetTimeAgo(DateTime date)
        {
            var diff = DateTime.UtcNow - date;
            if (diff.TotalMinutes < 60)
                return $"منذ {(int)diff.TotalMinutes} دقيقة";
            if (diff.TotalHours < 24)
                return $"منذ {(int)diff.TotalHours} ساعة";
            return $"منذ {(int)diff.TotalDays} أيام";
        }
    }
}