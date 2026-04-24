using AmisduMalade.Data;
using AmisduMalade.Models;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class CenterService : ICenterService
    {
        private readonly AppDbContext _db;

        public CenterService(AppDbContext db)
        {
            _db = db;
        }

        // جيب كل المراكز مع عدد المتطوعين
        public async Task<List<Center>> GetAllAsync()
        {
            return await _db.Centers
                .Where(c => c.IsActive)
                .OrderBy(c => c.Municipality)
                .ToListAsync();
        }

        public async Task<Center?> GetByIdAsync(int id)
        {
            return await _db.Centers.FindAsync(id);
        }

        // إحصائيات مركز محدد
        public async Task<object> GetStatsAsync(int id)
        {
            var center = await _db.Centers.FindAsync(id);
            if (center == null)
                throw new Exception("المركز غير موجود");

            // المتطوعون في هذه البلدية
            var volunteers = await _db.Volunteers
                .Where(v => v.Municipality == center.Municipality)
                .ToListAsync();

            var activeCount = volunteers.Count(v => v.Status == "Active");
            var pendingCount = volunteers.Count(v => v.Status == "Pending");

            // المقابلات في هذا المركز
            var interviews = await _db.Interviews
                .Where(i => i.CenterLocation == center.Name)
                .CountAsync();

            return new
            {
                center = center.Name,
                municipality = center.Municipality,
                totalVolunteers = volunteers.Count,
                activeVolunteers = activeCount,
                pendingVolunteers = pendingCount,
                totalInterviews = interviews
            };
        }
    }
}