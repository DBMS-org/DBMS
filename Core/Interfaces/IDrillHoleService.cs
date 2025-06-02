using Core.Entities;
using Core.DTOs;

namespace Core.Interfaces
{
    public interface IDrillHoleService
    {
        Task<IEnumerable<DrillHole>> GetAllDrillHolesAsync();
        Task<DrillHole?> GetDrillHoleByIdAsync(string id);
        Task<DrillHole> CreateDrillHoleAsync(DrillHole drillHole);
        Task UpdateDrillHoleAsync(DrillHole drillHole);
        Task DeleteDrillHoleAsync(string id);
        Task<IEnumerable<DrillHole>> CreateDrillHolesFromCsvAsync(CsvUploadRequest csvRequest);
    }
} 