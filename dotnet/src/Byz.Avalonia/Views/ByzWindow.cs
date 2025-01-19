using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Byz.Avalonia.Views;

public class ByzWindow : Window
{
    public static readonly StyledProperty<ObservableCollection<object>> HeaderItemsProperty =
        AvaloniaProperty.Register<ByzWindow, ObservableCollection<object>>(
            nameof(HeaderItems),
            defaultValue: new ObservableCollection<object>()
        );

    public static readonly StyledProperty<object?> StatusBarContentProperty =
        AvaloniaProperty.Register<ByzWindow, object?>(nameof(StatusBarContent), defaultValue: null);

    public static readonly StyledProperty<bool> IsWindowTitleVisibleProperty =
        AvaloniaProperty.Register<ByzWindow, bool>(nameof(IsWindowTitleVisible), defaultValue: true);

    public static readonly StyledProperty<bool> IsStatusBarVisibleProperty =
        AvaloniaProperty.Register<ByzWindow, bool>(nameof(IsStatusBarVisible), defaultValue: true);

    public static readonly StyledProperty<bool> IsWindowRestoreButtonVisibleProperty =
        AvaloniaProperty.Register<ByzWindow, bool>(nameof(IsWindowRestoreButtonVisible), defaultValue: true);

    public static readonly StyledProperty<bool> IsWindowMinimizeButtonVisibleProperty =
        AvaloniaProperty.Register<ByzWindow, bool>(nameof(IsWindowMinimizeButtonVisible), defaultValue: true);

    public static readonly StyledProperty<bool> IsWindowCloseButtonVisibleProperty =
        AvaloniaProperty.Register<ByzWindow, bool>(nameof(IsWindowCloseButtonVisible), defaultValue: true);

    public object? StatusBarContent
    {
        get => GetValue(StatusBarContentProperty);
        set => SetValue(StatusBarContentProperty, value);
    }

    public ObservableCollection<object> HeaderItems
    {
        get => GetValue(HeaderItemsProperty);
        set => SetValue(HeaderItemsProperty, value);
    }

    public bool IsWindowTitleVisible
    {
        get => GetValue(IsWindowTitleVisibleProperty);
        set => SetValue(IsWindowTitleVisibleProperty, value);
    }

    public bool IsStatusBarVisible
    {
        get => GetValue(IsStatusBarVisibleProperty);
        set => SetValue(IsStatusBarVisibleProperty, value);
    }

    public bool IsWindowRestoreButtonVisible
    {
        get => GetValue(IsWindowRestoreButtonVisibleProperty);
        set => SetValue(IsWindowRestoreButtonVisibleProperty, value);
    }

    public bool IsWindowMinimizeButtonVisible
    {
        get => GetValue(IsWindowMinimizeButtonVisibleProperty);
        set => SetValue(IsWindowMinimizeButtonVisibleProperty, value);
    }

    public bool IsWindowCloseButtonVisible
    {
        get => GetValue(IsWindowCloseButtonVisibleProperty);
        set => SetValue(IsWindowCloseButtonVisibleProperty, value);
    }

    protected override Type StyleKeyOverride { get => typeof(ByzWindow); }
}
