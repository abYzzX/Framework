using Avalonia;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Byz.Avalonia;
using Byz.Avalonia.Views;
using Microsoft.Extensions.Logging;

namespace AvaloniaApp;

public partial class MainWindow : ByzWindow
{
    public required ILogger<MainWindow> Logger { private get; init; }

    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        Logger.LogInformation("Loaded");
    }
}
