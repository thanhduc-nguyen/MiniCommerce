using MiniCommerce.Web.Models.Account;

namespace MiniCommerce.Web.Services.Account;

public interface IAppAuthenticationService
{
    Task<LoginResponseDto> GetToken(LoginRequestDto loginRequestDto);
}
