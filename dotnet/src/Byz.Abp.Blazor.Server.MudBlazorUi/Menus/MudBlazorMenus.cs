﻿namespace Abp.MudBlazorUi.Menus;

public class MudBlazorMenus
{
    public static class IdentityMenuNames
    {
        public const string GroupName = "AbpIdentity";

        public const string Roles = GroupName + ".Roles";
        public const string Users = GroupName + ".Users";
    }

    public static class TenantManagementMenuNames
    {
        public const string GroupName = "TenantManagement";

        public const string Tenants = GroupName + ".Tenants";
    }

    public static class AuditLoggingMenuNames
    {
        public const string Default = "AuditLogging";
    }
}
