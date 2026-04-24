using AmisduMalade.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        // GET: api/report/monthly/2026/4
        [HttpGet("monthly/{year}/{month}")]
        public async Task<IActionResult> Monthly(int year, int month)
        {
            var report = await _service.GetMonthlyReportAsync(year, month);
            return Ok(report);
        }

        // GET: api/report/annual/2026
        [HttpGet("annual/{year}")]
        public async Task<IActionResult> Annual(int year)
        {
            var report = await _service.GetAnnualReportAsync(year);
            return Ok(report);
        }
    }
}