using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services
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

        public async Task<IEnumerable<ProjectSiteDto>> GetAllProjectSitesAsync()
        {
            try
            {
                var projectSites = await _projectSiteRepository.GetAllAsync();
                return projectSites.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all project sites");
                throw;
            }
        }

        public async Task<ProjectSiteDto?> GetProjectSiteByIdAsync(int id)
        {
            try
            {
                var projectSite = await _projectSiteRepository.GetByIdAsync(id);
                return projectSite != null ? MapToDto(projectSite) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project site {ProjectSiteId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectSiteDto>> GetProjectSitesByProjectIdAsync(int projectId)
        {
            try
            {
                // Check if project exists
                var projectExists = await _projectSiteRepository.ProjectExistsAsync(projectId);
                if (!projectExists)
                {
                    throw new InvalidOperationException($"Project with ID {projectId} not found");
                }

                var projectSites = await _projectSiteRepository.GetByProjectIdAsync(projectId);
                return projectSites.Select(MapToDto);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error getting project sites for project {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<ProjectSiteDto> CreateProjectSiteAsync(CreateProjectSiteRequest request)
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
                    Status = request.Status,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdProjectSite = await _projectSiteRepository.CreateAsync(projectSite);
                return MapToDto(createdProjectSite);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error creating project site");
                throw;
            }
        }

        public async Task<bool> UpdateProjectSiteAsync(int id, ProjectSiteDto request)
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
                projectSite.Coordinates = SerializeCoordinates(request.Coordinates);
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

        private static ProjectSiteDto MapToDto(ProjectSite projectSite)
        {
            return new ProjectSiteDto
            {
                Id = projectSite.Id,
                ProjectId = projectSite.ProjectId,
                Name = projectSite.Name,
                Location = projectSite.Location,
                Coordinates = ParseCoordinates(projectSite.Coordinates),
                Status = projectSite.Status,
                Description = projectSite.Description,
                CreatedAt = projectSite.CreatedAt,
                UpdatedAt = projectSite.UpdatedAt,
                IsPatternApproved = projectSite.IsPatternApproved,
                IsSimulationConfirmed = projectSite.IsSimulationConfirmed,
                IsOperatorCompleted = projectSite.IsOperatorCompleted
            };
        }

        private static CoordinatesDto? ParseCoordinates(string coordinates)
        {
            if (string.IsNullOrEmpty(coordinates))
                return null;

            try
            {
                // Try to parse as JSON first (for structured coordinates)
                if (coordinates.StartsWith("{") && coordinates.EndsWith("}"))
                {
                    return JsonSerializer.Deserialize<CoordinatesDto>(coordinates);
                }

                // Try to parse as comma-separated lat,lng
                var parts = coordinates.Split(',');
                if (parts.Length == 2 &&
                    double.TryParse(parts[0].Trim(), out var lat) &&
                    double.TryParse(parts[1].Trim(), out var lng))
                {
                    return new CoordinatesDto { Latitude = lat, Longitude = lng };
                }

                return null;
            }
            catch
            {
                return null;
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