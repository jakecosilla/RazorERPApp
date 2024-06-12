using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazorERP.Core.Application.Users.Create;
using RazorERP.Core.Application.Users.Delete;
using RazorERP.Core.Application.Users.Get;
using RazorERP.Core.Application.Users.Requests;
using RazorERP.Core.Application.Users.Response;
using RazorERP.Core.Application.Users.Update;

namespace RazorERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IValidator<UserRequest> _validator;

        public UsersController(ISender mediator, IValidator<UserRequest> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var query = new GetUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var command = new CreateUserCommand(request);

            await _mediator.Send(command);

            return NoContent();
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] UserRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var command = new UpdateUserCommand(
                id,
                request
            );

            await _mediator.Send(command);

            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
