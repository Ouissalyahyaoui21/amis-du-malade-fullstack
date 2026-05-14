using Microsoft.Extensions.Logging;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.ViewModels;
using AmisDuMaladeApp.Views;

namespace AmisDuMaladeApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf",  "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ── Services (order matters: AuthToken first, ApiService depends on it) ──
        builder.Services.AddSingleton<LocalizationService>();
        builder.Services.AddSingleton<AuthTokenService>();
        builder.Services.AddSingleton<ApiService>();

        // ── ViewModels ─────────────────────────────────────────────────────────
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<VolunteerRegisterViewModel>();
        builder.Services.AddTransient<CareRequestViewModel>();
        builder.Services.AddTransient<ContributeViewModel>();
        builder.Services.AddTransient<AdminLoginViewModel>();
        builder.Services.AddTransient<AdminDashboardViewModel>();
        builder.Services.AddTransient<VolunteerDetailViewModel>();
        builder.Services.AddTransient<AboutViewModel>();

        // ── Pages ──────────────────────────────────────────────────────────────
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<VolunteerRegisterPage>();
        builder.Services.AddTransient<CareRequestPage>();
        builder.Services.AddTransient<ContributePage>();
        builder.Services.AddTransient<AdminLoginPage>();
        builder.Services.AddTransient<AdminDashboardPage>();
        builder.Services.AddTransient<VolunteerDetailPage>();
        builder.Services.AddTransient<AboutPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
