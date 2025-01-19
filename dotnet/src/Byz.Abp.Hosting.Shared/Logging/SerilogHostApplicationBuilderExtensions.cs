using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Byz.Abp.Hosting.Logging;

public static class SerilogHostApplicationBuilderExtensions
{
    public static HostApplicationBuilder UseConfiguredSerilog(this HostApplicationBuilder builder, ILogger? logger = null, bool dispose = false)
    {
        logger ??= new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        builder.Logging
            .ClearProviders()
            .AddSerilog(logger, dispose);

        return builder;
    }
}
