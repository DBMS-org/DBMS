using Domain.Entities.DrillingOperations;

namespace Application.Interfaces.DrillingOperations
{
    public interface IDrillPointRepository
    {
        // Basic CRUD operations
        Task<DrillPoint> AddAsync(DrillPoint drillPoint);
        Task<DrillPoint?> GetByIdAsync(string id, int projectId, int siteId);
        Task<List<DrillPoint>> GetAllAsync(int projectId, int siteId);
        Task<DrillPoint> UpdateAsync(DrillPoint drillPoint);
        Task<bool> DeleteAsync(string id, int projectId, int siteId);
        Task<bool> DeleteAllAsync(int projectId, int siteId);
        Task<bool> DeleteByProjectSiteAsync(int projectId, int siteId);
        
        // Bulk operations
        Task<List<DrillPoint>> AddRangeAsync(IEnumerable<DrillPoint> drillPoints);
        
        // Queries
        Task<bool> ExistsAsync(string id, int projectId, int siteId);
        Task<bool> ExistsAtCoordinatesAsync(double x, double y, int projectId, int siteId, string? excludePointId = null);
        Task<int> GetCountAsync(int projectId, int siteId);
        Task<List<DrillPoint>> GetByCoordinateRangeAsync(int projectId, int siteId, double minX, double maxX, double minY, double maxY);
        
        // Pattern settings (stored separately from drill points)
        Task<PatternSettings?> GetPatternSettingsAsync(int projectId, int siteId);
        Task<PatternSettings> SavePatternSettingsAsync(int projectId, int siteId, PatternSettings settings);

        // Completion tracking
        Task<bool> MarkAsCompletedAsync(string id, int projectId, int siteId, int completedByUserId);
        Task<bool> IsCompletedAsync(string id, int projectId, int siteId);
    }
} 