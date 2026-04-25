using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _service;
        public VisitController(IVisitService service) { _service = service; }

        // POST: api/visit/session
        [HttpPost("session")]
        public async Task<IActionResult> CreateSession([FromBody] CreateVisitSessionVM vm)
        {
            try
            {
                var result = await _service.CreateSessionAsync(vm);
                return Ok(new { message = "تم إنشاء جلسة الزيارة", id = result.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/visit/assignment/5
        [HttpGet("assignment/{assignmentId}")]
        public async Task<IActionResult> GetByAssignment(Guid assignmentId)
        {
            var sessions = await _service.GetByAssignmentAsync(assignmentId);
            return Ok(sessions);
        }

        // GET: api/visit/session/5
        [HttpGet("session/{id}")]
        public async Task<IActionResult> GetSession(Guid id)
        {
            var session = await _service.GetSessionByIdAsync(id);
            if (session == null)
                return NotFound(new { message = "الجلسة غير موجودة" });
            return Ok(session);
        }

        // PUT: api/visit/session/5
        [HttpPut("session/{id}")]
        public async Task<IActionResult> UpdateSession(Guid id, [FromBody] UpdateVisitSessionVM vm)
        {
            var result = await _service.UpdateSessionAsync(id, vm);
            if (!result) return NotFound(new { message = "الجلسة غير موجودة" });
            return Ok(new { message = "تم تحديث الجلسة" });
        }

        // POST: api/visit/session/5/note
        [HttpPost("session/{id}/note")]
        public async Task<IActionResult> AddNote(Guid id, [FromBody] AddVisitNoteVM vm)
        {
            try
            {
                var note = await _service.AddNoteAsync(id, vm);
                return Ok(new { message = "تمت إضافة الملاحظة", id = note.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/visit/session/5/rating
        [HttpPost("session/{id}/rating")]
        public async Task<IActionResult> AddRating(Guid id, [FromBody] AddVisitRatingVM vm)
        {
            try
            {
                var rating = await _service.AddRatingAsync(id, vm);
                return Ok(new { message = "تم إضافة التقييم", id = rating.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}