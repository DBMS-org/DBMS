using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.ExplosiveInventory
{
    /// <summary>
    /// Repository interface for Central Warehouse Inventory
    /// </summary>
    public interface ICentralWarehouseInventoryRepository
    {
        // Basic CRUD
        Task<CentralWarehouseInventory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CentralWarehouseInventory?> GetByBatchIdAsync(string batchId, CancellationToken cancellationToken = default);
        Task<List<CentralWarehouseInventory>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default);
        Task UpdateAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default);
        Task DeleteAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default);

        // Queries
        Task<List<CentralWarehouseInventory>> GetByExplosiveTypeAsync(ExplosiveType type, CancellationToken cancellationToken = default);
        Task<List<CentralWarehouseInventory>> GetByStatusAsync(InventoryStatus status, CancellationToken cancellationToken = default);
        Task<List<CentralWarehouseInventory>> GetExpiringBatchesAsync(int daysThreshold, CancellationToken cancellationToken = default);
        Task<List<CentralWarehouseInventory>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default);
        Task<List<CentralWarehouseInventory>> GetBySupplierAsync(string supplier, CancellationToken cancellationToken = default);

        // Filtered & Paged
        Task<(List<CentralWarehouseInventory> items, int totalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            ExplosiveType? explosiveType = null,
            InventoryStatus? status = null,
            string? supplier = null,
            string? batchId = null,
            bool? isExpired = null,
            bool? isExpiringSoon = null,
            DateTime? manufacturingDateFrom = null,
            DateTime? manufacturingDateTo = null,
            DateTime? expiryDateFrom = null,
            DateTime? expiryDateTo = null,
            string? sortBy = null,
            bool sortDescending = false,
            CancellationToken cancellationToken = default);

        // Checks
        Task<bool> ExistsAsync(string batchId, CancellationToken cancellationToken = default);
    }
}
