using MiniCommerce.Web.Models.Account;
using System.Security.Claims;

namespace MiniCommerce.Web.Services.Account;

public interface IJwtTokenGenerator
{
    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<Claim> claims);
}
