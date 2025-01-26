using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace Abp.MudBlazorUi.Menus;

public class AbpIdentityMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<IdentityResource>();

        var identityMenuItem = new ApplicationMenuItem(
            MudBlazorMenus.IdentityMenuNames.GroupName,
            l["Menu:IdentityManagement"],
            icon: "id_card"
        );
        administrationMenu.AddItem(identityMenuItem);

        identityMenuItem.AddItem(
            new ApplicationMenuItem(
                MudBlazorMenus.IdentityMenuNames.Roles,
                l["Roles"],
                url: "/identity/roles"
            ).RequirePermissions(IdentityPermissions.Roles.Default)
        );

        identityMenuItem.AddItem(
            new ApplicationMenuItem(
                MudBlazorMenus.IdentityMenuNames.Users,
                l["Users"],
                url: "/identity/users"
            ).RequirePermissions(IdentityPermissions.Users.Default)
        );

        return Task.CompletedTask;
    }
}
