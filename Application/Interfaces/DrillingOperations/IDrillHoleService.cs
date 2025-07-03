using Domain.Entities.DrillingOperations;
using Application.DTOs.Shared;

namespace Application.Interfaces.DrillingOperations
{
    public interface IDrillHoleService
    {
        Task<Result<IEnumerable<DrillHole>>> GetAllDrillHolesAsync();
        Task<Result<DrillHole>> GetDrillHoleByIdAsync(string id);
        Task<Result<DrillHole>> CreateDrillHoleAsync(DrillHole drillHole);
        Task<Result> UpdateDrillHoleAsync(DrillHole drillHole);
        Task<Result> DeleteDrillHoleAsync(string id);
        Task<Result<IEnumerable<DrillHole>>> GetDrillHolesByProjectIdAsync(int projectId);
        Task<Result<IEnumerable<DrillHole>>> GetDrillHolesBySiteIdAsync(int projectId, int siteId);
        Task<Result> DeleteDrillHolesByProjectIdAsync(int projectId);
        Task<Result> DeleteDrillHolesBySiteIdAsync(int projectId, int siteId);
        Task<Result<int>> GetDrillHoleCountAsync();
        Task<Result<int>> GetDrillHoleCountByProjectIdAsync(int projectId);
        Task<Result<int>> GetDrillHoleCountBySiteIdAsync(int projectId, int siteId);
    }
} 
