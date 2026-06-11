using Microsoft.Maui.Handlers;

namespace AmisDuMaladeApp.Platforms.Windows;

// Reserved for future title-bar direction fix.
// Microsoft.UI.Xaml.Window is NOT a FrameworkElement — FlowDirection
// must be addressed at the AppWindow.TitleBar level, not here.
public class TitleBarLtrWindowHandler : WindowHandler { }
