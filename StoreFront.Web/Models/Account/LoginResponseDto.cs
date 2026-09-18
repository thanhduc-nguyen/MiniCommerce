namespace StoreFront.Web.Models.Account;

public class LoginResponseDto
{
    public UserDto User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
