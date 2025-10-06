using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.Interfaces.ExplosiveInventory
{
    /// <summary>
    /// Repository interface for Inventory Transfer Requests
    /// </summary>
    public interface IInventoryTransferRequestRepository
    {
        // Basic CRUD
        Task<InventoryTransferRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<InventoryTransferRequest?> GetByRequestNumberAsync(string requestNumber, CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default);
        Task UpdateAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default);
        Task DeleteAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default);

        // Queries
        Task<List<InventoryTransferRequest>> GetByStatusAsync(TransferRequestStatus status, CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetPendingRequestsAsync(CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetUrgentRequestsAsync(CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetByDestinationStoreAsync(int storeId, CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetByUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<List<InventoryTransferRequest>> GetByInventoryBatchAsync(int inventoryId, CancellationToken cancellationToken = default);

        // Filtered & Paged
        Task<(List<InventoryTransferRequest> items, int totalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            TransferRequestStatus? status = null,
            int? destinationStoreId = null,
            int? requestedByUserId = null,
            bool? isOverdue = null,
            bool? isUrgent = null,
            DateTime? requestDateFrom = null,
            DateTime? requestDateTo = null,
            DateTime? requiredByDateFrom = null,
            DateTime? requiredByDateTo = null,
            string? sortBy = null,
            bool sortDescending = false,
            CancellationToken cancellationToken = default);

        // Checks
        Task<bool> ExistsAsync(string requestNumber, CancellationToken cancellationToken = default);
        Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetApprovedCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetUrgentCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetOverdueCountAsync(CancellationToken cancellationToken = default);
    }
}
