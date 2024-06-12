namespace RazorERP.Core.Domain.Users
{
    public sealed class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid id)
            : base($"The user with the ID = {id} was not found")
        {
        }
    }
}
