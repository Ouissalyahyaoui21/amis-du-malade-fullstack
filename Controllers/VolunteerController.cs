using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _service;
        public VolunteerController(IVolunteerService service) { _service = service; }

        // مفتوح - الموبايل يسجل
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] VolunteerRegisterVM vm)
        {
            var result = await _service.RegisterAsync(vm);
            return Ok(new { message = "تم تسجيل طلبك بنجاح", id = result.Id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var v = await _service.GetByIdAsync(id);
            if (v == null) return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(v);
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateVolunteerStatusVM vm)
        {
            var result = await _service.UpdateStatusAsync(id, vm.Status);
            if (!result) return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(new { message = "تم تحديث الحالة" });
        }
    }
}