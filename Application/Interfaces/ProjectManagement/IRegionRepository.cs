using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllActiveRegionsAsync();
        Task<Region?> GetByIdAsync(int id);
        Task<Region?> GetByNameAsync(string name);
        Task<Region> CreateAsync(Region region);
        Task<Region> UpdateAsync(Region region);
        Task<bool> DeleteAsync(int id);
    }
} 