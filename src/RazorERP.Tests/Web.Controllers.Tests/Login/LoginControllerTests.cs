using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RazorERP.API.Controllers;
using RazorERP.Core.Application.Companies.Login;
using RazorERP.Core.Application.Users.Login;

namespace RazorERP.Web.API.Tests.Login
{
    public class LoginControllerTests
    {
        private readonly Mock<ISender> _mockMediator;
        private readonly Mock<IValidator<LoginRequest>> _mockValidator;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockMediator = new Mock<ISender>();
            _mockValidator = new Mock<IValidator<LoginRequest>>();
            _controller = new LoginController(_mockMediator.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValidRequest()
        {
            // Arrange
            var request = new LoginRequest("john@example.com", "password123");
            var loginResponse = new LoginResponse("token");

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());
            _mockMediator.Setup(m => m.Send(It.IsAny<LoginCommand>(), default)).ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<LoginResponse>(okResult.Value);
            Assert.Equal(loginResponse.Token, returnValue.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnValidationProblem_WhenInvalidRequest()
        {
            // Arrange
            var request = new LoginRequest("", "password123");
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Email", "Email is required")
        };
            var validationResult = new ValidationResult(failures);

            _mockValidator.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var validationProblemResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, validationProblemResult.StatusCode);
        }
    }
}
