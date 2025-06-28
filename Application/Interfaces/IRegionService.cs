using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync();
        Task<RegionDto?> GetRegionByIdAsync(int id);
        Task<RegionDto?> GetRegionByNameAsync(string name);
    }
} 
