using RazorERP.Core.Domain.Users;
using MediatR;
using System.Linq;
using RazorERP.Core.Application.Users.Response;

namespace RazorERP.Core.Application.Users.Get
{
    internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponse>>
    {
        private readonly IUserRepository _UserRepository;

        public GetUsersQueryHandler(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public async Task<List<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _UserRepository.Get();
           
            var result = users.Select(User => new UserResponse(
                User.Id,
                User.FirstName,
                User.LastName,
                User.Email,
                User.PhoneNumber)).ToList();

            return result;
        }
    }
}
