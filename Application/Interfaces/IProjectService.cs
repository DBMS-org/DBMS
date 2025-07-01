using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request);
        Task<bool> UpdateProjectAsync(int id, UpdateProjectRequest request);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<ProjectSiteDto>> GetProjectSitesAsync(int projectId);
        Task<IEnumerable<ProjectDto>> SearchProjectsAsync(string? name = null, string? region = null, string? status = null);
        Task<ProjectDto?> GetProjectByOperatorAsync(int operatorId);
    }
} 