using Microsoft.Data.SqlClient;
using System.Data;

namespace RazorERP.Core.Abstractions
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
