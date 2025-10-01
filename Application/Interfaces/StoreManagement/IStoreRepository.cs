using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
        Task<Store?> GetByIdWithDetailsAsync(int id);
        Task<Store> CreateAsync(Store store);
        Task<bool> UpdateAsync(Store store);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Store>> GetByRegionIdAsync(int regionId);
        Task<IEnumerable<Store>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<Store>> GetByManagerIdAsync(int managerId);
        Task<IEnumerable<Store>> GetByStatusAsync(StoreStatus status);
        Task<IEnumerable<Store>> SearchAsync(string? storeName = null, int? regionId = null, int? projectId = null, StoreStatus? status = null);
        Task<Store?> GetByLicenseNumberAsync(string licenseNumber);
        Task<bool> IsLicenseNumberUniqueAsync(string licenseNumber, int? excludeStoreId = null);
        Task<decimal> GetTotalCapacityByRegionAsync(int regionId);
        Task<IEnumerable<Store>> GetStoresWithLowCapacityAsync(decimal thresholdPercentage = 0.8m);
        
        // Validation methods
        Task<bool> UserExistsAsync(int userId);
        Task<bool> RegionExistsAsync(int regionId);
        Task<bool> ProjectExistsAsync(int projectId);
    }
}