using MiniCommerce.Web.Models.Account;
using MiniCommerce.Web.Services.Account;

namespace MiniCommerce.Web.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (
            LoginRequestDto credential,
            IAppAuthenticationService authenticationService) =>
        {
            if (credential is null || string.IsNullOrEmpty(credential.UserName) || string.IsNullOrEmpty(credential.Password))
            {
                return Results.BadRequest(new LoginResponseDto { Message = "Invalid client request" });
            }

            var loginResponse = await authenticationService.GetToken(credential);
            if (loginResponse.User is null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return Results.BadRequest(new LoginResponseDto { Message = "Username or password is incorrect" });
            }

            loginResponse.Message = "Token retrieved successfully.";
            return Results.Ok(loginResponse);
        });

        return app;
    }
}
