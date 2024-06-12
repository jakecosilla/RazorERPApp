using Moq;
using RazorERP.Core.Application.Users.Update;
using RazorERP.Core.Application.Users.Requests;
using RazorERP.Core.Domain.Users;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Tests.Users
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateUser_When_RequestIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User(userId, "hashedPassword", "User", Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "1234567890");

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);

            var userRequest = new UserRequest(
                "newPassword123",
                "Admin",
                existingUser.CompanyId,
                "Jane",
                "Doe",
                "jane.doe@example.com",
                "0987654321"
            );

            var command = new UpdateUserCommand(userId, userRequest);

            _authenticationServiceMock
                .Setup(a => a.hashPassword(It.IsAny<string>()))
                .Returns("newHashedPassword");

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(r => r.Update(It.Is<User>(u =>
                u.Password == "newHashedPassword" &&
                u.Role == userRequest.Role &&
                u.CompanyId == userRequest.CompanyId &&
                u.FirstName == userRequest.FirstName &&
                u.LastName == userRequest.LastName &&
                u.Email == userRequest.Email &&
                u.PhoneNumber == userRequest.PhoneNumber
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowUserNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            var userRequest = new UserRequest(
                "newPassword123",
                "Admin",
                Guid.NewGuid(),
                "Jane",
                "Doe",
                "jane.doe@example.com",
                "0987654321"
            );

            var command = new UpdateUserCommand(userId, userRequest);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
