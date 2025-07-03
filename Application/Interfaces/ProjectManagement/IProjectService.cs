using Application.DTOs.ProjectManagement;
using Application.DTOs.Shared;
using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IProjectService
    {
        Task<Result<IEnumerable<Project>>> GetAllProjectsAsync();
        Task<Result<Project>> GetProjectByIdAsync(int id);
        Task<Result<Project>> CreateProjectAsync(CreateProjectRequest request);
        Task<Result> UpdateProjectAsync(int id, UpdateProjectRequest request);
        Task<Result> DeleteProjectAsync(int id);
        Task<Result<IEnumerable<ProjectSite>>> GetProjectSitesAsync(int projectId);
        Task<Result<IEnumerable<Project>>> SearchProjectsAsync(string? name = null, string? region = null, string? status = null);
        Task<Result<Project>> GetProjectByOperatorAsync(int operatorId);
    }
} 