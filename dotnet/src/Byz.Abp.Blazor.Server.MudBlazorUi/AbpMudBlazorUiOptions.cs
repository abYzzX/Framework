using System;
using System.Collections.Generic;
using System.Reflection;

namespace Abp.MudBlazorUi;

public class AbpMudBlazorUiOptions
{
    public List<Assembly> RouterAdditionalAssemblies { get; set; } = [];

    public LoginPageSettings LoginPage { get; set; } = new();

    public AppBarSettings AppBar { get; set; } = new();
    
    public IconOverwriteSettings IconOverwrite { get; set; } = new();
}

public class LoginPageSettings
{
    public string GradientColorStart { get; set; } = "#00709d";
    public string GradientColorEnd { get; set; } = "#6d017b";
}

public class AppBarSettings
{
    public bool ShowThemeSettings { get; set; } = true;
    public bool ShowDarkModeToggle { get; set; } = true;
    public bool ShowLanguageMenu { get; set; } = true;
}

public class IconOverwriteSettings
{
    public Dictionary<string, string> Overwrites { get; } = new(StringComparer.OrdinalIgnoreCase);
}
