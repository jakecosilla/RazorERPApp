using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RazorERP.API.Controllers;
using RazorERP.Core.Application.Users.Create;
using RazorERP.Core.Application.Users.Delete;
using RazorERP.Core.Application.Users.Get;
using RazorERP.Core.Application.Users.Requests;
using RazorERP.Core.Application.Users.Response;
using RazorERP.Core.Application.Users.Update;

namespace RazorERP.Web.API.Tests.Users
{
    public class UsersControllerTests
    {
        private readonly Mock<ISender> _mockMediator;
        private readonly Mock<IValidator<UserRequest>> _mockValidator;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockMediator = new Mock<ISender>();
            _mockValidator = new Mock<IValidator<UserRequest>>();
            _controller = new UsersController(_mockMediator.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetUsers_ShouldReturnOk_WithListOfUsers()
        {
            // Arrange
            var users = new List<UserResponse>
        {
            new UserResponse(Guid.NewGuid(), "John", "Doe", "john@example.com", "123-456-7890"),
            new UserResponse(Guid.NewGuid(), "Jane", "Doe", "jane@example.com", "098-765-4321")
        };

            _mockMediator.Setup(m => m.Send(It.IsAny<GetUsersQuery>(), default)).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<UserResponse>>(okResult.Value);
            Assert.Equal(users.Count, returnValue.Count);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnNoContent_WhenValidRequest()
        {
            // Arrange
            var request = new UserRequest("John", "Doe", Guid.NewGuid(), "john@example.com", "password123", "Admin", "123-456-7890");
            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<CreateUserCommand>(), default));

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnValidationProblem_WhenInvalidRequest()
        {
            // Arrange
            var request = new UserRequest(null, "Doe", Guid.NewGuid(), "john@example.com", "password123", "Admin", "123-456-7890");
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("FirstName", "First name is required")
        };
            var validationResult = new ValidationResult(failures);
            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            var validationProblemResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, validationProblemResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenValidRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserRequest("John", "Doe", Guid.NewGuid(), "john@example.com", "password123", "Admin", "123-456-7890");
            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateUserCommand>(), default));

            // Act
            var result = await _controller.UpdateUser(userId, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnValidationProblem_WhenInvalidRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new UserRequest("", "Doe", Guid.NewGuid(), "john@example.com", "password123", "Admin", "123-456-7890");
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("FirstName", "First name is required")
        };
            var validationResult = new ValidationResult(failures);
            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.UpdateUser(userId, request);

            // Assert
            var validationProblemResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, validationProblemResult.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockMediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default));

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

