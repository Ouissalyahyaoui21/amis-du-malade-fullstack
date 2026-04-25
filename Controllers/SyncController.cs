using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _service;
        public SyncController(ISyncService service) { _service = service; }

        // POST: api/sync/pull
        // الموبايل يسحب البيانات الجديدة
        [HttpPost("pull")]
        public async Task<IActionResult> Pull([FromBody] SyncPullRequestVM vm)
        {
            var result = await _service.PullAsync(vm);
            return Ok(result);
        }

        // POST: api/sync/push
        // الموبايل يرسل عمليات معلقة
        [HttpPost("push")]
        public async Task<IActionResult> Push([FromBody] PendingOperationVM vm)
        {
            var result = await _service.PushAsync(vm);
            return Ok(result);
        }
    }
}