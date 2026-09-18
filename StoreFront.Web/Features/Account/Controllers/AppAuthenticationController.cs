using Microsoft.AspNetCore.Mvc;
using StoreFront.Web.Features.Account.Services;
using StoreFront.Web.Models.Account;

namespace StoreFront.Web.Features.Account.Controllers;

[ApiController]
[Route("{controller}/{action}")]
public class AppAuthenticationController(IAppAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto credential)
    {
        var response = new LoginResponseDto();

        if (credential == null || string.IsNullOrEmpty(credential.UserName) || string.IsNullOrEmpty(credential.Password))
        {
            response.Message = "Invalid client request";
            return BadRequest(response);
        }

        var loginResponse = await authenticationService.GetToken(credential);
        if (loginResponse.User == null)
        {
            response.Message = "Username or password is incorrect";
            return BadRequest(response);
        }

        response.User = loginResponse.User;
        response.Token = loginResponse.Token;
        response.Message = "Token retrieved successfully.";

        return Ok(response);
    }
}
