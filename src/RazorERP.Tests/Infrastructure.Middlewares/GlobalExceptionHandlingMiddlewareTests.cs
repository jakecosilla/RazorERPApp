using System.Net;
using Microsoft.AspNetCore.Http;
using Moq;
using RazorERP.Core.Domain.Companies;
using RazorERP.Core.Domain.Users;
using RazorERP.Infrastructure.Middlewares.Global;

namespace RazorERP.Infrastructure.Middlewares.Tests
{
    public class GlobalExceptionHandlingMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _mockRequestDelegate;
        private readonly GlobalExceptionHandlingMiddleware _middleware;

        public GlobalExceptionHandlingMiddlewareTests()
        {
            _mockRequestDelegate = new Mock<RequestDelegate>();
            _middleware = new GlobalExceptionHandlingMiddleware(_mockRequestDelegate.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnNotFound_WhenUserNotFoundExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var userId = Guid.NewGuid();
            _mockRequestDelegate.Setup(rd => rd.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new UserNotFoundException(userId));

            var originalBodyStream = context.Response.Body;
            using var responseBody = new System.IO.MemoryStream();
            context.Response.Body = responseBody;

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
            var responseBodyString = await GetResponseBody(context);
            Assert.Contains($"The user with the ID = {userId} was not found", responseBodyString);

            // Reset the response body stream
            context.Response.Body = originalBodyStream;
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnNotFound_WhenCompanyNotFoundExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var companyId = Guid.NewGuid();
            _mockRequestDelegate.Setup(rd => rd.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new CompanyNotFoundException(companyId));

            var originalBodyStream = context.Response.Body;
            using var responseBody = new System.IO.MemoryStream();
            context.Response.Body = responseBody;

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
            var responseBodyString = await GetResponseBody(context);
            Assert.Contains($"The company with the ID = {companyId} was not found", responseBodyString);

            // Reset the response body stream
            context.Response.Body = originalBodyStream;
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnInternalServerError_WhenUnhandledExceptionThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _mockRequestDelegate.Setup(rd => rd.Invoke(It.IsAny<HttpContext>())).ThrowsAsync(new Exception("Unhandled exception"));

            var originalBodyStream = context.Response.Body;
            using var responseBody = new System.IO.MemoryStream();
            context.Response.Body = responseBody;

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            var responseBodyString = await GetResponseBody(context);
            Assert.Contains("Unhandled exception", responseBodyString);

            // Reset the response body stream
            context.Response.Body = originalBodyStream;
        }

        private async Task<string> GetResponseBody(HttpContext context)
        {
            context.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            using var reader = new System.IO.StreamReader(context.Response.Body);
            return await reader.ReadToEndAsync();
        }
    }
}
