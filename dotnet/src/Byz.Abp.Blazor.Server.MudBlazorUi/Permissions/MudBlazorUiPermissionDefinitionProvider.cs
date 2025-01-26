using Abp.MudBlazorUi.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Abp.MudBlazorUi.Permissions;

public class MudBlazorUiPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var mudBlazorGroup = context.AddGroup(MudBlazorUiPermissions.GroupName, L("Permission:GroupName"));
        var auditLogs = mudBlazorGroup.AddPermission(MudBlazorUiPermissions.AuditLogs.Default, L("Permission:AuditLogs"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AbpMudBlazorUiResource>(name);
    }
}
