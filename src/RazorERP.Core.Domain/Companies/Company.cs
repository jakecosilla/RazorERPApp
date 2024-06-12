namespace RazorERP.Core.Domain.Companies
{
    public class Company
    {
        public Company() { }

        public Company(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;
    }
}
