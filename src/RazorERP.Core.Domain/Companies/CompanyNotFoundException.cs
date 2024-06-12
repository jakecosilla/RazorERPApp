namespace RazorERP.Core.Domain.Companies
{
    public sealed class CompanyNotFoundException : Exception
    {
        public CompanyNotFoundException(Guid id)
            : base($"The company with the ID = {id} was not found")
        {
        }
    }
}
