using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectSiteService
    {
        Task<IEnumerable<ProjectSiteDto>> GetAllProjectSitesAsync();
        Task<ProjectSiteDto?> GetProjectSiteByIdAsync(int id);
        Task<IEnumerable<ProjectSiteDto>> GetProjectSitesByProjectIdAsync(int projectId);
        Task<ProjectSiteDto> CreateProjectSiteAsync(CreateProjectSiteRequest request);
        Task<bool> UpdateProjectSiteAsync(int id, ProjectSiteDto request);
        Task<bool> DeleteProjectSiteAsync(int id);
        Task<bool> ApprovePatternAsync(int id);
        Task<bool> RevokePatternAsync(int id);
        Task<bool> ConfirmSimulationAsync(int id);
        Task<bool> RevokeSimulationAsync(int id);
    }
} 