using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _service;
        public InterviewController(IInterviewService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] ScheduleInterviewVM vm)
        {
            var result = await _service.ScheduleAsync(vm);
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
            if (item == null) return NotFound(new { message = "المقابلة غير موجودة" });
            return Ok(item);
        }

        [HttpPut("{id}/result")]
        public async Task<IActionResult> RecordResult(Guid id, [FromBody] RecordInterviewResultVM vm)
        {
            var ok = await _service.RecordResultAsync(id, vm);
            if (!ok) return NotFound(new { message = "المقابلة غير موجودة" });
            return Ok(new { message = "تم تسجيل نتيجة المقابلة" });
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var ok = await _service.CancelAsync(id);
            if (!ok) return NotFound(new { message = "المقابلة غير موجودة" });
            return Ok(new { message = "تم إلغاء المقابلة" });
        }
    }
}
