using Application.DTOs.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Application.Utilities;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Domain.Entities.ProjectManagement;

namespace Application.Services.ProjectManagement
{
    public class ProjectSiteApplicationService : IProjectSiteService
    {
        private readonly IProjectSiteRepository _projectSiteRepository;
        private readonly ILogger<ProjectSiteApplicationService> _logger;

        public ProjectSiteApplicationService(
            IProjectSiteRepository projectSiteRepository,
            ILogger<ProjectSiteApplicationService> logger)
        {
            _projectSiteRepository = projectSiteRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProjectSite>> GetAllProjectSitesAsync()
        {
            try
            {
                return await _projectSiteRepository.GetAllAsync();
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
                return await _projectSiteRepository.GetByIdAsync(id);
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