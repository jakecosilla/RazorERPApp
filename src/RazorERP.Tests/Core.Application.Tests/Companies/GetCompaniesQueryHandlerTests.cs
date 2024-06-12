using Moq;
using RazorERP.Core.Application.Companies.Get;
using RazorERP.Core.Domain.Companies;

namespace RazorERP.Core.Application.Tests.Companies
{
    public class GetCompaniesQueryHandlerTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly GetCompaniesQueryHandler _handler;

        public GetCompaniesQueryHandlerTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _handler = new GetCompaniesQueryHandler(_companyRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnCompanyResponses_When_CompaniesExist()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company(Guid.NewGuid(), "Google"),
                new Company(Guid.NewGuid(), "Meta"),
                new Company(Guid.NewGuid(), "Toyota")
            };

            _companyRepositoryMock
                .Setup(r => r.Get())
                .ReturnsAsync(companies);

            var query = new GetCompaniesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(companies.Count, result.Count);
            for (int i = 0; i < companies.Count; i++)
            {
                Assert.Equal(companies[i].Id, result[i].Id);
                Assert.Equal(companies[i].Name, result[i].Name);
            }
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_When_NoCompaniesExist()
        {
            // Arrange
            _companyRepositoryMock
                .Setup(r => r.Get())
                .ReturnsAsync(new List<Company>());

            var query = new GetCompaniesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }
    }
}
