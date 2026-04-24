using AmisduMalade.Data;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }

        // تقرير شهري
        public async Task<object> GetMonthlyReportAsync(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            // إحصائيات المتطوعين
            var newVolunteers = await _db.Volunteers
                .CountAsync(v => v.CreatedAt >= start && v.CreatedAt < end);

            var acceptedVolunteers = await _db.Volunteers
                .CountAsync(v => v.Status == "Active"
                    && v.CreatedAt >= start && v.CreatedAt < end);

            // إحصائيات الطلبات
            var newRequests = await _db.PatientRequests
                .CountAsync(r => r.CreatedAt >= start && r.CreatedAt < end);

            var completedRequests = await _db.PatientRequests
                .CountAsync(r => r.Status == "Completed"
                    && r.CreatedAt >= start && r.CreatedAt < end);

            // إحصائيات المقابلات
            var interviews = await _db.Interviews
                .CountAsync(i => i.ScheduledAt >= start && i.ScheduledAt < end);

            // إحصائيات التدريبات
            var trainings = await _db.Trainings
                .CountAsync(t => t.StartDate >= start && t.StartDate < end);

            // توزيع المتطوعين حسب البلدية
            var byMunicipality = await _db.Volunteers
                .Where(v => v.CreatedAt >= start && v.CreatedAt < end)
                .GroupBy(v => v.Municipality)
                .Select(g => new { municipality = g.Key, count = g.Count() })
                .ToListAsync();

            return new
            {
                period = $"{month}/{year}",
                volunteers = new
                {
                    newRegistrations = newVolunteers,
                    accepted = acceptedVolunteers,
                    acceptanceRate = newVolunteers > 0
                        ? $"{(acceptedVolunteers * 100 / newVolunteers)}%"
                        : "0%"
                },
                requests = new
                {
                    total = newRequests,
                    completed = completedRequests,
                    completionRate = newRequests > 0
                        ? $"{(completedRequests * 100 / newRequests)}%"
                        : "0%"
                },
                interviews = interviews,
                trainings = trainings,
                volunteersByMunicipality = byMunicipality
            };
        }

        // تقرير سنوي
        public async Task<object> GetAnnualReportAsync(int year)
        {
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year + 1, 1, 1);

            var totalVolunteers = await _db.Volunteers
                .CountAsync(v => v.CreatedAt >= start && v.CreatedAt < end);

            var totalRequests = await _db.PatientRequests
                .CountAsync(r => r.CreatedAt >= start && r.CreatedAt < end);

            var totalTrainings = await _db.Trainings
                .CountAsync(t => t.StartDate >= start && t.StartDate < end);

            // إحصائيات شهرية للسنة
            var monthlyStats = new List<object>();
            string[] arabicMonths = {
                "يناير","فبراير","مارس","أبريل","مايو","يونيو",
                "يوليو","أغسطس","سبتمبر","أكتوبر","نوفمبر","ديسمبر"
            };

            for (int m = 1; m <= 12; m++)
            {
                var mStart = new DateTime(year, m, 1);
                var mEnd = mStart.AddMonths(1);

                var vCount = await _db.Volunteers
                    .CountAsync(v => v.CreatedAt >= mStart && v.CreatedAt < mEnd);
                var rCount = await _db.PatientRequests
                    .CountAsync(r => r.CreatedAt >= mStart && r.CreatedAt < mEnd);

                monthlyStats.Add(new
                {
                    month = arabicMonths[m - 1],
                    volunteers = vCount,
                    requests = rCount
                });
            }

            return new
            {
                year = year,
                summary = new
                {
                    totalVolunteers,
                    totalRequests,
                    totalTrainings
                },
                monthlyBreakdown = monthlyStats
            };
        }
    }
}