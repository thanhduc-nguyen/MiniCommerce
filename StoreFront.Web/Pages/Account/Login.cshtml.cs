using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreFront.Web.Services.Account;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StoreFront.Web.Pages.Account;

public partial class LoginModel(IIdentityService identityService) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public void OnGet(string? returnUrl = default)
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        ReturnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Content("~/") : returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken, string? returnUrl = default)
    {
        if (ModelState.IsValid)
        {
            var response = await identityService.LoginAsync(Input.Username, Input.Password, cancellationToken);

            if (response.User != null && !string.IsNullOrEmpty(response.Token))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(response.Token);
                var identity = new ClaimsIdentity(token.Claims, Constants.MyBearerScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe,
                };

                await HttpContext.SignInAsync(Constants.MyBearerScheme, claimsPrincipal, authProperties);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToPage("/Index");
            }
        }

        return Page();
    }
}
