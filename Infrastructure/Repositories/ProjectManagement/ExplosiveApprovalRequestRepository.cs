using Application.Interfaces.Infrastructure.Repositories;
using Domain.Entities.ProjectManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.ProjectManagement
{
    public class ExplosiveApprovalRequestRepository : IExplosiveApprovalRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExplosiveApprovalRequestRepository> _logger;

        public ExplosiveApprovalRequestRepository(
            ApplicationDbContext context,
            ILogger<ExplosiveApprovalRequestRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ExplosiveApprovalRequest?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(e => e.ProjectSite)
                    .Include(e => e.RequestedByUser)
                    .Include(e => e.ProcessedByUser)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval request {RequestId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetByProjectSiteIdAsync(int projectSiteId)
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(e => e.RequestedByUser)
                    .Include(e => e.ProcessedByUser)
                    .Where(e => e.ProjectSiteId == projectSiteId)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for project site {ProjectSiteId}", projectSiteId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(e => e.ProjectSite)
                    .Include(e => e.ProcessedByUser)
                    .Where(e => e.RequestedByUserId == userId)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetPendingRequestsAsync()
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(e => e.ProjectSite)
                    .Include(e => e.RequestedByUser)
                    .Where(e => e.Status == ExplosiveApprovalStatus.Pending)
                    .OrderBy(e => e.ExpectedUsageDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending explosive approval requests");
                throw;
            }
        }

        public async Task<ExplosiveApprovalRequest> CreateAsync(ExplosiveApprovalRequest request)
        {
            try
            {
                request.CreatedAt = DateTime.UtcNow;
                request.UpdatedAt = DateTime.UtcNow;
                
                _context.ExplosiveApprovalRequests.Add(request);
                await _context.SaveChangesAsync();
                
                return request;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive approval request for project site {ProjectSiteId}", request.ProjectSiteId);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ExplosiveApprovalRequest request)
        {
            try
            {
                request.UpdatedAt = DateTime.UtcNow;
                
                _context.ExplosiveApprovalRequests.Update(request);
                var result = await _context.SaveChangesAsync();
                
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive approval request {RequestId}", request.Id);
                throw;
            }
        }

        public async Task<bool> ApproveRequestAsync(int requestId, int approvedByUserId, string? approvalComments = null)
        {
            try
            {
                var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
                if (request == null || request.Status != ExplosiveApprovalStatus.Pending)
                {
                    return false;
                }

                // Validate that blasting date and timing are set before approval
                if (!request.BlastingDate.HasValue || string.IsNullOrWhiteSpace(request.BlastTiming))
                {
                    throw new InvalidOperationException(
                        "Cannot approve request: Blasting date and timing must be specified before approval.");
                }

                request.Status = ExplosiveApprovalStatus.Approved;
                request.ProcessedByUserId = approvedByUserId;
                request.ProcessedAt = DateTime.UtcNow;
                request.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(approvalComments))
                {
                    request.Comments = approvalComments;
                }

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> RejectRequestAsync(int requestId, int rejectedByUserId, string rejectionReason)
        {
            try
            {
                var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
                if (request == null || request.Status != ExplosiveApprovalStatus.Pending)
                {
                    return false;
                }

                request.Status = ExplosiveApprovalStatus.Rejected;
                request.ProcessedByUserId = rejectedByUserId;
                request.ProcessedAt = DateTime.UtcNow;
                request.RejectionReason = rejectionReason;
                request.UpdatedAt = DateTime.UtcNow;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> CancelRequestAsync(int requestId)
        {
            try
            {
                var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
                if (request == null || request.Status != ExplosiveApprovalStatus.Pending)
                {
                    return false;
                }

                request.Status = ExplosiveApprovalStatus.Cancelled;
                request.UpdatedAt = DateTime.UtcNow;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling explosive approval request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var request = await _context.ExplosiveApprovalRequests.FindAsync(id);
                if (request == null)
                {
                    return false;
                }

                _context.ExplosiveApprovalRequests.Remove(request);
                var result = await _context.SaveChangesAsync();
                
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive approval request {RequestId}", id);
                throw;
            }
        }

        public async Task<bool> UpdateBlastingTimingAsync(int requestId, DateTime? blastingDate, string? blastTiming)
        {
            try
            {
                var request = await _context.ExplosiveApprovalRequests.FindAsync(requestId);
                if (request == null)
                {
                    return false;
                }

                request.BlastingDate = blastingDate;
                request.BlastTiming = blastTiming;
                request.UpdatedAt = DateTime.UtcNow;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blasting timing for request {RequestId}", requestId);
                throw;
            }
        }

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetByRegionAsync(string region)
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(r => r.ProjectSite)
                        .ThenInclude(ps => ps.Project)
                            .ThenInclude(p => p.RegionNavigation)
                    .Include(r => r.RequestedByUser)
                    .Include(r => r.ProcessedByUser)
                    .Where(r => r.ProjectSite.Project.Region == region)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosive approval requests for region {Region}", region);
                throw;
            }
        }
    }
}