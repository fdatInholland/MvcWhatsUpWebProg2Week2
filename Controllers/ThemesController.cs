using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MvcWhatsUp.Controllers
{
    public class ThemesController : Controller
    {
        public IActionResult SetPreferredTheme(string? theme)
        {
            if (!theme.IsNullOrEmpty())
            {
                CookieOptions cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    Path = "/",
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true,
                };
                Response.Cookies.Append("PreferredTheme", theme, cookieOptions);
            }
            return RedirectToAction("Index", "Users");
        }
    }
}
