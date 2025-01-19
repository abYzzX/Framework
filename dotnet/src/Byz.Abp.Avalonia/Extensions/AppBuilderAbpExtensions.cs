using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Splat.Autofac;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Byz.Abp.Avalonia;

public static class AppBuilderAbpExtensions
{
    public static AppBuilder WithAbp<TStartupModule, TMainView>(this AppBuilder appBuilder,
        Action<AbpApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IAbpModule
        where TMainView : Control
    {
        appBuilder.AfterSetup(builder =>
        {
            var abpApplication = AbpApplicationFactory.Create<TStartupModule>(optionsAction);
            abpApplication.Services.AddSingleton<TMainView>();
            abpApplication.Initialize();
            global::Avalonia.Logging.Logger.Sink = abpApplication.ServiceProvider.GetRequiredService<AvaloniaLogSink>();
            RunAvaloniaApp<TMainView>(builder, abpApplication);
        });

        return appBuilder;
    }

    private static void RunAvaloniaApp<TMainView>(AppBuilder builder, IAbpApplicationWithInternalServiceProvider abpApplication)
        where TMainView : Control
    {
        if (builder.Instance is not { } instance)
        {
            throw new AbpInitializationException("No Avalonia application instance found");
        }

        if (instance.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && AbpTypeExtensions.IsAssignableTo<Window>(typeof(TMainView)))
        {
            desktop.Exit += (_, _) => abpApplication.Shutdown();
            desktop.MainWindow = abpApplication.ServiceProvider.GetRequiredService<TMainView>() as Window;
        }
        else if (instance.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = abpApplication.ServiceProvider.GetRequiredService<TMainView>() as Window;
            singleView.MainView!.Unloaded += (_, _) => abpApplication.Shutdown();
        }
        else
        {
            throw new AbpInitializationException(
                "ABP for Avalonia only works with IClassicDesktopStyleApplicationLifetime or ISingleViewApplicationLifetime");
        }
    }
}
