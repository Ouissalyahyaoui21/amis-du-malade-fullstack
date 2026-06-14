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

        public async Task<InterviewResponseVM> ScheduleAsync(ScheduleInterviewVM vm)
        {
            var interview = new VolunteerInterview
            {
                VolunteerId = vm.VolunteerId,
                ScheduledAt = vm.ScheduledAt,
                Location = vm.Location,
                Status = "Scheduled"
            };

            _db.VolunteerInterviews.Add(interview);

            // Update volunteer status to Interview
            var volunteer = await _db.Volunteers.FindAsync(vm.VolunteerId);
            if (volunteer != null)
                volunteer.Status = "Interview";

            await _db.SaveChangesAsync();

            await _db.Entry(interview).Reference(i => i.Volunteer).LoadAsync();
            return MapToVM(interview);
        }

        public async Task<List<InterviewResponseVM>> GetAllAsync()
        {
            var interviews = await _db.VolunteerInterviews
                .Include(i => i.Volunteer)
                .OrderByDescending(i => i.ScheduledAt)
                .ToListAsync();

            return interviews.Select(MapToVM).ToList();
        }

        public async Task<InterviewResponseVM?> GetByIdAsync(Guid id)
        {
            var interview = await _db.VolunteerInterviews
                .Include(i => i.Volunteer)
                .FirstOrDefaultAsync(i => i.Id == id);

            return interview == null ? null : MapToVM(interview);
        }

        public async Task<bool> RecordResultAsync(Guid id, RecordInterviewResultVM vm)
        {
            var interview = await _db.VolunteerInterviews
                .Include(i => i.Volunteer)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (interview == null) return false;

            interview.Status = "Completed";
            interview.Result = vm.Result;
            interview.Score = vm.Score;
            interview.Notes = vm.Notes;

            // Update volunteer status based on result
            if (interview.Volunteer != null)
            {
                interview.Volunteer.Status = vm.Result == "Accepted" ? "Approved" : "Rejected";
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAsync(Guid id)
        {
            var interview = await _db.VolunteerInterviews.FindAsync(id);
            if (interview == null) return false;

            interview.Status = "Cancelled";
            await _db.SaveChangesAsync();
            return true;
        }

        private static InterviewResponseVM MapToVM(VolunteerInterview i) => new()
        {
            Id = i.Id,
            VolunteerId = i.VolunteerId,
            VolunteerName = i.Volunteer?.FullName ?? "",
            VolunteerPhone = i.Volunteer?.Phone ?? "",
            ScheduledAt = i.ScheduledAt,
            Location = i.Location,
            Status = i.Status,
            Score = i.Score,
            Result = i.Result,
            Notes = i.Notes,
            CreatedAt = i.CreatedAt
        };
    }
}
