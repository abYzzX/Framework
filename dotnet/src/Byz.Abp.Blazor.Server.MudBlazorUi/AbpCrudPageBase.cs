using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.MudBlazorUi.Localization;
using Abp.MudBlazorUi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;

namespace Abp.MudBlazorUi;

public abstract class AbpCrudPageBase<
    TAppService,
    TDto,
    TKey,
    TGetListInput,
    TCreateUpdateDto> : AbpCrudPageBase<TAppService, TDto, TKey, TGetListInput, TCreateUpdateDto, TCreateUpdateDto> 
    where TAppService : ICrudAppService<
        TDto,
        TKey,
        TGetListInput,
        TCreateUpdateDto,
        TCreateUpdateDto
    >
    where TDto : IEntityDto<TKey>
    where TGetListInput : new()
    where TCreateUpdateDto : class, new()
{}

public abstract class AbpCrudPageBase<
    TAppService,
    TDto,
    TKey,
    TGetListInput,
    TCreateDto,
    TUpdateDto
> : AbpComponentBase
    where TAppService : ICrudAppService<
        TDto,
        TKey,
        TGetListInput,
        TCreateDto,
        TUpdateDto
    >
    where TDto : IEntityDto<TKey>
    where TGetListInput : new()
    where TCreateDto : class, new()
    where TUpdateDto : class, new()
{
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected TAppService AppService { get; set; } = default!;

    [Inject]
    public IAbpEnumLocalizer AbpEnumLocalizer { get; set; } = default!;

    [Inject]
    public IStringLocalizer<AbpMudBlazorUiResource> UL { get; set; } = default!;

    protected MudDataGrid<TDto>? Grid { get; set; }
    protected bool IsLoading { get; set; } = true;
    protected TKey? EditingEntityId { get; set; }
    protected string? CreatePolicyName { get; set; }
    protected string? UpdatePolicyName { get; set; }
    protected string? DeletePolicyName { get; set; }

    protected TGetListInput GetListInput { get; } = new();
    
    public bool HasCreatePermission { get; set; }
    public bool HasUpdatePermission { get; set; }
    public bool HasDeletePermission { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await TrySetPermissionsAsync();
    }

    protected virtual async Task<GridData<TDto>> LoadDataAsync(
        GridStateVirtualize<TDto> gridState, CancellationToken cancellationToken = default)
    {
        IsLoading = true;
        await UpdateGetListInputAsync(gridState);
        var result = await AppService.GetListAsync(GetListInput);
        var items = result.Items;

        if (gridState.FilterDefinitions.Count > 0)
        {
            items = items.Where(x => gridState.FilterDefinitions.All(f => f.GenerateFilterFunction().Invoke(x)))
                .ToList();
        }

        return new GridData<TDto>() { Items = items, TotalItems = (int)result.TotalCount };
    }

    protected virtual Task UpdateGetListInputAsync(GridStateVirtualize<TDto> gridState)
    {
        if (Grid == null)
        {
            return Task.CompletedTask;
        }

        if (GetListInput is ISortedResultRequest sortedResultRequestInput)
        {
            if (gridState.SortDefinitions.Count > 0)
            {
                var sortStr = new List<string>();
                foreach (var sortDefinition in gridState.SortDefinitions)
                {
                    if (Guid.TryParse(sortDefinition.SortBy, out var _))
                    {
                        if (Grid.GetColumnByPropertyName(sortDefinition.SortBy) is { } col
                            && col.UserAttributes.TryGetValue("Field", out var field))
                        {
                            sortStr.Add($"{field} {(sortDefinition.Descending ? "DESC" : "ASC")}");
                        }
                    }
                    else
                    {
                        sortStr.Add($"{sortDefinition.SortBy} {(sortDefinition.Descending ? "DESC" : "ASC")}");
                    }
                }

                sortedResultRequestInput.Sorting = string.Join(",", sortStr);
            }
            else
            {
                sortedResultRequestInput.Sorting = null;
            }
        }

        if (GetListInput is IPagedResultRequest pagedResultRequestInput)
        {
            pagedResultRequestInput.SkipCount = gridState.StartIndex;
        }

        if (GetListInput is ILimitedResultRequest limitedResultRequestInput
            && Grid.HasPager)
        {
            limitedResultRequestInput.MaxResultCount = Grid.RowsPerPage;
        }

        return Task.CompletedTask;
    }

    private async Task TrySetPermissionsAsync()
    {
        if (IsDisposed)
        {
            return;
        }

        await SetPermissionsAsync();
    }

    protected virtual async Task SetPermissionsAsync()
    {
        if (CreatePolicyName != null)
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
        }

        if (UpdatePolicyName != null)
        {
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
        }

        if (DeletePolicyName != null)
        {
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
        }
    }

    protected virtual async Task OpenCreateDialogAsync<TDialog>(
        string title,
        Func<DialogOptions>? func = null,
        Dictionary<string, object>? parameters = null
    )
        where TDialog : ComponentBase
    {
        var dialogParams = new DialogParameters
        {
            ["Model"] = new TCreateDto()
        };
        await OpenEditorDialogAsync<TDialog>(
            title,
            func,
            dialogParams,
            parameters,
            (d, m) => CreateEntityAsync(d, m as TCreateDto)
        );
    }

    protected virtual async Task OpenEditDialogAsync<TDialog>(
        string title,
        TDto dto,
        Func<DialogOptions>? func = null,
        Dictionary<string, object>? parameters = null
    )
        where TDialog : ComponentBase
    {
        var dialogParams = new DialogParameters
        {
            ["Model"] = ObjectMapper.Map<TDto, TUpdateDto>(dto),
            ["UpdateId"] = dto.Id
        };
        EditingEntityId = dto.Id;
        await OpenEditorDialogAsync<TDialog>(
            title,
            func,
            dialogParams,
            parameters,
            (d, m) => UpdateEntityAsync(d, m as TUpdateDto)
        );
    }

    private async Task OpenEditorDialogAsync<TDialog>(string title, Func<DialogOptions>? func,
                                                      DialogParameters dialogParams,
                                                      Dictionary<string, object>? parameters,
                                                      Func<IDialogReference, object, Task> submitFunc)
        where TDialog : ComponentBase
    {
        if (parameters is not null
            && parameters.Count > 0)
        {
            foreach (var (k, v) in parameters)
            {
                dialogParams[k] = v;
            }
        }

        IDialogReference? dlgHandler = null;
        dialogParams["OnCancel"] = () => CloseDialog(dlgHandler);
        dialogParams["OnSubmit"] = (object o) => submitFunc(dlgHandler, o);
        
        dlgHandler = await DialogService.ShowAsync<TDialog>(
            title: title,
            parameters: dialogParams,
            options: func is not null
                ? func()
                : new DialogOptions()
                {
                    MaxWidth = MaxWidth.Large, 
                    Position = DialogPosition.Center, 
                    BackdropClick = false,
                    BackgroundClass = "blurry-backdrop"
                }
        );

        var result = await dlgHandler.Result;
        dlgHandler.Close();

        if (result is { Canceled: false }
            && Grid != null)
        {
            await Grid.ReloadServerData();
        }
    }

    
    protected virtual void CloseDialog(IDialogReference? dlgHandler)
    {
        dlgHandler?.Close();
    }

    protected virtual async Task CreateEntityAsync(IDialogReference dlgHandler, TCreateDto? model)
    {
        try
        {
            _ = Check.NotNull(model, nameof(model));
            await AppService.CreateAsync(model);
            await Message.Success(UL["SavedSuccessfully"]);
            dlgHandler?.Close(DialogResult.Ok(model));
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }


    protected virtual async Task UpdateEntityAsync(IDialogReference dlgHandler, TUpdateDto? model)
    {
        try
        {
            _ = Check.NotNull(model, nameof(model));
            await AppService.UpdateAsync(EditingEntityId, model);
            await Message.Success(UL["SavedSuccessfully"]);
            dlgHandler?.Close(DialogResult.Ok(model));
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenDeleteConfirmDialogAsync(
        TKey id,
        string title = "Confirm",
        string confirm = "Confirm?"
    )
    {
        var result = await DialogService.ShowMessageBox(
            message: confirm,
            title: title,
            yesText: L["Delete"], cancelText: L["Cancel"]
        );

        if (result is true)
        {
            await AppService.DeleteAsync(id);
            await Grid.ReloadServerData();
            await Message.Success(UL["DeletedSuccessfully"]);
        }
    }

    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    protected virtual async Task CheckDeletePolicyAsync()
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    /// <summary>
    /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
    /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
    ///
    /// Does nothing if <paramref name="policyName"/> is null or empty.
    /// </summary>
    /// <param name="policyName">A policy name to check</param>
    protected virtual async Task CheckPolicyAsync(string? policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return;
        }

        await AuthorizationService.CheckAsync(policyName);
    }
}
