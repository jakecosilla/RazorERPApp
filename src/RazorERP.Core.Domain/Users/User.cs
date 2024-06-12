namespace RazorERP.Core.Domain.Users
{
    public class User
    {
        public User() { }
        public User(Guid id,string password, string role, Guid companyId, string firstName, string lastName, string email, string phoneNumber)
        {
            Id = id;
            Password = password;
            Role = role;
            CompanyId = companyId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Guid Id { get; private set; }
        public string Password { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;
        public Guid CompanyId { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        public void Update(string password, string role, Guid companyId, string firstName, string lastName, string email, string phoneNumber)
        {
            Password = password;
            Role = role;
            CompanyId = companyId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
