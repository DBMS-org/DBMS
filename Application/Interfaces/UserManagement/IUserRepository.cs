using Domain.Entities.UserManagement;

namespace Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNameOrEmailAsync(string name, string email);
        Task<bool> ExistsAsync(int id);
        Task<bool> CanConnectAsync();
        Task<int> GetCountAsync();
        Task<IEnumerable<User>> GetByRoleAndRegionAsync(string role, string? region = null);
    }
} 