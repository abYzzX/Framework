using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Localization;
using YamlDotNet.Serialization;

namespace Byz.Abp.Localization;

public static class YamlLocalizationDictionaryBuilder
{
    public static ILocalizationDictionary? BuildFromFile(string filePath)
    {
        try
        {
            return BuildFromYamlString(File.ReadAllText(filePath));
        }
        catch (Exception ex)
        {
            throw new AbpException("Invalid localization file format: " + filePath, ex);
        }
    }

    public static ILocalizationDictionary? BuildFromYamlString(string content)
    {
        var deserializer = new Deserializer();
        var yamlRoot = deserializer.Deserialize(content);
        var dictionary = new Dictionary<string, string>();

        BuildFromYamlObject(dictionary, yamlRoot, "");
        var localizedDictionary = new Dictionary<string, LocalizedString>(
                dictionary.Where(x => !string.IsNullOrEmpty(x.Key))
                        .Select(
                                x => new KeyValuePair<string, LocalizedString>(
                                        x.Key,
                                        new LocalizedString(x.Key, x.Value.NormalizeLineEndings())
                                )
                        )
        );

        return new StaticLocalizationDictionary(dictionary["Culture"], localizedDictionary);
    }

    private static void BuildFromYamlObject(Dictionary<string, string> dictionary, object? yamlObj, string prefix)
    {
        if (yamlObj == null)
        {
            return;
        }

        if (yamlObj is Dictionary<object, object> yamlDictionary)
        {
            var valuePrefix = string.IsNullOrEmpty(prefix) ? "" : prefix + ".";
            foreach (var (key, value) in yamlDictionary)
            {
                if (value is string valueStr)
                {
                    dictionary[valuePrefix + key] = valueStr;
                    continue;
                }
                var prefixSeparator = string.IsNullOrEmpty(prefix) ? string.Empty : ":";
                BuildFromYamlObject(dictionary, value, prefix + prefixSeparator + key);
            }
        }
    }
}
