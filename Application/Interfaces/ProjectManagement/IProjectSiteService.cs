using Application.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IProjectSiteService
    {
        Task<IEnumerable<ProjectSite>> GetAllProjectSitesAsync();
        Task<ProjectSite?> GetProjectSiteByIdAsync(int id);
        Task<IEnumerable<ProjectSite>> GetProjectSitesByProjectIdAsync(int projectId);
        Task<ProjectSite> CreateProjectSiteAsync(CreateProjectSiteRequest request);
        Task<bool> UpdateProjectSiteAsync(int id, ProjectSite request);
        Task<bool> DeleteProjectSiteAsync(int id);
        Task<bool> ApprovePatternAsync(int id);
        Task<bool> RevokePatternAsync(int id);
        Task<bool> ConfirmSimulationAsync(int id);
        Task<bool> RevokeSimulationAsync(int id);
        Task<bool> CompleteSiteAsync(int id, int completedByUserId);
        // Note: Explosive approval methods moved to IExplosiveApprovalRequestService
    }
}