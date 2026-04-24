using Microsoft.AspNetCore.Authorization;
using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerService _service;

        public VolunteerController(IVolunteerService service)
        {
            _service = service;
        }

        // POST: api/volunteer/register
        // مفتوح ← الموبايل يسجل بدون login
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] VolunteerRegisterVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.RegisterAsync(vm);
            return Ok(new { 
                message = "تم تسجيل طلبك بنجاح", 
                id = result.Id 
            });
        }

        // GET: api/volunteer
        // محمي ← فقط الإدارة
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var volunteers = await _service.GetAllAsync();
            return Ok(volunteers);
        }

        // GET: api/volunteer/5
        // محمي ← فقط الإدارة
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var volunteer = await _service.GetByIdAsync(id);
            if (volunteer == null)
                return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(volunteer);
        }

        // PUT: api/volunteer/5/status
        // محمي ← فقط الإدارة تقبل أو ترفض
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (!result)
                return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(new { message = "تم تحديث الحالة بنجاح" });
        }
    }
}