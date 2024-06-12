using System.Data;
using Dapper;
using RazorERP.Core.Abstractions;
using RazorERP.Core.Domain.Companies;
using RazorERP.Core.Domain.Users;

namespace RazorERP.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public CompanyRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Company>> Get()
        {
            using var connection = _connectionFactory.CreateConnection();
            var companies = await connection.QueryAsync<Company>(
                "SP_CompaniesGetAll",
                commandType: CommandType.StoredProcedure);
            return companies.ToList();
        }

        public async Task<Company?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Company>(
                "SP_CompanyGetById",
                new { CompanyId = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
