using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Byz.Abp.Configuration.Yaml;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddYamlConfiguration(this IHostBuilder hostBuilder, bool optional = true, bool reloadOnChange = true)
    {
        return hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddYamlFile("appsettings.yml", optional, reloadOnChange);
            if (!string.IsNullOrWhiteSpace(context.HostingEnvironment.EnvironmentName))
            {
                config.AddYamlFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.yml", true, reloadOnChange);
            }
            config.AddYamlFile("appsettings.secrets.yml", optional: true);
        });
    }
}
