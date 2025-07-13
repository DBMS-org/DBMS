using Application.Interfaces.Infrastructure;
using Application.DTOs.UserManagement;
using Application.DTOs.DrillingOperations;
using Domain.Entities.UserManagement;
using Domain.Entities.DrillingOperations;

namespace Application.Services.Infrastructure
{
    /// <summary>
    /// Demonstration service showing AutoMapper usage patterns
    /// </summary>
    public class AutoMapperDemoService
    {
        private readonly IMappingService _mappingService;

        public AutoMapperDemoService(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        /// <summary>
        /// Demonstrates entity to DTO mapping
        /// </summary>
        public UserDto MapUserToDto(User user)
        {
            return _mappingService.Map<UserDto>(user);
        }

        /// <summary>
        /// Demonstrates DTO to entity mapping
        /// </summary>
        public User MapCreateRequestToUser(CreateUserRequest request)
        {
            return _mappingService.Map<User>(request);
        }

        /// <summary>
        /// Demonstrates updating an existing entity from DTO
        /// </summary>
        public void UpdateUserFromDto(User existingUser, UpdateUserRequest request)
        {
            _mappingService.Map(request, existingUser);
        }

        /// <summary>
        /// Demonstrates collection mapping
        /// </summary>
        public IEnumerable<UserDto> MapUserCollection(IEnumerable<User> users)
        {
            return _mappingService.Map<IEnumerable<UserDto>>(users);
        }

        /// <summary>
        /// Demonstrates drill hole mapping
        /// </summary>
        public DrillHoleDto MapDrillHoleToDto(DrillHole drillHole)
        {
            return _mappingService.Map<DrillHoleDto>(drillHole);
        }

        /// <summary>
        /// Demonstrates drill hole collection mapping
        /// </summary>
        public IEnumerable<DrillHoleDto> MapDrillHoleCollection(IEnumerable<DrillHole> drillHoles)
        {
            return _mappingService.Map<IEnumerable<DrillHoleDto>>(drillHoles);
        }
    }
} 