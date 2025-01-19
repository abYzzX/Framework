using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Splat.Autofac;
using Volo.Abp;

namespace Byz.Abp.ReactiveUi;

public static class ServiceCollectionReactiveUiExtensions
{
    public static IServiceCollection UseReactiveUi(this IServiceCollection services)
    {
        // Allow Splat (used by ReactiveUi) to use autofac resolver
        var builder = services.GetContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);
        services.AddAutofacServiceProviderFactory(builder);
        return services;
    }
}
