using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreInventoryRepository
    {
        Task<IEnumerable<StoreInventory>> GetAllAsync();
        Task<StoreInventory?> GetByIdAsync(int id);
        Task<StoreInventory?> GetByIdWithDetailsAsync(int id);
        Task<StoreInventory> CreateAsync(StoreInventory inventory);
        Task<bool> UpdateAsync(StoreInventory inventory);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<StoreInventory>> GetByStoreIdAsync(int storeId);
        Task<StoreInventory?> GetByStoreAndExplosiveTypeAsync(int storeId, ExplosiveType explosiveType);
        Task<IEnumerable<StoreInventory>> GetByExplosiveTypeAsync(ExplosiveType explosiveType);
        Task<IEnumerable<StoreInventory>> GetLowStockInventoriesAsync();
        Task<IEnumerable<StoreInventory>> GetExpiredInventoriesAsync();
        Task<IEnumerable<StoreInventory>> GetExpiringInventoriesAsync(int daysFromNow = 30);
        Task<IEnumerable<StoreInventory>> GetInventoriesNearingCapacityAsync(decimal thresholdPercentage = 0.9m);
        Task<decimal> GetAvailableQuantityAsync(int storeId, ExplosiveType explosiveType);
        Task<decimal> GetTotalQuantityByExplosiveTypeAsync(ExplosiveType explosiveType);
        Task<IEnumerable<StoreInventory>> SearchAsync(int? storeId = null, ExplosiveType? explosiveType = null, string? batchNumber = null, string? supplier = null);
        Task<bool> HasSufficientStockAsync(int storeId, ExplosiveType explosiveType, decimal requiredQuantity);
    }
}