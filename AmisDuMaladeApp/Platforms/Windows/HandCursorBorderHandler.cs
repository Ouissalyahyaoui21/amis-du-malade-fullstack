using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AmisDuMaladeApp.Platforms.Windows;

public class HandCursorBorderHandler : BorderHandler
{
    protected override void ConnectHandler(ContentPanel platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView is Microsoft.Maui.Controls.Border border && border.GestureRecognizers.Count > 0)
            CursorHelper.ApplyHandCursor(platformView);
    }
}
