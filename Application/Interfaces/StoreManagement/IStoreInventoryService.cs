using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreInventoryService
    {
        Task<Result<IEnumerable<StoreInventoryDto>>> GetAllInventoriesAsync();
        Task<Result<StoreInventoryDto>> GetInventoryByIdAsync(int id);
        Task<Result<StoreInventoryDto>> CreateInventoryAsync(CreateStoreInventoryRequest request);
        Task<Result> UpdateInventoryQuantityAsync(int id, decimal newQuantity);
        Task<Result> DeleteInventoryAsync(int id);
        Task<Result<IEnumerable<StoreInventoryDto>>> GetInventoriesByStoreAsync(int storeId);
        Task<Result<IEnumerable<StoreInventoryDto>>> GetInventoriesByExplosiveTypeAsync(ExplosiveType explosiveType);
        Task<Result<IEnumerable<StoreInventoryDto>>> GetLowStockInventoriesAsync();
        Task<Result<IEnumerable<StoreInventoryDto>>> GetExpiringInventoriesAsync(int daysAhead = 30);
        Task<Result> AddStockAsync(int inventoryId, decimal quantity, string? batchNumber = null, string? supplier = null, DateTime? expiryDate = null);
        Task<Result> RemoveStockAsync(int inventoryId, decimal quantity);
        Task<Result> ReserveStockAsync(int inventoryId, decimal quantity);
        Task<Result> ReleaseReservedStockAsync(int inventoryId, decimal quantity);
        Task<Result<decimal>> GetAvailableQuantityAsync(int inventoryId);
    }
}