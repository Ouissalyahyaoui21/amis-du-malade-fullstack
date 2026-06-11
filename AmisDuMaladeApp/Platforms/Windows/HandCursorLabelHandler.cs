using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace AmisDuMaladeApp.Platforms.Windows;

public class HandCursorLabelHandler : LabelHandler
{
    protected override void ConnectHandler(TextBlock platformView)
    {
        base.ConnectHandler(platformView);
        if (VirtualView is Microsoft.Maui.Controls.Label labelView && labelView.GestureRecognizers.Count > 0)
        {
            CursorHelper.ApplyHandCursor(platformView);
        }
    }
}
