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

        // مفتوح — الموبايل يسجل مباشرة
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
            // تحويل إلى DTO مبسط متوافق مع الموبايل
            var result = list.Select(v => new
            {
                id                = v.Id,
                fullName          = v.FullName,
                phone             = v.Phone,
                email             = v.Email,
                municipality      = v.Municipality,
                volunteerCategory = v.VolunteerCategory,
                status            = v.Status,
                canHomeVisit      = v.CanHomeVisit,
                canHospitalVisit  = v.CanHospitalVisit,
                canNightPresence  = v.CanNightPresence,
                hasTransportation = v.HasTransportation,
                createdAt         = v.CreatedAt
            });
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var v = await _service.GetByIdAsync(id);
            if (v == null) return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(new
            {
                id                = v.Id,
                fullName          = v.FullName,
                phone             = v.Phone,
                email             = v.Email,
                municipality      = v.Municipality,
                volunteerCategory = v.VolunteerCategory,
                status            = v.Status,
                canHomeVisit      = v.CanHomeVisit,
                canHospitalVisit  = v.CanHospitalVisit,
                canNightPresence  = v.CanNightPresence,
                hasTransportation = v.HasTransportation,
                createdAt         = v.CreatedAt
            });
        }

        [Authorize]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateVolunteerStatusVM vm)
        {
            // التحقق من القيمة قبل الحفظ
            var validStatuses = new[] { "Pending", "Interview", "Approved", "Rejected", "Suspended" };
            if (!validStatuses.Contains(vm.Status))
                return BadRequest(new { message = "قيمة الحالة غير صالحة" });

            var result = await _service.UpdateStatusAsync(id, vm.Status);
            if (!result) return NotFound(new { message = "المتطوع غير موجود" });
            return Ok(new { message = "تم تحديث الحالة" });
        }
    }
}
