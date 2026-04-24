using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly AppDbContext _db;

        public InterviewService(AppDbContext db)
        {
            _db = db;
        }

        // تحديد موعد مقابلة لمتطوع
        public async Task<Interview> CreateAsync(CreateInterviewVM vm)
        {
            // تأكد أن المتطوع موجود
            var volunteer = await _db.Volunteers.FindAsync(vm.VolunteerId);
            if (volunteer == null)
                throw new Exception("المتطوع غير موجود");

            var interview = new Interview
            {
                VolunteerId = vm.VolunteerId,
                ScheduledAt = vm.ScheduledAt,
                CenterLocation = vm.CenterLocation,
                Status = "Scheduled"
            };

            // غيّر حالة المتطوع لـ "في انتظار المقابلة"
            volunteer.Status = "Interview";

            _db.Interviews.Add(interview);
            await _db.SaveChangesAsync();
            return interview;
        }

        // جيب كل المقابلات مع معلومات المتطوع
        public async Task<List<Interview>> GetAllAsync()
        {
            return await _db.Interviews
                .Include(i => i.Volunteer) // يجيب معلومات المتطوع معها
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();
        }

        // جيب مقابلات متطوع محدد
        public async Task<List<Interview>> GetByVolunteerAsync(int volunteerId)
        {
            return await _db.Interviews
                .Where(i => i.VolunteerId == volunteerId)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();
        }

        // تسجيل نتيجة المقابلة
        public async Task<bool> CompleteAsync(int id, CompleteInterviewVM vm)
        {
            var interview = await _db.Interviews
                .Include(i => i.Volunteer)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (interview == null) return false;

            // حدّث المقابلة
            interview.Status = "Completed";
            interview.Score = vm.Score;
            interview.Notes = vm.Notes;

            // حدّث حالة المتطوع تلقائياً حسب النتيجة
            if (vm.Result == "Accepted")
                interview.Volunteer.Status = "Active"; // ✅ مقبول ونشط
            else
                interview.Volunteer.Status = "Rejected"; // ❌ مرفوض

            await _db.SaveChangesAsync();
            return true;
        }

        // إلغاء مقابلة
        public async Task<bool> CancelAsync(int id)
        {
            var interview = await _db.Interviews.FindAsync(id);
            if (interview == null) return false;

            interview.Status = "Cancelled";

            // أرجع المتطوع لحالة Pending
            var volunteer = await _db.Volunteers.FindAsync(interview.VolunteerId);
            if (volunteer != null)
                volunteer.Status = "Pending";

            await _db.SaveChangesAsync();
            return true;
        }
    }
}