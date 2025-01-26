using System;
using System.Threading.Tasks;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;

namespace Abp.MudBlazorUi.Services;

public interface IMudBlazorUiMessageService : IUiMessageService
{
    public Task Normal(string message, string? key = null, Action<SnackbarOptions>? configure = null);
    public Task Info(string message, string? key = null, Action<SnackbarOptions>? configure = null);
    public Task Success(string message, string? key = null, Action<SnackbarOptions>? configure = null);
    public Task Warn(string message, string? key = null, Action<SnackbarOptions>? configure = null);
    public Task Error(string message, string? key = null, Action<SnackbarOptions>? configure = null);
}
