using Application.Interfaces.ProjectManagement;
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

        public ProjectSiteRepository(ApplicationDbContext context, ILogger<ProjectSiteRepository> logger)
        {
            _context = context;
            _logger = logger;
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
                var site = await _context.ProjectSites.FindAsync(id);
                if (site == null)
                {
                    return false;
                }

                site.IsPatternApproved = true;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
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
                var site = await _context.ProjectSites.FindAsync(id);
                if (site == null)
                {
                    return false;
                }

                site.IsPatternApproved = false;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
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
                var site = await _context.ProjectSites.FindAsync(id);
                if (site == null)
                {
                    return false;
                }

                site.IsSimulationConfirmed = true;
                site.UpdatedAt = DateTime.UtcNow;
                var result = await _context.SaveChangesAsync();
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
                var site = await _context.ProjectSites.FindAsync(id);
                if (site == null)
                {
                    return false;
                }

                // Validate that all required steps are completed
                if (!site.IsPatternApproved || !site.IsSimulationConfirmed || !site.IsOperatorCompleted)
                {
                    throw new InvalidOperationException(
                        "Cannot complete site: Pattern approval, simulation confirmation, and operator completion are all required.");
                }

                site.IsCompleted = true;
                site.CompletedAt = DateTime.UtcNow;
                site.CompletedByUserId = completedByUserId;
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

        // Note: Explosive approval methods have been moved to ExplosiveApprovalRequestRepository
        // as part of the new ExplosiveApprovalRequest entity implementation
    }
}