namespace RazorERP.Core.Application.Users.Requests
{
    public record UserRequest(
        string Password,
        string Role,
        Guid CompanyId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber);
}
