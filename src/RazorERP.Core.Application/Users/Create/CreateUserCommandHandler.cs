using RazorERP.Core.Domain.Users;
using MediatR;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Users.Create
{
    internal sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(
                Guid.NewGuid(),
                _authenticationService.hashPassword(request.User.Password),
                request.User.Role,
                request.User.CompanyId,
                request.User.FirstName,
                request.User.LastName,
                request.User.Email,
                request.User.PhoneNumber);

            await _userRepository.Create(user);
        }
    }
}
