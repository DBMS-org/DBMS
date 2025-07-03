namespace Application.Interfaces.BlastingOperations
{
    using Application.DTOs.Shared;

    public interface IWorkflowProgressService
    {
        Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId);
        Task<SiteWorkflowProgressDto> UpdateWorkflowProgressAsync(int projectId, int siteId, string stepId, bool completed);
        Task<bool> SetOperatorCompletionAsync(int projectId, int siteId, bool completed);
    }
} 