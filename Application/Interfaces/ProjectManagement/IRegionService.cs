using Domain.Entities.ProjectManagement;

namespace Application.Interfaces.ProjectManagement
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task<Region?> GetRegionByIdAsync(int id);
        Task<Region?> GetRegionByNameAsync(string name);
    }
} 
