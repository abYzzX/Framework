using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.MudBlazorUi.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Localization;

namespace Abp.MudBlazorUi.Components.Pages;

public partial class PermissionEditor
{
    private sealed record PermissionGroupViewModel(
        PermissionGroupDto Dto,
        List<TreeItemData<PermissionGrantInfoDto>> Children);

    [Parameter]
    public string ProviderName { get; set; } = default!;

    [Parameter]
    public string ProviderKey { get; set; } = default!;

    [CascadingParameter]
    public IMudDialogInstance MudDialog { get; set; }

    private List<PermissionGroupViewModel> Groups { get; set; } = [];

    public PermissionEditor()
    {
        LocalizationResource = typeof(AbpPermissionManagementResource);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var result = await PermissionAppService.GetAsync(ProviderName, ProviderKey);
        Groups = result.Groups.Select(x => new PermissionGroupViewModel(x, GenerateChildren(x.Permissions, null)))
            .ToList();
    }

    private List<TreeItemData<PermissionGrantInfoDto>> GenerateChildren(List<PermissionGrantInfoDto> permissions,
                                                                        string? parentName)
    {
        return permissions.Where(x => x.ParentName == parentName)
            .Select(
                x => new TreeItemData<PermissionGrantInfoDto>()
                {
                    Value = x, Text = x.DisplayName, Children = GenerateChildren(permissions, x.Name)
                }
            )
            .ToList();
    }

    private static List<PermissionGrantInfoDto> Flatten(TreeItemData<PermissionGrantInfoDto> root)
    {
        var flatList = new List<PermissionGrantInfoDto>();

        void Flatten(TreeItemData<PermissionGrantInfoDto> node)
        {
            flatList.Add(node.Value);
            foreach (var child in node.Children)
            {
                Flatten(child);
            }
        }
        
        Flatten(root);
        return flatList;
    }
    
    private async Task SaveAsync()
    {
        try
        {
            var updateDtos = Groups.SelectMany(x => x.Children.SelectMany(Flatten))
                .Select(x => new UpdatePermissionDto() { IsGranted = x.IsGranted, Name = x.Name, })
                .ToArray();

            if (!updateDtos.Any(x => x.IsGranted))
            {
                var result = await DialogService.ShowMessageBox(
                    L["Permissions"],
                    L["SaveWithoutAnyPermissionsWarningMessage"],
                    yesText: L["Yes"],
                    cancelText: L["Cancel"]
                );

                if (result == false)
                {
                    return;
                }
            }

            await PermissionAppService.UpdateAsync(
                ProviderName,
                ProviderKey,
                new UpdatePermissionsDto() { Permissions = updateDtos }
            );
            await CacheResetService.ResetAsync();
            
            await Message.Success(L["SuccessfullySaved"]);
            MudDialog.Close();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update permissions.");
            await Message.Error(ex.Message);
        }
    }
}
