using Microsoft.Maui.Handlers;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace AmisDuMaladeApp.Platforms.Windows;

// Custom handler that ensures a Picker (ComboBox) in an RTL page shows the
// selected item correctly. MAUI repeatedly pushes RTL FlowDirection onto the
// ComboBox which hides the ContentPresenter. We fix it at every stage:
// creation, connect, template load, and after each selection.
internal sealed class RtlPickerHandler : PickerHandler
{
    protected override ComboBox CreatePlatformView()
    {
        var cb = base.CreatePlatformView();
        Fix(cb);
        cb.SelectionChanged += OnSelectionChanged;
        cb.Loaded += (s, _) =>
        {
            if (s is ComboBox c) { Fix(c); FixTemplate(c); }
        };
        return cb;
    }

    protected override void ConnectHandler(ComboBox platformView)
    {
        base.ConnectHandler(platformView);
        Fix(platformView);
    }

    private static void OnSelectionChanged(object s, SelectionChangedEventArgs _)
    {
        if (s is not ComboBox cb) return;
        // Normal priority: run during the current UI update pass
        cb.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal,
            () => { Fix(cb); FixTemplate(cb); });
        // Low priority: run after MAUI's own mapper updates complete
        cb.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low,
            () => { Fix(cb); FixTemplate(cb); });
    }

    private static void Fix(ComboBox cb)
    {
        cb.FlowDirection              = FlowDirection.LeftToRight;
        cb.HorizontalContentAlignment = HorizontalAlignment.Stretch;
    }

    // Walk the WinUI visual tree and fix the ContentPresenter and any TextBlock
    // that the ComboBox template uses to render the selected item.
    private static void FixTemplate(DependencyObject node)
    {
        var count = VisualTreeHelper.GetChildrenCount(node);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(node, i);
            if (child is ContentPresenter cp)
            {
                cp.HorizontalAlignment        = HorizontalAlignment.Stretch;
                cp.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            if (child is TextBlock tb)
            {
                tb.TextAlignment = TextAlignment.Right;
            }
            FixTemplate(child);
        }
    }
}
