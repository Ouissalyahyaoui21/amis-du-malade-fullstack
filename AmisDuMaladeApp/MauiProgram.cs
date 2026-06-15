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

#if WINDOWS
        builder.ConfigureMauiHandlers(static h =>
        {
            h.AddHandler<Microsoft.Maui.Controls.Window, Platforms.Windows.TitleBarLtrWindowHandler>();
            h.AddHandler<Microsoft.Maui.Controls.Border, Platforms.Windows.HandCursorBorderHandler>();
            h.AddHandler<Microsoft.Maui.Controls.Label,  Platforms.Windows.HandCursorLabelHandler>();
            // Custom handler: fixes ComboBox FlowDirection/ContentPresenter in RTL pages
            h.AddHandler<Microsoft.Maui.Controls.Picker, Platforms.Windows.RtlPickerHandler>();
        });

        // ─── Entry/TextBox: keep typed text right-aligned in RTL pages ──────────────────
        // "HorizontalTextAlignment" fires unconditionally during handler setup — more reliable
        // than "FlowDirection" which MAUI may not invoke for Entry in .NET 10.
        // Running after MAUI's own mapper ensures we win even if MAUI computed Left.
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(
            "HorizontalTextAlignment",
            (handler, _) =>
            {
                if (handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox tb)
                    tb.TextAlignment = Microsoft.UI.Xaml.TextAlignment.Right;
            });
#endif

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
        builder.Services.AddSingleton<AppShell>();
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
