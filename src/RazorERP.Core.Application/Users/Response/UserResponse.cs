namespace RazorERP.Core.Application.Users.Response
{
    public record UserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber);
}
