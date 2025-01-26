using System.Threading.Tasks;
using Abp.MudBlazorUi.Permissions;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using static Abp.MudBlazorUi.Menus.MudBlazorMenus;

namespace Abp.MudBlazorUi.Menus;

public class AuditLoggingMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<AuditLoggingResource>();

        var auditLoggingMenuItem = new ApplicationMenuItem(
            AuditLoggingMenuNames.Default,
            l["Menu:AuditLogging"],
            url: "/auditlogs",
            icon: "description"
        ).RequirePermissions(MudBlazorUiPermissions.AuditLogs.Default);
        administrationMenu.AddItem(auditLoggingMenuItem);
        return Task.CompletedTask;
    }
}
