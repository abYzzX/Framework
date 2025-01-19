using Volo.Abp;
using Volo.Abp.Localization;

namespace Byz.Abp.Localization;

public static class LocalizationResourceExtensions
{
    public static TLocalizationResource AddVirtualYaml<TLocalizationResource>(
            this TLocalizationResource localizationResource,
            string virtualPath)
            where TLocalizationResource : LocalizationResourceBase
    {
        Check.NotNull(localizationResource, nameof(localizationResource));
        Check.NotNull(virtualPath, nameof(virtualPath));

        localizationResource.Contributors.Add(
                new YamlLocalizationResourceContributorBase(
                        virtualPath.EnsureStartsWith('/')
                )
        );

        return localizationResource;
    }
}
