using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Byz.Abp.Avalonia;

[DependsOn(typeof(AbpAutofacModule))]
public class ByzAbpAvaloniaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}
