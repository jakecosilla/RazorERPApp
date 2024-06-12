namespace RazorERP.Core.Domain.Authentication
{
    public interface IAuthenticationService
    {
        string hashPassword(string password);
        bool isPasswordValid(string requestPassword, string userPassword);
    }
}
