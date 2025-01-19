using Avalonia.Logging;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Byz.Abp.Avalonia;

[ExposeServices(typeof(AvaloniaLogSink))]
internal class AvaloniaLogSink : ILogSink, ISingletonDependency
{
    private readonly IDictionary<string, ILogger> _areaLoggers = new Dictionary<string, ILogger>();
    private readonly ILoggerFactory _loggerFactory;

    public AvaloniaLogSink(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public bool IsEnabled(LogEventLevel level, string area)
    {
        return GetLogger(area).IsEnabled(ToLogLevel(level));
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        GetLogger(area).Log(ToLogLevel(level), new EventId(0), messageTemplate);
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate,
        params object?[] propertyValues)
    {
        GetLogger(area).Log(ToLogLevel(level), new EventId(0), messageTemplate, propertyValues);
    }

    private ILogger GetLogger(string area)
    {
        return _areaLoggers.GetOrAdd(area, () => _loggerFactory.CreateLogger($"Avalonia.{area}"));
    }

    private static LogLevel ToLogLevel(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}
