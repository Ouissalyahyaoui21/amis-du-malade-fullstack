using Microsoft.AspNetCore.Authorization;
using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _service;

        public RequestController(IRequestService service)
        {
            _service = service;
        }

        // POST: api/request
        // مفتوح ← العائلة ترسل طلب بدون login
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestCreateVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(vm);
            return Ok(new {
                message = "تم إرسال طلبك بنجاح، سنتواصل معك قريباً",
                id = result.Id
            });
        }

        // GET: api/request
        // محمي ← فقط الإدارة
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _service.GetAllAsync();
            return Ok(requests);
        }

        // GET: api/request/5
        // محمي ← فقط الإدارة
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _service.GetByIdAsync(id);
            if (request == null)
                return NotFound(new { message = "الطلب غير موجود" });
            return Ok(request);
        }

        // PUT: api/request/5/status
        // محمي ← فقط الإدارة
        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (!result)
                return NotFound(new { message = "الطلب غير موجود" });
            return Ok(new { message = "تم تحديث حالة الطلب" });
        }
    }
}