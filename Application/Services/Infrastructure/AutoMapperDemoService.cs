using Application.Interfaces.Infrastructure;
using Application.DTOs.UserManagement;
using Application.DTOs.DrillingOperations;
using Domain.Entities.UserManagement;
using Domain.Entities.DrillingOperations;

namespace Application.Services.Infrastructure
{
    public class AutoMapperDemoService
    {
        private readonly IMappingService _mappingService;

        public AutoMapperDemoService(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public UserDto MapUserToDto(User user)
        {
            return _mappingService.Map<UserDto>(user);
        }

        public User MapCreateRequestToUser(CreateUserRequest request)
        {
            return _mappingService.Map<User>(request);
        }

        public void UpdateUserFromDto(User existingUser, UpdateUserRequest request)
        {
            _mappingService.Map(request, existingUser);
        }

        public IEnumerable<UserDto> MapUserCollection(IEnumerable<User> users)
        {
            return _mappingService.Map<IEnumerable<UserDto>>(users);
        }

        public DrillHoleDto MapDrillHoleToDto(DrillHole drillHole)
        {
            return _mappingService.Map<DrillHoleDto>(drillHole);
        }

        public IEnumerable<DrillHoleDto> MapDrillHoleCollection(IEnumerable<DrillHole> drillHoles)
        {
            return _mappingService.Map<IEnumerable<DrillHoleDto>>(drillHoles);
        }
    }
} 