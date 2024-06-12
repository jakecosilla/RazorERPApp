using FluentValidation;
using RazorERP.Core.Domain.Users;
using RazorERP.Core.Application.Users.Login;

namespace RazorERP.Core.Application.Users.Validations
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginRequest>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(c => c.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email is required")
                .MustAsync(async (email, cancellation) => await EmailExists(email))
                .WithMessage("Account does not exist");

            RuleFor(c => c.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required");
        }

        private async Task<bool> EmailExists(string email)
        {
            var user = await _userRepository.GetLoginByEmailAsync(email);
            return user != null;
        }

    }
}
