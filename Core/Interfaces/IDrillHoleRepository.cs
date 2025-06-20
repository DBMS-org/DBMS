using Core.Entities;

namespace Core.Interfaces
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
    }
} 