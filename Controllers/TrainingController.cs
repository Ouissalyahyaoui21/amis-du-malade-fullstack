using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _service;
        public TrainingController(ITrainingService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainingVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "الدورة التدريبية غير موجودة" });
            return Ok(item);
        }

        [HttpPost("{id}/enroll")]
        public async Task<IActionResult> Enroll(Guid id, [FromBody] EnrollVolunteerVM vm)
        {
            var result = await _service.EnrollVolunteerAsync(id, vm.VolunteerId);
            return Ok(result);
        }

        [HttpGet("{id}/enrollments")]
        public async Task<IActionResult> GetEnrollments(Guid id)
        {
            var list = await _service.GetEnrollmentsAsync(id);
            return Ok(list);
        }

        [HttpPut("enrollment/{enrollmentId}/complete")]
        public async Task<IActionResult> CompleteEnrollment(Guid enrollmentId, [FromQuery] string status = "Completed")
        {
            var ok = await _service.CompleteEnrollmentAsync(enrollmentId, status);
            if (!ok) return NotFound(new { message = "التسجيل غير موجود" });
            return Ok(new { message = "تم تحديث حالة التسجيل" });
        }
    }
}
