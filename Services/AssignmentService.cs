using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _db;

        public AssignmentService(AppDbContext db) { _db = db; }

        public async Task<Assignment> CreateAsync(CreateAssignmentVM vm)
        {
            var assignment = new Assignment
            {
                CareRequestId = vm.CareRequestId,
                VolunteerId = vm.VolunteerId,
                AssignmentType = vm.AssignmentType,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                Notes = vm.Notes,
                Status = "Assigned"
            };

            var request = await _db.CareRequests.FindAsync(vm.CareRequestId);
            if (request != null) request.Status = "Assigned";

            _db.Assignments.Add(assignment);
            await _db.SaveChangesAsync();
            return assignment;
        }

        public async Task<List<Assignment>> GetAllAsync()
        {
            return await _db.Assignments
                .Include(a => a.Volunteer)
                .Include(a => a.CareRequest).ThenInclude(r => r.Patient)
                .OrderByDescending(a => a.AssignedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var a = await _db.Assignments.FindAsync(id);
            if (a == null) return false;
            a.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}