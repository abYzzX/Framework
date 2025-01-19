using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Byz.Abp.ReactiveUi;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ByzAbpReactiveUiModule)
)]
public class ByzAbpReactiveUiAvaloniaModule : AbpModule
{
    
}
