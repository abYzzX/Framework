using System;
using Microsoft.AspNetCore.Http;

namespace Abp.MudBlazorUi.Utils;

public static class AbpMudBlazorUiCookieHelper
{
    private static string ThemeKey = ".AbpMudBlazorUi.Theme";

    public static void SetThemeCookie(HttpContext context, string theme)
    {
        context.Response.Cookies.Append(ThemeKey, theme, new CookieOptions
        {
            Path = "/",
            HttpOnly = false,
            IsEssential = true,
            Expires = DateTimeOffset.Now.AddYears(10)
        });
    }

    public static string? GetThemeCookie(HttpContext context)
    {
        context.Request.Cookies.TryGetValue(ThemeKey, out var theme);
        return theme;
    }
}
