using AmisduMalade.Data;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AmisduMalade.Services
{
    public class SyncService : ISyncService
    {
        private readonly AppDbContext _db;

        public SyncService(AppDbContext db) { _db = db; }

        // الموبايل يسحب البيانات الجديدة
        public async Task<object> PullAsync(SyncPullRequestVM vm)
        {
            object data = vm.EntityName switch
            {
                "volunteers" => await _db.Volunteers
                    .Include(v => v.Skills)
                    .Include(v => v.Availabilities)
                    .ToListAsync(),

                "patients" => await _db.Patients
                    .Include(p => p.Contacts)
                    .Include(p => p.MedicalConditions)
                    .ToListAsync(),

                "care_requests" => await _db.CareRequests
                    .Include(r => r.Patient)
                    .Include(r => r.RequiredSkills)
                    .ToListAsync(),

                "assignments" => await _db.Assignments
                    .Include(a => a.Volunteer)
                    .Include(a => a.CareRequest)
                    .ToListAsync(),

                "skills" => await _db.Skills.ToListAsync(),

                "medical_conditions" => await _db.MedicalConditions.ToListAsync(),

                "branches" => await _db.AssociationBranches.ToListAsync(),

                _ => new { error = "entity not supported" }
            };

            // سجل عملية المزامنة
            _db.SyncLogs.Add(new Models.SyncLog
            {
                EntityName = vm.EntityName,
                Direction = "Pull",
                RecordCount = data is System.Collections.IList list ? list.Count : 0,
                SyncedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return new
            {
                entityName = vm.EntityName,
                serverVersion = DateTime.UtcNow.Ticks,
                data
            };
        }

        // الموبايل يرسل عمليات معلقة
        public async Task<object> PushAsync(PendingOperationVM vm)
        {
            // سجل العملية
            _db.SyncLogs.Add(new Models.SyncLog
            {
                EntityName = vm.EntityName,
                Direction = "Push",
                OperationType = vm.OperationType,
                EntityId = vm.EntityId,
                SyncedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            return new
            {
                success = true,
                entityName = vm.EntityName,
                entityId = vm.EntityId,
                message = "تم استقبال العملية بنجاح"
            };
        }
    }
}