using Application.Common;
using Application.DTOs.ExplosiveInventory;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.ExplosiveInventory
{
    /// <summary>
    /// Service interface for central warehouse inventory management
    /// </summary>
    public interface ICentralInventoryService
    {
        // ===== ANFO Operations =====
        Task<Result<CentralInventoryDto>> CreateANFOBatchAsync(CreateANFOInventoryRequest request, CancellationToken cancellationToken = default);
        Task<Result<CentralInventoryDto>> UpdateANFOBatchAsync(int id, UpdateANFOInventoryRequest request, CancellationToken cancellationToken = default);

        // ===== Emulsion Operations =====
        Task<Result<CentralInventoryDto>> CreateEmulsionBatchAsync(CreateEmulsionInventoryRequest request, CancellationToken cancellationToken = default);
        Task<Result<CentralInventoryDto>> UpdateEmulsionBatchAsync(int id, UpdateEmulsionInventoryRequest request, CancellationToken cancellationToken = default);

        // ===== Query Operations =====
        Task<Result<PagedList<CentralInventoryDto>>> GetAllInventoryAsync(InventoryFilterDto filter, CancellationToken cancellationToken = default);
        Task<Result<CentralInventoryDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<CentralInventoryDto>> GetByBatchIdAsync(string batchId, CancellationToken cancellationToken = default);
        Task<Result<List<CentralInventoryDto>>> GetByExplosiveTypeAsync(ExplosiveType type, CancellationToken cancellationToken = default);
        Task<Result<List<CentralInventoryDto>>> GetExpiringBatchesAsync(int daysThreshold = 30, CancellationToken cancellationToken = default);
        Task<Result<List<CentralInventoryDto>>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default);

        // ===== Status Management =====
        Task<Result> QuarantineBatchAsync(int id, string reason, CancellationToken cancellationToken = default);
        Task<Result> ReleaseFromQuarantineAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> MarkAsExpiredAsync(int id, CancellationToken cancellationToken = default);

        // ===== Storage =====
        Task<Result> UpdateStorageLocationAsync(int id, string newLocation, CancellationToken cancellationToken = default);

        // ===== Dashboard =====
        Task<Result<InventoryDashboardDto>> GetDashboardDataAsync(CancellationToken cancellationToken = default);

        // ===== Delete =====
        Task<Result> DeleteBatchAsync(int id, CancellationToken cancellationToken = default);
    }
}
