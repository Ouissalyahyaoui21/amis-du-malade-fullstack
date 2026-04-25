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

        public async Task<string?> LoginAsync(LoginVM vm)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == vm.Email);
            if (user == null) return null;

            bool valid = BCrypt.Net.BCrypt.Verify(vm.Password, user.PasswordHash);
            if (!valid) return null;

            return GenerateToken(user);
        }

        public async Task<bool> RegisterAsync(RegisterUserVM vm)
        {
            var exists = await _db.Users.AnyAsync(u => u.Email == vm.Email);
            if (exists) return false;

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                Role = vm.Role,
                BranchId = vm.BranchId
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}