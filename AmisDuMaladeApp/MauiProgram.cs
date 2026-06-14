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
        });

        // Intercept MAUI's RTL FlowDirection re-propagation to Picker — runs AFTER MAUI's mapper.
        // Without this, MAUI keeps pushing page-level RTL onto the ComboBox and hiding selected text.
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            "FlowDirection",
            (handler, _) =>
            {
                if (handler.PlatformView is Microsoft.UI.Xaml.Controls.ComboBox cb)
                {
                    cb.FlowDirection              = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                    cb.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                }
            });

        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            nameof(Microsoft.Maui.Controls.Picker.Title),
            (handler, _) =>
            {
                if (handler.PlatformView is not Microsoft.UI.Xaml.Controls.ComboBox cb) return;
                cb.FlowDirection              = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                cb.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                cb.Loaded += (s, e) =>
                {
                    if (s is not Microsoft.UI.Xaml.Controls.ComboBox combo) return;
                    combo.FlowDirection              = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                    combo.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                    FixPresenter(combo);

                    static void FixPresenter(Microsoft.UI.Xaml.DependencyObject node)
                    {
                        var n = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(node);
                        for (var i = 0; i < n; i++)
                        {
                            var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(node, i);
                            if (child is Microsoft.UI.Xaml.Controls.ContentPresenter
                                { Name: "ContentPresenter" } cp)
                            {
                                cp.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                                return;
                            }
                            FixPresenter(child);
                        }
                    }
                };
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
