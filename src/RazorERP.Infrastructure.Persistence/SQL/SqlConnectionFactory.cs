using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RazorERP.Core.Abstractions;
using System.Data;

namespace RazorERP.Infrastructure.Persistence.SQL
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("RazorERPDB"));
        }
    }
}
