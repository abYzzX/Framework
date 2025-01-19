using Microsoft.Extensions.FileProviders;
using Volo.Abp.Localization;
using Volo.Abp.Localization.VirtualFiles;

namespace Byz.Abp.Localization;

public class YamlLocalizationResourceContributorBase : VirtualFileLocalizationResourceContributorBase
{
    public YamlLocalizationResourceContributorBase() : base(string.Empty) { }
    public YamlLocalizationResourceContributorBase(string virtualPath) : base(virtualPath) { }

    protected override bool CanParseFile(IFileInfo file)
    {
        return file.Name.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase)
               || file.Name.EndsWith(".yml", StringComparison.OrdinalIgnoreCase);
    }

    protected override ILocalizationDictionary? CreateDictionaryFromFileContent(string fileContent)
    {
        return YamlLocalizationDictionaryBuilder.BuildFromYamlString(fileContent);
    }
}
