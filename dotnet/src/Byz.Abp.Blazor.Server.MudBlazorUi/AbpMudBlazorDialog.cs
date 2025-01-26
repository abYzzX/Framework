using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components;

namespace Abp.MudBlazorUi;

public partial class AbpMudBlazorDialog<TModel, TKey> : AbpComponentBase
    where TModel : class
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    [Parameter]
    public TModel Model { get; set; }

    [Parameter]
    public TKey? UpdateId { get; set; }
    
    [Parameter]
    public Action OnCancel { get; set; }

    [Parameter]
    public Func<TModel, Task> OnSubmit { get; set; }

    protected virtual async Task Submit()
    {
        try
        {
            await OnSubmit(Model);
            MudDialog.Close(DialogResult.Ok(Model));
        }
        catch (Exception ex)
        {
            await Message.Error(ex.Message);
        }
    }

    protected virtual void Cancel()
    {
        OnCancel();
        MudDialog.Close(DialogResult.Cancel());
    }
}

