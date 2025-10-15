using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface IExplosiveApprovalRequestRepository
    {
        /// <summary>
        /// Retrieves an explosive approval request by its ID
        /// </summary>
        Task<ExplosiveApprovalRequest?> GetByIdAsync(int id);
        
        /// <summary>
        /// Retrieves all explosive approval requests for a specific project site
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetByProjectSiteIdAsync(int projectSiteId);
        
        /// <summary>
        /// Retrieves all explosive approval requests created by a specific user
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetByUserIdAsync(int userId);
        
        /// <summary>
        /// Retrieves all pending explosive approval requests
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetPendingRequestsAsync();
        
        /// <summary>
        /// Creates a new explosive approval request
        /// </summary>
        Task<ExplosiveApprovalRequest> CreateAsync(ExplosiveApprovalRequest request);
        
        /// <summary>
        /// Updates an existing explosive approval request
        /// </summary>
        Task<bool> UpdateAsync(ExplosiveApprovalRequest request);
        
        /// <summary>
        /// Approves an explosive approval request
        /// </summary>
        Task<bool> ApproveRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null);
        
        /// <summary>
        /// Rejects an explosive approval request
        /// </summary>
        Task<bool> RejectRequestAsync(int requestId, int rejectedByUserId, string rejectionReason);
        
        /// <summary>
        /// Cancels an explosive approval request
        /// </summary>
        Task<bool> CancelRequestAsync(int requestId);
        
        /// <summary>
        /// Deletes an explosive approval request
        /// </summary>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Updates the blasting date and timing for an explosive approval request
        /// </summary>
        Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming);

        /// <summary>
        /// Retrieves explosive approval requests filtered by region
        /// </summary>
        Task<IEnumerable<ExplosiveApprovalRequest>> GetByRegionAsync(string region);
    }
}