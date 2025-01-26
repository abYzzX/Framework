using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Abp.MudBlazorUi.Bundling;

public class BlazorGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        // var options = context.ServiceProvider.GetRequiredService<IOptions<AbpAspNetCoreComponentsWebOptions>>().Value;
        // if (!options.IsBlazorWebApp)
        // {
        //     context.Files.AddIfNotContains("/_framework/blazor.server.js");
        // }
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.js");
    }
}
