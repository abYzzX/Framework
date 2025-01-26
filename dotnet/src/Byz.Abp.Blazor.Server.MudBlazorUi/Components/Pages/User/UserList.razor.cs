using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;

namespace Abp.MudBlazorUi.Components.Pages.User;

public partial class UserList
{
    protected bool HasManagePermissionsPermission { get; set; }
    protected string ManagePermissionsPolicyName { get; }

    public UserList()
    {
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Users.Create;
        UpdatePolicyName = IdentityPermissions.Users.Update;
        DeletePolicyName = IdentityPermissions.Users.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Users.ManagePermissions;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(
            ManagePermissionsPolicyName
        );
    }

    protected async Task ShowPermissionDialogAsync(IdentityUserDto user)
    {
        await DialogService.ShowAsync<PermissionEditor>(
            $"{L["Permissions"]} - {user.UserName}",
            new DialogParameters() { { "ProviderName", "U" }, { "ProviderKey", user.Id.ToString() } },
            new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, BackgroundClass = "blurry-backdrop" }
        );
    }
}
