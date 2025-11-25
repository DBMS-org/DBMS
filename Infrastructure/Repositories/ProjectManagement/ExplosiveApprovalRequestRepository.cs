using Application.Interfaces;
using Application.Interfaces.Infrastructure.Repositories;
using Application.Interfaces.UserManagement;
using Domain.Entities.Notifications;
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
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public ExplosiveApprovalRequestRepository(
            ApplicationDbContext context,
            ILogger<ExplosiveApprovalRequestRepository> logger,
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
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

        public async Task<IEnumerable<ExplosiveApprovalRequest>> GetAllAsync()
        {
            try
            {
                return await _context.ExplosiveApprovalRequests
                    .Include(e => e.ProjectSite)
                        .ThenInclude(ps => ps.Project)
                            .ThenInclude(p => p.RegionNavigation)
                    .Include(e => e.RequestedByUser)
                    .Include(e => e.ProcessedByUser)
                    .OrderByDescending(e => e.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all explosive approval requests");
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

                // Create notifications for Store Managers in the same region
                try
                {
                    // Load related entities for notification
                    await _context.Entry(request)
                        .Reference(r => r.ProjectSite)
                        .Query()
                        .Include(ps => ps.Project)
                        .LoadAsync();

                    await _context.Entry(request)
                        .Reference(r => r.RequestedByUser)
                        .LoadAsync();

                    var region = request.ProjectSite?.Project?.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        var storeManagers = await _userRepository.GetByRoleAndRegionAsync("StoreManager", region);

                        if (storeManagers.Any())
                        {
                            var engineerName = request.RequestedByUser?.Name ?? "A blasting engineer";
                            var siteName = request.ProjectSite?.Name ?? "a site";
                            var projectName = request.ProjectSite?.Project?.Name ?? "a project";
                            var expectedDate = request.ExpectedUsageDate.ToString("MMM dd, yyyy");

                            // Determine priority based on request priority
                            var notificationPriority = request.Priority switch
                            {
                                Domain.Entities.ProjectManagement.RequestPriority.Critical => NotificationPriority.Critical,
                                Domain.Entities.ProjectManagement.RequestPriority.High => NotificationPriority.High,
                                _ => NotificationPriority.Normal
                            };

                            var notifications = storeManagers.Select(manager =>
                                Notification.Create(
                                    userId: manager.Id,
                                    type: NotificationType.ExplosiveRequestCreated,
                                    title: "New Explosive Approval Request",
                                    message: $"{engineerName} has submitted a new explosive approval request for {siteName} ({projectName}). Expected usage date: {expectedDate}.",
                                    priority: notificationPriority,
                                    relatedEntityType: "ExplosiveApprovalRequest",
                                    relatedEntityId: request.Id,
                                    actionUrl: $"/store-manager/requests/{request.Id}"
                                )
                            ).ToList();

                            await _notificationRepository.CreateBulkAsync(notifications);
                            _logger.LogInformation("Created {Count} notifications for Store Managers in {Region} for request {RequestId}",
                                notifications.Count, region, request.Id);
                        }
                    }
                }
                catch (Exception notifEx)
                {
                    // Log error but don't fail the creation if notification fails
                    _logger.LogError(notifEx, "Error creating notifications for new request {RequestId}", request.Id);
                }

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
                // Load request with related entities for notification
                var request = await _context.ExplosiveApprovalRequests
                    .Include(r => r.ProjectSite)
                        .ThenInclude(ps => ps.Project)
                    .Include(r => r.RequestedByUser)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

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

                if (result > 0)
                {
                    // Create notification for the blasting engineer
                    try
                    {
                        var approver = await _userRepository.GetByIdAsync(approvedByUserId);
                        var siteName = request.ProjectSite?.Name ?? "Unknown Site";
                        var blastingDate = request.BlastingDate?.ToString("MMM dd, yyyy") ?? "TBD";
                        var blastTiming = request.BlastTiming ?? "TBD";

                        var notification = Notification.Create(
                            userId: request.RequestedByUserId,
                            type: NotificationType.ExplosiveRequestApproved,
                            title: "Explosive Request Approved",
                            message: $"Your explosive approval request for {siteName} has been approved by {approver?.Name ?? "Store Manager"}. Blasting scheduled for {blastingDate} at {blastTiming}.",
                            priority: NotificationPriority.High,
                            relatedEntityType: "ExplosiveApprovalRequest",
                            relatedEntityId: requestId,
                            actionUrl: $"/blasting-engineer/site-dashboard/{request.ProjectSiteId}"
                        );

                        await _notificationRepository.CreateAsync(notification);
                        _logger.LogInformation("Created approval notification for user {UserId} for request {RequestId}",
                            request.RequestedByUserId, requestId);
                    }
                    catch (Exception notifEx)
                    {
                        // Log error but don't fail the approval if notification fails
                        _logger.LogError(notifEx, "Error creating notification for approved request {RequestId}", requestId);
                    }
                }

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
                // Load request with related entities for notification
                var request = await _context.ExplosiveApprovalRequests
                    .Include(r => r.ProjectSite)
                        .ThenInclude(ps => ps.Project)
                    .Include(r => r.RequestedByUser)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

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

                if (result > 0)
                {
                    // Create notification for the blasting engineer
                    try
                    {
                        var rejector = await _userRepository.GetByIdAsync(rejectedByUserId);
                        var siteName = request.ProjectSite?.Name ?? "Unknown Site";

                        var notification = Notification.Create(
                            userId: request.RequestedByUserId,
                            type: NotificationType.ExplosiveRequestRejected,
                            title: "Explosive Request Rejected",
                            message: $"Your explosive approval request for {siteName} was rejected by {rejector?.Name ?? "Store Manager"}. Reason: {rejectionReason}",
                            priority: NotificationPriority.High,
                            relatedEntityType: "ExplosiveApprovalRequest",
                            relatedEntityId: requestId,
                            actionUrl: $"/blasting-engineer/site-dashboard/{request.ProjectSiteId}"
                        );

                        await _notificationRepository.CreateAsync(notification);
                        _logger.LogInformation("Created rejection notification for user {UserId} for request {RequestId}",
                            request.RequestedByUserId, requestId);
                    }
                    catch (Exception notifEx)
                    {
                        // Log error but don't fail the rejection if notification fails
                        _logger.LogError(notifEx, "Error creating notification for rejected request {RequestId}", requestId);
                    }
                }

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
                // Load request with related entities for notification
                var request = await _context.ExplosiveApprovalRequests
                    .Include(r => r.ProjectSite)
                        .ThenInclude(ps => ps.Project)
                    .Include(r => r.RequestedByUser)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

                if (request == null)
                {
                    return false;
                }

                // Store previous values to check if timing was just added
                var hadBlastingDate = request.BlastingDate.HasValue;
                var hadBlastTiming = !string.IsNullOrWhiteSpace(request.BlastTiming);
                var wasReadyForApproval = hadBlastingDate && hadBlastTiming;

                request.BlastingDate = blastingDate;
                request.BlastTiming = blastTiming;
                request.UpdatedAt = DateTime.UtcNow;

                var result = await _context.SaveChangesAsync();

                // Check if timing info was just completed (ready for approval)
                var isNowReadyForApproval = blastingDate.HasValue && !string.IsNullOrWhiteSpace(blastTiming);
                var timingJustCompleted = !wasReadyForApproval && isNowReadyForApproval;

                // Send notification to Store Managers if timing was just completed
                if (result > 0 && timingJustCompleted && request.Status == ExplosiveApprovalStatus.Pending)
                {
                    try
                    {
                        var region = request.ProjectSite?.Project?.Region;
                        if (!string.IsNullOrEmpty(region))
                        {
                            var storeManagers = await _userRepository.GetByRoleAndRegionAsync("StoreManager", region);

                            if (storeManagers.Any())
                            {
                                var engineerName = request.RequestedByUser?.Name ?? "A blasting engineer";
                                var siteName = request.ProjectSite?.Name ?? "a site";
                                var projectName = request.ProjectSite?.Project?.Name ?? "a project";
                                var blastingDateStr = blastingDate?.ToString("MMM dd, yyyy") ?? "TBD";
                                var blastTimingStr = blastTiming ?? "TBD";

                                // Determine priority based on request priority
                                var notificationPriority = request.Priority switch
                                {
                                    Domain.Entities.ProjectManagement.RequestPriority.Critical => NotificationPriority.Critical,
                                    Domain.Entities.ProjectManagement.RequestPriority.High => NotificationPriority.High,
                                    _ => NotificationPriority.Normal
                                };

                                var notifications = storeManagers.Select(manager =>
                                    Notification.Create(
                                        userId: manager.Id,
                                        type: NotificationType.ExplosiveRequestUpdated,
                                        title: "Explosive Request Ready for Approval",
                                        message: $"{engineerName} has updated the blasting schedule for {siteName} ({projectName}). Scheduled for {blastingDateStr} at {blastTimingStr}. Request is now ready for approval.",
                                        priority: notificationPriority,
                                        relatedEntityType: "ExplosiveApprovalRequest",
                                        relatedEntityId: requestId,
                                        actionUrl: $"/store-manager/blasting-engineer-requests"
                                    )
                                ).ToList();

                                await _notificationRepository.CreateBulkAsync(notifications);
                                _logger.LogInformation("Created {Count} timing update notifications for Store Managers in {Region} for request {RequestId}",
                                    notifications.Count, region, requestId);
                            }
                        }
                    }
                    catch (Exception notifEx)
                    {
                        // Log error but don't fail the update if notification fails
                        _logger.LogError(notifEx, "Error creating notifications for updated timing for request {RequestId}", requestId);
                    }
                }

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
                    .Include(r => r.RequestedByUser)
                    .Include(r => r.ProcessedByUser)
                    .Where(r => r.ProjectSite != null &&
                                r.ProjectSite.Project != null &&
                                r.ProjectSite.Project.Region == region)
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