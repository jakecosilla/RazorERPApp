namespace RazorERP.Core.Domain.Companies
{
    public interface ICompanyRepository
    {
        Task<List<Company>> Get();
        Task<Company?> GetByIdAsync(Guid id);
    }
}
