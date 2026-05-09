using Microsoft.Extensions.Logging;
using AmisDuMaladeApp.Services;
using AmisDuMaladeApp.ViewModels;
using AmisDuMaladeApp.Views;
using AmisDuMaladeApp.Constants;

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
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Singleton services
        builder.Services.AddSingleton<LocalizationService>();
        builder.Services.AddSingleton<AuthTokenService>();

        // HttpClient with base address
        builder.Services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri(AppConstants.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        builder.Services.AddSingleton<ApiService>();

        // ViewModels
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<VolunteerRegisterViewModel>();
        builder.Services.AddTransient<CareRequestViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();

        // Pages
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<VolunteerRegisterPage>();
        builder.Services.AddTransient<CareRequestPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
