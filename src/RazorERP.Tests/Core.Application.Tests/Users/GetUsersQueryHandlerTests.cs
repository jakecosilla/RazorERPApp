using Moq;
using RazorERP.Core.Application.Users.Get;
using RazorERP.Core.Domain.Users;

namespace RazorERP.Core.Application.Tests.Users
{
    public class GetUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUsersQueryHandler _handler;

        public GetUsersQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUsersQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnUserResponses_When_UsersExist()
        {
            // Arrange
            var users = new List<User>
            {
                new User(Guid.NewGuid(), "hashedPassword", "User", Guid.NewGuid(), "John", "Doe", "john.doe@example.com", "1234567890"),
                new User(Guid.NewGuid(), "hashedPassword", "Admin", Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", "0987654321")
            };

            _userRepositoryMock
                .Setup(r => r.Get())
                .ReturnsAsync(users);

            var query = new GetUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(users.Count, result.Count);
            for (int i = 0; i < users.Count; i++)
            {
                Assert.Equal(users[i].Id, result[i].Id);
                Assert.Equal(users[i].FirstName, result[i].FirstName);
                Assert.Equal(users[i].LastName, result[i].LastName);
                Assert.Equal(users[i].Email, result[i].Email);
                Assert.Equal(users[i].PhoneNumber, result[i].PhoneNumber);
            }
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoUsersExist()
        {
            // Arrange
            _userRepositoryMock
                .Setup(r => r.Get())
                .ReturnsAsync(new List<User>());

            var query = new GetUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
