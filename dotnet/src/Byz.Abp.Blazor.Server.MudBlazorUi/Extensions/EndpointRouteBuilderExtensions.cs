using Abp.MudBlazorUi.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Abp.MudBlazorUi.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RazorComponentsEndpointConventionBuilder MapMudBlazor(
        this IEndpointRouteBuilder builder
    )
    {
        return builder
            .MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddAdditionalAssemblies(
                [
                    .. builder
                        .ServiceProvider.GetRequiredService<IOptions<AbpMudBlazorUiOptions>>()
                        .Value.RouterAdditionalAssemblies,
                ]
            );
    }
}
