using MiniCommerce.Web.Models.Account;

namespace MiniCommerce.Web.Services.Account;

public interface IIdentityService
{
    Task<LoginResponseDto> LoginAsync(string userName, string password, CancellationToken cancellationToken);
}
