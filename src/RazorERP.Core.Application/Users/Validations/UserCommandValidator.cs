using RazorERP.Core.Application.Users.Requests;
using FluentValidation;
using RazorERP.Core.Domain.Companies;

namespace RazorERP.Core.Application.Users.Validations
{
    public sealed class UserCommandValidator : AbstractValidator<UserRequest>
    {
        private readonly ICompanyRepository _companyRepository;

        public UserCommandValidator(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;

            RuleFor(c => c.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("FirstName is required");

            RuleFor(c => c.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("LastName is required");

            RuleFor(c => c.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(c => c.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("PhoneNumber is required");

            RuleFor(c => c.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");

            RuleFor(c => c.Role)
                .NotNull()
                .NotEmpty()
                .WithMessage("Role is required")
                .Must(BeAValidRole)
                .WithMessage("Role must be either 'Admin' or 'FlatUser'");

            RuleFor(c => c.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage("CompanyId is required")
                .MustAsync(async (companyId, cancellation) => await CompanyExists(companyId))
                .WithMessage("CompanyId does not exist");
        }

        private bool BeAValidRole(string role)
        {
            return role == "Admin" || role == "User";
        }

        private async Task<bool> CompanyExists(Guid companyId)
        {
            var company = await _companyRepository.GetByIdAsync(companyId);
            return company != null;
        }
    }
}
