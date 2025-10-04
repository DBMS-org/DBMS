using Application.Interfaces.StoreManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.StoreManagement
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StoreRepository> _logger;

        public StoreRepository(ApplicationDbContext context, ILogger<StoreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
        {
            try
            {
                return await _context.Stores
                    .Include(s => s.Region)
                    .Include(s => s.ManagerUser)
                    .Include(s => s.Inventories)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all stores from database");
                throw;
            }
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Stores
                    .Include(s => s.Region)
                    .Include(s => s.ManagerUser)
                    .Include(s => s.Inventories)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store {StoreId} from database", id);
                throw;
            }
        }

        public async Task<Store> CreateAsync(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();
                return store;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating store {StoreName} in database", store.StoreName);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Store store)
        {
            try
            {
                _context.Stores.Update(store);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating store {StoreId} in database", store.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null) return false;

                _context.Stores.Remove(store);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store {StoreId} from database", id);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetByRegionIdAsync(int regionId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.RegionId == regionId)
                    .Include(s => s.Region)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores by region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<Store?> GetStoreByManagerAsync(int managerUserId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.ManagerUserId == managerUserId)
                    .Include(s => s.Region)
                    .Include(s => s.ManagerUser)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store by manager {ManagerUserId} from database", managerUserId);
                throw;
            }
        }

        public async Task<decimal> GetStoreUtilizationAsync(int storeId)
        {
            try
            {
                var store = await _context.Stores
                    .Include(s => s.Inventories)
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store == null)
                    return 0;

                return store.GetUtilizationRate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating store utilization for store {StoreId}", storeId);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> SearchAsync(string? storeName = null, string? city = null, int? regionId = null, int? managerUserId = null, StoreStatus? status = null)
        {
            try
            {
                var query = _context.Stores.AsQueryable();

                if (!string.IsNullOrWhiteSpace(storeName))
                    query = query.Where(s => s.StoreName.Contains(storeName));

                if (!string.IsNullOrWhiteSpace(city))
                    query = query.Where(s => s.City.Contains(city));

                if (regionId.HasValue)
                    query = query.Where(s => s.RegionId == regionId.Value);

                if (managerUserId.HasValue)
                    query = query.Where(s => s.ManagerUserId == managerUserId.Value);

                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                return await query
                    .Include(s => s.Region)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching stores from database");
                throw;
            }
        }

        public async Task<decimal> GetTotalCapacityByRegionAsync(int regionId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.RegionId == regionId && s.Status == StoreStatus.Operational)
                    .SumAsync(s => s.StorageCapacity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total capacity by region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetStoresWithLowCapacityAsync(decimal thresholdPercentage = 0.8m)
        {
            try
            {
                var stores = await _context.Stores
                    .Include(s => s.Inventories)
                    .Where(s => s.Status == StoreStatus.Operational)
                    .ToListAsync();

                // Note: Must filter in-memory as EF Core cannot translate GetUtilizationRate() to SQL
                return stores.Where(s => s.GetUtilizationRate() >= thresholdPercentage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores with low capacity from database");
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user {UserId} exists in database", userId);
                throw;
            }
        }

        public async Task<bool> RegionExistsAsync(int regionId)
        {
            try
            {
                return await _context.Regions.AnyAsync(r => r.Id == regionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if region {RegionId} exists in database", regionId);
                throw;
            }
        }

        public async Task<bool> StoreExistsAsync(int id)
        {
            try
            {
                return await _context.Stores.AnyAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if store {StoreId} exists in database", id);
                throw;
            }
        }

        public async Task<bool> StoreNameExistsAsync(string storeName, int? excludeId = null)
        {
            try
            {
                var query = _context.Stores.Where(s => s.StoreName == storeName);
                if (excludeId.HasValue)
                {
                    query = query.Where(s => s.Id != excludeId.Value);
                }
                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if store name {StoreName} exists in database", storeName);
                throw;
            }
        }

        public async Task<int> GetTotalStoresCountAsync()
        {
            try
            {
                return await _context.Stores.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total stores count from database");
                throw;
            }
        }

        public async Task<int> GetActiveStoresCountAsync()
        {
            try
            {
                return await _context.Stores.CountAsync(s => s.Status == Domain.Entities.StoreManagement.Enums.StoreStatus.Operational);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active stores count from database");
                throw;
            }
        }

        public async Task<decimal> GetTotalStorageCapacityAsync()
        {
            try
            {
                return await _context.Stores.SumAsync(s => s.StorageCapacity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total storage capacity from database");
                throw;
            }
        }

        public async Task<decimal> GetTotalCurrentOccupancyAsync()
        {
            try
            {
                return await _context.StoreInventories.SumAsync(i => i.Quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total current occupancy from database");
                throw;
            }
        }


    }
}