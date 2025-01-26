using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using MudBlazor;
using Volo.Abp.DependencyInjection;

namespace Abp.MudBlazorUi.Services;

public class MudBlazorUiIconResolver : IMudBlazorUiIconResolver, ITransientDependency
{
    public required IOptions<AbpMudBlazorUiOptions> Options { private get; init; }
    
    public string this[string? iconName]
    {
        get => GetIcon(iconName);
    }

    private string GetIcon(string? iconName)
    {
        if (string.IsNullOrWhiteSpace(iconName))
        {
            return string.Empty;
        }

        if (iconName.Contains("<path", StringComparison.InvariantCultureIgnoreCase))
        {
            return iconName;
        }

        if (Options.Value.IconOverwrite.Overwrites.TryGetValue(iconName, out var overwriteIcon))
        {
            return GetIcon(overwriteIcon);
        }
        
        iconName = iconName.Replace(" ", string.Empty).Replace("-", string.Empty).Replace("_", string.Empty);

        if (typeof(Icons.Material.Filled).GetFields(
                BindingFlags.Default | BindingFlags.Public | BindingFlags.Static
            ).FirstOrDefault(x => x.Name.Equals(iconName, StringComparison.OrdinalIgnoreCase)) is { } field)
        {
            return field.GetValue(null)?.ToString() ?? Icons.Material.Filled.BrokenImage;
        }
        
        return Icons.Material.Filled.BrokenImage;
    }
}
