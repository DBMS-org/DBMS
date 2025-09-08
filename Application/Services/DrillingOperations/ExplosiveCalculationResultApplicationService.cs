using Domain.Entities.DrillingOperations;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Microsoft.Extensions.Logging;
using Application.DTOs.Shared;
using Application.Utilities;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.ProjectManagement;
using AutoMapper;
using System.Linq;

namespace Application.Services.DrillingOperations
{
    public class ExplosiveCalculationResultApplicationService : IExplosiveCalculationResultService
    {
        private readonly IExplosiveCalculationResultRepository _explosiveCalculationResultRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ExplosiveCalculationResultApplicationService> _logger;

        public ExplosiveCalculationResultApplicationService(
            IExplosiveCalculationResultRepository explosiveCalculationResultRepository,
            IProjectRepository projectRepository,
            IUserContext userContext,
            IMapper mapper,
            ILogger<ExplosiveCalculationResultApplicationService> logger)
        {
            _explosiveCalculationResultRepository = explosiveCalculationResultRepository;
            _projectRepository = projectRepository;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetAllExplosiveCalculationResultsAsync()
        {
            try
            {
                // Note: Repository requires projectId and siteId, so we'll need to get all projects first
                // This is a simplified implementation - in practice, you might want to add overloads
                var allResults = new List<ExplosiveCalculationResult>();
                
                // If the user is a blasting engineer, filter by their region
                if (_userContext.IsInRole("BlastingEngineer"))
                {
                    var region = _userContext.Region;
                    if (!string.IsNullOrEmpty(region))
                    {
                        var projects = await _projectRepository.SearchAsync(region: region);
                        foreach (var project in projects)
                        {
                            var projectResults = await _explosiveCalculationResultRepository.GetByProjectAsync(project.Id);
                            allResults.AddRange(projectResults);
                        }
                    }
                }
                else
                {
                    // For non-blasting engineers, we need a different approach
                    // This would typically require a repository method that gets all results
                    // For now, return empty - this should be implemented based on business requirements
                    return Result.Success(Enumerable.Empty<ExplosiveCalculationResult>());
                }

                return Result.Success(allResults.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all explosive calculation results");
                return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResult>> GetExplosiveCalculationResultByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.ArgumentNull);
                }

                var result = await _explosiveCalculationResultRepository.GetByIdAsync(id);
                if (result == null)
                {
                    return Result.Failure<ExplosiveCalculationResult>("Explosive calculation result not found");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result {Id}", id);
                return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResult>> GetExplosiveCalculationResultByCalculationIdAsync(string calculationId)
        {
            try
            {
                if (string.IsNullOrEmpty(calculationId))
                {
                    return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.ArgumentNull);
                }

                var result = await _explosiveCalculationResultRepository.GetByCalculationIdAsync(calculationId);
                if (result == null)
                {
                    return Result.Failure<ExplosiveCalculationResult>("Explosive calculation result not found");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result by calculation ID {CalculationId}", calculationId);
                return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResult>> CreateExplosiveCalculationResultAsync(ExplosiveCalculationResult explosiveCalculationResult)
        {
            try
            {
                if (explosiveCalculationResult == null)
                {
                    return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.ArgumentNull);
                }

                // Note: Removed check for existing CalculationId since we now allow one calculation per site
                // and delete existing calculations before creating new ones

                var createdResult = await _explosiveCalculationResultRepository.AddAsync(explosiveCalculationResult);
                return Result.Success(createdResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive calculation result {CalculationId}", explosiveCalculationResult?.CalculationId);
                return Result.Failure<ExplosiveCalculationResult>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateExplosiveCalculationResultAsync(ExplosiveCalculationResult explosiveCalculationResult)
        {
            try
            {
                if (explosiveCalculationResult == null)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                var existingResult = await _explosiveCalculationResultRepository.GetByIdAsync(explosiveCalculationResult.Id);
                if (existingResult == null)
                {
                    return Result.Failure("Explosive calculation result not found");
                }

                await _explosiveCalculationResultRepository.UpdateAsync(explosiveCalculationResult);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive calculation result {Id}", explosiveCalculationResult?.Id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteExplosiveCalculationResultAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                var existingResult = await _explosiveCalculationResultRepository.GetByIdAsync(id);
                if (existingResult == null)
                {
                    return Result.Failure("Explosive calculation result not found");
                }

                await _explosiveCalculationResultRepository.DeleteAsync(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation result {Id}", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsByProjectIdAsync(int projectId)
        {
            try
            {
                if (projectId <= 0)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.ArgumentNull);
                }

                var results = await _explosiveCalculationResultRepository.GetByProjectAsync(projectId);
                return Result.Success(results.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation results by project ID {ProjectId}", projectId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                if (projectId <= 0 || siteId <= 0)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.ArgumentNull);
                }

                var results = await _explosiveCalculationResultRepository.GetBySiteAsync(projectId, siteId);
                return Result.Success(results.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation results by site ID {ProjectId}/{SiteId}", projectId, siteId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResult>>> GetExplosiveCalculationResultsByUserIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.ArgumentNull);
                }

                var results = await _explosiveCalculationResultRepository.GetByUserAsync(userId);
                return Result.Success(results.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation results by user ID {UserId}", userId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResult>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteExplosiveCalculationResultsByProjectIdAsync(int projectId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (projectId <= 0)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                await _explosiveCalculationResultRepository.DeleteAllAsync(projectId, 0); // Use 0 as siteId to delete all sites in project
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation results by project ID {ProjectId}", projectId);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> DeleteExplosiveCalculationResultsBySiteIdAsync(int projectId, int siteId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (projectId <= 0 || siteId <= 0)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                await _explosiveCalculationResultRepository.DeleteAllAsync(projectId, siteId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation results by site ID {ProjectId}/{SiteId}", projectId, siteId);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetExplosiveCalculationResultCountAsync()
        {
            try
            {
                // Get all results across all projects and sites and count them
                var allResults = new List<ExplosiveCalculationResult>();
                // Since we don't have a global count method, we'll need to aggregate
                // This is a simplified implementation - in practice you might want to optimize this
                var count = 0; // Placeholder - would need project/site iteration
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result count");
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetExplosiveCalculationResultCountByProjectIdAsync(int projectId)
        {
            try
            {
                if (projectId <= 0)
                {
                    return Result.Failure<int>(ErrorCodes.Messages.ArgumentNull);
                }

                // Get results by project and count them
                var results = await _explosiveCalculationResultRepository.GetByProjectAsync(projectId);
                var count = results.Count;
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result count by project ID {ProjectId}", projectId);
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetExplosiveCalculationResultCountBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                if (projectId <= 0 || siteId <= 0)
                {
                    return Result.Failure<int>(ErrorCodes.Messages.ArgumentNull);
                }

                var count = await _explosiveCalculationResultRepository.GetCountAsync(projectId, siteId);
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result count by site ID {ProjectId}/{SiteId}", projectId, siteId);
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<int>> GetExplosiveCalculationResultCountByUserIdAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return Result.Failure<int>(ErrorCodes.Messages.ArgumentNull);
                }

                var results = await _explosiveCalculationResultRepository.GetByUserAsync(userId);
                var count = results.Count;
                return Result.Success(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result count by user ID {UserId}", userId);
                return Result.Failure<int>(ErrorCodes.Messages.InternalError);
            }
        }

        // DTO-based methods
        public async Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetAllExplosiveCalculationResultsDtoAsync()
        {
            try
            {
                var entitiesResult = await GetAllExplosiveCalculationResultsAsync();
                if (!entitiesResult.IsSuccess)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Error);
                }

                var dtos = _mapper.Map<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Value);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all explosive calculation results as DTOs");
                return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResultDto>> GetExplosiveCalculationResultDtoByIdAsync(int id)
        {
            try
            {
                var entityResult = await GetExplosiveCalculationResultByIdAsync(id);
                if (!entityResult.IsSuccess)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(entityResult.Error);
                }

                var dto = _mapper.Map<ExplosiveCalculationResultDto>(entityResult.Value);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result DTO by ID {Id}", id);
                return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResultDto>> GetExplosiveCalculationResultDtoByCalculationIdAsync(string calculationId)
        {
            try
            {
                var entityResult = await GetExplosiveCalculationResultByCalculationIdAsync(calculationId);
                if (!entityResult.IsSuccess)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(entityResult.Error);
                }

                var dto = _mapper.Map<ExplosiveCalculationResultDto>(entityResult.Value);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result DTO by calculation ID {CalculationId}", calculationId);
                return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResultDto>> CreateExplosiveCalculationResultFromDtoAsync(CreateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.ArgumentNull);
                }

                var entity = _mapper.Map<ExplosiveCalculationResult>(request);
                
                // Check if existing calculations exist for the same site
                if (entity.ProjectId > 0 && entity.SiteId > 0)
                {
                    var existingCalculations = await GetExplosiveCalculationResultsBySiteIdAsync(entity.ProjectId, entity.SiteId);
                    if (existingCalculations.IsSuccess && existingCalculations.Value.Any())
                    {
                        return Result.Failure<ExplosiveCalculationResultDto>("EXISTING_CALCULATION_FOUND");
                    }
                }
                
                var entityResult = await CreateExplosiveCalculationResultAsync(entity);
                
                if (!entityResult.IsSuccess)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(entityResult.Error);
                }

                var dto = _mapper.Map<ExplosiveCalculationResultDto>(entityResult.Value);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive calculation result from DTO");
                return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<ExplosiveCalculationResultDto>> CreateExplosiveCalculationResultWithConfirmationAsync(CreateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.ArgumentNull);
                }

                var entity = _mapper.Map<ExplosiveCalculationResult>(request);
                
                // Delete existing calculations for the same site after user confirmation
                if (entity.ProjectId > 0 && entity.SiteId > 0)
                {
                    _logger.LogInformation("Deleting existing calculations for project {ProjectId} and site {SiteId} after user confirmation", entity.ProjectId, entity.SiteId);
                    await DeleteExplosiveCalculationResultsBySiteIdAsync(entity.ProjectId, entity.SiteId);
                }
                
                var entityResult = await CreateExplosiveCalculationResultAsync(entity);
                
                if (!entityResult.IsSuccess)
                {
                    return Result.Failure<ExplosiveCalculationResultDto>(entityResult.Error);
                }

                var dto = _mapper.Map<ExplosiveCalculationResultDto>(entityResult.Value);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive calculation result with confirmation from DTO");
                return Result.Failure<ExplosiveCalculationResultDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateExplosiveCalculationResultFromDtoAsync(int id, UpdateExplosiveCalculationResultRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Result.Failure(ErrorCodes.Messages.ArgumentNull);
                }

                var existingEntityResult = await GetExplosiveCalculationResultByIdAsync(id);
                if (!existingEntityResult.IsSuccess)
                {
                    return Result.Failure(existingEntityResult.Error);
                }

                var existingEntity = existingEntityResult.Value;
                
                // Update only non-null properties from the request
                if (!string.IsNullOrEmpty(request.CalculationId))
                    existingEntity.CalculationId = request.CalculationId;
                if (request.PatternSettingsId.HasValue)
                    existingEntity.PatternSettingsId = request.PatternSettingsId;
                if (request.EmulsionDensity.HasValue)
                    existingEntity.EmulsionDensity = request.EmulsionDensity.Value;
                if (request.AnfoDensity.HasValue)
                    existingEntity.AnfoDensity = request.AnfoDensity.Value;
                if (request.EmulsionPerHole.HasValue)
                    existingEntity.EmulsionPerHole = request.EmulsionPerHole.Value;
                if (request.TotalDepth.HasValue)
                    existingEntity.TotalDepth = request.TotalDepth.Value;
                if (request.AverageDepth.HasValue)
                    existingEntity.AverageDepth = request.AverageDepth.Value;
                if (request.NumberOfFilledHoles.HasValue)
                    existingEntity.NumberOfFilledHoles = request.NumberOfFilledHoles.Value;
                if (request.EmulsionPerMeter.HasValue)
                    existingEntity.EmulsionPerMeter = request.EmulsionPerMeter.Value;
                if (request.AnfoPerMeter.HasValue)
                    existingEntity.AnfoPerMeter = request.AnfoPerMeter.Value;
                if (request.EmulsionCoveringSpace.HasValue)
                    existingEntity.EmulsionCoveringSpace = request.EmulsionCoveringSpace.Value;
                if (request.RemainingSpace.HasValue)
                    existingEntity.RemainingSpace = request.RemainingSpace.Value;
                if (request.AnfoCoveringSpace.HasValue)
                    existingEntity.AnfoCoveringSpace = request.AnfoCoveringSpace.Value;
                if (request.TotalAnfo.HasValue)
                    existingEntity.TotalAnfo = request.TotalAnfo.Value;
                if (request.TotalEmulsion.HasValue)
                    existingEntity.TotalEmulsion = request.TotalEmulsion.Value;
                if (request.TotalVolume.HasValue)
                    existingEntity.TotalVolume = request.TotalVolume.Value;

                return await UpdateExplosiveCalculationResultAsync(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive calculation result from DTO {Id}", id);
                return Result.Failure(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoByProjectIdAsync(int projectId)
        {
            try
            {
                var entitiesResult = await GetExplosiveCalculationResultsByProjectIdAsync(projectId);
                if (!entitiesResult.IsSuccess)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Error);
                }

                var dtos = _mapper.Map<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Value);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result DTOs by project ID {ProjectId}", projectId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoBySiteIdAsync(int projectId, int siteId)
        {
            try
            {
                var entitiesResult = await GetExplosiveCalculationResultsBySiteIdAsync(projectId, siteId);
                if (!entitiesResult.IsSuccess)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Error);
                }

                var dtos = _mapper.Map<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Value);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result DTOs by site ID {ProjectId}/{SiteId}", projectId, siteId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result<IEnumerable<ExplosiveCalculationResultDto>>> GetExplosiveCalculationResultsDtoByUserIdAsync(int userId)
        {
            try
            {
                var entitiesResult = await GetExplosiveCalculationResultsByUserIdAsync(userId);
                if (!entitiesResult.IsSuccess)
                {
                    return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Error);
                }

                var dtos = _mapper.Map<IEnumerable<ExplosiveCalculationResultDto>>(entitiesResult.Value);
                return Result.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting explosive calculation result DTOs by user ID {UserId}", userId);
                return Result.Failure<IEnumerable<ExplosiveCalculationResultDto>>(ErrorCodes.Messages.InternalError);
            }
        }
    }
}