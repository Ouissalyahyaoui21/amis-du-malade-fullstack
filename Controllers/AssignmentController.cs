using AmisduMalade.Services;
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

        public AssignmentController(IAssignmentService service)
        {
            _service = service;
        }

        // GET: api/assignment/suggestions/5
        // اقتراح أفضل المتطوعين لطلب محدد
        [HttpGet("suggestions/{requestId}")]
        public async Task<IActionResult> GetSuggestions(int requestId)
        {
            try
            {
                var suggestions = await _service.GetSuggestionsAsync(requestId);
                return Ok(new
                {
                    requestId,
                    totalSuggestions = suggestions.Count,
                    suggestions
                });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/assignment
        // تكليف متطوع بطلب
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignVM vm)
        {
            try
            {
                var result = await _service.AssignAsync(
                    vm.RequestId, vm.VolunteerId, vm.IsAutomatic);
                return Ok(new
                {
                    message = "تم التكليف بنجاح",
                    assignmentId = result.Id,
                    matchScore = result.MatchScore
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/assignment
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await _service.GetAllAsync();
            return Ok(assignments);
        }

        // PUT: api/assignment/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (!result)
                return NotFound(new { message = "التكليف غير موجود" });
            return Ok(new { message = "تم تحديث حالة التكليف" });
        }
    }

    // ViewModel داخل نفس الملف
    public class AssignVM
    {
        public int RequestId { get; set; }
        public int VolunteerId { get; set; }
        public bool IsAutomatic { get; set; } = false;
    }
}