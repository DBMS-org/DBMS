using Domain.Entities.DrillingOperations;
using Application.Interfaces.DrillingOperations;
using Microsoft.Extensions.Logging;
using Application.DTOs.Shared;
using Application.Utilities;

namespace Application.Services.DrillingOperations
{
    public class DrillHoleApplicationService : IDrillHoleService
    {
        private readonly IDrillHoleRepository _drillHoleRepository;
        private readonly ILogger<DrillHoleApplicationService> _logger;

        public DrillHoleApplicationService(
            IDrillHoleRepository drillHoleRepository,
            ILogger<DrillHoleApplicationService> logger)
        {
            _drillHoleRepository = drillHoleRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DrillHole>>> GetAllDrillHolesAsync()
        {
            try
            {
                var drillHoles = await _drillHoleRepository.GetAllAsync();
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
    }
} 