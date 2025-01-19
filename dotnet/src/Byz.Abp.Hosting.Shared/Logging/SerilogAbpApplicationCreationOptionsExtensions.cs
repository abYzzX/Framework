using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Volo.Abp;
using ILogger = Serilog.ILogger;

namespace Byz.Abp.Hosting.Logging;

public static class SerilogAbpApplicationCreationOptionsExtensions
{
    public static AbpApplicationCreationOptions UseConfiguredSerilog(this AbpApplicationCreationOptions options,
        ILogger? logger = null, bool dispose = false)
    {
        logger ??= new LoggerConfiguration()
            .ReadFrom.Configuration(options.Services.GetConfiguration())
            .CreateLogger();

        options.Services.AddLogging(builder => builder.ClearProviders().AddSerilog(logger, dispose));
        return options;
    }
}
