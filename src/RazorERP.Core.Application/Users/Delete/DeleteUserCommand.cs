using MediatR;

namespace RazorERP.Core.Application.Users.Delete
{
    public record DeleteUserCommand(
        Guid UserId) : IRequest;
}
