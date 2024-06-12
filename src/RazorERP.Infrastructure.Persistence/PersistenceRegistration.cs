using Microsoft.Extensions.DependencyInjection;
using RazorERP.Core.Abstractions;
using RazorERP.Core.Domain.Companies;
using RazorERP.Core.Domain.Users;
using RazorERP.Infrastructure.Persistence.Repositories;
using RazorERP.Infrastructure.Persistence.SQL;

namespace RazorERP.Infrastructure.Persistence
{
    public static class PersistenceRegistration
    {
        public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
;
            return services;
        }
    }
}
