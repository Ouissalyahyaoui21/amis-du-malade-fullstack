using Microsoft.Maui.Handlers;
using Microsoft.Maui;

namespace AmisDuMaladeApp.Platforms.Windows
{
    /// <summary>
    /// Keeps WinUI caption buttons (minimize / maximize / close) on the top-right corner
    /// even when the app content uses Arabic (RTL).
    /// </summary>
    public class TitleBarLtrWindowHandler : WindowHandler
    {
        protected override void ConnectHandler(Microsoft.UI.Xaml.Window platformView)
        {
            base.ConnectHandler(platformView);

            if (platformView.Content is Microsoft.UI.Xaml.FrameworkElement frameworkElement)
            {
                frameworkElement.FlowDirection = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
            }
        }

        public override void UpdateValue(string property)
        {
            base.UpdateValue(property);

            if (property == nameof(IWindow.FlowDirection))
            {
                if (PlatformView.Content is Microsoft.UI.Xaml.FrameworkElement frameworkElement)
                {
                    frameworkElement.FlowDirection = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
                }
            }
        }
    }
}
