using Microsoft.Extensions.Logging;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;
using Application.Interfaces.DrillingOperations;
using Application.Interfaces.BlastingOperations;
using Application.Interfaces.ProjectManagement;
using Application.Interfaces.Infrastructure;
using Application.Utilities;
using Domain.Entities.DrillingOperations;
using Domain.Services;
using Domain.Exceptions;

namespace Application.Services.DrillingOperations
{
    public class DrillPointPatternApplicationService : IDrillPointPatternService
    {
        private readonly IDrillPointRepository _drillPointRepository;
        private readonly ISiteBlastingService _siteBlastingService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserContext _userContext;
        private readonly DrillPointDomainService _domainService;
        private readonly ILogger<DrillPointPatternApplicationService> _logger;
        
        private const int MAX_DRILL_POINTS = 500;
        private const int COORDINATE_PRECISION_DECIMAL_PLACES = 2;
        
        public DrillPointPatternApplicationService(
            IDrillPointRepository drillPointRepository,
            ISiteBlastingService siteBlastingService,
            IProjectRepository projectRepository,
            IUserContext userContext,
            DrillPointDomainService domainService,
            ILogger<DrillPointPatternApplicationService> logger)
        {
            _drillPointRepository = drillPointRepository;
            _siteBlastingService = siteBlastingService;
            _projectRepository = projectRepository;
            _userContext = userContext;
            _domainService = domainService;
            _logger = logger;
        }
        
        private async Task<bool> ValidateRegionAccessAsync(int projectId)
        {
            if (_userContext.IsInRole("Admin"))
            {
                return true; // Admins have access to all regions
            }

            if (_userContext.IsInRole("BlastingEngineer") || _userContext.IsInRole("Operator"))
            {
                var region = _userContext.Region;
                if (!string.IsNullOrEmpty(region))
                {
                    var project = await _projectRepository.GetByIdAsync(projectId);
                    return project != null && project.Region == region;
                }
            }

            return false;
        }
        
        public async Task<DrillPointDto> CreateDrillPointAsync(CreateDrillPointRequest request)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(request.ProjectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {request.ProjectId}");
                }

                // Validate coordinates
                if (!_domainService.ValidateCoordinates(request.X, request.Y))
                {
                    throw new InvalidCoordinatesException(request.X, request.Y);
                }
                
                // Check max points limit
                var currentCount = await _drillPointRepository.GetCountAsync(request.ProjectId, request.SiteId);
                if (!_domainService.ValidateDrillPointCount(currentCount, MAX_DRILL_POINTS))
                {
                    throw new MaxDrillPointsExceededException(MAX_DRILL_POINTS);
                }
                
                // Check for duplicates
                var existingPoints = await _drillPointRepository.GetAllAsync(request.ProjectId, request.SiteId);
                if (!_domainService.ValidateUniqueCoordinates(request.X, request.Y, existingPoints))
                {
                    throw new DuplicateCoordinatesException(request.X, request.Y);
                }
                
                // Generate unique ID
                var nextId = await GenerateNextDrillPointIdAsync(request.ProjectId, request.SiteId);
                
                var drillPoint = new DrillPoint
                {
                    Id = nextId,
                    X = Math.Round(request.X, COORDINATE_PRECISION_DECIMAL_PLACES),
                    Y = Math.Round(request.Y, COORDINATE_PRECISION_DECIMAL_PLACES),
                    Depth = request.Depth,
                    Spacing = request.Spacing,
                    Burden = request.Burden,
                    Diameter = request.Diameter,
                    Stemming = request.Stemming,
                    Subdrill = request.Subdrill,
                    Volume = request.Volume,
                    ANFO = request.ANFO,
                    Emulsion = request.Emulsion,
                    ProjectId = request.ProjectId,
                    SiteId = request.SiteId
                };
                
                // Calculate optimal values if not provided
                if (drillPoint.Diameter <= 0)
                {
                    drillPoint.Diameter = _domainService.CalculateOptimalDiameter(drillPoint.Burden, drillPoint.Spacing);
                }
                
