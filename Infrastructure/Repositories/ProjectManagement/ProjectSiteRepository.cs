using Application.Interfaces;
using Application.Interfaces.ProjectManagement;
using Application.Interfaces.UserManagement;
using Domain.Entities.Notifications;
using Domain.Entities.ProjectManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.ProjectManagement
{
    public class ProjectSiteRepository : IProjectSiteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectSiteRepository> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public ProjectSiteRepository(
            ApplicationDbContext context,
            ILogger<ProjectSiteRepository> logger,
            INotificationRepository notificationRepository,
            IUserRepository userRepository)
        {
            _context = context;
            _logger = logger;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ProjectSite>> GetAllAsync()
        {
            try
            {
                return await _context.ProjectSites.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all project sites from database");
                throw;
            }
        }

        public async Task<ProjectSite?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.ProjectSites.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project site {ProjectSiteId} from database", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectSite>> GetByProjectIdAsync(int projectId)
        {
            try
            {
                return await _context.ProjectSites
                    .Where(ps => ps.ProjectId == projectId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project sites for project {ProjectId} from database", projectId);
                throw;
            }
        }

        public async Task<ProjectSite> CreateAsync(ProjectSite projectSite)
        {
            try
            {
                _context.ProjectSites.Add(projectSite);
                await _context.SaveChangesAsync();
                return projectSite;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project site in database");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ProjectSite projectSite)
        {
            try
            {
                _context.ProjectSites.Update(projectSite);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project site {ProjectSiteId} in database", projectSite.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var projectSite = await _context.ProjectSites.FindAsync(id);
                if (projectSite == null)
                {
                    return false;
                }

                _context.ProjectSites.Remove(projectSite);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project site {ProjectSiteId} from database", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.ProjectSites.AnyAsync(ps => ps.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if project site {ProjectSiteId} exists in database", id);
                throw;
            }
        }

        public async Task<bool> ProjectExistsAsync(int projectId)
        {
            try
            {
                return await _context.Projects.AnyAsync(p => p.Id == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if project {ProjectId} exists in database", projectId);
                throw;
            }
        }

        public async Task<bool> ApprovePatternAsync(int id)
        {
            try
            {
                // Load site with related entities for notification
                var site = await _context.ProjectSites
                    .Include(s => s.Project)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (site == null)
                {
                    return false;
                }

                site.IsPatternApproved = true;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    // Notify all operators about the pattern approval
                    var operators = await _userRepository.GetByRoleAndRegionAsync("Operator");
                    foreach (var operatorUser in operators)
                    {
                        var notification = Notification.Create(
                            userId: operatorUser.Id,
                            type: NotificationType.PatternApproved,
                            title: "Pattern Approved",
                            message: $"Drilling pattern approved for {site.Project?.Name ?? "project"} - {site.Name}",
                            priority: NotificationPriority.High,
                            relatedEntityType: "ProjectSite",
                            relatedEntityId: site.Id,
                            actionUrl: $"/operator/pattern-view?projectId={site.ProjectId}&siteId={site.Id}"
                        );
                        await _notificationRepository.CreateAsync(notification);
                    }
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving pattern for project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> RevokePatternAsync(int id)
        {
            try
            {
                // Load site with related entities for notification
                var site = await _context.ProjectSites
                    .Include(s => s.Project)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (site == null)
                {
                    return false;
                }

                site.IsPatternApproved = false;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    // Notify all operators about the pattern approval revocation
                    var operators = await _userRepository.GetByRoleAndRegionAsync("Operator");
                    foreach (var operatorUser in operators)
                    {
                        var notification = Notification.Create(
                            userId: operatorUser.Id,
                            type: NotificationType.PatternApprovalRevoked,
                            title: "Pattern Approval Revoked",
                            message: $"Drilling pattern approval revoked for {site.Project?.Name ?? "project"} - {site.Name}",
                            priority: NotificationPriority.High,
                            relatedEntityType: "ProjectSite",
                            relatedEntityId: site.Id,
                            actionUrl: $"/operator/pattern-view?projectId={site.ProjectId}&siteId={site.Id}"
                        );
                        await _notificationRepository.CreateAsync(notification);
                    }
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking pattern for project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> ConfirmSimulationAsync(int id)
        {
            try
            {
                // Load site with related entities for notification
                var site = await _context.ProjectSites
                    .Include(s => s.Project)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (site == null)
                {
                    return false;
                }

                site.IsSimulationConfirmed = true;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();

                // Create notifications for Admins
                if (result > 0)
                {
                    try
                    {
                        var admins = await _userRepository.GetByRoleAndRegionAsync("Admin", null);

                        if (admins.Any())
                        {
                            var siteName = site.Name ?? "Unknown Site";
                            var projectName = site.Project?.Name ?? "Unknown Project";

                            var notifications = admins.Select(admin =>
                                Notification.Create(
                                    userId: admin.Id,
                                    type: NotificationType.BlastSimulationConfirmed,
                                    title: "Blast Simulation Confirmed",
                                    message: $"A blasting engineer has confirmed the simulation for {siteName} ({projectName}) and is requesting final approval.",
                                    priority: NotificationPriority.High,
                                    relatedEntityType: "ProjectSite",
                                    relatedEntityId: site.Id,
                                    actionUrl: null
                                )
                            ).ToList();

                            await _notificationRepository.CreateBulkAsync(notifications);
                            _logger.LogInformation("Created {Count} notifications for Admins for simulation confirmation of site {SiteId}",
                                notifications.Count, site.Id);
                        }
                    }
                    catch (Exception notifEx)
                    {
                        // Log error but don't fail the confirmation if notification fails
                        _logger.LogError(notifEx, "Error creating notifications for simulation confirmation of site {SiteId}", id);
                    }
                }

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming simulation for project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> RevokeSimulationAsync(int id)
        {
            try
            {
                var site = await _context.ProjectSites.FindAsync(id);
                if (site == null)
                {
                    return false;
                }

                site.IsSimulationConfirmed = false;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking simulation for project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> CompleteSiteAsync(int id, int completedByUserId)
        {
            try
            {
                var site = await _context.ProjectSites
                    .Include(s => s.ExplosiveApprovalRequests)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (site == null)
                {
                    return false;
                }

                // Check if already completed
                if (site.IsOperatorCompleted)
                {
                    throw new InvalidOperationException(
                        "Cannot complete site: Site is already marked as completed.");
                }

                // Validate that pattern is approved
                if (!site.IsPatternApproved)
                {
                    throw new InvalidOperationException(
                        "Cannot complete site: Pattern approval is required.");
                }

                // Validate that explosive approval request exists and is approved
                var approvedRequest = site.ExplosiveApprovalRequests
                    .Where(r => r.IsActive)
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefault();

                if (approvedRequest == null || approvedRequest.Status != ExplosiveApprovalStatus.Approved)
                {
                    throw new InvalidOperationException(
                        "Cannot complete site: Explosive approval is required.");
                }

                // Mark the site as completed by operator
                site.IsOperatorCompleted = true;
                site.UpdatedAt = DateTime.UtcNow;

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing project site {ProjectSiteId}", id);
                throw;
            }
        }

        /// <summary>
        /// UC-9: Checks if a site name already exists under a specific project.
        /// </summary>
        public async Task<bool> SiteNameExistsInProjectAsync(int projectId, string siteName)
        {
            try
            {
                return await _context.ProjectSites
                    .AnyAsync(ps => ps.ProjectId == projectId &&
                                   ps.Name.ToLower() == siteName.ToLower() &&
                                   ps.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if site name '{SiteName}' exists in project {ProjectId}", siteName, projectId);
                throw;
            }
        }

        // Note: Explosive approval methods have been moved to ExplosiveApprovalRequestRepository
        // as part of the new ExplosiveApprovalRequest entity implementation
    }
}