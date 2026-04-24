using AmisduMalade.Services;
using AmisduMalade.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AmisduMalade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        // POST: api/auth/login
        // الإدارة تدخل email + password وتاخذ TOKEN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM vm)
        {
            var token = await _service.LoginAsync(vm);

            if (token == null)
                return Unauthorized(new { 
                    message = "البريد الإلكتروني أو كلمة المرور غير صحيحة" 
                });

            return Ok(new {
                message = "تم تسجيل الدخول بنجاح",
                token = token
            });
        }

        // POST: api/auth/register
        // إنشاء حساب إدارة جديد (مرة واحدة فقط)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAdminVM vm)
        {
            var result = await _service.RegisterAdminAsync(vm);

            if (!result)
                return BadRequest(new { 
                    message = "البريد الإلكتروني مستخدم مسبقاً" 
                });

            return Ok(new { message = "تم إنشاء الحساب بنجاح" });
        }
    }
}