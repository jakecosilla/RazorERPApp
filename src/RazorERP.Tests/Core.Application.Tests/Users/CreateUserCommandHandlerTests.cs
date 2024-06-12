using Moq;
using RazorERP.Core.Application.Users.Create;
using RazorERP.Core.Application.Users.Requests;
using RazorERP.Core.Domain.Users;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Tests.Users
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new CreateUserCommandHandler(_userRepositoryMock.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_CreateUser_When_RequestIsValid()
        {
            // Arrange
            var userRequest = new UserRequest(
                "password123",
                "User",
                Guid.NewGuid(),
                "John",
                "Doe",
                "john.doe@example.com",
                "1234567890"
            );

            var command = new CreateUserCommand(userRequest);

            _authenticationServiceMock
                .Setup(a => a.hashPassword(It.IsAny<string>()))
                .Returns("hashedPassword");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(r => r.Create(It.Is<User>(u =>
                u.Password == "hashedPassword" &&
                u.Role == userRequest.Role &&
                u.CompanyId == userRequest.CompanyId &&
                u.FirstName == userRequest.FirstName &&
                u.LastName == userRequest.LastName &&
                u.Email == userRequest.Email &&
                u.PhoneNumber == userRequest.PhoneNumber
            )), Times.Once);
        }
    }
}
