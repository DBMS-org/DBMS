using Application.DTOs.ProjectManagement;
using Application.DTOs.Shared;
using Application.Exceptions;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.ProjectManagement;
using Application.Utilities;
using Domain.Entities.ProjectManagement;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Application.Services.ProjectManagement
{
    public class ProjectApplicationService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidationService _validationService;
        private readonly ILogger<ProjectApplicationService> _logger;

        public ProjectApplicationService(
            IProjectRepository projectRepository, 
            IValidationService validationService,
            ILogger<ProjectApplicationService> logger)
        {
            _projectRepository = projectRepository;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Project>>> GetAllProjectsAsync()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                return Result.Success(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all projects");
                return Result.Failure<IEnumerable<Project>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<Project>> GetProjectByIdAsync(int id)
        {
            try
            {
                var project = await _projectRepository.GetByIdWithDetailsAsync(id);
                if (project == null)
                {
                    return Result.Failure<Project>(ErrorCodes.Messages.ProjectNotFound(id));
                }
                return Result.Success(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project {ProjectId}", id);
                return Result.Failure<Project>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<Project>> CreateProjectAsync(CreateProjectRequest request)
        {
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    return Result.Failure<Project>(validationResult.Errors);
                }

                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _projectRepository.UserExistsAsync(request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        return Result.Failure<Project>(ErrorCodes.Messages.UserNotFound(request.AssignedUserId.Value));
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
                    Status = SafeDataConverter.ParseEnumWithDefault<ProjectStatus>(request.Status, ProjectStatus.Planned, "Status"),
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AssignedUserId = request.AssignedUserId
                };

                var createdProject = await _projectRepository.CreateAsync(project);
                var projectWithDetails = await _projectRepository.GetByIdWithDetailsAsync(createdProject.Id);
                return Result.Success(projectWithDetails ?? createdProject);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for project creation {ProjectName}: {Errors}", 
                    request.Name, string.Join(", ", ex.ValidationErrors));
                return Result.Failure<Project>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error creating project {ProjectName}", request.Name);
                return Result.Failure<Project>("Database error occurred while creating project");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating project {ProjectName}", request.Name);
                return Result.Failure<Project>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateProjectAsync(int id, UpdateProjectRequest request)
        {
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    return Result.Failure(validationResult.Errors);
                }

                var project = await _projectRepository.GetByIdAsync(id);
                if (project == null)
                {
                    return Result.Failure(ErrorCodes.Messages.ProjectNotFound(id));
                }

                // Validate assigned user exists if provided
                if (request.AssignedUserId.HasValue)
                {
                    var userExists = await _projectRepository.UserExistsAsync(request.AssignedUserId.Value);
                    if (!userExists)
                    {
                        return Result.Failure(ErrorCodes.Messages.UserNotFound(request.AssignedUserId.Value));
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
                project.Status = SafeDataConverter.ParseEnumWithDefault<ProjectStatus>(request.Status, project.Status, "Status");
                project.Description = request.Description;
                project.StartDate = request.StartDate;
                project.EndDate = request.EndDate;
                project.AssignedUserId = request.AssignedUserId;
                project.UpdatedAt = DateTime.UtcNow;

                var updateResult = await _projectRepository.UpdateAsync(project);
                return updateResult ? Result.Success() : Result.Failure(ErrorCodes.Messages.InternalError);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for project update {ProjectId}: {Errors}", 
                    id, string.Join(", ", ex.ValidationErrors));
                return Result.Failure(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error updating project {ProjectId}", id);
                return Result.Failure("Database error occurred while updating project");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating project {ProjectId}", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteProjectAsync(int id)
        {
            try
            {
                var deleteResult = await _projectRepository.DeleteAsync(id);
                return deleteResult ? Result.Success() : Result.Failure(ErrorCodes.Messages.ProjectNotFound(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting project {ProjectId}", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ProjectSite>>> GetProjectSitesAsync(int projectId)
        {
            try
            {
                var projectSites = await _projectRepository.GetProjectSitesAsync(projectId);
                return Result.Success(projectSites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project sites for project {ProjectId}", projectId);
                return Result.Failure<IEnumerable<ProjectSite>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<Project>>> SearchProjectsAsync(string? name = null, string? region = null, string? status = null)
        {
            try
            {
                var projects = await _projectRepository.SearchAsync(name, region, status);
                return Result.Success(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching projects with name={Name}, region={Region}, status={Status}", name, region, status);
                return Result.Failure<IEnumerable<Project>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<Project>> GetProjectByOperatorAsync(int operatorId)
        {
            try
            {
                var project = await _projectRepository.GetByOperatorIdAsync(operatorId);
                if (project == null)
                {
                    return Result.Failure<Project>(ErrorCodes.Messages.ProjectNotFound(operatorId));
                }
                return Result.Success(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting project for operator {OperatorId}", operatorId);
                return Result.Failure<Project>(ErrorCodes.Messages.InternalError);
            }
        }
    }
}