using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StoreFront.Web.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreFront.Web.Features.Account.Services;

public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        };

        foreach (var claim in claims)
        {
            claimsForToken.Add(claim);
        }

        foreach (var role in roles)
        {
            claimsForToken.Add(new Claim(ClaimTypes.Role, role));
        }

        var jwtSecurityToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler()
           .WriteToken(jwtSecurityToken);

        return tokenToReturn;
    }
}
