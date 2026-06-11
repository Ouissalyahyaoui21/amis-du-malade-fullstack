using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace AmisDuMaladeApp.Platforms.Windows;

/// <summary>
/// Overrides LabelHandler to show a hand cursor when the Label
/// has one or more GestureRecognizers (e.g., the admin-login link).
/// </summary>
public class HandCursorLabelHandler : LabelHandler
{
    protected override void ConnectHandler(TextBlock platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView?.GestureRecognizers.Count > 0)
            CursorHelper.ApplyHandCursor(platformView);
    }
}
