using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserApplicationService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserApplicationService> _logger;

        public UserApplicationService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            ILogger<UserApplicationService> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return users.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user != null ? MapToDto(user) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {UserId}", id);
                throw;
            }
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.ExistsByNameOrEmailAsync(request.Name, request.Email);
                if (existingUser)
                {
                    throw new InvalidOperationException("User with this name or email already exists");
                }

                // Hash password
                var hashedPassword = _passwordService.HashPassword(request.Password);

                // Create user entity
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hashedPassword,
                    Role = request.Role,
                    Status = "Active", // Default status
                    Region = request.Region,
                    Country = request.Country,
                    OmanPhone = request.OmanPhone,
                    CountryPhone = request.CountryPhone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);
                return MapToDto(createdUser);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error creating user {Email}", request.Email);
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return false;
                }

                // Update user properties
                user.Name = request.Name;
                user.Email = request.Email;
                user.Role = request.Role;
                user.Status = request.Status;
                user.Region = request.Region;
                user.Country = request.Country;
                user.OmanPhone = request.OmanPhone;
                user.CountryPhone = request.CountryPhone;
                user.UpdatedAt = DateTime.UtcNow;

                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                return await _userRepository.CanConnectAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing database connection");
                throw;
            }
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                Region = user.Region,
                Country = user.Country,
                OmanPhone = user.OmanPhone,
                CountryPhone = user.CountryPhone,
                CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(user.UpdatedAt, DateTimeKind.Utc)
            };
        }
    }
} 