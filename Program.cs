using AmisduMalade.Data;
using AmisduMalade.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── كل الـ Services ──────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ICareRequestService, CareRequestService>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IContributionService, ContributionService>();

// ── JWT ──────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ── تأكد من إنشاء جداول قاعدة البيانات تلقائياً + Seed بيانات الإدارة ─────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AmisduMalade.Data.AppDbContext>();
    db.Database.EnsureCreated();

    // Seed مستخدم الإدارة الافتراضي إذا لم يكن موجوداً
    if (!db.Users.Any(u => u.Email == "admin@amisdumalade.dz"))
    {
        db.Users.Add(new AmisduMalade.Models.User
        {
            Id           = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            FullName     = "مدير الجمعية",
            Email        = "admin@amisdumalade.dz",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin2024!"),
            Role         = "Admin",
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        });
        db.SaveChanges();
    }
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();