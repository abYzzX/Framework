using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Byz.Abp.Localization;

[DependsOn(
    typeof(AbpLocalizationModule)
)]
public class ByzAbpLocalizationYamlModule : AbpModule
{
    
}
