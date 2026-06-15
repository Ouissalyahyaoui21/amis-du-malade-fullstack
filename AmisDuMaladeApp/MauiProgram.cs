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

        // ─── Picker/ComboBox: keep selected text visible and right-aligned in RTL pages ───
        // MAUI repeatedly pushes the page's RTL FlowDirection down to every child handler;
        // intercept it each time to restore LTR so the selected-item presenter stays visible.
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
                };
                // WinUI rebuilds the selected-item ContentPresenter on each selection, which can
                // reset alignment. Re-apply at Low priority so we run after the UI update finishes.
                cb.SelectionChanged += (s, e) =>
                {
                    if (s is not Microsoft.UI.Xaml.Controls.ComboBox combo) return;
                    combo.DispatcherQueue.TryEnqueue(
                        Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
                        () =>
                        {
                            combo.FlowDirection              = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                            combo.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                        });
                };
            });

        // ─── Picker: also re-apply fix at the MAUI SelectedIndex level ─────────────────
        // Covers cases where WinUI SelectionChanged fires before our deferred DispatcherQueue
        // item, or where MAUI updates SelectedIndex independently of the native event.
        Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(
            nameof(Microsoft.Maui.Controls.Picker.SelectedIndex),
            (handler, _) =>
            {
                if (handler.PlatformView is Microsoft.UI.Xaml.Controls.ComboBox cb)
                {
                    cb.FlowDirection              = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                    cb.HorizontalContentAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
                }
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
