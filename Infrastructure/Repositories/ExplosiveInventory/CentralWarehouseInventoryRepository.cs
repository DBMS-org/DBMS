using Application.Interfaces.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ExplosiveInventory
{
    /// <summary>
    /// Repository implementation for Central Warehouse Inventory
    /// </summary>
    public class CentralWarehouseInventoryRepository : ICentralWarehouseInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CentralWarehouseInventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== Basic CRUD =====

        public async Task<CentralWarehouseInventory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Include(x => x.TransferRequests)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<CentralWarehouseInventory?> GetByBatchIdAsync(string batchId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .FirstOrDefaultAsync(x => x.BatchId == batchId, cancellationToken);
        }

        public async Task<List<CentralWarehouseInventory>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default)
        {
            await _context.Set<CentralWarehouseInventory>().AddAsync(inventory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default)
        {
            inventory.MarkUpdated();
            _context.Set<CentralWarehouseInventory>().Update(inventory);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(CentralWarehouseInventory inventory, CancellationToken cancellationToken = default)
        {
            inventory.Deactivate();
            _context.Set<CentralWarehouseInventory>().Update(inventory);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // ===== Queries =====

        public async Task<List<CentralWarehouseInventory>> GetByExplosiveTypeAsync(
            ExplosiveType type,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.ExplosiveType == type && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CentralWarehouseInventory>> GetByStatusAsync(
            InventoryStatus status,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.Status == status && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CentralWarehouseInventory>> GetExpiringBatchesAsync(
            int daysThreshold,
            CancellationToken cancellationToken = default)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.IsActive &&
                           x.ExpiryDate <= thresholdDate &&
                           x.ExpiryDate > DateTime.UtcNow)
                .OrderBy(x => x.ExpiryDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CentralWarehouseInventory>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.IsActive && x.ExpiryDate <= DateTime.UtcNow)
                .OrderBy(x => x.ExpiryDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CentralWarehouseInventory>> GetBySupplierAsync(
            string supplier,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.Supplier == supplier && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        // ===== Filtered & Paged =====

        public async Task<(List<CentralWarehouseInventory> items, int totalCount)> GetPagedAsync(
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
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<CentralWarehouseInventory>()
                .Include(x => x.ANFOProperties)
                .Include(x => x.EmulsionProperties)
                .Include(x => x.CentralWarehouse)
                .Where(x => x.IsActive)
                .AsQueryable();

            // Apply filters
            if (explosiveType.HasValue)
                query = query.Where(x => x.ExplosiveType == explosiveType.Value);

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(supplier))
                query = query.Where(x => x.Supplier.Contains(supplier));

            if (!string.IsNullOrWhiteSpace(batchId))
                query = query.Where(x => x.BatchId.Contains(batchId));

            if (isExpired.HasValue && isExpired.Value)
                query = query.Where(x => x.ExpiryDate <= DateTime.UtcNow);

            if (isExpiringSoon.HasValue && isExpiringSoon.Value)
            {
                var threshold = DateTime.UtcNow.AddDays(30);
                query = query.Where(x => x.ExpiryDate <= threshold && x.ExpiryDate > DateTime.UtcNow);
            }

            if (manufacturingDateFrom.HasValue)
                query = query.Where(x => x.ManufacturingDate >= manufacturingDateFrom.Value);

            if (manufacturingDateTo.HasValue)
                query = query.Where(x => x.ManufacturingDate <= manufacturingDateTo.Value);

            if (expiryDateFrom.HasValue)
                query = query.Where(x => x.ExpiryDate >= expiryDateFrom.Value);

            if (expiryDateTo.HasValue)
                query = query.Where(x => x.ExpiryDate <= expiryDateTo.Value);

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

        public async Task<bool> ExistsAsync(string batchId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<CentralWarehouseInventory>()
                .AnyAsync(x => x.BatchId == batchId && x.IsActive, cancellationToken);
        }

        // ===== Private Helpers =====

        private IQueryable<CentralWarehouseInventory> ApplySorting(
            IQueryable<CentralWarehouseInventory> query,
            string? sortBy,
            bool sortDescending)
        {
            return sortBy?.ToLower() switch
            {
                "batchid" => sortDescending ? query.OrderByDescending(x => x.BatchId) : query.OrderBy(x => x.BatchId),
                "explosivetype" => sortDescending ? query.OrderByDescending(x => x.ExplosiveType) : query.OrderBy(x => x.ExplosiveType),
                "quantity" => sortDescending ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity),
                "availablequantity" => sortDescending ? query.OrderByDescending(x => x.Quantity - x.AllocatedQuantity) : query.OrderBy(x => x.Quantity - x.AllocatedQuantity),
                "expirydate" => sortDescending ? query.OrderByDescending(x => x.ExpiryDate) : query.OrderBy(x => x.ExpiryDate),
                "supplier" => sortDescending ? query.OrderByDescending(x => x.Supplier) : query.OrderBy(x => x.Supplier),
                "status" => sortDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
                _ => sortDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt)
            };
        }
    }
}
