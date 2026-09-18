using Microsoft.AspNetCore.Identity;
using StoreFront.Web.Models.Account;

namespace StoreFront.Web.Features.Account.Services;

public class AppAuthenticationService(
    IdentityApiDbContext identityApiDbContext,
    IJwtTokenGenerator bearerTokenGenerator,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IAppAuthenticationService
{
    public async Task<LoginResponseDto> GetToken(LoginRequestDto loginRequestDto)
    {
        var user = identityApiDbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDto { User = new UserDto(), Token = string.Empty };
        }

        var roles = await userManager.GetRolesAsync(user);
        var claims = await userManager.GetClaimsAsync(user);
        var token = bearerTokenGenerator.GenerateToken(user, roles, claims);

        UserDto userDTO = new()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
        };

        var loginResponseDto = new LoginResponseDto()
        {
            User = userDTO,
            Token = token,
            Message = "Token retrieved successfully."
        };

        return loginResponseDto;
    }
}
