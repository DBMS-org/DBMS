using Application.Interfaces.StoreManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.StoreManagement
{
    public class StoreInventoryRepository : IStoreInventoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StoreInventoryRepository> _logger;

        public StoreInventoryRepository(ApplicationDbContext context, ILogger<StoreInventoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<StoreInventory>> GetAllAsync()
        {
            try
            {
                return await _context.StoreInventories
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all store inventories from database");
                throw;
            }
        }

        public async Task<StoreInventory?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.StoreInventories.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store inventory {InventoryId} from database", id);
                throw;
            }
        }

        public async Task<StoreInventory?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.StoreInventories
                    .Include(si => si.Store)
                    .FirstOrDefaultAsync(si => si.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store inventory {InventoryId} with details from database", id);
                throw;
            }
        }

        public async Task<StoreInventory> CreateAsync(StoreInventory inventory)
        {
            try
            {
                _context.StoreInventories.Add(inventory);
                await _context.SaveChangesAsync();
                return inventory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating store inventory for store {StoreId} and explosive type {ExplosiveType} in database", 
                    inventory.StoreId, inventory.ExplosiveType);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(StoreInventory inventory)
        {
            try
            {
                _context.StoreInventories.Update(inventory);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating store inventory {InventoryId} in database", inventory.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var inventory = await _context.StoreInventories.FindAsync(id);
                if (inventory == null) return false;

                _context.StoreInventories.Remove(inventory);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store inventory {InventoryId} from database", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.StoreInventories.AnyAsync(si => si.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if store inventory {InventoryId} exists in database", id);
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetByStoreIdAsync(int storeId)
        {
            try
            {
                return await _context.StoreInventories
                    .Where(si => si.StoreId == storeId)
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store inventories by store {StoreId} from database", storeId);
                throw;
            }
        }

        public async Task<StoreInventory?> GetByStoreAndExplosiveTypeAsync(int storeId, ExplosiveType explosiveType)
        {
            try
            {
                return await _context.StoreInventories
                    .FirstOrDefaultAsync(si => si.StoreId == storeId && si.ExplosiveType == explosiveType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store inventory by store {StoreId} and explosive type {ExplosiveType} from database", 
                    storeId, explosiveType);
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetByExplosiveTypeAsync(ExplosiveType explosiveType)
        {
            try
            {
                return await _context.StoreInventories
                    .Where(si => si.ExplosiveType == explosiveType)
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store inventories by explosive type {ExplosiveType} from database", explosiveType);
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetLowStockInventoriesAsync()
        {
            try
            {
                return await _context.StoreInventories
                    .Where(si => si.Quantity <= si.MinimumStockLevel)
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting low stock inventories from database");
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetExpiredInventoriesAsync()
        {
            try
            {
                var currentDate = DateTime.UtcNow.Date;
                return await _context.StoreInventories
                    .Where(si => si.ExpiryDate.HasValue && si.ExpiryDate.Value.Date <= currentDate)
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expired inventories from database");
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetExpiringInventoriesAsync(int daysFromNow = 30)
        {
            try
            {
                var thresholdDate = DateTime.UtcNow.Date.AddDays(daysFromNow);
                return await _context.StoreInventories
                    .Where(si => si.ExpiryDate.HasValue && si.ExpiryDate.Value.Date <= thresholdDate && si.ExpiryDate.Value.Date >= DateTime.UtcNow.Date)
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expiring inventories from database");
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> GetInventoriesNearingCapacityAsync(decimal thresholdPercentage = 0.9m)
        {
            try
            {
                return await _context.StoreInventories
                    .Where(si => si.Quantity >= (si.MaximumStockLevel * thresholdPercentage))
                    .Include(si => si.Store)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventories nearing capacity from database");
                throw;
            }
        }

        public async Task<decimal> GetAvailableQuantityAsync(int storeId, ExplosiveType explosiveType)
        {
            try
            {
                var inventory = await _context.StoreInventories
                    .FirstOrDefaultAsync(si => si.StoreId == storeId && si.ExplosiveType == explosiveType);
                
                return inventory?.GetAvailableQuantity() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available quantity for store {StoreId} and explosive type {ExplosiveType} from database", 
                    storeId, explosiveType);
                throw;
            }
        }

        public async Task<decimal> GetTotalQuantityByExplosiveTypeAsync(ExplosiveType explosiveType)
        {
            try
            {
                return await _context.StoreInventories
                    .Where(si => si.ExplosiveType == explosiveType)
                    .SumAsync(si => si.Quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total quantity by explosive type {ExplosiveType} from database", explosiveType);
                throw;
            }
        }

        public async Task<IEnumerable<StoreInventory>> SearchAsync(int? storeId = null, ExplosiveType? explosiveType = null, string? batchNumber = null, string? supplier = null)
        {
            try
            {
                var query = _context.StoreInventories
                    .Include(si => si.Store)
                    .AsQueryable();

                if (storeId.HasValue)
                    query = query.Where(si => si.StoreId == storeId.Value);

                if (explosiveType.HasValue)
                    query = query.Where(si => si.ExplosiveType == explosiveType.Value);

                if (!string.IsNullOrEmpty(batchNumber))
                    query = query.Where(si => si.BatchNumber != null && si.BatchNumber.Contains(batchNumber));

                if (!string.IsNullOrEmpty(supplier))
                    query = query.Where(si => si.Supplier != null && si.Supplier.Contains(supplier));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching store inventories in database");
                throw;
            }
        }

        public async Task<bool> HasSufficientStockAsync(int storeId, ExplosiveType explosiveType, decimal requiredQuantity)
        {
            try
            {
                var availableQuantity = await GetAvailableQuantityAsync(storeId, explosiveType);
                return availableQuantity >= requiredQuantity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking sufficient stock for store {StoreId} and explosive type {ExplosiveType} in database", 
                    storeId, explosiveType);
                throw;
            }
        }
    }
}