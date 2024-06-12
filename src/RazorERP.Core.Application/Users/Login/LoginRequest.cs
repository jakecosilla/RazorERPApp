namespace RazorERP.Core.Application.Users.Login
{
    public record LoginRequest(
        string Email,
        string Password);
}
