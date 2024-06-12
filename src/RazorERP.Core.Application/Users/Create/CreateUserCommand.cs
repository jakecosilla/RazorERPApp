using RazorERP.Core.Application.Users.Requests;
using MediatR;

namespace RazorERP.Core.Application.Users.Create
{
    public record CreateUserCommand(
        UserRequest User) : IRequest;
}
