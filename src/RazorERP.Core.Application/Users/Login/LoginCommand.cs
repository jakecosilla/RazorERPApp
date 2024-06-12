using MediatR;
using RazorERP.Core.Application.Companies.Login;

namespace RazorERP.Core.Application.Users.Login
{
    public record LoginCommand(
        LoginRequest UserLogin) : IRequest<LoginResponse>;
}
