using Microsoft.Extensions.Logging;
using Application.DTOs;
using Domain.Entities;
using Application.Interfaces;
using System.Text.Json;

namespace Application.Services
{
    public class SiteBlastingApplicationService : ISiteBlastingService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<SiteBlastingApplicationService> _logger;

        public SiteBlastingApplicationService(
            ISiteBlastingRepository repository, 
            ILogger<SiteBlastingApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        #region Site Blasting Data Operations

        public async Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType)
        {
            try
            {
                var data = await _repository.GetSiteDataAsync(projectId, siteId, dataType);
                if (data == null) return null;

                return ConvertToSiteBlastingDto(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    projectId, siteId, dataType);
                throw;
            }
        }

        public async Task<List<SiteBlastingDataDto>> GetAllSiteDataAsync(int projectId, int siteId)
        {
            try
            {
                var dataList = await _repository.GetAllSiteDataAsync(projectId, siteId);
                return dataList.Select(ConvertToSiteBlastingDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<SiteBlastingDataDto> SaveSiteDataAsync(CreateSiteBlastingDataRequest request, int userId)
        {
            try
            {
                // Validate project and site exist
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                // Check if data already exists for this type
                var existingData = await _repository.GetSiteDataAsync(request.ProjectId, request.SiteId, request.DataType);

                if (existingData != null)
                {
                    // Update existing data
                    existingData.JsonData = request.JsonData;
                    existingData.UpdatedAt = DateTime.UtcNow;
                    var updatedData = await _repository.UpdateSiteDataAsync(existingData);
                    return ConvertToSiteBlastingDto(updatedData);
                }
                else
                {
                    // Create new data
                    var newData = new SiteBlastingData
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = request.DataType,
                        JsonData = request.JsonData,
                        CreatedByUserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    var savedData = await _repository.SaveSiteDataAsync(newData);
                    return ConvertToSiteBlastingDto(savedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    request.ProjectId, request.SiteId, request.DataType);
                throw;
            }
        }

        public async Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType)
        {
            try
            {
                return await _repository.DeleteSiteDataAsync(projectId, siteId, dataType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting site data for project {ProjectId}, site {SiteId}, type {DataType}", 
                    projectId, siteId, dataType);
                throw;
            }
        }

        public async Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId)
        {
            try
            {
                return await _repository.DeleteAllSiteDataAsync(projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<bool> CleanupSiteDataAsync(CleanupSiteDataRequest request)
        {
            try
            {
                var hasData = false;

                foreach (var dataType in request.DataTypesToClean)
                {
                    var deleted = await _repository.DeleteSiteDataAsync(request.ProjectId, request.SiteId, dataType);
                    if (deleted) hasData = true;
                }

                return hasData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up site data for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId)
        {
            try
            {
                // Validate project and site exist
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                // Handle pattern data
                if (!string.IsNullOrEmpty(request.PatternData))
                {
                    var patternRequest = new CreateSiteBlastingDataRequest
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = "pattern_data",
                        JsonData = request.PatternData
                    };
                    await SaveSiteDataAsync(patternRequest, userId);
                }

                // Handle connections data
                if (!string.IsNullOrEmpty(request.ConnectionsData))
                {
                    var connectionsRequest = new CreateSiteBlastingDataRequest
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = "connections_data",
                        JsonData = request.ConnectionsData
                    };
                    await SaveSiteDataAsync(connectionsRequest, userId);
                }

                // Handle simulation settings
                if (!string.IsNullOrEmpty(request.SimulationSettingsData))
                {
                    var settingsRequest = new CreateSiteBlastingDataRequest
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = "simulation_settings",
                        JsonData = request.SimulationSettingsData
                    };
                    await SaveSiteDataAsync(settingsRequest, userId);
                }

                // Handle simulation state
                if (!string.IsNullOrEmpty(request.SimulationStateData))
                {
                    var stateRequest = new CreateSiteBlastingDataRequest
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = "simulation_state",
                        JsonData = request.SimulationStateData
                    };
                    await SaveSiteDataAsync(stateRequest, userId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving bulk site data for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<Dictionary<string, SiteBlastingDataDto>> GetBulkSiteDataAsync(int projectId, int siteId)
        {
            try
            {
                var allData = await _repository.GetAllSiteDataAsync(projectId, siteId);
                return allData.ToDictionary(d => d.DataType, ConvertToSiteBlastingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bulk site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        #endregion

        #region Drill Pattern Operations

        public async Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId)
        {
            try
            {
                var patterns = await _repository.GetDrillPatternsAsync(projectId, siteId);
                return patterns.Select(ConvertToDrillPatternDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill patterns for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<DrillPatternDto?> GetDrillPatternAsync(int id)
        {
            try
            {
                var pattern = await _repository.GetDrillPatternAsync(id);
                return pattern != null ? ConvertToDrillPatternDto(pattern) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill pattern {Id}", id);
                throw;
            }
        }

        public async Task<DrillPatternDto> CreateDrillPatternAsync(CreateDrillPatternRequest request, int userId)
        {
            try
            {
                // Validate project and site exist
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                var pattern = new DrillPattern
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

                var savedPattern = await _repository.CreateDrillPatternAsync(pattern);
                return ConvertToDrillPatternDto(savedPattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill pattern for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<DrillPatternDto> UpdateDrillPatternAsync(int id, CreateDrillPatternRequest request, int userId)
        {
            try
            {
                // Validate ownership
                if (!await _repository.ValidateDrillPatternOwnershipAsync(id, request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid drill pattern ownership");
                }

                var existingPattern = await _repository.GetDrillPatternAsync(id);
                if (existingPattern == null)
                {
                    throw new ArgumentException("Drill pattern not found");
                }

                // Update properties
                existingPattern.Name = request.Name;
                existingPattern.Description = request.Description;
                existingPattern.Spacing = request.Spacing;
                existingPattern.Burden = request.Burden;
                existingPattern.Depth = request.Depth;
                existingPattern.DrillPointsJson = request.DrillPointsJson;
                existingPattern.UpdatedAt = DateTime.UtcNow;

                var updatedPattern = await _repository.UpdateDrillPatternAsync(existingPattern);
                return ConvertToDrillPatternDto(updatedPattern);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill pattern {Id}", id);
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
                _logger.LogError(ex, "Error deleting drill pattern {Id}", id);
                throw;
            }
        }

        #endregion

        #region Blast Sequence Operations

        public async Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            try
            {
                var sequences = await _repository.GetBlastSequencesAsync(projectId, siteId);
                return sequences.Select(ConvertToBlastSequenceDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequences for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<BlastSequenceDto?> GetBlastSequenceAsync(int id)
        {
            try
            {
                var sequence = await _repository.GetBlastSequenceAsync(id);
                return sequence != null ? ConvertToBlastSequenceDto(sequence) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blast sequence {Id}", id);
                throw;
            }
        }

        public async Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId)
        {
            try
            {
                // Validate project and site exist
                if (!await _repository.ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                var sequence = new BlastSequence
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

                var savedSequence = await _repository.CreateBlastSequenceAsync(sequence);
                return ConvertToBlastSequenceDto(savedSequence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blast sequence for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }

        public async Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, CreateBlastSequenceRequest request, int userId)
        {
            try
            {
                // Validate ownership
                if (!await _repository.ValidateBlastSequenceOwnershipAsync(id, request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid blast sequence ownership");
                }

                var existingSequence = await _repository.GetBlastSequenceAsync(id);
                if (existingSequence == null)
                {
                    throw new ArgumentException("Blast sequence not found");
                }

                // Update properties
                existingSequence.Name = request.Name;
                existingSequence.Description = request.Description;
                existingSequence.DrillPatternId = request.DrillPatternId;
                existingSequence.ConnectionsJson = request.ConnectionsJson;
                existingSequence.SimulationSettingsJson = request.SimulationSettingsJson;
                existingSequence.UpdatedAt = DateTime.UtcNow;

                var updatedSequence = await _repository.UpdateBlastSequenceAsync(existingSequence);
                return ConvertToBlastSequenceDto(updatedSequence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blast sequence {Id}", id);
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
                _logger.LogError(ex, "Error deleting blast sequence {Id}", id);
                throw;
            }
        }

        #endregion

        #region Workflow Operations

        public async Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId)
        {
            try
            {
                var progressData = await _repository.GetWorkflowProgressDataAsync(projectId, siteId);
                
                if (progressData?.JsonData != null)
                {
                    var progress = JsonSerializer.Deserialize<SiteWorkflowProgressDto>(progressData.JsonData);
                    if (progress != null) return progress;
                }

                // Return default progress if none exists
                return new SiteWorkflowProgressDto
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    SiteName = string.Empty,
                    PatternCreator = new WorkflowStepDto { Completed = false, Progress = 0 },
                    SequenceDesigner = new WorkflowStepDto { Completed = false, Progress = 0 },
                    Simulator = new WorkflowStepDto { Completed = false, Progress = 0 },
                    OverallProgress = 0,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow progress for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<SiteWorkflowProgressDto> UpdateWorkflowProgressAsync(int projectId, int siteId, string stepId, bool completed)
        {
            try
            {
                var currentProgress = await GetWorkflowProgressAsync(projectId, siteId);
                
                // Update the appropriate step based on stepId
                switch (stepId.ToLower())
                {
                    case "pattern_creator":
                    case "drill_pattern":
                        currentProgress.PatternCreator.Completed = completed;
                        currentProgress.PatternCreator.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                    case "sequence_designer":
                    case "blast_sequence":
                        currentProgress.SequenceDesigner.Completed = completed;
                        currentProgress.SequenceDesigner.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                    case "simulator":
                    case "operator_completion":
                        currentProgress.Simulator.Completed = completed;
                        currentProgress.Simulator.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                }
                
                var progressData = new SiteBlastingData
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    DataType = "workflow_progress",
                    JsonData = JsonSerializer.Serialize(currentProgress),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.SaveWorkflowProgressDataAsync(progressData);
                return currentProgress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow progress for project {ProjectId}, site {SiteId}, step {StepId}", 
                    projectId, siteId, stepId);
                throw;
            }
        }

        public async Task<bool> SetOperatorCompletionAsync(int projectId, int siteId, bool completed)
        {
            try
            {
                await UpdateWorkflowProgressAsync(projectId, siteId, "operator_completion", completed);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting operator completion for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        #endregion

        #region Validation Methods

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            try
            {
                return await _repository.ValidateProjectSiteExistsAsync(projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating project {ProjectId} and site {SiteId}", projectId, siteId);
                throw;
            }
        }

        public async Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId)
        {
            try
            {
                return await _repository.ValidateDrillPatternOwnershipAsync(patternId, projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating drill pattern ownership {PatternId} for project {ProjectId}, site {SiteId}", 
                    patternId, projectId, siteId);
                throw;
            }
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            try
            {
                return await _repository.ValidateBlastSequenceOwnershipAsync(sequenceId, projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating blast sequence ownership {SequenceId} for project {ProjectId}, site {SiteId}", 
                    sequenceId, projectId, siteId);
                throw;
            }
        }

        #endregion

        #region Helper Methods - DTO Conversion

        private static SiteBlastingDataDto ConvertToSiteBlastingDto(SiteBlastingData data)
        {
            return new SiteBlastingDataDto
            {
                Id = data.Id,
                ProjectId = data.ProjectId,
                SiteId = data.SiteId,
                DataType = data.DataType,
                JsonData = data.JsonData,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                CreatedByUserId = data.CreatedByUserId,
                CreatedByName = data.CreatedBy?.Name ?? "Unknown"
            };
        }

        private static DrillPatternDto ConvertToDrillPatternDto(DrillPattern pattern)
        {
            return new DrillPatternDto
            {
                Id = pattern.Id,
                ProjectId = pattern.ProjectId,
                SiteId = pattern.SiteId,
                Name = pattern.Name,
                Description = pattern.Description,
                Spacing = pattern.Spacing,
                Burden = pattern.Burden,
                Depth = pattern.Depth,
                DrillPointsJson = pattern.DrillPointsJson,
                IsActive = pattern.IsActive,
                CreatedAt = pattern.CreatedAt,
                UpdatedAt = pattern.UpdatedAt,
                CreatedByUserId = pattern.CreatedByUserId,
                CreatedByName = pattern.CreatedBy?.Name ?? "Unknown"
            };
        }

        private static BlastSequenceDto ConvertToBlastSequenceDto(BlastSequence sequence)
        {
            return new BlastSequenceDto
            {
                Id = sequence.Id,
                ProjectId = sequence.ProjectId,
                SiteId = sequence.SiteId,
                DrillPatternId = sequence.DrillPatternId,
                Name = sequence.Name,
                Description = sequence.Description,
                ConnectionsJson = sequence.ConnectionsJson,
                SimulationSettingsJson = sequence.SimulationSettingsJson,
                IsActive = sequence.IsActive,
                CreatedAt = sequence.CreatedAt,
                UpdatedAt = sequence.UpdatedAt,
                CreatedByUserId = sequence.CreatedByUserId,
                CreatedByName = sequence.CreatedBy?.Name ?? "Unknown"
            };
        }

        #endregion
    }
}