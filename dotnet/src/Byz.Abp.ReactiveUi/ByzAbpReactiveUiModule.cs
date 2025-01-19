using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Byz.Abp.ReactiveUi;

[DependsOn(typeof(AbpAutofacModule))]
public class ByzAbpReactiveUiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // Overwrite ReactiveUi View Locator
        Locator.CurrentMutable.Register(() => context.ServiceProvider.GetRequiredService<IAbpViewLocator>(), typeof(IViewLocator));
    }
}
