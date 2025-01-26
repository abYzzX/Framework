using Abp.MudBlazorUi.Utils;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp.MudBlazorUi.Controllers;

public class ThemeController : AbpControllerBase
{
    [HttpGet("/theme/switch")]
    public IActionResult SwitchAsync(string value, string returnUrl = "~/")
    {
        AbpMudBlazorUiCookieHelper.SetThemeCookie(HttpContext, value);
        return Redirect(returnUrl);
    }
}
