using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AmisDuMaladeApp.Platforms.Windows;

/// <summary>
/// Overrides BorderHandler to show a hand cursor when the Border
/// has one or more GestureRecognizers (i.e., it is tappable).
/// </summary>
public class HandCursorBorderHandler : BorderHandler
{
    protected override void ConnectHandler(ContentPanel platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView?.GestureRecognizers.Count > 0)
            CursorHelper.ApplyHandCursor(platformView);
    }
}
