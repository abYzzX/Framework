using Byz.Abp.Avalonia;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AvaloniaApp;

[DependsOn(
    typeof(ByzAbpAvaloniaModule),
    typeof(AbpAutofacModule)
)]
public class AvaloniaAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Console.WriteLine("Configure AvaloniaAppModule");
    }
}
