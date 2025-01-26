using Microsoft.AspNetCore.Builder;

namespace Abp.MudBlazorUi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMudBlazor(this IApplicationBuilder app)
    {
        app.UseStatusCodePagesWithRedirects("/404");
        app.UseConfiguredEndpoints(builder =>
        {
            builder.MapMudBlazor();
        });
        return app;
    }
}
