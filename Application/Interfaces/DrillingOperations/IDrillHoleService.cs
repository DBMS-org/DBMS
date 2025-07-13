using Domain.Entities.DrillingOperations;
using Application.DTOs.Shared;
using Application.DTOs.DrillingOperations;
using System.Threading;

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
        Task<Result> DeleteDrillHolesByProjectIdAsync(int projectId, CancellationToken cancellationToken = default);
        Task<Result> DeleteDrillHolesBySiteIdAsync(int projectId, int siteId, CancellationToken cancellationToken = default);
        Task<Result<int>> GetDrillHoleCountAsync();
        Task<Result<int>> GetDrillHoleCountByProjectIdAsync(int projectId);
        Task<Result<int>> GetDrillHoleCountBySiteIdAsync(int projectId, int siteId);
        
        // DTO-based methods for better frontend-backend mapping
        Task<Result<IEnumerable<DrillHoleDto>>> GetAllDrillHolesDtoAsync();
        Task<Result<DrillHoleDto>> GetDrillHoleDtoByIdAsync(string id);
        Task<Result<DrillHoleDto>> CreateDrillHoleFromDtoAsync(CreateDrillHoleRequest request);
        Task<Result> UpdateDrillHoleFromDtoAsync(string id, UpdateDrillHoleRequest request);
        Task<Result<IEnumerable<DrillHoleDto>>> GetDrillHolesDtoBySiteIdAsync(int projectId, int siteId);
    }
} 
