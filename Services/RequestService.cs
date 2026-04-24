using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AmisduMalade.Services
{
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _db;

        public RequestService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PatientRequest> CreateAsync(RequestCreateVM vm)
        {
            var request = new PatientRequest
            {
                RequesterName = vm.RequesterName,
                RequesterPhone = vm.RequesterPhone,
                RequesterCity = vm.RequesterCity,
                RelationToPatient = vm.RelationToPatient,
                PatientName = vm.PatientName,
                PatientAge = vm.PatientAge,
                PatientGender = vm.PatientGender,
                PatientAddress = vm.PatientAddress,
                HealthConditions = JsonSerializer.Serialize(vm.HealthConditions),
                PatientDescription = vm.PatientDescription,
                PatientLocation = vm.PatientLocation,
                StartDate = vm.StartDate,
                Duration = vm.Duration,
                NeedsNightCare = vm.NeedsNightCare,
                RequiredSkills = JsonSerializer.Serialize(vm.RequiredSkills),
                Notes = vm.Notes,
                Status = "Pending"
            };

            _db.PatientRequests.Add(request);
            await _db.SaveChangesAsync();
            return request;
        }

        public async Task<List<PatientRequest>> GetAllAsync()
        {
            return await _db.PatientRequests.ToListAsync();
        }

        public async Task<PatientRequest?> GetByIdAsync(int id)
        {
            return await _db.PatientRequests.FindAsync(id);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var request = await _db.PatientRequests.FindAsync(id);
            if (request == null) return false;

            request.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}