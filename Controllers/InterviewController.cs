using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // كل العمليات محمية - فقط الإدارة
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _service;

        public InterviewController(IInterviewService service)
        {
            _service = service;
        }

        // POST: api/interview
        // تحديد موعد مقابلة لمتطوع
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInterviewVM vm)
        {
            try
            {
                var result = await _service.CreateAsync(vm);
                return Ok(new {
                    message = "تم تحديد موعد المقابلة بنجاح",
                    id = result.Id,
                    scheduledAt = result.ScheduledAt,
                    center = result.CenterLocation
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/interview
        // جيب كل المقابلات
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var interviews = await _service.GetAllAsync();
            return Ok(interviews);
        }

        // GET: api/interview/volunteer/5
        // جيب مقابلات متطوع محدد
        [HttpGet("volunteer/{volunteerId}")]
        public async Task<IActionResult> GetByVolunteer(int volunteerId)
        {
            var interviews = await _service.GetByVolunteerAsync(volunteerId);
            return Ok(interviews);
        }

        // PUT: api/interview/5/complete
        // تسجيل نتيجة المقابلة
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompleteInterviewVM vm)
        {
            var result = await _service.CompleteAsync(id, vm);
            if (!result)
                return NotFound(new { message = "المقابلة غير موجودة" });

            var statusMsg = vm.Result == "Accepted" ? "تم قبول المتطوع ✅" : "تم رفض المتطوع ❌";
            return Ok(new { message = statusMsg });
        }

        // PUT: api/interview/5/cancel
        // إلغاء مقابلة
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);
            if (!result)
                return NotFound(new { message = "المقابلة غير موجودة" });
            return Ok(new { message = "تم إلغاء المقابلة" });
        }
    }
}