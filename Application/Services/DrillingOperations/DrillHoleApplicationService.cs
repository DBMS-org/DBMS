using Domain.Entities.DrillingOperations;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.Extensions.Logging;
using Application.DTOs.Shared;
using Application.Utilities;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.ProjectManagement;
using System.Linq;

namespace Application.Services.DrillingOperations
{
    public class DrillHoleApplicationService : IDrillHoleService
    {
        private readonly IDrillHoleRepository _drillHoleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<DrillHoleApplicationService> _logger;

        public DrillHoleApplicationService(
            IDrillHoleRepository drillHoleRepository,
            IProjectRepository projectRepository,
            IUserContext userContext,
            ILogger<DrillHoleApplicationService> logger)
        {
            _drillHoleRepository = drillHoleRepository;
            _projectRepository = projectRepository;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DrillHole>>> GetAllDrillHolesAsync()
        {
            try
            {
                var drillHoles = await _drillHoleRepository.GetAllAsync();

                // If the user is a blasting engineer, filter by their region
                if (_userContext.IsInRole("BlastingEngineer"))
                {
                    var region = _userContext.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        // Get projects for that region
                        var projects = await _projectRepository.SearchAsync(region: region);
                        var projectIds = projects.Select(p => p.Id).ToHashSet();

                        drillHoles = drillHoles.Where(h => projectIds.Contains(h.ProjectId));
                    }
                    else
                    {
                        // No region claim, return empty set
                        drillHoles = Enumerable.Empty<DrillHole>();
                    }
                }

                return Result.Success(drillHoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all drill holes");
                return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<DrillHole>> GetDrillHoleByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Result.Failure<DrillHole>(ErrorCodes.Messages.ArgumentNull);
                }

                var drillHole = await _drillHoleRepository.GetByIdAsync(id);
                if (drillHole == null)
                {
                    return Result.Failure<DrillHole>(ErrorCodes.Messages.DrillHoleNotFound(id));
                }

                return Result.Success(drillHole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill hole {DrillHoleId}", id);
                return Result.Failure<DrillHole>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<DrillHole>> CreateDrillHoleAsync(DrillHole drillHole)
        {
            try
            {
                if (drillHole == null)
                {
                    return Result.Failure<DrillHole>(ErrorCodes.Messages.ArgumentNull);
                }

                // Check if drill hole already exists
                var existingDrillHole = await _drillHoleRepository.GetByIdAsync(drillHole.Id);
                if (existingDrillHole != null)
                {
                    return Result.Failure<DrillHole>(ErrorCodes.Messages.DrillHoleAlreadyExists(drillHole.Id));
                }

                var createdDrillHole = await _drillHoleRepository.AddAsync(drillHole);
                return Result.Success(createdDrillHole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill hole {DrillHoleId}", drillHole?.Id);
                return Result.Failure<DrillHole>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateDrillHoleAsync(DrillHole drillHole)
        {
            try
            {
                if (drillHole == null)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                var existingDrillHole = await _drillHoleRepository.GetByIdAsync(drillHole.Id);
                if (existingDrillHole == null)
                {
                    return Result.Failure(ErrorCodes.Messages.DrillHoleNotFound(drillHole.Id));
                }

                await _drillHoleRepository.UpdateAsync(drillHole);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill hole {DrillHoleId}", drillHole?.Id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteDrillHoleAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                var existingDrillHole = await _drillHoleRepository.GetByIdAsync(id);
                if (existingDrillHole == null)
                {
                    return Result.Failure(ErrorCodes.Messages.DrillHoleNotFound(id));
                }

                await _drillHoleRepository.DeleteAsync(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill hole {DrillHoleId}", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<DrillHole>>> GetDrillHolesByProjectIdAsync(int projectId)
        {
            try
            {
                var drillHoles = await _drillHoleRepository.GetByProjectIdAsync(projectId);
                return Result.Success(drillHoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill holes for project {ProjectId}", projectId);
                return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<DrillHole>>> GetDrillHolesBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                var drillHoles = await _drillHoleRepository.GetBySiteIdAsync(projectId, siteId);
                return Result.Success(drillHoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill holes for project {ProjectId} and site {SiteId}", projectId, siteId);
                return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteDrillHolesByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Use resource manager for timeout handling
                await ResourceManager.ExecuteWithTimeoutAsync(
                    async (ct) => await _drillHoleRepository.DeleteByProjectIdAsync(projectId),
                    $"DeleteDrillHolesByProject_{projectId}",
                    timeoutMinutes: 10, // Bulk delete timeout
                    cancellationToken: cancellationToken);
                
                return Result.Success();
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout deleting drill holes for project {ProjectId}", projectId);
                return Result.Failure("Operation timed out while deleting drill holes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill holes for project {ProjectId}", projectId);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteDrillHolesBySiteIdAsync(int projectId, int siteId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Use resource manager for timeout handling
                await ResourceManager.ExecuteWithTimeoutAsync(
                    async (ct) => await _drillHoleRepository.DeleteBySiteIdAsync(projectId, siteId),
                    $"DeleteDrillHolesBySite_{projectId}_{siteId}",
                    timeoutMinutes: 10, // Bulk delete timeout
                    cancellationToken: cancellationToken);
                
                return Result.Success();
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout deleting drill holes for project {ProjectId} and site {SiteId}", projectId, siteId);
                return Result.Failure("Operation timed out while deleting drill holes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill holes for project {ProjectId} and site {SiteId}", projectId, siteId);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetDrillHoleCountAsync()
        {
            try
            {
                var count = await _drillHoleRepository.GetCountAsync();
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill hole count");
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetDrillHoleCountByProjectIdAsync(int projectId)
        {
            try
            {
                var count = await _drillHoleRepository.GetCountByProjectIdAsync(projectId);
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill hole count for project {ProjectId}", projectId);
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetDrillHoleCountBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                var count = await _drillHoleRepository.GetCountBySiteIdAsync(projectId, siteId);
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill hole count for project {ProjectId} and site {SiteId}", projectId, siteId);
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        // DTO-based methods for better frontend-backend mapping
        public async Task<Result<IEnumerable<DrillHoleDto>>> GetAllDrillHolesDtoAsync()
        {
            try
            {
                var result = await GetAllDrillHolesAsync();
                if (result.IsFailure)
                {
                    return Result.Failure<IEnumerable<DrillHoleDto>>(result.Error);
                }

                var drillHoleDtos = result.Value.Select(MapToDto);
                return Result.Success(drillHoleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all drill holes as DTOs");
                return Result.Failure<IEnumerable<DrillHoleDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<DrillHoleDto>> GetDrillHoleDtoByIdAsync(string id)
        {
            try
            {
                var result = await GetDrillHoleByIdAsync(id);
                if (result.IsFailure)
                {
                    return Result.Failure<DrillHoleDto>(result.Error);
                }

                var drillHoleDto = MapToDto(result.Value);
                return Result.Success(drillHoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill hole {DrillHoleId} as DTO", id);
                return Result.Failure<DrillHoleDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<DrillHoleDto>> CreateDrillHoleFromDtoAsync(CreateDrillHoleRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result.Failure<DrillHoleDto>(ErrorCodes.Messages.ArgumentNull);
                }

                // Map DTO to entity
                var drillHole = new DrillHole
                {
                    Id = Guid.NewGuid().ToString(), // Generate new ID
                    Name = request.Name,
                    Easting = request.Easting,
                    Northing = request.Northing,
                    Elevation = request.Elevation,
                    Length = request.Length,
                    Depth = request.Depth,
                    Azimuth = request.Azimuth,
                    Dip = request.Dip,
                    ActualDepth = request.ActualDepth,
                    Stemming = request.Stemming,
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await CreateDrillHoleAsync(drillHole);
                if (result.IsFailure)
                {
                    return Result.Failure<DrillHoleDto>(result.Error);
                }

                var drillHoleDto = MapToDto(result.Value);
                return Result.Success(drillHoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill hole from DTO");
                return Result.Failure<DrillHoleDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateDrillHoleFromDtoAsync(string id, UpdateDrillHoleRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                // Get existing drill hole
                var existingResult = await GetDrillHoleByIdAsync(id);
                if (existingResult.IsFailure)
                {
                    return Result.Failure(existingResult.Error);
                }

                var existingDrillHole = existingResult.Value;

                // Update properties
                existingDrillHole.Name = request.Name;
                existingDrillHole.Easting = request.Easting;
                existingDrillHole.Northing = request.Northing;
                existingDrillHole.Elevation = request.Elevation;
                existingDrillHole.Length = request.Length;
                existingDrillHole.Depth = request.Depth;
                existingDrillHole.Azimuth = request.Azimuth;
                existingDrillHole.Dip = request.Dip;
                existingDrillHole.ActualDepth = request.ActualDepth;
                existingDrillHole.Stemming = request.Stemming;
                existingDrillHole.UpdatedAt = DateTime.UtcNow;

                var result = await UpdateDrillHoleAsync(existingDrillHole);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill hole {DrillHoleId} from DTO", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<DrillHoleDto>>> GetDrillHolesDtoBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                var result = await GetDrillHolesBySiteIdAsync(projectId, siteId);
                if (result.IsFailure)
                {
                    return Result.Failure<IEnumerable<DrillHoleDto>>(result.Error);
                }

                var drillHoleDtos = result.Value.Select(MapToDto);
                return Result.Success(drillHoleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill holes for project {ProjectId} and site {SiteId} as DTOs", projectId, siteId);
                return Result.Failure<IEnumerable<DrillHoleDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        // Helper method to map entity to DTO
        private DrillHoleDto MapToDto(DrillHole drillHole)
        {
            return new DrillHoleDto
            {
                SerialNumber = drillHole.SerialNumber,
                Id = drillHole.Id,
                Name = drillHole.Name,
                Easting = drillHole.Easting,
                Northing = drillHole.Northing,
                Elevation = drillHole.Elevation,
                Length = drillHole.Length,
                Depth = drillHole.Depth,
                Azimuth = drillHole.Azimuth,
                Dip = drillHole.Dip,
                ActualDepth = drillHole.ActualDepth,
                Stemming = drillHole.Stemming,
                ProjectId = drillHole.ProjectId,
                SiteId = drillHole.SiteId,
                CreatedAt = drillHole.CreatedAt,
                UpdatedAt = drillHole.UpdatedAt
            };
        }
    }
} 