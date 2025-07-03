using Application.Interfaces.BlastingOperations;
using Application.DTOs.BlastingOperations;
using Application.DTOs.Shared;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;
using Application.Utilities;

namespace Application.Services.BlastingOperations
{
    public class SiteBlastingDataApplicationService : ISiteBlastingDataService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<SiteBlastingDataApplicationService> _logger;

        public SiteBlastingDataApplicationService(ISiteBlastingRepository repository, ILogger<SiteBlastingDataApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType)
        {
            try
            {
                var data = await _repository.GetSiteDataAsync(projectId, siteId, dataType);
                return data == null ? null : ConvertToDto(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site data for project {ProjectId}, site {SiteId}, type {DataType}", projectId, siteId, dataType);
                throw;
            }
        }

        public async Task<List<SiteBlastingDataDto>> GetAllSiteDataAsync(int projectId, int siteId)
        {
            try
            {
                var list = await _repository.GetAllSiteDataAsync(projectId, siteId);
                return list.Select(ConvertToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all site data for project {ProjectId}, site {SiteId}", projectId, siteId);
                throw;
            }
        }

        public async Task<SiteBlastingDataDto> SaveSiteDataAsync(CreateSiteBlastingDataRequest request, int userId)
        {
            try
            {
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                    throw new ArgumentException("Invalid project or site ID");

                var existing = await _repository.GetSiteDataAsync(request.ProjectId, request.SiteId, request.DataType);
                if (existing != null)
                {
                    existing.JsonData = request.JsonData;
                    existing.UpdatedAt = DateTime.UtcNow;
                    var updated = await _repository.UpdateSiteDataAsync(existing);
                    return ConvertToDto(updated);
                }

                var entity = new SiteBlastingData
                {
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId,
                    DataType = request.DataType,
                    JsonData = request.JsonData,
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var saved = await _repository.SaveSiteDataAsync(entity);
                return ConvertToDto(saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving site data for project {ProjectId}, site {SiteId}, type {DataType}", request.ProjectId, request.SiteId, request.DataType);
                throw;
            }
        }

        public async Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType) => await _repository.DeleteSiteDataAsync(projectId, siteId, dataType);

        public async Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId) => await _repository.DeleteAllSiteDataAsync(projectId, siteId);

        public async Task<bool> CleanupSiteDataAsync(CleanupSiteDataRequest request)
        {
            try
            {
                var hasData = false;
                var types = request.CleanAll ? (await _repository.GetAllSiteDataAsync(request.ProjectId, request.SiteId)).Select(d=>d.DataType).Distinct() : request.DataTypesToClean;
                foreach(var dataType in types)
                {
                    var deleted = await _repository.DeleteSiteDataAsync(request.ProjectId, request.SiteId, dataType);
                    if (deleted) hasData = true;
                }
                return hasData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning site data for project {ProjectId}, site {SiteId}", request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Use resource manager for timeout handling
                return await ResourceManager.ExecuteWithTimeoutAsync(
                    async (ct) => await SaveBulkSiteDataInternalAsync(request, userId, ct),
                    $"SaveBulkSiteData_{request.ProjectId}_{request.SiteId}",
                    timeoutMinutes: 5, // Bulk save timeout
                    cancellationToken: cancellationToken);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout saving bulk site data for project {ProjectId}, site {SiteId}", request.ProjectId, request.SiteId);
                throw new InvalidOperationException("Bulk save operation timed out", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving bulk site data for project {ProjectId}, site {SiteId}", request.ProjectId, request.SiteId);
                throw;
            }
        }

        private async Task<bool> SaveBulkSiteDataInternalAsync(BulkSiteDataRequest request, int userId, CancellationToken cancellationToken)
        {
            if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                throw new ArgumentException("Invalid project or site ID");

            // Create data requests for processing
            var dataRequests = new List<(string json, string type)>();
            
            if (!string.IsNullOrEmpty(request.PatternData))
                dataRequests.Add((request.PatternData, "pattern_data"));
            if (!string.IsNullOrEmpty(request.ConnectionsData))
                dataRequests.Add((request.ConnectionsData, "connections_data"));
            if (!string.IsNullOrEmpty(request.SimulationSettingsData))
                dataRequests.Add((request.SimulationSettingsData, "simulation_settings"));
            if (!string.IsNullOrEmpty(request.SimulationStateData))
                dataRequests.Add((request.SimulationStateData, "simulation_state"));

            // Process in batches with cancellation support
            await ResourceManager.ProcessInBatchesAsync(
                dataRequests,
                batchSize: 2, // Process 2 items at a time
                async (batch, ct) =>
                {
                    var tasks = batch.Select(async dataRequest =>
                    {
                        ct.ThrowIfCancellationRequested();
                        
                        var createRequest = new CreateSiteBlastingDataRequest
                        {
                            ProjectId = request.ProjectId,
                            SiteId = request.SiteId,
                            DataType = dataRequest.type,
                            JsonData = dataRequest.json
                        };
                        
                        await SaveSiteDataAsync(createRequest, userId);
                    });
                    
                    await Task.WhenAll(tasks);
                },
                cancellationToken);
            
            return true;
        }

        public async Task<Dictionary<string, SiteBlastingDataDto>> GetBulkSiteDataAsync(int projectId, int siteId)
        {
            var data = await _repository.GetAllSiteDataAsync(projectId, siteId);
            return data.ToDictionary(d => d.DataType, ConvertToDto);
        }

        public Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId) =>
            _repository.ValidateProjectSiteExistsAsync(projectId, siteId);

        private static SiteBlastingDataDto ConvertToDto(SiteBlastingData entity) => new()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            SiteId = entity.SiteId,
            DataType = entity.DataType,
            JsonData = entity.JsonData,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreatedByUserId = entity.CreatedByUserId,
            CreatedByName = "N/A" // populate later via user cache/service
        };
    }
} 