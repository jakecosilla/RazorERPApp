using RazorERP.Core.Domain.Users;
using MediatR;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Users.Update
{
    internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public UpdateUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new UserNotFoundException(request.UserId);

            user.Update(
                _authenticationService.hashPassword(request.User.Password),
                request.User.Role,
                request.User.CompanyId,
                request.User.FirstName,
                request.User.LastName,
                request.User.Email,
                request.User.PhoneNumber);

            await _userRepository.Update(user);
        }
    }
}
