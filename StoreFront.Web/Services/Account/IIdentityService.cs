using StoreFront.Web.Models.Account;

namespace StoreFront.Web.Services.Account;

public interface IIdentityService
{
    Task<LoginResponseDto> LoginAsync(string userName, string password, CancellationToken cancellationToken);
}
