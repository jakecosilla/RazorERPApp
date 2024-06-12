using MediatR;
using RazorERP.Core.Application.Users.Response;

namespace RazorERP.Core.Application.Users.Get
{
    public record GetUsersQuery() : IRequest<List<UserResponse>>;
}
