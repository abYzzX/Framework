using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace Byz.Abp.Configuration.Yaml;

public static class AbpApplicationCreationOptionsExtensions
{
    public static AbpApplicationCreationOptions AddYamlConfiguration(this AbpApplicationCreationOptions options, bool optional = false, bool reloadOnChange = true)
    {
        var builder = new ConfigurationBuilder();
        builder.AddYamlFile("appsettings.yml", optional, reloadOnChange);
        if (!string.IsNullOrWhiteSpace(options.Environment))
        {
            builder.AddYamlFile($"appsettings.{options.Environment}.yml", true, reloadOnChange);
        }
        options.Services.ReplaceConfiguration(builder.Build());

        return options;
    }
}
