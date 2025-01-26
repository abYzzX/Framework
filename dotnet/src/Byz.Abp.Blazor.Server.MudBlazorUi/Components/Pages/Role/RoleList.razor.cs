using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.MudBlazorUi;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;

namespace Abp.MudBlazorUi.Components.Pages.Role;

public partial class RoleList
{
    protected bool HasManagePermissionsPermission { get; set; }
    protected string ManagePermissionsPolicyName { get; }

    public RoleList()
    {
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Roles.Create;
        UpdatePolicyName = IdentityPermissions.Roles.Update;
        DeletePolicyName = IdentityPermissions.Roles.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(
            ManagePermissionsPolicyName
        );
    }

    protected async Task ShowPermissionDialogAsync(IdentityRoleDto role)
    {
        await DialogService.ShowAsync<PermissionEditor>(
            $"{L["Permissions"]} - {role.Name}",
            new DialogParameters() { { "ProviderName", "R" }, { "ProviderKey", role.Name } },
            new DialogOptions()
            {
                MaxWidth = MaxWidth.Medium, FullWidth = true, BackgroundClass = "blurry-backdrop"
            }
        );
    }
}
