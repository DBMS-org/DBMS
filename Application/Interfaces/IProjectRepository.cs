using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project?> GetByIdWithDetailsAsync(int id);
        Task<Project> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ProjectSite>> GetProjectSitesAsync(int projectId);
        Task<IEnumerable<Project>> SearchAsync(string? name = null, string? region = null, string? status = null);
        Task<Project?> GetByOperatorIdAsync(int operatorId);
        Task<bool> ExistsAsync(int id);
        Task<bool> UserExistsAsync(int userId);
    }
} 