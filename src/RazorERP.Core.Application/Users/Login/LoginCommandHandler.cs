using RazorERP.Core.Domain.Users;
using MediatR;
using RazorERP.Core.Application.Users.Login;
using RazorERP.Core.Application.Companies.Login;
using RazorERP.Core.Application.Abstractions;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Users.Create
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the user by email
            var user = await _userRepository.GetLoginByEmailAsync(request.UserLogin.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email.");
            }

            // Verify the password
            var isPasswordValid = _authenticationService.isPasswordValid(request.UserLogin.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid password.");
            }

            // Generate the JWT token
            var token = _jwtProvider.Generate(user);

            return new LoginResponse(token);
        }
    }
}
