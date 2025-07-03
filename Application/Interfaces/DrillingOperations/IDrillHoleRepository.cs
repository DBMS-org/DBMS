using Domain.Entities.DrillingOperations;

namespace Application.Interfaces.DrillingOperations
{
    public interface IDrillHoleRepository
    {
        Task<IEnumerable<DrillHole>> GetAllAsync();
        Task<DrillHole?> GetByIdAsync(string id);
        Task<DrillHole> AddAsync(DrillHole drillHole);
        Task UpdateAsync(DrillHole drillHole);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task AddRangeAsync(IEnumerable<DrillHole> drillHoles);
        Task ClearAllAsync();

        Task<IEnumerable<DrillHole>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<DrillHole>> GetBySiteIdAsync(int projectId, int siteId);
        Task DeleteByProjectIdAsync(int projectId);
        Task DeleteBySiteIdAsync(int projectId, int siteId);

        Task<int> GetCountAsync();
        Task<int> GetCountByProjectIdAsync(int projectId);
        Task<int> GetCountBySiteIdAsync(int projectId, int siteId);
    }
} 
