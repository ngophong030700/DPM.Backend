using Identity.Domain.Users;

namespace Shared.Infrastructure.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
