using MiniCommerce.Web.Models.Account;

namespace MiniCommerce.Web.Services.Account;

public class IdentityService(IAppAuthenticationService appAuthenticationService) : IIdentityService
{
    public async Task<LoginResponseDto> LoginAsync(string userName, string password, CancellationToken cancellationToken)
    {
        var request = new LoginRequestDto
        {
            UserName = userName,
            Password = password
        };

        if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
        {
            return new LoginResponseDto();
        }

        var loginResponse = await appAuthenticationService.GetToken(request);

        if (loginResponse != null)
        {
            return loginResponse;
        }
        else
        {
            return new LoginResponseDto();
        }
    }
}
