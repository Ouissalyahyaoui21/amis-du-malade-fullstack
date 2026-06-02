using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareRequestController : ControllerBase
    {
        private readonly ICareRequestService _service;
        public CareRequestController(ICareRequestService service) { _service = service; }

        // مفتوح — الموبايل يرسل بيانات المريض + الطلب مباشرة
        [HttpPost]
        public async Task<IActionResult> CreatePublic([FromBody] CreateCareRequestPublicVM vm)
        {
            if (string.IsNullOrWhiteSpace(vm.PatientName) || string.IsNullOrWhiteSpace(vm.RequesterPhone))
                return BadRequest(new { message = "اسم المريض ورقم الهاتف مطلوبان" });

            var result = await _service.CreatePublicAsync(vm);
            return Ok(new { message = "تم إرسال الطلب بنجاح", id = result.Id, referenceNumber = result.ReferenceNumber });
        }

        // مفتوح (Admin) — إنشاء طلب داخلي مع PatientId موجود
        [Authorize]
        [HttpPost("admin")]
        public async Task<IActionResult> CreateInternal([FromBody] CreateCareRequestVM vm)
        {
            var result = await _service.CreateAsync(vm);
            return Ok(new { message = "تم إنشاء الطلب", id = result.Id });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            // تحويل إلى نموذج مبسط للموبايل
            var result = list.Select(r => new
            {
                id           = r.Id,
                requesterName = r.Patient.Contacts.FirstOrDefault(c => c.IsPrimaryContact)?.FullName ?? r.Patient.FullName,
                patientName  = r.Patient.FullName,
                status       = r.Status,
                priority     = r.PriorityLevel,
                municipality = r.Municipality,
                createdAt    = r.CreatedAt
            });
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var r = await _service.GetByIdAsync(id);
            if (r == null) return NotFound(new { message = "الطلب غير موجود" });
            return Ok(r);
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (!result) return NotFound(new { message = "الطلب غير موجود" });
            return Ok(new { message = "تم تحديث الحالة" });
        }

        // اقتراحات ذكية
        [Authorize]
        [HttpGet("{id}/suggestions")]
        public async Task<IActionResult> GetSuggestions(Guid id)
        {
            try
            {
                var suggestions = await _service.GetSuggestionsAsync(id);
                return Ok(new { requestId = id, suggestions });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}