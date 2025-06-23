using Core.DTOs;

namespace Core.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync();
        Task<RegionDto?> GetRegionByIdAsync(int id);
        Task<RegionDto?> GetRegionByNameAsync(string name);
    }
} 