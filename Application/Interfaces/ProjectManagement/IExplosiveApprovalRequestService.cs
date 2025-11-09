using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IExplosiveApprovalRequestService
    {
        /// <summary>
        /// Retrieves an explosive approval request by its ID
        /// </summary>
        Task<ExplosiveApprovalRequest?> GetExplosiveApprovalRequestByIdAsync(int id);
        
        /// <summary>
        /// Retrieves all explosive approval requests for a specific project site
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByProjectSiteIdAsync(int projectSiteId);
        
        /// <summary>
        /// Retrieves all explosive approval requests created by a specific user
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByUserIdAsync(int userId);
        
        /// <summary>
        /// Retrieves all pending explosive approval requests
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetPendingExplosiveApprovalRequestsAsync();
        
        /// <summary>
        /// Creates a new explosive approval request
        /// </summary>
        Task<ExplosiveApprovalRequest> CreateExplosiveApprovalRequestAsync(
            int projectSiteId,
            int requestedByUserId,
            DateTime expectedUsageDate,
            string? comments = null,
            RequestPriority priority = RequestPriority.Normal,
            ExplosiveApprovalType approvalType = ExplosiveApprovalType.Standard,
            DateTime? blastingDate = null,
            string? blastTiming = null);
        
        /// <summary>
        /// Updates an existing explosive approval request
        /// </summary>
        Task<bool> UpdateExplosiveApprovalRequestAsync(ExplosiveApprovalRequest request);
        
        /// <summary>
        /// Approves an explosive approval request
        /// </summary>
        Task<bool> ApproveExplosiveApprovalRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null);
        
        /// <summary>
        /// Rejects an explosive approval request
        /// </summary>
        Task<bool> RejectExplosiveApprovalRequestAsync(int requestId, int rejectedByUserId, string rejectionReason);
        
        /// <summary>
        /// Cancels an explosive approval request
        /// </summary>
        Task<bool> CancelExplosiveApprovalRequestAsync(int requestId);
        
        /// <summary>
        /// Deletes an explosive approval request
        /// </summary>
        Task<bool> DeleteExplosiveApprovalRequestAsync(int id);

        /// <summary>
        /// Updates the blasting date and timing for an explosive approval request
        /// </summary>
        Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming);

        /// <summary>
        /// Checks if a project site has any pending explosive approval requests
        /// </summary>
        Task<bool> HasPendingExplosiveApprovalRequestAsync(int projectSiteId);

        /// <summary>
        /// Gets the latest explosive approval request for a project site
        /// </summary>
        Task<ExplosiveApprovalRequest?> GetLatestExplosiveApprovalRequestAsync(int projectSiteId);

        /// <summary>
        /// Retrieves explosive approval requests filtered by store manager's region
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByRegionAsync(string region);
    }
}