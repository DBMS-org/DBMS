using Domain.Entities.UserManagement;

namespace Application.Interfaces.Infrastructure
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        bool IsTokenBlacklisted(string token);
        Task BlacklistTokenAsync(string token);
        Task CleanupExpiredTokensAsync();
    }
} 