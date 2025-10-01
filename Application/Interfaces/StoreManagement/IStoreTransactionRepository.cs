using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreTransactionRepository
    {
        Task<IEnumerable<StoreTransaction>> GetAllAsync();
        Task<StoreTransaction?> GetByIdAsync(int id);
        Task<StoreTransaction?> GetByIdWithDetailsAsync(int id);
        Task<StoreTransaction> CreateAsync(StoreTransaction transaction);
        Task<bool> UpdateAsync(StoreTransaction transaction);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<StoreTransaction>> GetByStoreIdAsync(int storeId);
        Task<IEnumerable<StoreTransaction>> GetByStoreInventoryIdAsync(int storeInventoryId);
        Task<IEnumerable<StoreTransaction>> GetByTransactionTypeAsync(TransactionType transactionType);
        Task<IEnumerable<StoreTransaction>> GetByExplosiveTypeAsync(ExplosiveType explosiveType);
        Task<IEnumerable<StoreTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<StoreTransaction>> GetByProcessedByUserIdAsync(int userId);
        Task<IEnumerable<StoreTransaction>> GetByReferenceNumberAsync(string referenceNumber);
        Task<IEnumerable<StoreTransaction>> SearchAsync(
            int? storeId = null, 
            TransactionType? transactionType = null, 
            ExplosiveType? explosiveType = null,
            DateTime? startDate = null, 
            DateTime? endDate = null,
            int? processedByUserId = null,
            string? referenceNumber = null);
        Task<decimal> GetTotalQuantityByTypeAndDateRangeAsync(int storeId, TransactionType transactionType, DateTime startDate, DateTime endDate);
        Task<IEnumerable<StoreTransaction>> GetRecentTransactionsAsync(int storeId, int count = 10);
        Task<IEnumerable<StoreTransaction>> GetTransfersBetweenStoresAsync(int sourceStoreId, int destinationStoreId);
    }
}