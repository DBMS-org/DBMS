using Application.DTOs.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Application.Utilities;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Domain.Entities.ProjectManagement;
using Application.Interfaces.Infrastructure;
using System.Linq;

namespace Application.Services.ProjectManagement
{
    public class ProjectSiteApplicationService : IProjectSiteService
    {
        private readonly IProjectSiteRepository _projectSiteRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<ProjectSiteApplicationService> _logger;

        public ProjectSiteApplicationService(
            IProjectSiteRepository projectSiteRepository,
            IProjectRepository projectRepository,
            IUserContext userContext,
            ILogger<ProjectSiteApplicationService> logger)
        {
            _projectSiteRepository = projectSiteRepository;
            _projectRepository = projectRepository;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<IEnumerable<ProjectSite>> GetAllProjectSitesAsync()
        {
            try
            {
                var sites = await _projectSiteRepository.GetAllAsync();

                if (_userContext.IsInRole("BlastingEngineer"))
                {
                    var region = _userContext.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        // Fetch projects in the same region
                        var projects = await _projectRepository.SearchAsync(region: region);
                        var projectIds = projects.Select(p => p.Id).ToHashSet();
                        sites = sites.Where(s => projectIds.Contains(s.ProjectId));
                    }
                    else
                    {
                        sites = Enumerable.Empty<ProjectSite>();
                    }
                }

                return sites;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all project sites");
                throw;
            }
        }

        public async Task<ProjectSite?> GetProjectSiteByIdAsync(int id)
        {
            try
            {
                var projectSite = await _projectSiteRepository.GetByIdAsync(id);
                if (projectSite == null)
                {
                    return null;
                }

                // Apply region filtering for BlastingEngineer
                if (_userContext.IsInRole("BlastingEngineer"))
                {
                    var region = _userContext.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        // Check if the project site belongs to a project in user's region
                        var project = await _projectRepository.GetByIdAsync(projectSite.ProjectId);
                        if (project == null || project.Region != region)
                        {
                            return null; // Site not in user's region
                        }
                    }
                    else
                    {
                        return null; // No region claim
                    }
                }

                return projectSite;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectSite>> GetProjectSitesByProjectIdAsync(int projectId)
        {
            try
            {
                // Check if project exists
                var projectExists = await _projectSiteRepository.ProjectExistsAsync(projectId);
                if (!projectExists)
                {
                    throw new InvalidOperationException($"Project with ID {projectId} not found");
                }

                // Apply region filtering for BlastingEngineer
                if (_userContext.IsInRole("BlastingEngineer"))
                {
                    var region = _userContext.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        // Check if the project belongs to user's region
                        var project = await _projectRepository.GetByIdAsync(projectId);
                        if (project == null || project.Region != region)
                        {
                            return Enumerable.Empty<ProjectSite>(); // Project not in user's region
                        }
                    }
                    else
                    {
                        return Enumerable.Empty<ProjectSite>(); // No region claim
                    }
                }

                return await _projectSiteRepository.GetByProjectIdAsync(projectId);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error getting project sites for project {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<ProjectSite> CreateProjectSiteAsync(CreateProjectSiteRequest request)
        {
            try
            {
                // Validate that the project exists
                var projectExists = await _projectSiteRepository.ProjectExistsAsync(request.ProjectId);
                if (!projectExists)
                {
                    throw new InvalidOperationException($"Project with ID {request.ProjectId} not found");
                }

                var projectSite = new ProjectSite
                {
                    ProjectId = request.ProjectId,
                    Name = request.Name,
                    Location = request.Location,
                    Coordinates = SerializeCoordinates(request.Coordinates),
                    Status = SafeDataConverter.ParseEnumWithDefault<ProjectSiteStatus>(request.Status, ProjectSiteStatus.Planned, "Status"),
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return await _projectSiteRepository.CreateAsync(projectSite);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error creating project site");
                throw;
            }
        }

        public async Task<bool> UpdateProjectSiteAsync(int id, ProjectSite request)
        {
            try
            {
                var projectSite = await _projectSiteRepository.GetByIdAsync(id);
                if (projectSite == null)
                {
                    return false;
                }

                // Validate that the project exists
                var projectExists = await _projectSiteRepository.ProjectExistsAsync(request.ProjectId);
                if (!projectExists)
                {
                    throw new InvalidOperationException($"Project with ID {request.ProjectId} not found");
                }

                // Update project site properties
                projectSite.ProjectId = request.ProjectId;
                projectSite.Name = request.Name;
                projectSite.Location = request.Location;
                projectSite.Coordinates = request.Coordinates;
                projectSite.Status = request.Status;
                projectSite.Description = request.Description;
                projectSite.UpdatedAt = DateTime.UtcNow;

                return await _projectSiteRepository.UpdateAsync(projectSite);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error updating project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteProjectSiteAsync(int id)
        {
            try
            {
                return await _projectSiteRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<bool> ApprovePatternAsync(int id)
        {
            try
            {
                return await _projectSiteRepository.ApprovePatternAsync(id);
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
                return await _projectSiteRepository.RevokePatternAsync(id);
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
                return await _projectSiteRepository.ConfirmSimulationAsync(id);
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
                return await _projectSiteRepository.RevokeSimulationAsync(id);
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
                return await _projectSiteRepository.CompleteSiteAsync(id, completedByUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing project site {ProjectSiteId}", id);
                throw;
            }
        }

        // Note: Explosive approval methods have been moved to ExplosiveApprovalRequestApplicationService
        // as part of the new ExplosiveApprovalRequest entity implementation

        private static string SerializeCoordinates(CoordinatesDto? coordinates)
        {
            if (coordinates == null)
                return string.Empty;

            try
            {
                return JsonSerializer.Serialize(coordinates);
            }
            catch
            {
                return $"{coordinates.Latitude},{coordinates.Longitude}";
            }
        }
    }
}