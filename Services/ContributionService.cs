using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class ContributionService : IContributionService
    {
        private readonly AppDbContext _db;
        public ContributionService(AppDbContext db) { _db = db; }

        public async Task<ContributionResponseVM> CreateAsync(CreateContributionVM vm)
        {
            var entity = new Contribution
            {
                ContributorName = vm.ContributorName,
                Phone           = vm.Phone,
                Type            = vm.Type,
                Amount          = vm.Amount,
                Description     = vm.Description,
                Message         = vm.Message,
                Status          = "Pending"
            };
            _db.Contributions.Add(entity);
            await _db.SaveChangesAsync();
            return ToVM(entity);
        }

        public async Task<List<ContributionResponseVM>> GetAllAsync() =>
            await _db.Contributions
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new ContributionResponseVM
                {
                    Id              = c.Id,
                    ContributorName = c.ContributorName,
                    Phone           = c.Phone,
                    Type            = c.Type,
                    Amount          = c.Amount.HasValue ? c.Amount.Value.ToString("F2") : null,
                    Description     = c.Description,
                    Message         = c.Message,
                    Status          = c.Status,
                    CreatedAt       = c.CreatedAt
                })
                .ToListAsync();

        public async Task<ContributionResponseVM?> GetByIdAsync(Guid id)
        {
            var c = await _db.Contributions.FindAsync(id);
            return c == null ? null : ToVM(c);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, string status)
        {
            var c = await _db.Contributions.FindAsync(id);
            if (c == null) return false;
            c.Status = status;
            if (status == "Confirmed") c.ConfirmedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        private static ContributionResponseVM ToVM(Contribution c) => new()
        {
            Id              = c.Id,
            ContributorName = c.ContributorName,
            Phone           = c.Phone,
            Type            = c.Type,
            Amount          = c.Amount.HasValue ? c.Amount.Value.ToString("F2") : null,
            Description     = c.Description,
            Message         = c.Message,
            Status          = c.Status,
            CreatedAt       = c.CreatedAt
        };
    }
}
