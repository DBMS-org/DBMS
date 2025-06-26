using Domain.Entities;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IDrillHoleService
    {
        Task<IEnumerable<DrillHole>> GetAllDrillHolesAsync();
        Task<DrillHole?> GetDrillHoleByIdAsync(int id);
        Task<DrillHole?> GetDrillHoleByDrillHoleIdAsync(string drillHoleId);
        Task<IEnumerable<DrillHole>> GetDrillHolesByProjectIdAsync(int projectId);
        Task<IEnumerable<DrillHole>> GetDrillHolesBySiteIdAsync(int projectId, int siteId);
        Task<DrillHole> CreateDrillHoleAsync(DrillHole drillHole);
        Task UpdateDrillHoleAsync(DrillHole drillHole);
        Task DeleteDrillHoleAsync(int id);
        Task<IEnumerable<DrillHole>> CreateDrillHolesFromCsvAsync(CsvUploadRequest csvRequest);
        Task DeleteDrillHolesByProjectIdAsync(int projectId);
        Task DeleteDrillHolesBySiteIdAsync(int projectId, int siteId);
        Task<int> GetDrillHoleCountAsync();
        Task<int> GetDrillHoleCountByProjectIdAsync(int projectId);
        Task<int> GetDrillHoleCountBySiteIdAsync(int projectId, int siteId);
    }
} 
