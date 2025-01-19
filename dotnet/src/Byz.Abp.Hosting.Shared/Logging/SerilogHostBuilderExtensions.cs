using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Byz.Abp.Hosting.Logging;

public static class SerilogHostBuilderExtensions
{
    /// <summary>
    ///     Sets Serilog as the logging provider by reading the application configuration.
    /// </summary>
    /// <param name="builder">
    ///     The host builder to configure.
    /// </param>
    /// <param name="configureLogger">
    ///     The delegate for configuring the <see cref="LoggerConfiguration"/> that will be used to construct a <see cref="Serilog.Core.Logger"/>.
    /// </param>
    /// <param name="preserveStaticLogger">
    ///     Indicates whether to preserve the value of <see cref="Log.Logger"/>.
    /// </param>
    /// <param name="writeToProviders">
    ///     By default, Serilog does not write events to <see cref="ILoggerProvider"/>
    ///     registered through the Microsoft.Extensions.Logging API. Normally, equivalent
    ///     Serilog sinks are used in place of providers. Specify true to write events to
    ///     all providers.
    /// </param>
    /// <remarks>
    ///     A <see cref="HostBuilderContext"/> is supplied so that configuration
    ///     and hosting information can be used. The logger will be shut down when application
    ///     services are disposed.
    /// </remarks>
    /// <returns>The host builder.</returns>
    public static IHostBuilder UseConfiguredSerilog(
        this IHostBuilder builder,
        Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger = null,
        bool preserveStaticLogger = false,
        bool writeToProviders = false
    )
    {
        builder.UseSerilog((context, services, configuration) =>
        {
            configuration = configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);

            configureLogger?.Invoke(context, services, configuration);
        }, preserveStaticLogger, writeToProviders);

        return builder;
    }
}
