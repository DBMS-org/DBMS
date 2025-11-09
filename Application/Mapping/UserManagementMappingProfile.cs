using AutoMapper;
using Application.DTOs.UserManagement;
using Domain.Entities.UserManagement;
using Domain.Common;

namespace Application.Mapping
{
    public class UserManagementMappingProfile : Profile
    {
        public UserManagementMappingProfile()
        {
            // Entity to DTO mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Don't update password hash from DTO
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetCode, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetCodeExpiry, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Active))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.LastLoginAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetCode, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetCodeExpiry, opt => opt.Ignore())
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            // Authentication result mappings
            CreateMap<User, AuthenticationResult>()
                .ForMember(dest => dest.Token, opt => opt.Ignore()) // Will be set by service
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
        }
    }
} 