using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RazorERP.API.Controllers;
using RazorERP.Core.Application.Companies.Get;
using RazorERP.Core.Application.Users.Requests;

namespace RazorERP.Tests.Web.Controllers.Tests.Companies
{
    public class CompaniesControllerTests
    {
        private readonly Mock<ISender> _mockMediator;
        private readonly Mock<IValidator<UserRequest>> _mockValidator;
        private readonly CompaniesController _controller;

        public CompaniesControllerTests()
        {
            _mockMediator = new Mock<ISender>();
            _mockValidator = new Mock<IValidator<UserRequest>>();
            _controller = new CompaniesController(_mockMediator.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetCompanies_ShouldReturnOk_WithListOfCompanies()
        {
            // Arrange
            var companies = new List<CompanyResponse>
        {
            new CompanyResponse(Guid.NewGuid(), "Company1", "Address1"),
            new CompanyResponse(Guid.NewGuid(), "Company2", "Address2")
        };

            _mockMediator.Setup(m => m.Send(It.IsAny<GetCompaniesQuery>(), default)).ReturnsAsync(companies);

            // Act
            var result = await _controller.GetCompanies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CompanyResponse>>(okResult.Value);
            Assert.Equal(companies.Count, returnValue.Count);
        }
    }
}
