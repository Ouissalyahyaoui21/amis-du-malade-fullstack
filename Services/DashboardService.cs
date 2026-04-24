using AmisduMalade.Data;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardVM> GetDashboardAsync()
        {
            var now = DateTime.Now;
            var thisMonth = new DateTime(now.Year, now.Month, 1);
            var lastMonth = thisMonth.AddMonths(-1);

            // ── الأرقام الأساسية ──────────────────────────────
            var totalVolunteers = await _db.Volunteers.CountAsync();
            
            var activeVolunteers = await _db.Volunteers
                .CountAsync(v => v.Status == "Active");
            
            var pendingRequests = await _db.PatientRequests
                .CountAsync(r => r.Status == "Pending");
            
            // المرضى المخدومين = الطلبات المكتملة هذا الشهر
            var patientsThisMonth = await _db.PatientRequests
                .CountAsync(r => r.Status == "Completed" 
                             && r.CreatedAt >= thisMonth);

            // ── المقارنة الشهرية ──────────────────────────────
            var newThisMonth = await _db.Volunteers
                .CountAsync(v => v.CreatedAt >= thisMonth);
            
            var newLastMonth = await _db.Volunteers
                .CountAsync(v => v.CreatedAt >= lastMonth 
                             && v.CreatedAt < thisMonth);

            // ── توزيع المتطوعين حسب الحالة ───────────────────
            var byStatus = await _db.Volunteers
                .GroupBy(v => v.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            // ── توزيع حسب البلدية ─────────────────────────────
            var byMunicipality = await _db.Volunteers
                .GroupBy(v => v.Municipality)
                .Select(g => new { Municipality = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(6) // أعلى 6 بلديات
                .ToListAsync();

            // ── التسجيلات الشهرية (آخر 6 أشهر) ──────────────
            var monthlyStats = new List<MonthlyStatVM>();
            string[] arabicMonths = { "يناير","فبراير","مارس","أبريل","مايو","يونيو",
                                      "يوليو","أغسطس","سبتمبر","أكتوبر","نوفمبر","ديسمبر" };
            
            for (int i = 5; i >= 0; i--)
            {
                var monthStart = thisMonth.AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var count = await _db.Volunteers
                    .CountAsync(v => v.CreatedAt >= monthStart 
                                 && v.CreatedAt < monthEnd);
                monthlyStats.Add(new MonthlyStatVM
                {
                    Month = arabicMonths[monthStart.Month - 1],
                    Count = count
                });
            }

            // ── آخر الأنشطة ───────────────────────────────────
            var recentActivities = new List<ActivityVM>();
            
            // آخر 3 متطوعين
            var lastVolunteers = await _db.Volunteers
                .OrderByDescending(v => v.CreatedAt)
                .Take(3)
                .ToListAsync();
            
            foreach (var v in lastVolunteers)
            {
                recentActivities.Add(new ActivityVM
                {
                    Description = $"تسجيل جديد: {v.FullName}",
                    TimeAgo = GetTimeAgo(v.CreatedAt),
                    Type = "volunteer"
                });
            }
            
            // آخر 2 طلبات
            var lastRequests = await _db.PatientRequests
                .OrderByDescending(r => r.CreatedAt)
                .Take(2)
                .ToListAsync();
            
            foreach (var r in lastRequests)
            {
                recentActivities.Add(new ActivityVM
                {
                    Description = $"طلب جديد لمريض: {r.PatientName}",
                    TimeAgo = GetTimeAgo(r.CreatedAt),
                    Type = "request"
                });
            }
            
            // رتّب الأنشطة حسب الأحدث
            recentActivities = recentActivities
                .OrderByDescending(a => a.TimeAgo)
                .Take(5)
                .ToList();

            // ── تجميع كل شيء ─────────────────────────────────
            return new DashboardVM
            {
                TotalVolunteers = totalVolunteers,
                ActiveVolunteers = activeVolunteers,
                PendingRequests = pendingRequests,
                PatientsServedThisMonth = patientsThisMonth,
                NewVolunteersThisMonth = newThisMonth,
                NewVolunteersLastMonth = newLastMonth,
                VolunteersByStatus = byStatus
                    .ToDictionary(x => x.Status, x => x.Count),
                VolunteersByMunicipality = byMunicipality
                    .ToDictionary(x => x.Municipality, x => x.Count),
                MonthlyRegistrations = monthlyStats,
                RecentActivities = recentActivities
            };
        }

        // دالة مساعدة - تحول التاريخ لـ "منذ X"
        private string GetTimeAgo(DateTime date)
        {
            var diff = DateTime.Now - date;
            if (diff.TotalMinutes < 60)
                return $"منذ {(int)diff.TotalMinutes} دقيقة";
            if (diff.TotalHours < 24)
                return $"منذ {(int)diff.TotalHours} ساعة";
            if (diff.TotalDays < 7)
                return $"منذ {(int)diff.TotalDays} أيام";
            return date.ToString("dd/MM/yyyy");
        }
    }
}