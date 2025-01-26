using Volo.Abp.Reflection;

namespace Abp.MudBlazorUi.Permissions;

public static class MudBlazorUiPermissions
{
    public const string GroupName = "AbpMudBlazorUI";

    public static class AuditLogs
    {
        public const string Default = GroupName + ".AuditLogs";
    }
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(MudBlazorUiPermissions));
    }
}
