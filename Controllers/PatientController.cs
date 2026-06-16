using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(new { message = "تم إضافة المريض بنجاح", id = result.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            var result = list.Select(p => new
            {
                id           = p.Id,
                fullName     = p.FullName,
                phone        = p.Phone,
                age          = p.BirthDate.HasValue
                                   ? (int?)((DateTime.UtcNow - p.BirthDate.Value).TotalDays / 365)
                                   : null,
                municipality = p.Municipality,
                createdAt    = p.CreatedAt
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound(new { message = "المريض غير موجود" });

            var result = new
            {
                id            = p.Id,
                fullName      = p.FullName,
                phone         = p.Phone,
                age           = p.BirthDate.HasValue
                                    ? (int?)((DateTime.UtcNow - p.BirthDate.Value).TotalDays / 365)
                                    : null,
                gender        = p.Gender,
                address       = p.Address,
                municipality  = p.Municipality,
                residenceType = p.CurrentResidenceType,
                hospitalName  = p.CurrentHospital?.Name,
                mobilityStatus  = p.MobilityStatus,
                dependencyLevel = p.DependencyLevel,
                notes         = p.Notes,
                createdAt     = p.CreatedAt,
                contacts      = p.Contacts.Select(c => new
                {
                    fullName          = c.FullName,
                    phone             = c.Phone,
                    relationToPatient = c.RelationToPatient,
                    isPrimaryContact  = c.IsPrimaryContact
                }),
                conditions = p.MedicalConditions.Select(m => new
                {
                    name     = m.MedicalCondition.Name,
                    severity = m.Severity
                }),
                careRequests = p.CareRequests.Select(r => new
                {
                    id        = r.Id,
                    status    = r.Status,
                    createdAt = r.CreatedAt
                })
            };
            return Ok(result);
        }
    }
}