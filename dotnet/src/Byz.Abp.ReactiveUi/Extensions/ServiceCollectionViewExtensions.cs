using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Volo.Abp;

namespace Byz.Abp.ReactiveUi;

public static class ServiceCollectionViewExtensions
{
    public static IServiceCollection AddReactiveUiView<TView>(this IServiceCollection services)
        where TView : class, IViewFor
    {
        services.Configure<AbpReactiveViewLocatorSettings>(settings => settings.ViewForTypes.Add(typeof(TView)));
        return services;
    }

    public static IServiceCollection AddReactiveUiViews(this IServiceCollection services, params Type[] viewTypes)
    {
        services.Configure<AbpReactiveViewLocatorSettings>(settings =>
        {
            foreach (var viewType in viewTypes)
            {
                if (!IsViewFor(viewType))
                {
                    throw new AbpInitializationException($"{viewType} is not a reactive ui view");
                }

                settings.ViewForTypes.Add(viewType);
            }
        });

        return services;
    }

    public static IServiceCollection AddReactiveUiViews(this IServiceCollection services, params Assembly[] assemblies)
    {
        var viewTypes = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(IsViewFor);
        AddReactiveUiViews(services, viewTypes.ToArray());
        return services;
    }

    private static bool IsViewFor(Type type)
    {
        return type.IsAssignableTo(typeof(IViewFor))
               && !type.IsAbstract
               && !type.IsGenericType
               && !type.IsGenericTypeDefinition
               && !type.IsInterface;
    }
}
