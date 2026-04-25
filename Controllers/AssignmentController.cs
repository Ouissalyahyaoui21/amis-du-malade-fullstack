using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _service;
        public AssignmentController(IAssignmentService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(new { message = "تم التكليف بنجاح", id = result.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (!result) return NotFound(new { message = "التكليف غير موجود" });
            return Ok(new { message = "تم التحديث" });
        }
    }
}