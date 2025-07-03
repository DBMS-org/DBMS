using Application.Interfaces.ProjectManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.ProjectManagement
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ApplicationDbContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.ProjectSites)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all projects from database");
                throw;
            }
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Projects.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project {ProjectId} from database", id);
                throw;
            }
        }

        public async Task<Project?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.ProjectSites)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project {ProjectId} with details from database", id);
                throw;
            }
        }

        public async Task<Project> CreateAsync(Project project)
        {
            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project in database");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            try
            {
                _context.Projects.Update(project);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project {ProjectId} in database", project.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    return false;
                }

                _context.Projects.Remove(project);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId} from database", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectSite>> GetProjectSitesAsync(int projectId)
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

        public async Task<IEnumerable<Project>> SearchAsync(string? name = null, string? region = null, string? status = null)
        {
            try
            {
                var query = _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.ProjectSites)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(region))
                {
                    query = query.Where(p => p.Region == region);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    if (Enum.TryParse<ProjectStatus>(status, true, out var statusEnum))
                    {
                        query = query.Where(p => p.Status == statusEnum);
                    }
                    else
                    {
                        _logger.LogWarning("Invalid status parameter {Status}", status);
                    }
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching projects in database with name={Name}, region={Region}, status={Status}", name, region, status);
                throw;
            }
        }

        public async Task<Project?> GetByOperatorIdAsync(int operatorId)
        {
            try
            {
                return await _context.Projects
                    .Include(p => p.AssignedUser)
                    .Include(p => p.ProjectSites)
                    .FirstOrDefaultAsync(p => p.AssignedUserId == operatorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project by operator {OperatorId} from database", operatorId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Projects.AnyAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if project {ProjectId} exists in database", id);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} exists in database", userId);
                throw;
            }
        }
    }
} 