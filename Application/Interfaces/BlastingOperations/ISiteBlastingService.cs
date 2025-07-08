using Application.DTOs.BlastingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface ISiteBlastingService
    {
        // Site Data Management
        Task<bool> DeleteAllWorkflowDataAsync(int projectId, int siteId);
        Task<bool> HasWorkflowDataAsync(int projectId, int siteId);
        
        // Validation
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
    }
} 
