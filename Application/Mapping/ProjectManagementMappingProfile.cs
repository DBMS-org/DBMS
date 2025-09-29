using AutoMapper;
using Application.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Project Management mappings
    /// </summary>
    public class ProjectManagementMappingProfile : Profile
    {
        public ProjectManagementMappingProfile()
        {
            // Project mappings
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<CreateProjectRequest, Project>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ProjectStatus>(src.Status)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore())
                .ForMember(dest => dest.RegionNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectSites, opt => opt.Ignore())
                .ForMember(dest => dest.Machines, opt => opt.Ignore());

            CreateMap<UpdateProjectRequest, Project>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<ProjectStatus>(src.Status)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore())
                .ForMember(dest => dest.RegionNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectSites, opt => opt.Ignore())
                .ForMember(dest => dest.Machines, opt => opt.Ignore());

            // ProjectSite mappings
            CreateMap<ProjectSite, ProjectSiteDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            CreateMap<CreateProjectSiteRequest, ProjectSite>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProjectSiteStatus.Planned))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore());

            // Region mappings
            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)));

            // ExplosiveApprovalRequest mappings
            CreateMap<ExplosiveApprovalRequest, ExplosiveApprovalRequestDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.ApprovalType, opt => opt.MapFrom(src => src.ApprovalType.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.UpdatedAt, DateTimeKind.Utc)))
                .ForMember(dest => dest.ProcessedAt, opt => opt.MapFrom(src => src.ProcessedAt.HasValue ? DateTime.SpecifyKind(src.ProcessedAt.Value, DateTimeKind.Utc) : (DateTime?)null));

            // Coordinates mappings
            CreateMap<CoordinatesDto, string>()
                .ConvertUsing(src => $"{src.Latitude},{src.Longitude}");

            CreateMap<string, CoordinatesDto>()
                .ConvertUsing((src, dest) => 
                {
                    var parts = src.Split(',');
                    if (parts.Length == 2)
                    {
                        dest.Latitude = double.Parse(parts[0]);
                        dest.Longitude = double.Parse(parts[1]);
                    }
                    return dest;
                });
        }
    }
}