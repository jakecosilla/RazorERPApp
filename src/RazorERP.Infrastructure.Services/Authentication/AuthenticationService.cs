using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Infrastructure.Services.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        public string hashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool isPasswordValid(string requestPassword, string userPassword) => BCrypt.Net.BCrypt.Verify(requestPassword, userPassword);
    }
}
