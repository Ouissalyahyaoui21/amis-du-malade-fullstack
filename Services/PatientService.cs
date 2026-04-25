using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AmisduMalade.Services
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _db;

        public PatientService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Patient> CreateAsync(CreatePatientVM vm)
        {
            var patient = new Patient
            {
                FullName = vm.FullName,
                BirthDate = vm.BirthDate,
                Gender = vm.Gender,
                Phone = vm.Phone,
                Address = vm.Address,
                Municipality = vm.Municipality,
                CurrentResidenceType = vm.CurrentResidenceType,
                CurrentHospitalId = vm.CurrentHospitalId,
                MobilityStatus = vm.MobilityStatus,
                DependencyLevel = vm.DependencyLevel,
                Notes = vm.Notes
            };

            foreach (var c in vm.Contacts)
            {
                patient.Contacts.Add(new PatientContact
                {
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Email = c.Email,
                    RelationToPatient = c.RelationToPatient,
                    IsPrimaryContact = c.IsPrimaryContact
                });
            }

            foreach (var m in vm.MedicalConditions)
            {
                patient.MedicalConditions.Add(new PatientMedicalCondition
                {
                    MedicalConditionId = m.MedicalConditionId,
                    Severity = m.Severity,
                    Notes = m.Notes
                });
            }

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();
            return patient;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _db.Patients
                .Include(p => p.Contacts)
                .Include(p => p.MedicalConditions)
                    .ThenInclude(m => m.MedicalCondition)
                .Include(p => p.CurrentHospital)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await _db.Patients
                .Include(p => p.Contacts)
                .Include(p => p.MedicalConditions)
                    .ThenInclude(m => m.MedicalCondition)
                .Include(p => p.CareRequests)
                .Include(p => p.CurrentHospital)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}