using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        // User CRUD operations
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(CreateUserRequest request);
        Task<UserDto> UpdateUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> DeactivateUserAsync(int id);
        Task<bool> ActivateUserAsync(int id);
        
        // User authentication helpers
        Task<bool> UpdateLastLoginAsync(int userId);
        Task<bool> IsUserActiveAsync(int userId);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
        
        // Role management
        Task<List<UserRoleAssignmentDto>> GetUserRolesAsync(int userId);
        Task<bool> AssignRolesToUserAsync(AssignRoleRequest request);
        Task<bool> RevokeRolesFromUserAsync(RevokeRoleRequest request);
        Task<bool> UpdateUserRolesAsync(UpdateUserRolesRequest request);
        Task<bool> HasRoleAsync(int userId, string roleName);
        Task<bool> HasAnyRoleAsync(int userId, List<string> roleNames);
        
        // User queries
        Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
        Task<List<UserDto>> GetUsersByRegionAsync(string region);
        Task<List<UserDto>> GetActiveUsersAsync();
    }
} 