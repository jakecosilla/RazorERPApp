using Microsoft.Extensions.DependencyInjection;
using RazorERP.Core.Application.Abstractions;
using RazorERP.Core.Domain.Authentication;
using RazorERP.Infrastructure.Services.Authentication;

namespace RazorERP.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            ;
            return services;
        }
    }
}
