using AmisduMalade.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CenterController : ControllerBase
    {
        private readonly ICenterService _service;

        public CenterController(ICenterService service)
        {
            _service = service;
        }

        // GET: api/center
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var centers = await _service.GetAllAsync();
            return Ok(centers);
        }

        // GET: api/center/1/stats
        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetStats(int id)
        {
            try
            {
                var stats = await _service.GetStatsAsync(id);
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}