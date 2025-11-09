using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IExplosiveApprovalRequestService
    {
        Task<ExplosiveApprovalRequest?> GetExplosiveApprovalRequestByIdAsync(int id);
        
        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByProjectSiteIdAsync(int projectSiteId);
        
        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByUserIdAsync(int userId);
        
        Task<IEnumerable<ExplosiveApprovalRequest>> GetPendingExplosiveApprovalRequestsAsync();
        
        Task<ExplosiveApprovalRequest> CreateExplosiveApprovalRequestAsync(
            int projectSiteId,
            int requestedByUserId,
            DateTime expectedUsageDate,
            string? comments = null,
            RequestPriority priority = RequestPriority.Normal,
            ExplosiveApprovalType approvalType = ExplosiveApprovalType.Standard,
            DateTime? blastingDate = null,
            string? blastTiming = null);
        
        Task<bool> UpdateExplosiveApprovalRequestAsync(ExplosiveApprovalRequest request);
        
        Task<bool> ApproveExplosiveApprovalRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null);
        
        Task<bool> RejectExplosiveApprovalRequestAsync(int requestId, int rejectedByUserId, string rejectionReason);
        
        Task<bool> CancelExplosiveApprovalRequestAsync(int requestId);
        
        Task<bool> DeleteExplosiveApprovalRequestAsync(int id);

        Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming);

        Task<bool> HasPendingExplosiveApprovalRequestAsync(int projectSiteId);

        Task<ExplosiveApprovalRequest?> GetLatestExplosiveApprovalRequestAsync(int projectSiteId);

        Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByRegionAsync(string region);
    }
}