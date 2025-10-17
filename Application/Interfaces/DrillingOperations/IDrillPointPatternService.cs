using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;

namespace Application.Interfaces.DrillingOperations
{
    public interface IDrillPointPatternService
    {
        // Drill point management
        Task<DrillPointDto> CreateDrillPointAsync(CreateDrillPointRequest request);
        Task<bool> UpdateDrillPointPositionAsync(UpdateDrillPointPositionRequest request);
        Task<bool> RemoveDrillPointAsync(string pointId, int projectId, int siteId);
        Task<List<DrillPointDto>> GetDrillPointsAsync(int projectId, int siteId);
        Task<DrillPointDto?> GetDrillPointAsync(string pointId, int projectId, int siteId);
        Task<bool> ClearAllDrillPointsAsync(int projectId, int siteId);
        
        // Pattern settings management
        Task<PatternSettingsDto> GetPatternSettingsAsync(int projectId, int siteId);
        Task UpdatePatternSettingsAsync(int projectId, int siteId, PatternSettingsDto settings);
        
        // Pattern data operations
        Task<PatternDataDto> GetPatternDataAsync(int projectId, int siteId);
        Task<bool> SavePatternAsync(int projectId, int siteId, List<DrillPointDto> drillPoints, string patternName);
        Task<PatternDataDto> ExportPatternForBlastDesignerAsync(int projectId, int siteId);
        
        // CSV data processing
        Task<PatternDataDto> ProcessUploadedCsvDataAsync(ProcessCsvDataRequest request);
        
        // Pattern analysis
        Task<(double spacing, double burden)> CalculateGridPitchAsync(int projectId, int siteId);
        Task<PatternDataDto> AnchorPatternToOriginAsync(int projectId, int siteId);
        
        // Validation
        Task<bool> ValidateCoordinatesAsync(double x, double y);
        Task<bool> ValidateUniqueCoordinatesAsync(double x, double y, int projectId, int siteId, string? excludePointId = null);
        Task<bool> ValidateDrillPointCountAsync(int projectId, int siteId, int maxPoints);
    }
} 