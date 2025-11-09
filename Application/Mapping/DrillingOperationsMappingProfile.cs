using AutoMapper;
using Application.DTOs.DrillingOperations;
using Domain.Entities.DrillingOperations;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Drilling Operations mappings
    /// </summary>
    public class DrillingOperationsMappingProfile : Profile
    {
        public DrillingOperationsMappingProfile()
        {
            // DrillHole mappings
            CreateMap<DrillHole, DrillHoleDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<CreateDrillHoleRequest, DrillHole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateDrillHoleRequest, DrillHole>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't update ID
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore()) // Don't update project context
                .ForMember(dest => dest.SiteId, opt => opt.Ignore()) // Don't update site context
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Don't update creation time
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // DrillPoint mappings
            CreateMap<DrillPoint, DrillPointDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<DrillPointDto, DrillPoint>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            // PatternSettings mappings
            CreateMap<PatternSettings, PatternSettingsDto>()
                .ForMember(dest => dest.Spacing, opt => opt.MapFrom(src => src.Spacing))
                .ForMember(dest => dest.Burden, opt => opt.MapFrom(src => src.Burden))
                .ForMember(dest => dest.Depth, opt => opt.MapFrom(src => src.Depth));
            
            CreateMap<PatternSettingsDto, PatternSettings>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't map ID from DTO
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => "Default"))
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Site, opt => opt.Ignore());

            // ExplosiveCalculationResult mappings
            CreateMap<ExplosiveCalculationResult, ExplosiveCalculationResultDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<CreateExplosiveCalculationResultRequest, ExplosiveCalculationResult>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Site, opt => opt.Ignore())
                .ForMember(dest => dest.PatternSettings, opt => opt.Ignore())
                .ForMember(dest => dest.OwningUser, opt => opt.Ignore());

            CreateMap<UpdateExplosiveCalculationResultRequest, ExplosiveCalculationResult>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Don't update ID
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore()) // Don't update project context
                .ForMember(dest => dest.SiteId, opt => opt.Ignore()) // Don't update site context
                .ForMember(dest => dest.OwningUserId, opt => opt.Ignore()) // Don't update owner
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Don't update creation time
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.Site, opt => opt.Ignore())
                .ForMember(dest => dest.PatternSettings, opt => opt.Ignore())
                .ForMember(dest => dest.OwningUser, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Only map non-null values
        }
    }
}