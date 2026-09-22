using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniCommerce.Web.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(string? returnUrl = default)
    {
        await HttpContext.SignOutAsync(Constants.MyBearerScheme);
        return RedirectToPage("/Index");
    }
}
