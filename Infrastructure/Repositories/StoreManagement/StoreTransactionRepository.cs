using Application.Interfaces.StoreManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.StoreManagement
{
    public class StoreTransactionRepository : IStoreTransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StoreTransactionRepository> _logger;

        public StoreTransactionRepository(ApplicationDbContext context, ILogger<StoreTransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<StoreTransaction>> GetAllAsync()
        {
            try
            {
                return await _context.StoreTransactions
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all store transactions from database");
                throw;
            }
        }

        public async Task<StoreTransaction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.StoreTransactions.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transaction {TransactionId} from database", id);
                throw;
            }
        }

        public async Task<StoreTransaction?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.StoreTransactions
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .FirstOrDefaultAsync(st => st.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transaction {TransactionId} with details from database", id);
                throw;
            }
        }

        public async Task<StoreTransaction> CreateAsync(StoreTransaction transaction)
        {
            try
            {
                _context.StoreTransactions.Add(transaction);
                await _context.SaveChangesAsync();
                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating store transaction for store {StoreId} and type {TransactionType} in database", 
                    transaction.StoreId, transaction.TransactionType);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(StoreTransaction transaction)
        {
            try
            {
                _context.StoreTransactions.Update(transaction);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating store transaction {TransactionId} in database", transaction.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var transaction = await _context.StoreTransactions.FindAsync(id);
                if (transaction == null) return false;

                _context.StoreTransactions.Remove(transaction);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store transaction {TransactionId} from database", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.StoreTransactions.AnyAsync(st => st.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if store transaction {TransactionId} exists in database", id);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByStoreIdAsync(int storeId)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.StoreId == storeId)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by store {StoreId} from database", storeId);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByStoreInventoryIdAsync(int storeInventoryId)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.StoreInventoryId == storeInventoryId)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by inventory {InventoryId} from database", storeInventoryId);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByTransactionTypeAsync(TransactionType transactionType)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.TransactionType == transactionType)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by type {TransactionType} from database", transactionType);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByExplosiveTypeAsync(ExplosiveType explosiveType)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.ExplosiveType == explosiveType)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by explosive type {ExplosiveType} from database", explosiveType);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.TransactionDate >= startDate && st.TransactionDate <= endDate)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by date range from {StartDate} to {EndDate} from database", 
                    startDate, endDate);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByProcessedByUserIdAsync(int userId)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.ProcessedByUserId == userId)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by user {UserId} from database", userId);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetByReferenceNumberAsync(string referenceNumber)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.ReferenceNumber == referenceNumber)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store transactions by reference number {ReferenceNumber} from database", referenceNumber);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> SearchAsync(
            int? storeId = null, 
            TransactionType? transactionType = null, 
            ExplosiveType? explosiveType = null,
            DateTime? startDate = null, 
            DateTime? endDate = null,
            int? processedByUserId = null,
            string? referenceNumber = null)
        {
            try
            {
                var query = _context.StoreTransactions
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .AsQueryable();

                if (storeId.HasValue)
                    query = query.Where(st => st.StoreId == storeId.Value);

                if (transactionType.HasValue)
                    query = query.Where(st => st.TransactionType == transactionType.Value);

                if (explosiveType.HasValue)
                    query = query.Where(st => st.ExplosiveType == explosiveType.Value);

                if (startDate.HasValue)
                    query = query.Where(st => st.TransactionDate >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(st => st.TransactionDate <= endDate.Value);

                if (processedByUserId.HasValue)
                    query = query.Where(st => st.ProcessedByUserId == processedByUserId.Value);

                if (!string.IsNullOrEmpty(referenceNumber))
                    query = query.Where(st => st.ReferenceNumber != null && st.ReferenceNumber.Contains(referenceNumber));

                return await query.OrderByDescending(st => st.TransactionDate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching store transactions in database");
                throw;
            }
        }

        public async Task<decimal> GetTotalQuantityByTypeAndDateRangeAsync(int storeId, TransactionType transactionType, DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.StoreId == storeId && 
                                st.TransactionType == transactionType && 
                                st.TransactionDate >= startDate && 
                                st.TransactionDate <= endDate)
                    .SumAsync(st => st.Quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting total quantity by type {TransactionType} and date range for store {StoreId} from database", 
                    transactionType, storeId);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetRecentTransactionsAsync(int storeId, int count = 10)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.StoreId == storeId)
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent transactions for store {StoreId} from database", storeId);
                throw;
            }
        }

        public async Task<IEnumerable<StoreTransaction>> GetTransfersBetweenStoresAsync(int sourceStoreId, int destinationStoreId)
        {
            try
            {
                return await _context.StoreTransactions
                    .Where(st => st.TransactionType == TransactionType.Transfer &&
                                ((st.StoreId == sourceStoreId && st.RelatedStoreId == destinationStoreId) ||
                                 (st.StoreId == destinationStoreId && st.RelatedStoreId == sourceStoreId)))
                    .Include(st => st.Store)
                    .Include(st => st.StoreInventory)
                    .Include(st => st.ProcessedByUser)
                    .Include(st => st.RelatedStore)
                    .OrderByDescending(st => st.TransactionDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transfers between stores {SourceStoreId} and {DestinationStoreId} from database", 
                    sourceStoreId, destinationStoreId);
                throw;
            }
        }
    }
}