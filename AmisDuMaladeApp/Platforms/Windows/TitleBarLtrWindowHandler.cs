using Microsoft.Maui.Handlers;

namespace AmisDuMaladeApp.Platforms.Windows;

/// <summary>
/// Keeps WinUI Window.FlowDirection = LTR at all times so that the system
/// caption buttons (minimize / maximize / close) stay on the top-right corner
/// even when the app content uses Arabic (RTL).
/// </summary>
public class TitleBarLtrWindowHandler : WindowHandler
{
    protected override void ConnectHandler(Microsoft.UI.Xaml.Window platformView)
    {
        base.ConnectHandler(platformView);
        platformView.FlowDirection = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
    }

    public override void UpdateValue(string property)
    {
        base.UpdateValue(property);
        if (PlatformView is { FlowDirection: not Microsoft.UI.Xaml.FlowDirection.LeftToRight })
            PlatformView.FlowDirection = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
    }
}
