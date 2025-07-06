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

        public BlastSequenceApplicationService(
            ISiteBlastingRepository repository,
            ILogger<BlastSequenceApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Public API

        public async Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            var sequences = await _repository.GetBlastSequencesAsync(projectId, siteId);
            return sequences.Select(ConvertToDto).ToList();
        }

        public async Task<BlastSequenceDto?> GetBlastSequenceAsync(int id)
        {
            var sequence = await _repository.GetBlastSequenceAsync(id);
            return sequence == null ? null : ConvertToDto(sequence);
        }

        public async Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId)
        {
            var entity = new BlastSequence
            {
                ProjectId = request.ProjectId,
                SiteId = request.SiteId,
                Name = request.Name,
                Description = request.Description,
                DelayBetweenHoles = request.DelayBetweenHoles,
                DelayBetweenRows = request.DelayBetweenRows,
                SimulationSettingsJson = request.SimulationSettingsJson,
                CreatedByUserId = userId
            };

            var saved = await _repository.CreateBlastSequenceAsync(entity);
            return ConvertToDto(saved);
        }

        public async Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, UpdateBlastSequenceRequest request, int userId)
        {
            var existing = await _repository.GetBlastSequenceAsync(id);
            if (existing == null)
                throw new ArgumentException($"Blast sequence with ID {id} not found");

            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.DelayBetweenHoles = request.DelayBetweenHoles;
            existing.DelayBetweenRows = request.DelayBetweenRows;
            existing.SimulationSettingsJson = request.SimulationSettingsJson;

            var updated = await _repository.UpdateBlastSequenceAsync(existing);
            return ConvertToDto(updated);
        }

        public async Task<bool> DeleteBlastSequenceAsync(int id)
        {
            return await _repository.DeleteBlastSequenceAsync(id);
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            return await _repository.ValidateBlastSequenceOwnershipAsync(sequenceId, projectId, siteId);
        }

        #endregion

        #region Mapping

        private static BlastSequenceDto ConvertToDto(BlastSequence entity) => new()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SiteId = entity.SiteId,
            Name = entity.Name,
            Description = entity.Description,
            DelayBetweenHoles = entity.DelayBetweenHoles,
            DelayBetweenRows = entity.DelayBetweenRows,
            SimulationSettingsJson = entity.SimulationSettingsJson,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreatedByUserId = entity.CreatedByUserId
        };

        #endregion
    }
} 