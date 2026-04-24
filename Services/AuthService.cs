using AmisduMalade.Data;
using AmisduMalade.Models;
using AmisduMalade.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmisduMalade.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // تسجيل الدخول - يرجع TOKEN أو null
        public async Task<string?> LoginAsync(LoginVM vm)
        {
            // 1. دور على المسؤول في DB
            var admin = await _db.Admins
                .FirstOrDefaultAsync(a => a.Email == vm.Email);

            if (admin == null) return null;

            // 2. تحقق من كلمة المرور المشفرة
            bool isValid = BCrypt.Net.BCrypt.Verify(vm.Password, admin.PasswordHash);
            if (!isValid) return null;

            // 3. أنشئ الـ TOKEN
            return GenerateToken(admin);
        }

        // إنشاء حساب إدارة جديد
        public async Task<bool> RegisterAdminAsync(RegisterAdminVM vm)
        {
            // تحقق إذا الـ email موجود مسبقاً
            var exists = await _db.Admins.AnyAsync(a => a.Email == vm.Email);
            if (exists) return false;

            var admin = new Admin
            {
                Email = vm.Email,
                Username = vm.Username,
                // شفّر كلمة المرور قبل الحفظ
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password)
            };

            _db.Admins.Add(admin);
            await _db.SaveChangesAsync();
            return true;
        }

        // دالة داخلية لإنشاء JWT Token
        private string GenerateToken(Admin admin)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            // المعلومات المحفوظة داخل الـ TOKEN
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim("AdminId", admin.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7), // صالح 7 أيام
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}