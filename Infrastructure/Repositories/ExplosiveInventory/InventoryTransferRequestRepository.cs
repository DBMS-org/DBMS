using Application.Interfaces.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ExplosiveInventory
{
    /// <summary>
    /// Repository implementation for Inventory Transfer Requests
    /// </summary>
    public class InventoryTransferRequestRepository : IInventoryTransferRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryTransferRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Basic CRUD =====

        public async Task<InventoryTransferRequest?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                    .ThenInclude(x => x.ANFOProperties)
                .Include(x => x.CentralInventory)
                    .ThenInclude(x => x.EmulsionProperties)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Include(x => x.ApprovedByUser)
                .Include(x => x.ProcessedByUser)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<InventoryTransferRequest?> GetByRequestNumberAsync(
            string requestNumber,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Include(x => x.ApprovedByUser)
                .Include(x => x.ProcessedByUser)
                .FirstOrDefaultAsync(x => x.RequestNumber == requestNumber, cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default)
        {
            await _context.Set<InventoryTransferRequest>().AddAsync(request, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default)
        {
            request.MarkUpdated();
            _context.Set<InventoryTransferRequest>().Update(request);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(InventoryTransferRequest request, CancellationToken cancellationToken = default)
        {
            request.Deactivate();
            _context.Set<InventoryTransferRequest>().Update(request);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // ===== Queries =====

        public async Task<List<InventoryTransferRequest>> GetByStatusAsync(
            TransferRequestStatus status,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.Status == status && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetPendingRequestsAsync(CancellationToken cancellationToken = default)
        {
            return await GetByStatusAsync(TransferRequestStatus.Pending, cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetUrgentRequestsAsync(CancellationToken cancellationToken = default)
        {
            var sevenDaysFromNow = DateTime.UtcNow.AddDays(7);

            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.IsActive &&
                           x.Status == TransferRequestStatus.Pending &&
                           x.RequiredByDate.HasValue &&
                           x.RequiredByDate.Value <= sevenDaysFromNow)
                .OrderBy(x => x.RequiredByDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.IsActive &&
                           x.RequiredByDate.HasValue &&
                           x.RequiredByDate.Value < now &&
                           x.Status != TransferRequestStatus.Completed &&
                           x.Status != TransferRequestStatus.Cancelled &&
                           x.Status != TransferRequestStatus.Rejected)
                .OrderBy(x => x.RequiredByDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetByDestinationStoreAsync(
            int storeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.DestinationStoreId == storeId && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetByUserAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.RequestedByUserId == userId && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<InventoryTransferRequest>> GetByInventoryBatchAsync(
            int inventoryId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Where(x => x.CentralWarehouseInventoryId == inventoryId && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        // ===== Filtered & Paged =====

        public async Task<(List<InventoryTransferRequest> items, int totalCount)> GetPagedAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<InventoryTransferRequest>()
                .Include(x => x.CentralInventory)
                .Include(x => x.DestinationStore)
                .Include(x => x.RequestedByUser)
                .Include(x => x.ApprovedByUser)
                .Where(x => x.IsActive)
                .AsQueryable();

            // Apply filters
            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (destinationStoreId.HasValue)
                query = query.Where(x => x.DestinationStoreId == destinationStoreId.Value);

            if (requestedByUserId.HasValue)
                query = query.Where(x => x.RequestedByUserId == requestedByUserId.Value);

            if (isOverdue.HasValue && isOverdue.Value)
            {
                var now = DateTime.UtcNow;
                query = query.Where(x => x.RequiredByDate.HasValue &&
                                        x.RequiredByDate.Value < now &&
                                        x.Status != TransferRequestStatus.Completed &&
                                        x.Status != TransferRequestStatus.Cancelled &&
                                        x.Status != TransferRequestStatus.Rejected);
            }

            if (isUrgent.HasValue && isUrgent.Value)
            {
                var threshold = DateTime.UtcNow.AddDays(7);
                query = query.Where(x => x.RequiredByDate.HasValue &&
                                        x.RequiredByDate.Value <= threshold &&
                                        x.Status == TransferRequestStatus.Pending);
            }

            if (requestDateFrom.HasValue)
                query = query.Where(x => x.RequestDate >= requestDateFrom.Value);

            if (requestDateTo.HasValue)
                query = query.Where(x => x.RequestDate <= requestDateTo.Value);

            if (requiredByDateFrom.HasValue)
                query = query.Where(x => x.RequiredByDate.HasValue && x.RequiredByDate.Value >= requiredByDateFrom.Value);

            if (requiredByDateTo.HasValue)
                query = query.Where(x => x.RequiredByDate.HasValue && x.RequiredByDate.Value <= requiredByDateTo.Value);

            // Apply sorting
            query = ApplySorting(query, sortBy, sortDescending);

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        // ===== Checks =====

        public async Task<bool> ExistsAsync(string requestNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .AnyAsync(x => x.RequestNumber == requestNumber && x.IsActive, cancellationToken);
        }

        public async Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .CountAsync(x => x.Status == TransferRequestStatus.Pending && x.IsActive, cancellationToken);
        }

        public async Task<int> GetApprovedCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<InventoryTransferRequest>()
                .CountAsync(x => x.Status == TransferRequestStatus.Approved && x.IsActive, cancellationToken);
        }

        public async Task<int> GetUrgentCountAsync(CancellationToken cancellationToken = default)
        {
            var threshold = DateTime.UtcNow.AddDays(7);
            return await _context.Set<InventoryTransferRequest>()
                .CountAsync(x => x.IsActive &&
                                x.Status == TransferRequestStatus.Pending &&
                                x.RequiredByDate.HasValue &&
                                x.RequiredByDate.Value <= threshold, cancellationToken);
        }

        public async Task<int> GetOverdueCountAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _context.Set<InventoryTransferRequest>()
                .CountAsync(x => x.IsActive &&
                                x.RequiredByDate.HasValue &&
                                x.RequiredByDate.Value < now &&
                                x.Status != TransferRequestStatus.Completed &&
                                x.Status != TransferRequestStatus.Cancelled &&
                                x.Status != TransferRequestStatus.Rejected, cancellationToken);
        }

        // ===== Private Helpers =====

        private IQueryable<InventoryTransferRequest> ApplySorting(
            IQueryable<InventoryTransferRequest> query,
            string? sortBy,
            bool sortDescending)
        {
            return sortBy?.ToLower() switch
            {
                "requestnumber" => sortDescending ? query.OrderByDescending(x => x.RequestNumber) : query.OrderBy(x => x.RequestNumber),
                "requestdate" => sortDescending ? query.OrderByDescending(x => x.RequestDate) : query.OrderBy(x => x.RequestDate),
                "requiredbydate" => sortDescending ? query.OrderByDescending(x => x.RequiredByDate) : query.OrderBy(x => x.RequiredByDate),
                "status" => sortDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
                "requestedquantity" => sortDescending ? query.OrderByDescending(x => x.RequestedQuantity) : query.OrderBy(x => x.RequestedQuantity),
                _ => sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt)
            };
        }
    }
}
