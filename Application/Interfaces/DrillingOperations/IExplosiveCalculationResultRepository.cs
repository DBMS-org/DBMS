using Domain.Entities.DrillingOperations;

namespace Application.Interfaces.DrillingOperations
{
    public interface IExplosiveCalculationResultRepository
    {
        // Basic CRUD operations
        Task<ExplosiveCalculationResult> AddAsync(ExplosiveCalculationResult calculationResult);
        Task<ExplosiveCalculationResult?> GetByIdAsync(int id);
        Task<ExplosiveCalculationResult?> GetByCalculationIdAsync(string calculationId);
        Task<List<ExplosiveCalculationResult>> GetAllAsync(int projectId, int siteId);
        Task<ExplosiveCalculationResult> UpdateAsync(ExplosiveCalculationResult calculationResult);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByCalculationIdAsync(string calculationId);
        Task<bool> DeleteAllAsync(int projectId, int siteId);
        
        // Queries
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByCalculationIdAsync(string calculationId);
        Task<int> GetCountAsync(int projectId, int siteId);
        Task<List<ExplosiveCalculationResult>> GetByProjectAsync(int projectId);
        Task<List<ExplosiveCalculationResult>> GetBySiteAsync(int projectId, int siteId);
        Task<List<ExplosiveCalculationResult>> GetByPatternSettingsAsync(int patternSettingsId);
        Task<List<ExplosiveCalculationResult>> GetByUserAsync(int userId);
        
        // Advanced queries
        Task<ExplosiveCalculationResult?> GetLatestByProjectSiteAsync(int projectId, int siteId);
        Task<List<ExplosiveCalculationResult>> GetByDateRangeAsync(int projectId, int siteId, DateTime startDate, DateTime endDate);
        Task<List<ExplosiveCalculationResult>> GetWithDrillPointsAsync(int projectId, int siteId);
        
        
    }
}