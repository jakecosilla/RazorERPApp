using MediatR;
using Microsoft.AspNetCore.Mvc;
using RazorERP.Core.Application.Users.Login;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using RazorERP.Core.Application.Companies.Login;

namespace RazorERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IValidator<LoginRequest> _validator;

        public LoginController(ISender mediator, IValidator<LoginRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var command = new LoginCommand(request);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
