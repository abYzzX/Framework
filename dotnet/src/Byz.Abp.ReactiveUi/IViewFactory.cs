using ReactiveUI;

namespace Byz.Abp.ReactiveUi;

public interface IViewFactory
{
    TView? CreateView<TView>() where TView : class, IViewFor;
    IViewFor? CreateView(Type viewType);
}
