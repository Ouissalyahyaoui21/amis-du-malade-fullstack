using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class VisitService : IVisitService
    {
        private readonly AppDbContext _db;

        public VisitService(AppDbContext db) { _db = db; }

        // إنشاء جلسة زيارة جديدة
        public async Task<VisitSession> CreateSessionAsync(CreateVisitSessionVM vm)
        {
            // تحقق من التكليف
            var assignment = await _db.Assignments.FindAsync(vm.AssignmentId);
            if (assignment == null)
                throw new Exception("التكليف غير موجود");

            var session = new VisitSession
            {
                AssignmentId = vm.AssignmentId,
                SessionType = vm.SessionType,
                ScheduledStart = vm.ScheduledStart,
                ScheduledEnd = vm.ScheduledEnd,
                LocationNotes = vm.LocationNotes,
                Status = "Planned"
            };

            _db.VisitSessions.Add(session);
            await _db.SaveChangesAsync();
            return session;
        }

        // جيب كل جلسات تكليف محدد
        public async Task<List<VisitSession>> GetByAssignmentAsync(Guid assignmentId)
        {
            return await _db.VisitSessions
                .Include(s => s.Notes)
                .Include(s => s.Rating)
                .Where(s => s.AssignmentId == assignmentId)
                .OrderBy(s => s.ScheduledStart)
                .ToListAsync();
        }

        // جيب جلسة محددة
        public async Task<VisitSession?> GetSessionByIdAsync(Guid id)
        {
            return await _db.VisitSessions
                .Include(s => s.Notes)
                .Include(s => s.Rating)
                .Include(s => s.Assignment)
                    .ThenInclude(a => a.Volunteer)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // تحديث حالة الجلسة
        public async Task<bool> UpdateSessionAsync(Guid id, UpdateVisitSessionVM vm)
        {
            var session = await _db.VisitSessions.FindAsync(id);
            if (session == null) return false;

            session.Status = vm.Status;
            if (vm.ActualStart.HasValue)
                session.ActualStart = vm.ActualStart;
            if (vm.ActualEnd.HasValue)
                session.ActualEnd = vm.ActualEnd;
            if (vm.SessionSummary != null)
                session.SessionSummary = vm.SessionSummary;

            await _db.SaveChangesAsync();
            return true;
        }

        // إضافة ملاحظة للجلسة
        public async Task<VisitNote> AddNoteAsync(Guid sessionId, AddVisitNoteVM vm)
        {
            var session = await _db.VisitSessions.FindAsync(sessionId);
            if (session == null)
                throw new Exception("الجلسة غير موجودة");

            var note = new VisitNote
            {
                VisitSessionId = sessionId,
                VolunteerId = vm.VolunteerId,
                NoteType = vm.NoteType,
                Content = vm.Content
            };

            _db.VisitNotes.Add(note);
            await _db.SaveChangesAsync();
            return note;
        }

        // إضافة تقييم للجلسة
        public async Task<VisitRating> AddRatingAsync(Guid sessionId, AddVisitRatingVM vm)
        {
            var session = await _db.VisitSessions.FindAsync(sessionId);
            if (session == null)
                throw new Exception("الجلسة غير موجودة");

            // تحقق أنه ما في تقييم مسبق
            var exists = await _db.VisitRatings
                .AnyAsync(r => r.VisitSessionId == sessionId);
            if (exists)
                throw new Exception("تم تقييم هذه الجلسة مسبقاً");

            var rating = new VisitRating
            {
                VisitSessionId = sessionId,
                Score = vm.Score,
                Comment = vm.Comment
            };

            _db.VisitRatings.Add(rating);
            await _db.SaveChangesAsync();
            return rating;
        }
    }
}