using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Application.Interfaces.StoreManagement
{
    public interface IStoreTransactionService
    {
        Task<Result<IEnumerable<StoreTransactionDto>>> GetAllTransactionsAsync();
        Task<Result<StoreTransactionDto>> GetTransactionByIdAsync(int id);
        Task<Result<StoreTransactionDto>> CreateTransactionAsync(CreateStoreTransactionRequest request);
        Task<Result> DeleteTransactionAsync(int id);
        Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByStoreAsync(int storeId);
        Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByTypeAsync(TransactionType transactionType);
        Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByExplosiveTypeAsync(ExplosiveType explosiveType);
        Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByUserAsync(int userId);
        Task<Result<StoreTransactionDto>> ProcessStockInAsync(int storeId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null);
        Task<Result<StoreTransactionDto>> ProcessStockOutAsync(int storeId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null);
        Task<Result<StoreTransactionDto>> ProcessTransferAsync(int fromStoreId, int toStoreId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null);
        Task<Result<StoreTransactionDto>> ProcessAdjustmentAsync(int storeId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null);
    }
}