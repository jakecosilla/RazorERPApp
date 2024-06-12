using RazorERP.Core.Domain.Users;
using MediatR;
using RazorERP.Core.Application.Users.Delete;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Users.Update
{
    internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public DeleteUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new UserNotFoundException(request.UserId);

            await _userRepository.Delete(request.UserId);
        }
    }
}
