using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service) { _service = service; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM vm)
        {
            var (token, error) = await _service.LoginAsync(vm);
            if (token == null)
                return Unauthorized(new { message = error });
            return Ok(new { message = "تم تسجيل الدخول", token });
        }

        // محمي — فقط أدمن يقدر يصنع حساب جديد
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserVM vm)
        {
            var result = await _service.RegisterAsync(vm);
            if (!result)
                return BadRequest(new { message = "البريد مستخدم مسبقاً" });
            return Ok(new { message = "تم إنشاء الحساب بنجاح" });
        }
    }
}
