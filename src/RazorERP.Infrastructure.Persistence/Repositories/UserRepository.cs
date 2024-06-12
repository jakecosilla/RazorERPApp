using System.Data;
using Dapper;
using RazorERP.Core.Abstractions;
using RazorERP.Core.Domain.Users;

namespace RazorERP.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task Create(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SP_UserAdd",
                new
                {
                    user.Password,
                    user.Role,
                    user.CompanyId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<User>> Get()
        {
            using var connection = _connectionFactory.CreateConnection();
            var users = await connection.QueryAsync<User>(
                "SP_UsersGetAll",
                commandType: CommandType.StoredProcedure);
            return users.ToList();
        }

        public async Task<List<User>> GetAllByCompanyId(Guid companyId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var users = await connection.QueryAsync<User>(
                "SP_UsersGetByCompany",
                new { CompanyId = companyId },
                commandType: CommandType.StoredProcedure);
            return users.ToList();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SP_UserGetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User?> GetLoginByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(
                "SP_UserGetLoginByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<List<User>> GetByNameAsync(string name)
        {
            using var connection = _connectionFactory.CreateConnection();
            var users = await connection.QueryAsync<User>(
                "SP_UsersGetByName",
                new { Name = name },
                commandType: CommandType.StoredProcedure);
            return users.ToList();
        }

        public async Task Update(User user)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SP_UserUpdate",
                new
                {
                    user.Id,
                    user.Role,
                    user.Password,
                    user.CompanyId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task Delete(Guid id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(
                "SP_UserDelete",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
