using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class AlertService : IAlertService
    {
        private readonly AppDbContext _db;

        public AlertService(AppDbContext db) { _db = db; }

        // إنشاء تنبيه جديد
        public async Task<Alert> CreateAsync(CreateAlertVM vm)
        {
            var alert = new Alert
            {
                PatientId = vm.PatientId,
                VolunteerId = vm.VolunteerId,
                CareRequestId = vm.CareRequestId,
                VisitSessionId = vm.VisitSessionId,
                Severity = vm.Severity,
                Title = vm.Title,
                Description = vm.Description,
                Status = "Open"
            };

            _db.Alerts.Add(alert);
            await _db.SaveChangesAsync();
            return alert;
        }

        // جيب كل التنبيهات
        public async Task<List<Alert>> GetAllAsync()
        {
            return await _db.Alerts
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        // جيب التنبيهات المفتوحة فقط
        public async Task<List<Alert>> GetOpenAsync()
        {
            return await _db.Alerts
                .Where(a => a.Status == "Open" || a.Status == "InProgress")
                .OrderBy(a => a.Severity == "Critical" ? 0 :
                              a.Severity == "High" ? 1 :
                              a.Severity == "Medium" ? 2 : 3)
                .ToListAsync();
        }

        // حل التنبيه
        public async Task<bool> ResolveAsync(Guid id, ResolveAlertVM vm)
        {
            var alert = await _db.Alerts.FindAsync(id);
            if (alert == null) return false;

            alert.Status = "Resolved";
            alert.ResolvedByUserId = vm.ResolvedByUserId;
            alert.ResolvedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}