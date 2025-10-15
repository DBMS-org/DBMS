using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IProjectSiteRepository
    {
        Task<IEnumerable<ProjectSite>> GetAllAsync();
        Task<ProjectSite?> GetByIdAsync(int id);
        Task<IEnumerable<ProjectSite>> GetByProjectIdAsync(int projectId);
        Task<ProjectSite> CreateAsync(ProjectSite projectSite);
        Task<bool> UpdateAsync(ProjectSite projectSite);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ProjectExistsAsync(int projectId);
        Task<bool> ApprovePatternAsync(int id);
        Task<bool> RevokePatternAsync(int id);
        Task<bool> ConfirmSimulationAsync(int id);
        Task<bool> RevokeSimulationAsync(int id);
        Task<bool> CompleteSiteAsync(int id, int completedByUserId);
        // Note: Explosive approval methods moved to IExplosiveApprovalRequestRepository
    }
}