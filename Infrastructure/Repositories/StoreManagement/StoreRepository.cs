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
                    .Include(s => s.Project)
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
                    .Include(s => s.Project)
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

        public async Task<Store?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.Stores
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .Include(s => s.Inventories)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store {StoreId} with details from database", id);
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

        public async Task<bool> ExistsAsync(int id)
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

        public async Task<IEnumerable<Store>> GetByRegionIdAsync(int regionId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.RegionId == regionId)
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores by region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetByProjectIdAsync(int projectId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.ProjectId == projectId)
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores by project {ProjectId} from database", projectId);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetByManagerIdAsync(int managerId)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.ManagerUserId == managerId)
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores by manager {ManagerId} from database", managerId);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> GetByStatusAsync(StoreStatus status)
        {
            try
            {
                return await _context.Stores
                    .Where(s => s.Status == status)
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stores by status {Status} from database", status);
                throw;
            }
        }

        public async Task<IEnumerable<Store>> SearchAsync(string? name = null, int? regionId = null, int? projectId = null, StoreStatus? status = null)
        {
            try
            {
                var query = _context.Stores.AsQueryable();
                
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(s => s.StoreName.Contains(name));
                
                if (regionId.HasValue)
                    query = query.Where(s => s.RegionId == regionId.Value);
                
                if (projectId.HasValue)
                    query = query.Where(s => s.ProjectId == projectId.Value);
                
                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);
                
                return await query
                    .Include(s => s.Region)
                    .Include(s => s.Project)
                    .Include(s => s.ManagerUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching stores from database");
                throw;
            }
        }

        public async Task<Store?> GetByLicenseNumberAsync(string licenseNumber)
        {
            try
            {
                // Since Store entity doesn't have LicenseNumber property, this method should be removed
                // or the Store entity should be updated to include LicenseNumber
                throw new NotImplementedException("LicenseNumber property does not exist in Store entity");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store by license number {LicenseNumber} from database", licenseNumber);
                throw;
            }
        }

        public async Task<bool> IsLicenseNumberUniqueAsync(string licenseNumber, int? excludeStoreId = null)
        {
            try
            {
                // Since Store entity doesn't have LicenseNumber property, this method should be removed
                // or the Store entity should be updated to include LicenseNumber
                throw new NotImplementedException("LicenseNumber property does not exist in Store entity");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking license number uniqueness for {LicenseNumber}", licenseNumber);
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

                return stores.Where(s =>
                {
                    var totalUsed = s.Inventories.Sum(i => i.Quantity);
                    var utilizationPercentage = s.StorageCapacity > 0 ? totalUsed / s.StorageCapacity : 0;
                    return utilizationPercentage >= thresholdPercentage;
                });
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

        public async Task<bool> ProjectExistsAsync(int projectId)
        {
            try
            {
                return await _context.Projects.AnyAsync(p => p.Id == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if project {ProjectId} exists in database", projectId);
                throw;
            }
        }
    }
}