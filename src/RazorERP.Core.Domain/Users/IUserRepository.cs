using RazorERP.Core.Domain.Companies;

namespace RazorERP.Core.Domain.Users
{
    public interface IUserRepository
    {
        Task<List<User>> Get();
        Task<List<User>> GetAllByCompanyId(Guid companyId);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetLoginByEmailAsync(string email);
        Task<List<User>> GetByNameAsync(string name);
        Task Create(User user);
        Task Update(User user);
        Task Delete(Guid id);
    }
}
