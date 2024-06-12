using Moq;
using RazorERP.Core.Application.Users.Delete;
using RazorERP.Core.Application.Users.Update;
using RazorERP.Core.Domain.Users;
using RazorERP.Core.Domain.Authentication;

namespace RazorERP.Core.Application.Tests.Users
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteUser_When_UserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "hashedPassword", "User", Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "1234567890");

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(user);

            var command = new DeleteUserCommand(userId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(r => r.Delete(userId), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowUserNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            var command = new DeleteUserCommand(userId);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
