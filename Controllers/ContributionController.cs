using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _service;
        public ContributionController(IContributionService service) { _service = service; }

        // POST: api/contribution  — public (anyone can submit a contribution)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateContributionVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.ContributorName) || string.IsNullOrWhiteSpace(vm.Phone))
                return BadRequest(new { message = "الاسم ورقم الهاتف مطلوبان" });

            var result = await _service.CreateAsync(vm);
            return Ok(new { message = "شكراً على مساهمتك", id = result.Id });
        }

        // GET: api/contribution  — admin only
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        // GET: api/contribution/{id}  — admin only
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "المساهمة غير موجودة" });
            return Ok(item);
        }

        // PUT: api/contribution/{id}/status  — admin only
        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateContributionStatusVM vm)
        {
            var allowed = new[] { "Pending", "Confirmed", "Distributed" };
            if (!allowed.Contains(vm.Status))
                return BadRequest(new { message = "الحالة غير صالحة" });

            var result = await _service.UpdateStatusAsync(id, vm.Status);
            if (!result) return NotFound(new { message = "المساهمة غير موجودة" });
            return Ok(new { message = "تم تحديث الحالة" });
        }
    }
}
