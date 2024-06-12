using Moq;
using RazorERP.Core.Application.Users.Create;
using RazorERP.Core.Application.Users.Login;
using RazorERP.Core.Application.Abstractions;
using RazorERP.Core.Domain.Users;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Tests.Users
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IJwtProvider> _jwtProviderMock;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _handler = new LoginCommandHandler(_userRepositoryMock.Object, _authenticationServiceMock.Object, _jwtProviderMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnToken_When_CredentialsAreValid()
        {
            // Arrange
            var userLoginRequest = new LoginRequest("test@example.com", "password123");
            var user = new User(Guid.NewGuid(), "hashedPassword", "User", Guid.NewGuid(), "John", "Doe", "test@example.com", "1234567890");
            var command = new LoginCommand(userLoginRequest);
            var expectedToken = "generatedToken";

            _userRepositoryMock
                .Setup(r => r.GetLoginByEmailAsync(userLoginRequest.Email))
                .ReturnsAsync(user);

            _authenticationServiceMock
                .Setup(a => a.isPasswordValid(userLoginRequest.Password, user.Password))
                .Returns(true);

            _jwtProviderMock
                .Setup(j => j.Generate(user))
                .Returns(expectedToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedToken, result.Token);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorizedAccessException_When_EmailIsInvalid()
        {
            // Arrange
            var userLoginRequest = new LoginRequest("invalid@example.com", "password123");
            var command = new LoginCommand(userLoginRequest);

            _userRepositoryMock
                .Setup(r => r.GetLoginByEmailAsync(userLoginRequest.Email))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorizedAccessException_When_PasswordIsInvalid()
        {
            // Arrange
            var userLoginRequest = new LoginRequest("test@example.com", "invalidPassword");
            var user = new User(Guid.NewGuid(), "hashedPassword", "User", Guid.NewGuid(), "John", "Doe", "test@example.com", "1234567890");
            var command = new LoginCommand(userLoginRequest);

            _userRepositoryMock
                .Setup(r => r.GetLoginByEmailAsync(userLoginRequest.Email))
                .ReturnsAsync(user);

            _authenticationServiceMock
                .Setup(a => a.isPasswordValid(userLoginRequest.Password, user.Password))
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
