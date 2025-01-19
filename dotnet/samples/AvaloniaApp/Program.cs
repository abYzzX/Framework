using Avalonia;
using System;
using Avalonia.ReactiveUI;
using Byz.Abp.Avalonia;
using Byz.Abp.Configuration.Yaml;
using Byz.Abp.Hosting.Logging;
using DynamicData.Kernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volo.Abp;

namespace AvaloniaApp;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddYamlFile(Path.Combine(AppContext.BaseDirectory, "appsettings.yml"))
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        BuildAvaloniaApp()
            .WithAbp<AvaloniaAppModule, MainWindow>(ConfigureAbp)
            .StartWithClassicDesktopLifetime(args);
    }

    private static void ConfigureAbp(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
        options.AddYamlConfiguration();
        options.UseConfiguredSerilog();
    }
    
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI();
}
