using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task<Region?> GetRegionByIdAsync(int id);
        Task<Region?> GetRegionByNameAsync(string name);
    }
} 
