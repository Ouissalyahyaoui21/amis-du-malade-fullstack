using System.Runtime.InteropServices;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using WinRT.Interop;

namespace AmisDuMaladeApp.Platforms.Windows
{
    /// <summary>
    /// Keeps WinUI caption buttons (minimize / maximize / close) on the top-right corner
    /// even when the app content uses Arabic (RTL).
    /// Removes WS_EX_LAYOUTRTL from the Win32 window so the non-client area stays LTR.
    /// </summary>
    public class TitleBarLtrWindowHandler : WindowHandler
    {
        private const int  GWL_EXSTYLE      = -20;
        private const uint WS_EX_LAYOUTRTL  = 0x00400000;
        private const uint WS_EX_RTLREADING = 0x00002000;

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern nint GetWindowLongPtrW(nint hwnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern nint SetWindowLongPtrW(nint hwnd, int index, nint value);

        protected override void ConnectHandler(Microsoft.UI.Xaml.Window platformView)
        {
            base.ConnectHandler(platformView);
            FixTitleBarDirection(platformView);
            // Backstop: re-apply after the window is fully shown and MAUI content is loaded
            platformView.Activated += OnWindowActivated;
        }

        protected override void DisconnectHandler(Microsoft.UI.Xaml.Window platformView)
        {
            platformView.Activated -= OnWindowActivated;
            base.DisconnectHandler(platformView);
        }

        private void OnWindowActivated(object sender, Microsoft.UI.Xaml.WindowActivatedEventArgs _)
            => FixTitleBarDirection(PlatformView);

        public override void UpdateValue(string property)
        {
            base.UpdateValue(property);
            // "Content" fires when MAUI sets the page; "FlowDirection" fires on RTL changes
            if (property is "Content" or nameof(IWindow.FlowDirection))
                FixTitleBarDirection(PlatformView);
        }

        private static void FixTitleBarDirection(Microsoft.UI.Xaml.Window window)
        {
            try
            {
                var hwnd    = WindowNative.GetWindowHandle(window);
                var exStyle = (uint)GetWindowLongPtrW(hwnd, GWL_EXSTYLE);
                exStyle    &= ~(WS_EX_LAYOUTRTL | WS_EX_RTLREADING);
                SetWindowLongPtrW(hwnd, GWL_EXSTYLE, (nint)exStyle);

                if (window.Content is Microsoft.UI.Xaml.FrameworkElement fe)
                    fe.FlowDirection = Microsoft.UI.Xaml.FlowDirection.LeftToRight;
            }
            catch { /* ignored — non-critical platform call */ }
        }
    }
}
