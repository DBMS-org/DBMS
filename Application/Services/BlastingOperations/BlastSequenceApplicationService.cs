using Application.DTOs.BlastingOperations;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.BlastingOperations
{
    public class BlastSequenceApplicationService : IBlastSequenceService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<BlastSequenceApplicationService> _logger;

        public BlastSequenceApplicationService(ISiteBlastingRepository repository, ILogger<BlastSequenceApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Public API

        public async Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            try
            {
                var sequences = await _repository.GetBlastSequencesAsync(projectId, siteId);
                return sequences.Select(ConvertToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequences for project {ProjectId}, site {SiteId}", projectId, siteId);
                throw;
            }
        }

        public async Task<BlastSequenceDto?> GetBlastSequenceAsync(int id)
        {
            try
            {
                var sequence = await _repository.GetBlastSequenceAsync(id);
                return sequence == null ? null : ConvertToDto(sequence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequence {SequenceId}", id);
                throw;
            }
        }

        public async Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId)
        {
            try
            {
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                    throw new ArgumentException("Invalid project or site ID");

                var entity = new BlastSequence
                {
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId,
                    DrillPatternId = request.DrillPatternId,
                    Name = request.Name,
                    Description = request.Description,
                    ConnectionsJson = request.ConnectionsJson,
                    SimulationSettingsJson = request.SimulationSettingsJson,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var saved = await _repository.CreateBlastSequenceAsync(entity);
                return ConvertToDto(saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blast sequence for project {ProjectId}, site {SiteId}", request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, CreateBlastSequenceRequest request, int userId)
        {
            try
            {
                if (!await _repository.ValidateBlastSequenceOwnershipAsync(id, request.ProjectId, request.SiteId))
                    throw new ArgumentException("Invalid blast sequence ownership");

                var existing = await _repository.GetBlastSequenceAsync(id);
                if (existing == null)
                    throw new ArgumentException("Blast sequence not found");

                existing.Name = request.Name;
                existing.Description = request.Description;
                existing.DrillPatternId = request.DrillPatternId;
                existing.ConnectionsJson = request.ConnectionsJson;
                existing.SimulationSettingsJson = request.SimulationSettingsJson;
                existing.UpdatedAt = DateTime.UtcNow;

                var updated = await _repository.UpdateBlastSequenceAsync(existing);
                return ConvertToDto(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blast sequence {SequenceId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteBlastSequenceAsync(int id)
        {
            try
            {
                return await _repository.DeleteBlastSequenceAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blast sequence {SequenceId}", id);
                throw;
            }
        }

        public Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
            => _repository.ValidateBlastSequenceOwnershipAsync(sequenceId, projectId, siteId);

        #endregion

        #region Mapping

        private static BlastSequenceDto ConvertToDto(BlastSequence entity) => new()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SiteId = entity.SiteId,
            DrillPatternId = entity.DrillPatternId,
            Name = entity.Name,
            Description = entity.Description,
            ConnectionsJson = entity.ConnectionsJson,
            SimulationSettingsJson = entity.SimulationSettingsJson,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByName = entity.CreatedBy?.Name ?? "Unknown"
        };

        #endregion
    }
} 