using ReactiveUI;
using Volo.Abp.Collections;

namespace Byz.Abp.ReactiveUi;

internal class AbpReactiveViewLocatorSettings
{
    public TypeList<IViewFor> ViewForTypes { get; } = new();
}
