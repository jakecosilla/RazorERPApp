
using Microsoft.AspNetCore.Http;
using RazorERP.Core.Domain.Companies;
using RazorERP.Core.Domain.Users;
using System.Net;
using System.Text.Json;

namespace RazorERP.Infrastructure.Middlewares.Global
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case UserNotFoundException:
                    case CompanyNotFoundException:
                    case KeyNotFoundException:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new
                {
                    message = error?.Message,
                    details = response.StatusCode == (int)HttpStatusCode.InternalServerError ?
                        (error?.InnerException != null ? error?.InnerException.Message : error?.GetType().Name.ToString()) : error?.GetType().Name.ToString(),
                });


                await response.WriteAsync(result);
            }
        }
    }
}
