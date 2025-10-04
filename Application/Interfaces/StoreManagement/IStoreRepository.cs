using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
        Task<Store> CreateAsync(Store store);
        Task<bool> UpdateAsync(Store store);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Store>> GetByRegionIdAsync(int regionId);
        Task<IEnumerable<Store>> SearchAsync(string? storeName = null, string? city = null, int? regionId = null, int? managerUserId = null, StoreStatus? status = null);
        Task<bool> StoreExistsAsync(int id);
        Task<bool> StoreNameExistsAsync(string storeName, int? excludeId = null);
        Task<bool> RegionExistsAsync(int regionId);
        Task<bool> UserExistsAsync(int userId);
        Task<int> GetTotalStoresCountAsync();
        Task<int> GetActiveStoresCountAsync();
        Task<decimal> GetTotalStorageCapacityAsync();
        Task<decimal> GetTotalCurrentOccupancyAsync();
        Task<Store?> GetStoreByManagerAsync(int managerUserId);
        Task<decimal> GetStoreUtilizationAsync(int storeId);
    }
}