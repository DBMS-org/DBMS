using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDrillHoleRepository
    {
        Task<IEnumerable<DrillHole>> GetAllAsync();
        Task<DrillHole?> GetByIdAsync(int id);
        Task<DrillHole> AddAsync(DrillHole drillHole);
        Task UpdateAsync(DrillHole drillHole);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddRangeAsync(IEnumerable<DrillHole> drillHoles);
        Task ClearAllAsync();

        Task<IEnumerable<DrillHole>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<DrillHole>> GetBySiteIdAsync(int projectId, int siteId);
        Task DeleteByProjectIdAsync(int projectId);
        Task DeleteBySiteIdAsync(int projectId, int siteId);

        Task<int> GetCountAsync();
        Task<int> GetCountByProjectIdAsync(int projectId);
        Task<int> GetCountBySiteIdAsync(int projectId, int siteId);
        
        // Additional methods for DrillHoleId string lookup
        Task<DrillHole?> GetByDrillHoleIdAsync(string drillHoleId);
        Task<bool> ExistsByDrillHoleIdAsync(string drillHoleId);
    }
} 
