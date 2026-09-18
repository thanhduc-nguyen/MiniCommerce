using StoreFront.Web.Models.Account;

namespace StoreFront.Web.Features.Account.Services;

public interface IAppAuthenticationService
{
    Task<LoginResponseDto> GetToken(LoginRequestDto loginRequestDto);
}