                if (drillPoint.Stemming <= 0)
                {
                    drillPoint.Stemming = _domainService.CalculateOptimalStemming(drillPoint.Depth, drillPoint.Burden);
                }
                
                // Validate stemming and diameter
                if (!_domainService.ValidateStemming(drillPoint.Stemming, drillPoint.Depth, drillPoint.Burden))
                {
                    throw new ArgumentException($"Invalid stemming value: {drillPoint.Stemming}m for depth: {drillPoint.Depth}m and burden: {drillPoint.Burden}m");
                }
                
                if (!_domainService.ValidateDiameter(drillPoint.Diameter, drillPoint.Burden, drillPoint.Spacing))
                {
                    throw new ArgumentException($"Invalid diameter value: {drillPoint.Diameter}m for burden: {drillPoint.Burden}m and spacing: {drillPoint.Spacing}m");
                }
                
                var savedPoint = await _drillPointRepository.AddAsync(drillPoint);
                
                _logger.LogInformation("Drill point created: {PointId} at ({X}, {Y})", 
                    savedPoint.Id, savedPoint.X, savedPoint.Y);
                    
                return ConvertToDto(savedPoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drill point for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }
        
        public async Task<bool> UpdateDrillPointPositionAsync(UpdateDrillPointPositionRequest request)
        {
            try
            {
                var existingPoint = await _drillPointRepository.GetByIdAsync(
                    request.PointId, 
                    request.ProjectId, 
                    request.SiteId);
                    
                if (existingPoint == null)
                {
                    throw new DrillPointNotFoundException(request.PointId);
                }

                // Validate region access
                if (!await ValidateRegionAccessAsync(existingPoint.ProjectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {existingPoint.ProjectId}");
                }
                
                // Validate new coordinates
                if (!_domainService.ValidateCoordinates(request.X, request.Y))
                {
                    throw new InvalidCoordinatesException(request.X, request.Y);
                }
                
                // Check for duplicates (excluding current point)
                var otherPoints = (await _drillPointRepository.GetAllAsync(existingPoint.ProjectId, existingPoint.SiteId))
                    .Where(p => p.Id != request.PointId);
                    
                if (!_domainService.ValidateUniqueCoordinates(request.X, request.Y, otherPoints))
                {
                    throw new DuplicateCoordinatesException(request.X, request.Y);
                }
                
                existingPoint.X = Math.Round(request.X, COORDINATE_PRECISION_DECIMAL_PLACES);
                existingPoint.Y = Math.Round(request.Y, COORDINATE_PRECISION_DECIMAL_PLACES);
                existingPoint.UpdatedAt = DateTime.UtcNow;
                
                await _drillPointRepository.UpdateAsync(existingPoint);
                
                _logger.LogInformation("Drill point position updated: {PointId} to ({X}, {Y})", 
                    request.PointId, request.X, request.Y);
                    
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating drill point position: {PointId}", request.PointId);
                throw;
            }
        }
        
        public async Task<bool> RemoveDrillPointAsync(string pointId, int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var result = await _drillPointRepository.DeleteAsync(pointId, projectId, siteId);
                
                if (result)
                {
                    _logger.LogInformation("Drill point removed: {PointId}", pointId);
                }
                else
                {
                    _logger.LogWarning("Drill point not found for removal: {PointId}", pointId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing drill point: {PointId}", pointId);
                throw;
            }
        }
        
        public async Task<List<DrillPointDto>> GetDrillPointsAsync(int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var drillPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
                return drillPoints.Select(ConvertToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill points for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<DrillPointDto?> GetDrillPointAsync(string pointId, int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var drillPoint = await _drillPointRepository.GetByIdAsync(pointId, projectId, siteId);
                return drillPoint != null ? ConvertToDto(drillPoint) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting drill point: {PointId}", pointId);
                throw;
            }
        }
        
        public async Task<bool> ClearAllDrillPointsAsync(int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var result = await _drillPointRepository.DeleteAllAsync(projectId, siteId);
                
                if (result)
                {
                    _logger.LogInformation("All drill points cleared for project {ProjectId}, site {SiteId}", 
                        projectId, siteId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing drill points for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<PatternSettingsDto> GetPatternSettingsAsync(int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var settings = await _drillPointRepository.GetPatternSettingsAsync(projectId, siteId);
                return settings != null ? ConvertToDto(settings) : new PatternSettingsDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern settings for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task UpdatePatternSettingsAsync(int projectId, int siteId, PatternSettingsDto settingsDto)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var settings = new PatternSettings
                {
                    Spacing = settingsDto.Spacing,
                    Burden = settingsDto.Burden,
                    Depth = settingsDto.Depth
                };
                
                await _drillPointRepository.SavePatternSettingsAsync(projectId, siteId, settings);
                
                _logger.LogInformation("Pattern settings updated for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pattern settings for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<PatternDataDto> GetPatternDataAsync(int projectId, int siteId)
        {
            try
            {
                var drillPoints = await GetDrillPointsAsync(projectId, siteId);
                var settings = await GetPatternSettingsAsync(projectId, siteId);
                
                return new PatternDataDto
                {
                    DrillPoints = drillPoints,
                    Settings = settings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pattern data for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<bool> SavePatternAsync(int projectId, int siteId, List<DrillPointDto> drillPoints, string patternName)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var currentUserId = GetCurrentUserId();
                if (!currentUserId.HasValue)
                {
                    _logger.LogError("Current user ID not found in context");
                    return false;
                }

                _logger.LogInformation("Starting SavePatternAsync for project {ProjectId}, site {SiteId} with {Count} drill points", 
                    projectId, siteId, drillPoints.Count);

                // Log the drill points being saved including explosive properties
                foreach (var dp in drillPoints)
                {
                    _logger.LogInformation("Drill Point: Id={Id}, X={X}, Y={Y}, Depth={Depth}, Spacing={Spacing}, Burden={Burden}, Volume={Volume}, ANFO={ANFO}, Emulsion={Emulsion}", 
                        dp.Id, dp.X, dp.Y, dp.Depth, dp.Spacing, dp.Burden, dp.Volume, dp.ANFO, dp.Emulsion);
                }

                // Clear existing drill points for this project/site
                await _drillPointRepository.DeleteByProjectSiteAsync(projectId, siteId);
                _logger.LogInformation("Cleared existing drill points for project {ProjectId}, site {SiteId}", projectId, siteId);

                // Convert DTOs to entities and save including explosive properties
                var drillPointEntities = drillPoints.Select(dp => new DrillPoint
                {
                    Id = dp.Id,
                    ProjectId = projectId,
                    SiteId = siteId,
                    X = dp.X,
                    Y = dp.Y,
                    Depth = dp.Depth,
                    Spacing = dp.Spacing,
                    Burden = dp.Burden,
                    Diameter = dp.Diameter,
                    Stemming = dp.Stemming,
                    Subdrill = dp.Subdrill,
                    // Include explosive calculation properties
                    Volume = dp.Volume,
                    ANFO = dp.ANFO,
                    Emulsion = dp.Emulsion,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                await _drillPointRepository.AddRangeAsync(drillPointEntities);

                _logger.LogInformation("Successfully saved {Count} drill points for pattern '{PatternName}'", 
                    drillPointEntities.Count, patternName);

                // Verify the data was saved by reading it back
                var savedPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
                _logger.LogInformation("Verification: Found {Count} drill points after save", savedPoints.Count);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving pattern '{PatternName}' for project {ProjectId}, site {SiteId}", 
                    patternName, projectId, siteId);
                return false;
            }
        }
        
        public async Task<PatternDataDto> ExportPatternForBlastDesignerAsync(int projectId, int siteId)
        {
            return await GetPatternDataAsync(projectId, siteId);
        }
        
        public async Task<PatternDataDto> ProcessUploadedCsvDataAsync(ProcessCsvDataRequest request)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(request.ProjectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {request.ProjectId}");
                }

                if (!request.CsvData.Any())
                {
                    throw new ArgumentException("No CSV data provided");
                }
                
                // Clear existing drill points
                await ClearAllDrillPointsAsync(request.ProjectId, request.SiteId);
                
                // Convert CSV data to drill points
        var drillPoints = ConvertCsvDataToDrillPoints(request.CsvData, request.ProjectId, request.SiteId);
        
        // Use domain service to anchor points to origin
        var anchoredPoints = _domainService.AnchorPointsToOrigin(drillPoints);
        
        // Calculate grid pitch using domain service
        var (spacing, burden) = _domainService.CalculateGridPitch(anchoredPoints);
        
        // Calculate stemming and diameter for points that don't have them
        var enhancedPoints = anchoredPoints.Select(point => 
        {
            var enhancedPoint = new DrillPoint
            {
                Id = point.Id,
                X = point.X,
                Y = point.Y,
                Depth = point.Depth,
                Spacing = point.Spacing,
                Burden = point.Burden,
                Diameter = point.Diameter > 0 ? point.Diameter : _domainService.CalculateOptimalDiameter(point.Burden, point.Spacing),
                Stemming = point.Stemming > 0 ? point.Stemming : _domainService.CalculateOptimalStemming(point.Depth, point.Burden),
                ProjectId = point.ProjectId,
                SiteId = point.SiteId,
                CreatedAt = point.CreatedAt,
                UpdatedAt = point.UpdatedAt
            };
            
            return enhancedPoint;
        }).ToList();
                
                // Save drill points
        var savedPoints = await _drillPointRepository.AddRangeAsync(enhancedPoints);
                
                // Update pattern settings with calculated values
                var settings = new PatternSettingsDto
                {
                    Spacing = spacing,
                    Burden = burden,
                    Depth = 10.0 // Default depth
                };
                
                await UpdatePatternSettingsAsync(request.ProjectId, request.SiteId, settings);
                
                _logger.LogInformation("CSV data processed: {Count} drill points, spacing: {Spacing}, burden: {Burden}", 
                    savedPoints.Count, spacing, burden);
                
                return new PatternDataDto
        {
            DrillPoints = savedPoints.Select(ConvertToDto).ToList(),
            Settings = settings
        };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSV data for project {ProjectId}, site {SiteId}", 
                    request.ProjectId, request.SiteId);
                throw;
            }
        }
        
        public async Task<(double spacing, double burden)> CalculateGridPitchAsync(int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var drillPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
                return _domainService.CalculateGridPitch(drillPoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating grid pitch for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<PatternDataDto> AnchorPatternToOriginAsync(int projectId, int siteId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var drillPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
                var anchoredPoints = _domainService.AnchorPointsToOrigin(drillPoints);
                
                // Save anchored points back to repository
                await _drillPointRepository.DeleteAllAsync(projectId, siteId);
                await _drillPointRepository.AddRangeAsync(anchoredPoints);
                
                var settings = await GetPatternSettingsAsync(projectId, siteId);
                
                return new PatternDataDto
                {
                    DrillPoints = anchoredPoints.Select(ConvertToDto).ToList(),
                    Settings = settings
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anchoring pattern to origin for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<bool> ValidateCoordinatesAsync(double x, double y)
        {
            return await Task.FromResult(_domainService.ValidateCoordinates(x, y));
        }
        
        public async Task<bool> ValidateUniqueCoordinatesAsync(double x, double y, int projectId, int siteId, string? excludePointId = null)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var existingPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
                
                if (!string.IsNullOrEmpty(excludePointId))
                {
                    existingPoints = existingPoints.Where(p => p.Id != excludePointId).ToList();
                }
                
                return _domainService.ValidateUniqueCoordinates(x, y, existingPoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating unique coordinates for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        public async Task<bool> ValidateDrillPointCountAsync(int projectId, int siteId, int maxPoints)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var currentCount = await _drillPointRepository.GetCountAsync(projectId, siteId);
                return _domainService.ValidateDrillPointCount(currentCount, maxPoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating drill point count for project {ProjectId}, site {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }
        
        private async Task<string> GenerateNextDrillPointIdAsync(int projectId, int siteId)
        {
            var existingPoints = await _drillPointRepository.GetAllAsync(projectId, siteId);
            
            if (!existingPoints.Any())
            {
                return "DH1";
            }
            
            var maxId = existingPoints
                .Select(p => p.Id)
                .Where(id => id.StartsWith("DH"))
                .Select(id => SafeDataConverter.ExtractIdFromString(id, "DH", 0))
                .DefaultIfEmpty(0)
                .Max();
                
            return $"DH{maxId + 1}";
        }
        
        private List<DrillPoint> ConvertCsvDataToDrillPoints(List<DrillHoleDataDto> csvData, int projectId, int siteId)
        {
            return csvData.Select((hole, index) => new DrillPoint
            {
                Id = !string.IsNullOrEmpty(hole.Id?.ToString()) ? hole.Id.ToString() : Guid.NewGuid().ToString(),
                X = hole.Easting ?? 0,
                Y = hole.Northing ?? 0,
                Depth = hole.Depth ?? hole.Length ?? 10.0,
                Spacing = 3.0, // Default, will be updated after calculation
                Burden = 2.5,  // Default, will be updated after calculation
                Diameter = 0.0, // Will be calculated if not provided
                Stemming = 0.0, // Will be calculated
                ProjectId = projectId,
                SiteId = siteId
            }).ToList();
        }
        
        private static DrillPointDto ConvertToDto(DrillPoint drillPoint)
        {
            return new DrillPointDto
            {
                Id = drillPoint.Id,
                X = drillPoint.X,
                Y = drillPoint.Y,
                Depth = drillPoint.Depth,
                Spacing = drillPoint.Spacing,
                Burden = drillPoint.Burden,
                Diameter = drillPoint.Diameter,
                Stemming = drillPoint.Stemming,
                Subdrill = drillPoint.Subdrill,
                Volume = drillPoint.Volume,
                ANFO = drillPoint.ANFO,
                Emulsion = drillPoint.Emulsion,
                ProjectId = drillPoint.ProjectId,
                SiteId = drillPoint.SiteId,
                IsCompleted = drillPoint.IsCompleted,
                CompletedAt = drillPoint.CompletedAt,
                CompletedByUserId = drillPoint.CompletedByUserId,
                CreatedAt = drillPoint.CreatedAt,
                UpdatedAt = drillPoint.UpdatedAt
            };
        }
        
        private static PatternSettingsDto ConvertToDto(PatternSettings settings)
        {
            return new PatternSettingsDto
            {
                Spacing = settings.Spacing,
                Burden = settings.Burden,
                Depth = settings.Depth,
                Diameter = settings.Diameter,
                Stemming = settings.Stemming
            };
        }
        
        private Guid? GetCurrentUserId()
        {
            // Implement the logic to retrieve the current user ID from the context
            // This is a placeholder and should be replaced with the actual implementation
            return Guid.NewGuid(); // Placeholder return, actual implementation needed
        }

        public async Task<bool> MarkDrillPointAsCompletedAsync(string pointId, int projectId, int siteId, int completedByUserId)
        {
            try
            {
                // Validate region access
                if (!await ValidateRegionAccessAsync(projectId))
                {
                    throw new UnauthorizedAccessException($"Access denied to project {projectId}");
                }

                var result = await _drillPointRepository.MarkAsCompletedAsync(pointId, projectId, siteId, completedByUserId);

                if (result)
                {
                    _logger.LogInformation("Drill point {PointId} marked as completed by user {UserId}",
                        pointId, completedByUserId);
                }
                else
                {
                    _logger.LogWarning("Failed to mark drill point {PointId} as completed", pointId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking drill point {PointId} as completed", pointId);
                throw;
            }
        }
    }
}