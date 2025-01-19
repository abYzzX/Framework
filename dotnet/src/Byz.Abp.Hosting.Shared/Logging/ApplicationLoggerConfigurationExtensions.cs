using Serilog;
using Serilog.Configuration;
using Volo.Abp;

namespace Byz.Abp.Hosting.Logging;

public static class ApplicationLoggerConfigurationExtensions
{
    public static LoggerConfiguration WithApplicationVersion(this LoggerEnrichmentConfiguration configuration)
    {
        Check.NotNull(configuration, nameof(configuration));
        return configuration.With<ApplicationVersionEnricher>();
    }

    public static LoggerConfiguration WithApplicationType(this LoggerEnrichmentConfiguration configuration)
    {
        Check.NotNull(configuration, nameof(configuration));
        return configuration.WithProperty("ApplicationType", "dotnet");
    }
}
