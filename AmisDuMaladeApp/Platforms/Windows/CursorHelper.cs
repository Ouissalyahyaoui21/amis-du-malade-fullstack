using System.Reflection;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace AmisDuMaladeApp.Platforms.Windows;

internal static class CursorHelper
{
    private static readonly PropertyInfo? _protectedCursor =
        typeof(UIElement).GetProperty("ProtectedCursor",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    private static readonly InputSystemCursor _hand =
        InputSystemCursor.Create(InputSystemCursorShape.Hand);

    public static void ApplyHandCursor(UIElement element)
    {
        element.PointerEntered -= OnEnterHand;
        element.PointerExited  -= OnExitHand;
        element.PointerEntered += OnEnterHand;
        element.PointerExited  += OnExitHand;
    }

    private static void OnEnterHand(object sender, PointerRoutedEventArgs _)
    {
        if (sender is UIElement el)
            try { _protectedCursor?.SetValue(el, _hand); } catch { /* ignored */ }
    }

    private static void OnExitHand(object sender, PointerRoutedEventArgs _)
    {
        if (sender is UIElement el)
            try { _protectedCursor?.SetValue(el, null); } catch { /* ignored */ }
    }
}
