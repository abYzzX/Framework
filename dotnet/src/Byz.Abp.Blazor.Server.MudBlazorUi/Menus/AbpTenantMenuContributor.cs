using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace Abp.MudBlazorUi.Menus;

public class AbpTenantMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<AbpTenantManagementResource>();

        var tenantManagementMenuItem = new ApplicationMenuItem(
            MudBlazorMenus.TenantManagementMenuNames.GroupName,
            l["Menu:TenantManagement"],
            icon: "reduce_capacity"
        );
        administrationMenu.AddItem(tenantManagementMenuItem);

        tenantManagementMenuItem.AddItem(
            new ApplicationMenuItem(
                MudBlazorMenus.TenantManagementMenuNames.Tenants,
                l["Tenants"],
                url: "/tenantManagement/tenants"
            ).RequirePermissions(TenantManagementPermissions.Tenants.Default)
        );

        return Task.CompletedTask;
    }
}
