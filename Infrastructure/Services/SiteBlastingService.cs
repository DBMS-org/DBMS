using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class SiteBlastingService : ISiteBlastingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SiteBlastingService> _logger;

        public SiteBlastingService(ApplicationDbContext context, ILogger<SiteBlastingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Site Blasting Data Operations (Generic JSON Storage)

        public async Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType)
        {
            try
            {
                var data = await _context.SiteBlastingData
                    .Include(s => s.CreatedBy)
                    .FirstOrDefaultAsync(s => s.ProjectId == projectId && 
                                           s.SiteId == siteId && 
                                           s.DataType == dataType);

                if (data == null) return null;

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
                var dataList = await _context.SiteBlastingData
                    .Include(s => s.CreatedBy)
                    .Where(s => s.ProjectId == projectId && s.SiteId == siteId)
                    .ToListAsync();

                return dataList.Select(data => new SiteBlastingDataDto
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
                }).ToList();
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
                if (!await ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                // Check if data already exists for this type
                var existingData = await _context.SiteBlastingData
                    .FirstOrDefaultAsync(s => s.ProjectId == request.ProjectId && 
                                           s.SiteId == request.SiteId && 
                                           s.DataType == request.DataType);

                if (existingData != null)
                {
                    // Update existing data
                    existingData.JsonData = request.JsonData;
                    existingData.UpdatedAt = DateTime.UtcNow;
                    _context.SiteBlastingData.Update(existingData);
                }
                else
                {
                    // Create new data
                    existingData = new SiteBlastingData
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = request.DataType,
                        JsonData = request.JsonData,
                        CreatedByUserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.SiteBlastingData.Add(existingData);
                }

                await _context.SaveChangesAsync();

                // Load the created/updated entity with navigation properties
                await _context.Entry(existingData)
                    .Reference(s => s.CreatedBy)
                    .LoadAsync();

                return new SiteBlastingDataDto
                {
                    Id = existingData.Id,
                    ProjectId = existingData.ProjectId,
                    SiteId = existingData.SiteId,
                    DataType = existingData.DataType,
                    JsonData = existingData.JsonData,
                    CreatedAt = existingData.CreatedAt,
                    UpdatedAt = existingData.UpdatedAt,
                    CreatedByUserId = existingData.CreatedByUserId,
                    CreatedByName = existingData.CreatedBy?.Name ?? "Unknown"
                };
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
                var data = await _context.SiteBlastingData
                    .FirstOrDefaultAsync(s => s.ProjectId == projectId && 
                                           s.SiteId == siteId && 
                                           s.DataType == dataType);

                if (data == null) return false;

                _context.SiteBlastingData.Remove(data);
                await _context.SaveChangesAsync();
                return true;
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
                // Delete all SiteBlastingData
                var siteData = await _context.SiteBlastingData
                    .Where(s => s.ProjectId == projectId && s.SiteId == siteId)
                    .ToListAsync();

                // Delete all DrillPatterns
                var drillPatterns = await _context.DrillPatterns
                    .Where(p => p.ProjectId == projectId && p.SiteId == siteId)
                    .ToListAsync();

                // Delete all BlastSequences
                var blastSequences = await _context.BlastSequences
                    .Where(b => b.ProjectId == projectId && b.SiteId == siteId)
                    .ToListAsync();

                var hasData = siteData.Any() || drillPatterns.Any() || blastSequences.Any();

                if (hasData)
                {
                    if (siteData.Any())
                        _context.SiteBlastingData.RemoveRange(siteData);
                    
                    if (drillPatterns.Any())
                        _context.DrillPatterns.RemoveRange(drillPatterns);
                    
                    if (blastSequences.Any())
                        _context.BlastSequences.RemoveRange(blastSequences);

                    await _context.SaveChangesAsync();
                }

                return hasData;
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
                var query = _context.SiteBlastingData
                    .Where(s => s.ProjectId == request.ProjectId && s.SiteId == request.SiteId);

                if (!request.CleanAll && request.DataTypesToClean.Any())
                {
                    query = query.Where(s => request.DataTypesToClean.Contains(s.DataType));
                }

                var dataToDelete = await query.ToListAsync();
                
                if (dataToDelete.Any())
                {
                    _context.SiteBlastingData.RemoveRange(dataToDelete);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up site data for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId)
        {
            try
            {
                if (!await ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                var dataTypes = new Dictionary<string, string?>
                {
                    { "pattern", request.PatternData },
                    { "connections", request.ConnectionsData },
                    { "simulation_settings", request.SimulationSettingsData },
                    { "simulation_state", request.SimulationStateData }
                };

                foreach (var (dataType, jsonData) in dataTypes)
                {
                    if (string.IsNullOrEmpty(jsonData)) continue;

                    var saveRequest = new CreateSiteBlastingDataRequest
                    {
                        ProjectId = request.ProjectId,
                        SiteId = request.SiteId,
                        DataType = dataType,
                        JsonData = jsonData
                    };

                    await SaveSiteDataAsync(saveRequest, userId);
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
                var allData = await GetAllSiteDataAsync(projectId, siteId);
                return allData.ToDictionary(d => d.DataType, d => d);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bulk site data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        #endregion

        #region Drill Pattern Operations (Structured Storage)

        public async Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId)
        {
            try
            {
                var patterns = await _context.DrillPatterns
                    .Include(p => p.CreatedBy)
                    .Where(p => p.ProjectId == projectId && p.SiteId == siteId && p.IsActive)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                return patterns.Select(p => ConvertToDto(p)).ToList();
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
                var pattern = await _context.DrillPatterns
                    .Include(p => p.CreatedBy)
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

                return pattern != null ? ConvertToDto(pattern) : null;
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
                if (!await ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
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

                _context.DrillPatterns.Add(pattern);
                await _context.SaveChangesAsync();

                await _context.Entry(pattern)
                    .Reference(p => p.CreatedBy)
                    .LoadAsync();

                return ConvertToDto(pattern);
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
                var pattern = await _context.DrillPatterns
                    .Include(p => p.CreatedBy)
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

                if (pattern == null)
                {
                    throw new ArgumentException("Drill pattern not found");
                }

                if (!await ValidateDrillPatternOwnershipAsync(id, request.ProjectId, request.SiteId))
                {
                    throw new UnauthorizedAccessException("Access denied to this drill pattern");
                }

                pattern.Name = request.Name;
                pattern.Description = request.Description;
                pattern.Spacing = request.Spacing;
                pattern.Burden = request.Burden;
                pattern.Depth = request.Depth;
                pattern.DrillPointsJson = request.DrillPointsJson;
                pattern.UpdatedAt = DateTime.UtcNow;

                _context.DrillPatterns.Update(pattern);
                await _context.SaveChangesAsync();

                return ConvertToDto(pattern);
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
                var pattern = await _context.DrillPatterns
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

                if (pattern == null) return false;

                // Soft delete
                pattern.IsActive = false;
                pattern.UpdatedAt = DateTime.UtcNow;

                _context.DrillPatterns.Update(pattern);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill pattern {Id}", id);
                throw;
            }
        }

        #endregion

        #region Blast Sequence Operations (Structured Storage)

        public async Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            try
            {
                var sequences = await _context.BlastSequences
                    .Include(s => s.CreatedBy)
                    .Include(s => s.DrillPattern)
                    .Where(s => s.ProjectId == projectId && s.SiteId == siteId && s.IsActive)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync();

                return sequences.Select(s => ConvertToDto(s)).ToList();
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
                var sequence = await _context.BlastSequences
                    .Include(s => s.CreatedBy)
                    .Include(s => s.DrillPattern)
                    .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

                return sequence != null ? ConvertToDto(sequence) : null;
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
                if (!await ValidateProjectSiteExistsAsync(request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid project or site ID");
                }

                if (!await ValidateDrillPatternOwnershipAsync(request.DrillPatternId, request.ProjectId, request.SiteId))
                {
                    throw new ArgumentException("Invalid drill pattern ID for this project/site");
                }

                var sequence = new BlastSequence
                {
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId,
                    DrillPatternId = request.DrillPatternId,
                    Name = request.Name,
                    Description = request.Description,
                    ConnectionsJson = request.ConnectionsJson,
                    SimulationSettingsJson = request.SimulationSettingsJson ?? "{}",
                    CreatedByUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.BlastSequences.Add(sequence);
                await _context.SaveChangesAsync();

                await _context.Entry(sequence)
                    .Reference(s => s.CreatedBy)
                    .LoadAsync();
                await _context.Entry(sequence)
                    .Reference(s => s.DrillPattern)
                    .LoadAsync();

                return ConvertToDto(sequence);
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
                var sequence = await _context.BlastSequences
                    .Include(s => s.CreatedBy)
                    .Include(s => s.DrillPattern)
                    .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

                if (sequence == null)
                {
                    throw new ArgumentException("Blast sequence not found");
                }

                if (!await ValidateBlastSequenceOwnershipAsync(id, request.ProjectId, request.SiteId))
                {
                    throw new UnauthorizedAccessException("Access denied to this blast sequence");
                }

                sequence.Name = request.Name;
                sequence.Description = request.Description;
                sequence.ConnectionsJson = request.ConnectionsJson;
                sequence.SimulationSettingsJson = request.SimulationSettingsJson ?? "{}";
                sequence.UpdatedAt = DateTime.UtcNow;

                _context.BlastSequences.Update(sequence);
                await _context.SaveChangesAsync();

                return ConvertToDto(sequence);
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
                var sequence = await _context.BlastSequences
                    .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

                if (sequence == null) return false;

                // Soft delete
                sequence.IsActive = false;
                sequence.UpdatedAt = DateTime.UtcNow;

                _context.BlastSequences.Update(sequence);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blast sequence {Id}", id);
                throw;
            }
        }

        #endregion

        #region Workflow Progress Tracking

        public async Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId)
        {
            try
            {
                var allData = await GetAllSiteDataAsync(projectId, siteId);
                var site = await _context.ProjectSites.FindAsync(siteId);

                var progress = new SiteWorkflowProgressDto
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    SiteName = site?.Name ?? "Unknown Site"
                };

                // Check pattern data
                var patternData = allData.FirstOrDefault(d => d.DataType == "pattern");
                if (patternData != null)
                {
                    progress.PatternCreator.Completed = true;
                    progress.PatternCreator.Progress = 100;
                    progress.PatternCreator.LastModified = patternData.UpdatedAt;
                    progress.PatternCreator.LastModifiedBy = patternData.CreatedByName;
                }

                // Check connections data - just check if data exists and is not empty
                var connectionsData = allData.FirstOrDefault(d => d.DataType == "connections");
                if (connectionsData != null && !string.IsNullOrWhiteSpace(connectionsData.JsonData) && connectionsData.JsonData != "{}" && connectionsData.JsonData != "[]")
                {
                    progress.SequenceDesigner.Completed = true;
                    progress.SequenceDesigner.Progress = 100;
                    progress.SequenceDesigner.LastModified = connectionsData.UpdatedAt;
                    progress.SequenceDesigner.LastModifiedBy = connectionsData.CreatedByName;
                }

                // Check simulation settings
                var simulationData = allData.FirstOrDefault(d => d.DataType == "simulation_settings");
                if (simulationData != null)
                {
                    progress.Simulator.Completed = true;
                    progress.Simulator.Progress = 100;
                    progress.Simulator.LastModified = simulationData.UpdatedAt;
                    progress.Simulator.LastModifiedBy = simulationData.CreatedByName;
                }

                // Calculate overall progress
                var totalSteps = 3;
                var completedSteps = new[] { progress.PatternCreator, progress.SequenceDesigner, progress.Simulator }
                    .Count(step => step.Completed);
                progress.OverallProgress = (completedSteps * 100) / totalSteps;

                // Last updated is the most recent modification
                var lastUpdated = allData.OrderByDescending(d => d.UpdatedAt).FirstOrDefault()?.UpdatedAt ?? DateTime.MinValue;
                progress.LastUpdated = lastUpdated;

                return progress;
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
                // This method is for external progress updates if needed
                // Most progress tracking happens automatically when data is saved
                return await GetWorkflowProgressAsync(projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow progress for project {ProjectId}, site {SiteId}, step {StepId}", 
                    projectId, siteId, stepId);
                throw;
            }
        }

        #endregion

        #region Validation Methods

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            try
            {
                return await _context.ProjectSites
                    .AnyAsync(ps => ps.Id == siteId && ps.ProjectId == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating project site exists: project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                return false;
            }
        }

        public async Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId)
        {
            try
            {
                return await _context.DrillPatterns
                    .AnyAsync(p => p.Id == patternId && p.ProjectId == projectId && p.SiteId == siteId && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating drill pattern ownership: pattern {PatternId}, project {ProjectId}, site {SiteId}", 
                    patternId, projectId, siteId);
                return false;
            }
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            try
            {
                return await _context.BlastSequences
                    .AnyAsync(s => s.Id == sequenceId && s.ProjectId == projectId && s.SiteId == siteId && s.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating blast sequence ownership: sequence {SequenceId}, project {ProjectId}, site {SiteId}", 
                    sequenceId, projectId, siteId);
                return false;
            }
        }

        #endregion

        #region Helper Methods

        private static DrillPatternDto ConvertToDto(DrillPattern pattern)
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

        private static BlastSequenceDto ConvertToDto(BlastSequence sequence)
        {
            var dto = new BlastSequenceDto
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

            // Add drill pattern info if available for context
            if (sequence.DrillPattern != null)
            {
                dto.DrillPattern = ConvertToDto(sequence.DrillPattern);
            }

            return dto;
        }

        #endregion
    }
} 