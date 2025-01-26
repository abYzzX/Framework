using Abp.MudBlazorUi.Bundling;
using Abp.MudBlazorUi.Localization;
using Abp.MudBlazorUi.Menus;
using Abp.MudBlazorUi.Services;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor;
using MudBlazor.Services;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Server.Configuration;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling.Localization;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Abp.MudBlazorUi;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpAutoMapperModule)
)]
public class AbpMudBlazorUiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddRazorPages();
        context.Services.AddServerSideBlazor();
        context.Services.AddMudServices();
        
        // Add services to the container.
        context.Services.AddRazorComponents().AddInteractiveServerComponents();
        context.Services.AddCascadingAuthenticationState();

        context.Services.Replace(
            ServiceDescriptor.Singleton<
                ICurrentApplicationConfigurationCacheResetService,
                BlazorServerCurrentApplicationConfigurationCacheResetService
            >()
        );

        // Replace the default IUiMessageService with Radzen.Blazor's implementation by NotificationService
        context.Services.Replace(
            ServiceDescriptor.Transient<IUiMessageService, MudBlazorUiMessageService>()
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMudBlazorUiModule>("Abp.MudBlazorUi");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options
                .Resources.Add<AbpMudBlazorUiResource>("en")
                .AddBaseTypes(
                    typeof(AbpValidationResource),
                    typeof(AbpUiResource),
                    typeof(AbpExceptionHandlingResource),
                    typeof(AuditLoggingResource)
                )
                .AddVirtualJson("/Localization/UI");
        });

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpMudBlazorUiModule>(true);
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Add(
                BlazorMudBlazorThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(BlazorGlobalStyleContributor));
                }
            );

            options.ScriptBundles.Add(
                BlazorMudBlazorThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddContributors(typeof(BlazorGlobalScriptContributor));
                }
            );
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Clear();
            options.MenuContributors.Add(new DefaultMudBlazorMenuContributor());
            options.MenuContributors.Add(new AbpIdentityMenuContributor());
            options.MenuContributors.Add(new AbpTenantMenuContributor());
            options.MenuContributors.Add(new AuditLoggingMenuContributor());
        });
        
        Configure<AbpMudBlazorUiOptions>(
            options =>
            {
                options.IconOverwrite.Overwrites.Add("id_card", Icons.Material.Rounded.Badge);
            });
    }
}
