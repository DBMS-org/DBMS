using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services
{
    public class ProjectApplicationService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ProjectApplicationService> _logger;

        public ProjectApplicationService(IProjectRepository projectRepository, ILogger<ProjectApplicationService> logger)
        {
            _projectRepository = projectRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                return projects.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all projects");
                throw;
            }
        }

        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            try
            {
                var project = await _projectRepository.GetByIdWithDetailsAsync(id);
                return project != null ? MapToDto(project) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project {ProjectId}", id);
                throw;
            }
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request)
        {
            try
            {
                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _projectRepository.UserExistsAsync(request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        throw new ArgumentException($"User with ID {request.AssignedUserId.Value} not found");
                    }
                }

                // Business rule: If operator is assigned to different project, unassign first
                if (request.AssignedUserId.HasValue)
                {
                    var existingProject = await _projectRepository.GetByOperatorIdAsync(request.AssignedUserId.Value);
                    if (existingProject != null)
                    {
                        existingProject.AssignedUserId = null;
                        await _projectRepository.UpdateAsync(existingProject);
                    }
                }

                var project = new Project
                {
                    Name = request.Name,
                    Region = request.Region,
                    Status = request.Status,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AssignedUserId = request.AssignedUserId
                };

                var createdProject = await _projectRepository.CreateAsync(project);
                var detailedProject = await _projectRepository.GetByIdWithDetailsAsync(createdProject.Id);
                
                return MapToDto(detailedProject!);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error creating project");
                throw;
            }
        }

        public async Task<bool> UpdateProjectAsync(int id, UpdateProjectRequest request)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(id);
                if (project == null)
                {
                    return false;
                }

                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _projectRepository.UserExistsAsync(request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        throw new ArgumentException($"User with ID {request.AssignedUserId.Value} not found");
                    }
                }

                // Business rule: If operator is being reassigned, unassign from other projects
                if (request.AssignedUserId.HasValue && request.AssignedUserId != project.AssignedUserId)
                {
                    var existingProject = await _projectRepository.GetByOperatorIdAsync(request.AssignedUserId.Value);
                    if (existingProject != null && existingProject.Id != id)
                    {
                        existingProject.AssignedUserId = null;
                        await _projectRepository.UpdateAsync(existingProject);
                    }
                }

                // Update project properties
                project.Name = request.Name;
                project.Region = request.Region;
                project.Status = request.Status;
                project.Description = request.Description;
                project.StartDate = request.StartDate;
                project.EndDate = request.EndDate;
                project.AssignedUserId = request.AssignedUserId;
                project.UpdatedAt = DateTime.UtcNow;

                return await _projectRepository.UpdateAsync(project);
            }
            catch (Exception ex) when (!(ex is ArgumentException))
            {
                _logger.LogError(ex, "Error updating project {ProjectId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            try
            {
                return await _projectRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectSiteDto>> GetProjectSitesAsync(int projectId)
        {
            try
            {
                var projectSites = await _projectRepository.GetProjectSitesAsync(projectId);
                return projectSites.Select(MapProjectSiteToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project sites for project {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<IEnumerable<ProjectDto>> SearchProjectsAsync(string? name = null, string? region = null, string? status = null)
        {
            try
            {
                var projects = await _projectRepository.SearchAsync(name, region, status);
                return projects.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching projects with name={Name}, region={Region}, status={Status}", name, region, status);
                throw;
            }
        }

        public async Task<ProjectDto?> GetProjectByOperatorAsync(int operatorId)
        {
            try
            {
                var project = await _projectRepository.GetByOperatorIdAsync(operatorId);
                return project != null ? MapToDto(project) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project for operator {OperatorId}", operatorId);
                throw;
            }
        }

        private static ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Region = project.Region,
                Status = project.Status,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                AssignedUserId = project.AssignedUserId,
                AssignedUserName = project.AssignedUser?.Name,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                ProjectSites = project.ProjectSites?.Select(MapProjectSiteToDto).ToList() ?? new List<ProjectSiteDto>()
            };
        }

        private static ProjectSiteDto MapProjectSiteToDto(ProjectSite projectSite)
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
                return JsonSerializer.Deserialize<CoordinatesDto>(coordinates);
            }
            catch
            {
                return null;
            }
        }
    }
}