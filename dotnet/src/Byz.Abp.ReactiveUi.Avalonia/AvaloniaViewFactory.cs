using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace Byz.Abp.ReactiveUi;

[ExposeServices(typeof(IViewFactory))]
public class AvaloniaViewFactory : IViewFactory, ITransientDependency
{
    public required IServiceScopeFactory ScopeFactory { private get; init; }
    
    public TView? CreateView<TView>() where TView : class, IViewFor
    {
        return CreateView(typeof(TView)) as TView;
    }

    public IViewFor? CreateView(Type viewType)
    {
        var scope = ScopeFactory.CreateScope();
        var view = scope.ServiceProvider.GetRequiredService(viewType);
        if (view is Window window)
        {
            window.Closed += (_, _) => scope.Dispose();
        }
        else if (view is Control control)
        {
            control.Unloaded += (_, _) => scope.Dispose();
        }
        
        return view as IViewFor;
    }
}
