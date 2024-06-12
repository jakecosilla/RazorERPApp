using RazorERP.Core.Domain.Users;

namespace RazorERP.Core.Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}
