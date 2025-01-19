using ReactiveUI;

namespace Byz.Abp.ReactiveUi;

public interface IAbpViewLocator : IViewLocator
{
    IViewFor<TViewModel>? ResolveView<TViewModel>(string? contract = null) where TViewModel : class;
}
