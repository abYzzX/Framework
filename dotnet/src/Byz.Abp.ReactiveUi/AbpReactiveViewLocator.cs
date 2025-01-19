using System.Reactive.Disposables;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReactiveUI;
using Volo.Abp.DependencyInjection;

namespace Byz.Abp.ReactiveUi;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAbpViewLocator), typeof(IViewLocator))]
internal class AbpReactiveViewLocator : IAbpViewLocator, ISingletonDependency
{
    private const string DefaultContract = "AbpDefault";

    private sealed record ViewForRecord(Type ViewType, Type ModelType, string Contract);

    private readonly List<ViewForRecord> _viewForRecords = new();
    private readonly IViewFactory _viewFactory;
    private readonly AbpReactiveViewLocatorSettings _settings;
    private readonly ILogger<AbpReactiveViewLocator> _logger;

    public AbpReactiveViewLocator(
        IOptions<AbpReactiveViewLocatorSettings> options,
        IViewFactory viewFactory,
        ILogger<AbpReactiveViewLocator> logger)
    {
        _settings = options.Value;
        _viewFactory = viewFactory;
        _logger = logger;
        LoadViewTypes();
    }

    public IViewFor? ResolveView<TViewModel>(TViewModel? viewModel, string? contract = null)
    {
        contract ??= DefaultContract;
        var viewModelType = viewModel?.GetType() ?? typeof(TViewModel);
        _logger.LogDebug("Resolving view for model {ModelType} with contract '{Contract}'", viewModelType, contract);
        var viewType = _viewForRecords.FirstOrDefault(r => r.ModelType == viewModelType && r.Contract.Equals(contract, StringComparison.Ordinal))?.ViewType;

        if (viewType == null)
        {
            _logger.LogWarning("No view found for model {ModelType} with contract '{Contract}'", viewModelType, contract);
            return null;
        }
        
        return _viewFactory.CreateView(viewType);
    }

    public IViewFor<TViewModel>? ResolveView<TViewModel>(string? contract = null) where TViewModel : class
    {
        return ResolveView<TViewModel>(null, contract) as IViewFor<TViewModel>;
    }

    private void LoadViewTypes()
    {
        _logger.LogInformation("Initializing ReactiveViewLocator...");
        foreach (var viewForType in _settings.ViewForTypes)
        {
            var interfaceType = viewForType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType
                                     && i.GetGenericTypeDefinition() == typeof(IViewFor<>));
            
            if (interfaceType is null)
            {
                _logger.LogDebug("  - {Type} is not a IViewFor<> type", viewForType.Name);
                continue;
            }

            var modelType = interfaceType.GetGenericArguments()[0];
            var contract = viewForType.GetCustomAttribute<ViewContractAttribute>()?.Contract ?? DefaultContract;
            _viewForRecords.Add(new ViewForRecord(viewForType, modelType, contract));
            _logger.LogInformation("  - {Type} -> {ModelType} ('{Contract}')",
                viewForType.Name, modelType, contract);
        }
    }
}
