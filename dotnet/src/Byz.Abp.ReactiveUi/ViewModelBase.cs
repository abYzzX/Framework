using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using ReactiveUI;
using Volo.Abp;

namespace Byz.Abp.ReactiveUi;

public class ViewModelBase : ReactiveObject, IActivatableViewModel, IDisposable
{
    private readonly ConcurrentDictionary<string, object> _properties = new(StringComparer.OrdinalIgnoreCase);

    public ViewModelActivator Activator { get; }

    public ViewModelBase()
    {
        Activator = new();
        this.WhenActivated(disposables =>
        {
            OnActivatedAsync()
                .ToObservable()
                .Subscribe(_ => { })
                .DisposeWith(disposables);
        });
    }

    protected T GetValue<T>([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        return _properties.TryGetValue(propertyName, out var property)
            ? (T)property
            : default!;
    }

    protected bool SetValue<T>(T value, [CallerMemberName] string? propertyName = null)
    {
        _ = Check.NotNullOrWhiteSpace(propertyName, nameof(propertyName));

        if (_properties.ContainsKey(propertyName)
            && EqualityComparer<T>.Default.Equals(GetValue<T>(propertyName), value))
        {
            return false;
        }

        if (value is null)
        {
            _properties.Remove(propertyName, out _);
        }
        else
        {
            _properties[propertyName] = value;
        }

        this.RaisePropertyChanged(propertyName);
        return true;
    }

    protected virtual Task OnActivatedAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Activator.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
