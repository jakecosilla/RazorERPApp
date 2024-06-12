using RazorERP.Core.Application.Users.Requests;
using RazorERP.Core.Domain.Users;
using MediatR;

namespace RazorERP.Core.Application.Users.Update
{
    public record UpdateUserCommand(
        Guid UserId,
        UserRequest User) : IRequest;
}
