using StoreFront.Web.Models.Account;
using System.Security.Claims;

namespace StoreFront.Web.Features.Account.Services;

public interface IJwtTokenGenerator
{
    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<Claim> claims);
}
