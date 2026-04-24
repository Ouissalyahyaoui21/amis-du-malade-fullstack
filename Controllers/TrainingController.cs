using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _service;

        public TrainingController(ITrainingService service)
        {
            _service = service;
        }

        // POST: api/training
        // إنشاء دورة جديدة
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrainingVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(new {
                message = "تم إنشاء الدورة التدريبية بنجاح",
                id = result.Id,
                title = result.Title
            });
        }

        // GET: api/training
        // جيب كل الدورات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var trainings = await _service.GetAllAsync();
            return Ok(trainings);
        }

        // GET: api/training/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var training = await _service.GetByIdAsync(id);
            if (training == null)
                return NotFound(new { message = "الدورة غير موجودة" });
            return Ok(training);
        }

        // POST: api/training/5/enroll
        // تسجيل متطوع في الدورة
        [HttpPost("{id}/enroll")]
        public async Task<IActionResult> Enroll(
            int id, [FromBody] EnrollVolunteerVM vm)
        {
            try
            {
                await _service.EnrollAsync(id, vm);
                return Ok(new { message = "تم تسجيل المتطوع في الدورة بنجاح" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/training/5/enrollment/3
        // تحديث نتيجة متطوع في الدورة
        [HttpPut("{id}/enrollment/{volunteerId}")]
        public async Task<IActionResult> UpdateEnrollment(
            int id, int volunteerId, [FromBody] UpdateEnrollmentVM vm)
        {
            var result = await _service
                .UpdateEnrollmentAsync(id, volunteerId, vm);
            if (!result)
                return NotFound(new { message = "التسجيل غير موجود" });

            var msg = vm.Status == "Completed"
                ? "أتم المتطوع الدورة بنجاح ✅"
                : "لم يجتز المتطوع الدورة ❌";
            return Ok(new { message = msg });
        }

        // PUT: api/training/5/complete
        // إنهاء الدورة
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _service.CompleteTrainingAsync(id);
            if (!result)
                return NotFound(new { message = "الدورة غير موجودة" });
            return Ok(new { message = "تم إنهاء الدورة التدريبية" });
        }
    }
}