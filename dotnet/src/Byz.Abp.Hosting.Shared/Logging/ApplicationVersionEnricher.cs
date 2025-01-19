using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Byz.Abp.Hosting.Logging;

public class ApplicationVersionEnricher : ILogEventEnricher
{
    public const string ApplicationVersionName = "ApplicationVersion";

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(GetVersionProperty(propertyFactory));
    }

    private LogEventProperty GetVersionProperty(ILogEventPropertyFactory propertyFactory)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        return propertyFactory.CreateProperty(ApplicationVersionName, version);
    }
}
