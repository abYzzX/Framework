using MudBlazor;

namespace Abp.MudBlazorUi.Services;

public interface IMudBlazorUiIconResolver
{
    string this[string? iconName] { get; } 
}
