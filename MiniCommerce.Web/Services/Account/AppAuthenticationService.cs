using Microsoft.AspNetCore.Identity;
using MiniCommerce.Web.Data;
using MiniCommerce.Web.Models.Account;

namespace MiniCommerce.Web.Services.Account;

public class AppAuthenticationService(
    AppDbContext dbContext,
    IJwtTokenGenerator bearerTokenGenerator,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IAppAuthenticationService
{
    public async Task<LoginResponseDto> GetToken(LoginRequestDto loginRequestDto)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.UserName!.ToLower() == loginRequestDto.UserName.ToLower());

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
