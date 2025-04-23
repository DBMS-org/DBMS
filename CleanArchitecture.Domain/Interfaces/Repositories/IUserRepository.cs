using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> CreateAsync(User user, string password);
        Task UpdateAsync(User user);
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
        Task<bool> IsInRoleAsync(int userId, string roleName);
        Task<bool> UpdateFailedLoginAttemptsAsync(User user);
        Task<bool> ResetPasswordAsync(User user, string newPassword);
    }
} 