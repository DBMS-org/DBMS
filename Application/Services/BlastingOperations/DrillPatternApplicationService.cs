using Application.DTOs.DrillingOperations;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.DrillingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.BlastingOperations
{
    public class DrillPatternApplicationService : IDrillPatternService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<DrillPatternApplicationService> _logger;

        public DrillPatternApplicationService(ISiteBlastingRepository repository, ILogger<DrillPatternApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Public API

        public async Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId)
        {
            try
            {
                var patterns = await _repository.GetDrillPatternsAsync(projectId, siteId);
                return patterns.Select(ConvertToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill patterns for project {ProjectId}, site {SiteId}", projectId, siteId);
                throw;
            }
        }

        public async Task<DrillPatternDto?> GetDrillPatternAsync(int id)
        {
            try
            {
                var pattern = await _repository.GetDrillPatternAsync(id);
                return pattern == null ? null : ConvertToDto(pattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill pattern {PatternId}", id);
                throw;
            }
        }

        public async Task<DrillPatternDto> CreateDrillPatternAsync(CreateDrillPatternRequest request, int userId)
        {
            try
            {
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                    throw new ArgumentException("Invalid project or site ID");

                var entity = new DrillPattern
                {
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId,
                    Name = request.Name,
                    Description = request.Description,
                    Spacing = request.Spacing,
                    Burden = request.Burden,
                    Depth = request.Depth,
                    DrillPointsJson = request.DrillPointsJson,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var saved = await _repository.CreateDrillPatternAsync(entity);
                return ConvertToDto(saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill pattern for project {ProjectId}, site {SiteId}", request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<DrillPatternDto> UpdateDrillPatternAsync(int id, CreateDrillPatternRequest request, int userId)
        {
            try
            {
                if (!await _repository.ValidateDrillPatternOwnershipAsync(id, request.ProjectId, request.SiteId))
                    throw new ArgumentException("Invalid drill pattern ownership");

                var existing = await _repository.GetDrillPatternAsync(id);
                if (existing == null)
                    throw new ArgumentException("Drill pattern not found");

                existing.Name = request.Name;
                existing.Description = request.Description;
                existing.Spacing = request.Spacing;
                existing.Burden = request.Burden;
                existing.Depth = request.Depth;
                existing.DrillPointsJson = request.DrillPointsJson;
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateDrillPatternAsync(existing);
                return ConvertToDto(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill pattern {PatternId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteDrillPatternAsync(int id)
        {
            try
            {
                return await _repository.DeleteDrillPatternAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill pattern {PatternId}", id);
                throw;
            }
        }

        public Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId)
            => _repository.ValidateDrillPatternOwnershipAsync(patternId, projectId, siteId);

        #endregion

        #region Mapping

        private static DrillPatternDto ConvertToDto(DrillPattern entity) => new()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SiteId = entity.SiteId,
            Name = entity.Name,
            Description = entity.Description,
            Spacing = entity.Spacing,
            Burden = entity.Burden,
            Depth = entity.Depth,
            DrillPointsJson = entity.DrillPointsJson,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByName = entity.CreatedBy?.Name ?? "Unknown"
        };

        #endregion
    }
} 