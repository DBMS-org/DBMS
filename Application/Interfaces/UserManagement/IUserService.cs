using Application.DTOs.UserManagement;
using Application.DTOs.Shared;

namespace Application.Interfaces.UserManagement
{
    public interface IUserService
    {
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<Result<UserDto>> GetUserByIdAsync(int id);
        Task<Result<UserDto>> CreateUserAsync(CreateUserRequest request);
        Task<Result> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<Result> DeleteUserAsync(int id);
        Task<Result> TestConnectionAsync();
    }
} 