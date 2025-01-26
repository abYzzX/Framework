using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Abp.MudBlazorUi.Bundling;

public class BlazorGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.css");
        context.Files.AddIfNotContains("/_content/Byz.Abp.Blazor.Server.MudBlazorUi/css/abp-mudblazor.css");
        context.Files.AddIfNotContains("/_content/Byz.Abp.Blazor.Server.MudBlazorUi/css/roboto.css");
    }
}
