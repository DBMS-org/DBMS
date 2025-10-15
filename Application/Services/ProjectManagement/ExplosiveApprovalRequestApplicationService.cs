using Application.Interfaces.Infrastructure.Repositories;
using Application.Interfaces.ProjectManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProjectManagement
{
    public class ExplosiveApprovalRequestApplicationService : IExplosiveApprovalRequestService
    {
        private readonly IExplosiveApprovalRequestRepository _explosiveApprovalRequestRepository;
        private readonly IProjectSiteRepository _projectSiteRepository;
        private readonly ILogger<ExplosiveApprovalRequestApplicationService> _logger;

        public ExplosiveApprovalRequestApplicationService(
            IExplosiveApprovalRequestRepository explosiveApprovalRequestRepository,
            IProjectSiteRepository projectSiteRepository,
            ILogger<ExplosiveApprovalRequestApplicationService> logger)
        {
            _explosiveApprovalRequestRepository = explosiveApprovalRequestRepository;
            _projectSiteRepository = projectSiteRepository;
            _logger = logger;
        }

        public async Task<ExplosiveApprovalRequest?> GetExplosiveApprovalRequestByIdAsync(int id)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval request {RequestId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByProjectSiteIdAsync(int projectSiteId)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.GetByProjectSiteIdAsync(projectSiteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for project site {ProjectSiteId}", projectSiteId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByUserIdAsync(int userId)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetPendingExplosiveApprovalRequestsAsync()
        {
            try
            {
                return await _explosiveApprovalRequestRepository.GetPendingRequestsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending explosive approval requests");
                throw;
            }
        }

        public async Task<ExplosiveApprovalRequest> CreateExplosiveApprovalRequestAsync(
            int projectSiteId,
            int requestedByUserId,
            DateTime expectedUsageDate,
            string? comments = null,
            RequestPriority priority = RequestPriority.Normal,
            ExplosiveApprovalType approvalType = ExplosiveApprovalType.Standard,
            DateTime? blastingDate = null,
            string? blastTiming = null)
        {
            try
            {
                // Validate that the project site exists
                var projectSiteExists = await _projectSiteRepository.ExistsAsync(projectSiteId);
                if (!projectSiteExists)
                {
                    throw new ArgumentException($"Project site with ID {projectSiteId} does not exist.");
                }

                // Check if there's already a pending request for this project site
                var existingRequests = await _explosiveApprovalRequestRepository.GetByProjectSiteIdAsync(projectSiteId);
                var hasPendingRequest = existingRequests.Any(r => r.Status == ExplosiveApprovalStatus.Pending);

                if (hasPendingRequest)
                {
                    throw new InvalidOperationException($"There is already a pending explosive approval request for project site {projectSiteId}.");
                }

                var request = new ExplosiveApprovalRequest
                {
                    ProjectSiteId = projectSiteId,
                    RequestedByUserId = requestedByUserId,
                    ExpectedUsageDate = expectedUsageDate,
                    Comments = comments,
                    Priority = priority,
                    ApprovalType = approvalType,
                    Status = ExplosiveApprovalStatus.Pending,
                    BlastingDate = blastingDate,
                    BlastTiming = blastTiming
                };

                return await _explosiveApprovalRequestRepository.CreateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive approval request for project site {ProjectSiteId}", projectSiteId);
                throw;
            }
        }

        public async Task<bool> UpdateExplosiveApprovalRequestAsync(ExplosiveApprovalRequest request)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.UpdateAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive approval request {RequestId}", request.Id);
                throw;
            }
        }

        public async Task<bool> ApproveExplosiveApprovalRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.ApproveRequestAsync(requestId, approvedByUserId, approvalComments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> RejectExplosiveApprovalRequestAsync(int requestId, int rejectedByUserId, string rejectionReason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rejectionReason))
                {
                    throw new ArgumentException("Rejection reason is required when rejecting an explosive approval request.");
                }

                return await _explosiveApprovalRequestRepository.RejectRequestAsync(requestId, rejectedByUserId, rejectionReason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> CancelExplosiveApprovalRequestAsync(int requestId)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.CancelRequestAsync(requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> DeleteExplosiveApprovalRequestAsync(int id)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive approval request {RequestId}", id);
                throw;
            }
        }

        public async Task<bool> HasPendingExplosiveApprovalRequestAsync(int projectSiteId)
        {
            try
            {
                var requests = await _explosiveApprovalRequestRepository.GetByProjectSiteIdAsync(projectSiteId);
                return requests.Any(r => r.Status == ExplosiveApprovalStatus.Pending);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for pending explosive approval requests for project site {ProjectSiteId}", projectSiteId);
                throw;
            }
        }

        public async Task<ExplosiveApprovalRequest?> GetLatestExplosiveApprovalRequestAsync(int projectSiteId)
        {
            try
            {
                var requests = await _explosiveApprovalRequestRepository.GetByProjectSiteIdAsync(projectSiteId);
                return requests.OrderByDescending(r => r.CreatedAt).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest explosive approval request for project site {ProjectSiteId}", projectSiteId);
                throw;
            }
        }

        public async Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming)
        {
            try
            {
                // Validate timing format if provided
                if (!string.IsNullOrWhiteSpace(blastTiming))
                {
                    if (!TimeSpan.TryParse(blastTiming, out _))
                    {
                        throw new ArgumentException("Invalid timing format. Expected format: HH:mm (e.g., 14:30)");
                    }
                }

                return await _explosiveApprovalRequestRepository.UpdateBlastingTimingAsync(
                    requestId, blastingDate, blastTiming);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blasting timing for request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetExplosiveApprovalRequestsByRegionAsync(string region)
        {
            try
            {
                return await _explosiveApprovalRequestRepository.GetByRegionAsync(region);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for region {Region}", region);
                throw;
            }
        }
    }
}