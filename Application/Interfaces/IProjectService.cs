using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        // Project CRUD operations
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request);
        Task<ProjectDto> UpdateProjectAsync(UpdateProjectRequest request);
        Task<bool> DeleteProjectAsync(int id);
        
        // Project queries
        Task<List<ProjectDto>> GetProjectsByRegionAsync(int regionId);
        Task<List<ProjectDto>> GetProjectsByUserAsync(int userId);
        Task<List<ProjectDto>> GetActiveProjectsAsync();
        Task<List<ProjectDto>> GetCompletedProjectsAsync();
        Task<List<ProjectDto>> GetOverdueProjectsAsync();
        
        // Project assignment
        Task<bool> AssignProjectToUserAsync(int projectId, int userId);
        Task<bool> UnassignProjectFromUserAsync(int projectId);
        
        // Project status management
        Task<bool> CompleteProjectAsync(int projectId);
        Task<bool> ActivateProjectAsync(int projectId);
        Task<bool> DeactivateProjectAsync(int projectId);
        
        // Project validation
        Task<bool> ValidateProjectExistsAsync(int projectId);
        Task<bool> IsUserAssignedToProjectAsync(int projectId, int userId);
        
        // Project sites
        Task<List<ProjectSiteDto>> GetProjectSitesAsync(int projectId);
    }
} 