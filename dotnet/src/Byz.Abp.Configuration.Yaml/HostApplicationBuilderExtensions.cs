using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Byz.Abp.Configuration.Yaml;

public static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder AddYamlConfiguration(this HostApplicationBuilder builder, bool optional = false, bool reloadOnChange = true)
    {
        builder.Configuration.AddYamlFile("appsettings.yml", optional, reloadOnChange);
        if (!string.IsNullOrWhiteSpace(builder.Environment.EnvironmentName))
        {
            builder.Configuration.AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yml", true, reloadOnChange);
        }

        return builder;
    }
}
