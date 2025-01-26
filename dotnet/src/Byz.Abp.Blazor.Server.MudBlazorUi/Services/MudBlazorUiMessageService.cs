using System;
using System.Threading.Tasks;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.DependencyInjection;

namespace Abp.MudBlazorUi.Services;

[ExposeServices(typeof(IMudBlazorUiMessageService), typeof(IUiMessageService))]
public class MudBlazorUiMessageService : IMudBlazorUiMessageService, ITransientDependency
{
    protected ISnackbar Snackbar { get; }

    public MudBlazorUiMessageService(ISnackbar snackbar)
    {
        Snackbar = snackbar;
    }

    public Task Info(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        Snackbar.Add(message, Severity.Info, null, title);
        return Task.CompletedTask;
    }

    public Task Success(
        string message,
        string? title = null,
        Action<UiMessageOptions>? options = null
    )
    {
        Snackbar.Add(message, Severity.Success, null, title);
        return Task.CompletedTask;
    }

    public Task Warn(string message, string? title = null, Action<UiMessageOptions>? options = null)
    {
        Snackbar.Add(message, Severity.Warning, null, title);
        return Task.CompletedTask;
    }

    public Task Error(
        string message,
        string? title = null,
        Action<UiMessageOptions>? options = null
    )
    {
        Snackbar.Add(message, Severity.Error, null, title);
        return Task.CompletedTask;
    }

    public Task<bool> Confirm(
        string message,
        string? title = null,
        Action<UiMessageOptions>? options = null
    )
    {
        return Task.FromResult(true);
    }

    public Task Normal(string message, string? key = null, Action<SnackbarOptions>? configure = null)
    {
        Snackbar.Add(message, Severity.Normal, configure, key);
        return Task.CompletedTask;
    }

    public Task Info(string message, string? key = null, Action<SnackbarOptions>? configure = null)
    {
        Snackbar.Add(message, Severity.Info, configure, key);
        return Task.CompletedTask;
    }

    public Task Success(string message, string? key = null, Action<SnackbarOptions>? configure = null)
    {
        Snackbar.Add(message, Severity.Success, configure, key);
        return Task.CompletedTask;
    }

    public Task Warn(string message, string? key = null, Action<SnackbarOptions>? configure = null)
    {
        Snackbar.Add(message, Severity.Warning, configure, key);
        return Task.CompletedTask;
    }

    public Task Error(string message, string? key = null, Action<SnackbarOptions>? configure = null)
    {
        Snackbar.Add(message, Severity.Error, configure, key);
        return Task.CompletedTask;
    }
}
