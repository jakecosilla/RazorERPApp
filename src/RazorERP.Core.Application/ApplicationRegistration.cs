using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RazorERP.Core.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(ApplicationRegistration).Assembly;

            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
