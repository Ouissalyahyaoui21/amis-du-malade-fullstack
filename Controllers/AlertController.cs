using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService _service;
        public AlertController(IAlertService service) { _service = service; }

        // POST: api/alert
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlertVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(new { message = "تم إنشاء التنبيه", id = result.Id });
        }

        // GET: api/alert
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alerts = await _service.GetAllAsync();
            return Ok(alerts);
        }

        // GET: api/alert/open
        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            var alerts = await _service.GetOpenAsync();
            return Ok(alerts);
        }

        // PUT: api/alert/5/resolve
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> Resolve(Guid id, [FromBody] ResolveAlertVM vm)
        {
            var result = await _service.ResolveAsync(id, vm);
            if (!result) return NotFound(new { message = "التنبيه غير موجود" });
            return Ok(new { message = "تم حل التنبيه ✅" });
        }
    }
}