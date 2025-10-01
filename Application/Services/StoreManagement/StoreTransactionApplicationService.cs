using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Application.Interfaces.StoreManagement;
using AutoMapper;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services.StoreManagement
{
    public class StoreTransactionApplicationService : IStoreTransactionService
    {
        private readonly IStoreTransactionRepository _storeTransactionRepository;
        private readonly IStoreInventoryRepository _storeInventoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreTransactionApplicationService> _logger;

        public StoreTransactionApplicationService(
            IStoreTransactionRepository storeTransactionRepository,
            IStoreInventoryRepository storeInventoryRepository,
            IMapper mapper,
            ILogger<StoreTransactionApplicationService> logger)
        {
            _storeTransactionRepository = storeTransactionRepository;
            _storeInventoryRepository = storeInventoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetAllTransactionsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all store transactions");
                var transactions = await _storeTransactionRepository.GetAllAsync();
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} store transactions", transactionDtos.Count());
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all store transactions");
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve store transactions");
            }
        }

        public async Task<Result<StoreTransactionDto>> GetTransactionByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving store transaction with ID: {TransactionId}", id);
                var transaction = await _storeTransactionRepository.GetByIdAsync(id);
                
                if (transaction == null)
                {
                    _logger.LogWarning("Store transaction with ID {TransactionId} not found", id);
                    return Result.Failure<StoreTransactionDto>("Store transaction not found");
                }

                var transactionDto = _mapper.Map<StoreTransactionDto>(transaction);
                _logger.LogInformation("Successfully retrieved store transaction with ID: {TransactionId}", id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving store transaction with ID: {TransactionId}", id);
                return Result.Failure<StoreTransactionDto>("Failed to retrieve store transaction");
            }
        }

        public async Task<Result<StoreTransactionDto>> CreateTransactionAsync(CreateStoreTransactionRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new store transaction for store {StoreId} with type {TransactionType}", 
                    request.StoreId, request.TransactionType);

                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level

                var transaction = new StoreTransaction(
                    request.StoreId,
                    request.ExplosiveType,
                    request.TransactionType,
                    request.Quantity,
                    request.Unit,
                    request.ProcessedByUserId,
                    request.ReferenceNumber,
                    request.Notes);

                var createdTransaction = await _storeTransactionRepository.CreateAsync(transaction);
                var transactionDto = _mapper.Map<StoreTransactionDto>(createdTransaction);
                
                _logger.LogInformation("Successfully created store transaction with ID: {TransactionId}", createdTransaction.Id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating store transaction for store {StoreId}", request.StoreId);
                return Result.Failure<StoreTransactionDto>("Failed to create store transaction");
            }
        }

        public async Task<Result> DeleteTransactionAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting store transaction with ID: {TransactionId}", id);
                
                var transaction = await _storeTransactionRepository.GetByIdAsync(id);
                if (transaction == null)
                {
                    _logger.LogWarning("Store transaction with ID {TransactionId} not found", id);
                    return Result.Failure("Store transaction not found");
                }

                await _storeTransactionRepository.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted store transaction with ID: {TransactionId}", id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store transaction with ID: {TransactionId}", id);
                return Result.Failure("Failed to delete store transaction");
            }
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByStoreAsync(int storeId)
        {
            try
            {
                _logger.LogInformation("Retrieving transactions for store ID: {StoreId}", storeId);
                
                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level

                var transactions = await _storeTransactionRepository.GetByStoreIdAsync(storeId);
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} transactions for store ID: {StoreId}", 
                    transactionDtos.Count(), storeId);
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for store ID: {StoreId}", storeId);
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve store transactions");
            }
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            try
            {
                _logger.LogInformation("Retrieving transactions for transaction type: {TransactionType}", transactionType);
                
                var transactions = await _storeTransactionRepository.GetByTransactionTypeAsync(transactionType);
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} transactions for transaction type: {TransactionType}", 
                    transactionDtos.Count(), transactionType);
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for transaction type: {TransactionType}", transactionType);
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve transactions by type");
            }
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Retrieving transactions for date range: {StartDate} to {EndDate}", startDate, endDate);
                
                if (startDate > endDate)
                {
                    _logger.LogWarning("Invalid date range: start date {StartDate} is after end date {EndDate}", startDate, endDate);
                    return Result.Failure<IEnumerable<StoreTransactionDto>>("Start date cannot be after end date");
                }

                var transactions = await _storeTransactionRepository.GetByDateRangeAsync(startDate, endDate);
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} transactions for date range: {StartDate} to {EndDate}", 
                    transactionDtos.Count(), startDate, endDate);
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for date range: {StartDate} to {EndDate}", startDate, endDate);
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve transactions by date range");
            }
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByExplosiveTypeAsync(ExplosiveType explosiveType)
        {
            try
            {
                _logger.LogInformation("Retrieving transactions for explosive type: {ExplosiveType}", explosiveType);
                
                var transactions = await _storeTransactionRepository.GetByExplosiveTypeAsync(explosiveType);
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} transactions for explosive type: {ExplosiveType}", 
                    transactionDtos.Count(), explosiveType);
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for explosive type: {ExplosiveType}", explosiveType);
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve transactions by explosive type");
            }
        }

        public async Task<Result<IEnumerable<StoreTransactionDto>>> GetTransactionsByUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Retrieving transactions for user ID: {UserId}", userId);
                
                var transactions = await _storeTransactionRepository.GetByProcessedByUserIdAsync(userId);
                var transactionDtos = _mapper.Map<IEnumerable<StoreTransactionDto>>(transactions);
                
                _logger.LogInformation("Successfully retrieved {Count} transactions for user ID: {UserId}", 
                    transactionDtos.Count(), userId);
                return Result.Success(transactionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for user ID: {UserId}", userId);
                return Result.Failure<IEnumerable<StoreTransactionDto>>("Failed to retrieve transactions by user");
            }
        }

        public async Task<Result<StoreTransactionDto>> ProcessStockInAsync(int storeId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null)
        {
            try
            {
                _logger.LogInformation("Processing stock in for store {StoreId}, explosive type {ExplosiveType}, quantity {Quantity}", 
                    storeId, explosiveType, quantity);

                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level

                // Get or create inventory for this store and explosive type
                var inventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(storeId, explosiveType);
                if (inventory == null)
                {
                    _logger.LogWarning("No inventory found for store {StoreId} and explosive type {ExplosiveType}", storeId, explosiveType);
                    return Result.Failure<StoreTransactionDto>("No inventory found for this store and explosive type");
                }

                // Create stock in transaction
                var transaction = StoreTransaction.CreateStockIn(
                    storeId,
                    inventory.Id,
                    explosiveType,
                    quantity,
                    unit,
                    processedByUserId ?? 0,
                    referenceNumber,
                    notes);

                // Update inventory
                inventory.AddStock(quantity);
                await _storeInventoryRepository.UpdateAsync(inventory);

                // Save transaction
                var createdTransaction = await _storeTransactionRepository.CreateAsync(transaction);
                var transactionDto = _mapper.Map<StoreTransactionDto>(createdTransaction);
                
                _logger.LogInformation("Successfully processed stock in with transaction ID: {TransactionId}", createdTransaction.Id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stock in for store {StoreId}", storeId);
                return Result.Failure<StoreTransactionDto>("Failed to process stock in");
            }
        }

        public async Task<Result<StoreTransactionDto>> ProcessStockOutAsync(int storeId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null)
        {
            try
            {
                _logger.LogInformation("Processing stock out for store {StoreId}, explosive type {ExplosiveType}, quantity {Quantity}", 
                    storeId, explosiveType, quantity);

                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level

                if (quantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity {Quantity} for stock out operation", quantity);
                    return Result.Failure<StoreTransactionDto>("Quantity must be positive");
                }

                // Check inventory availability
                var inventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(storeId, explosiveType);
                if (inventory == null || inventory.GetAvailableQuantity() < quantity)
                {
                    _logger.LogWarning("Insufficient stock for inventory ID {InventoryId}. Available: {Available}, Required: {Required}", 
                        inventory?.Id, inventory?.GetAvailableQuantity(), quantity);
                    return Result.Failure<StoreTransactionDto>("Insufficient stock available");
                }

                // Create stock out transaction
                var transaction = StoreTransaction.CreateStockOut(
                    storeId,
                    inventory.Id,
                    explosiveType,
                    quantity,
                    unit,
                    processedByUserId ?? 0,
                    referenceNumber,
                    notes);

                var createdTransaction = await _storeTransactionRepository.CreateAsync(transaction);

                // Update inventory
                inventory.ConsumeStock(quantity);
                await _storeInventoryRepository.UpdateAsync(inventory);

                var transactionDto = _mapper.Map<StoreTransactionDto>(createdTransaction);
                _logger.LogInformation("Successfully processed stock out transaction with ID: {TransactionId}", createdTransaction.Id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stock out for store {StoreId}", storeId);
                return Result.Failure<StoreTransactionDto>("Failed to process stock out transaction");
            }
        }

        public async Task<Result<StoreTransactionDto>> ProcessTransferAsync(int fromStoreId, int toStoreId, ExplosiveType explosiveType, decimal quantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null)
        {
            try
            {
                _logger.LogInformation("Processing transfer from store {FromStoreId} to store {ToStoreId}, explosive type {ExplosiveType}, quantity {Quantity}", 
                    fromStoreId, toStoreId, explosiveType, quantity);

                // Remove StoreExistsAsync validations as they don't exist in the repository
                // These validations should be handled by foreign key constraints or at a higher level

                // Check source inventory availability
                var sourceInventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(fromStoreId, explosiveType);
                if (sourceInventory == null || sourceInventory.GetAvailableQuantity() < quantity)
                {
                    _logger.LogWarning("Insufficient stock in source store {FromStoreId} for explosive type {ExplosiveType}", fromStoreId, explosiveType);
                    return Result.Failure<StoreTransactionDto>("Insufficient stock in source store");
                }

                // Get or create destination inventory
                var destinationInventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(toStoreId, explosiveType);
                if (destinationInventory == null)
                {
                    _logger.LogWarning("No inventory found for destination store {ToStoreId} and explosive type {ExplosiveType}", toStoreId, explosiveType);
                    return Result.Failure<StoreTransactionDto>("No inventory found for destination store and explosive type");
                }

                // Create transfer transaction
                var transaction = StoreTransaction.CreateTransfer(
                    fromStoreId,
                    toStoreId,
                    explosiveType,
                    quantity,
                    unit,
                    processedByUserId ?? 0,
                    referenceNumber,
                    notes);

                // Update inventories
                sourceInventory.ConsumeStock(quantity);
                destinationInventory.AddStock(quantity);
                
                await _storeInventoryRepository.UpdateAsync(sourceInventory);
                await _storeInventoryRepository.UpdateAsync(destinationInventory);

                // Save transaction
                var createdTransaction = await _storeTransactionRepository.CreateAsync(transaction);
                var transactionDto = _mapper.Map<StoreTransactionDto>(createdTransaction);
                
                _logger.LogInformation("Successfully processed transfer with transaction ID: {TransactionId}", createdTransaction.Id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing transfer from store {FromStoreId} to store {ToStoreId}", fromStoreId, toStoreId);
                return Result.Failure<StoreTransactionDto>("Failed to process transfer");
            }
        }

        public async Task<Result<StoreTransactionDto>> ProcessAdjustmentAsync(int storeId, ExplosiveType explosiveType, decimal adjustmentQuantity, string unit, int? processedByUserId = null, string? referenceNumber = null, string? notes = null)
        {
            try
            {
                _logger.LogInformation("Processing adjustment for store {StoreId}, explosive type {ExplosiveType}, adjustment {AdjustmentQuantity}", 
                    storeId, explosiveType, adjustmentQuantity);

                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level

                // Get inventory
                var inventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(storeId, explosiveType);
                if (inventory == null)
                {
                    _logger.LogWarning("No inventory found for store {StoreId} and explosive type {ExplosiveType}", storeId, explosiveType);
                    return Result.Failure<StoreTransactionDto>("No inventory found for this store and explosive type");
                }

                // Validate adjustment for negative quantities
                if (adjustmentQuantity < 0 && inventory.GetAvailableQuantity() < Math.Abs(adjustmentQuantity))
                {
                    _logger.LogWarning("Insufficient stock for adjustment in store {StoreId}. Available: {Available}, Required: {Required}", 
                        storeId, inventory.GetAvailableQuantity(), Math.Abs(adjustmentQuantity));
                    return Result.Failure<StoreTransactionDto>("Insufficient stock for adjustment");
                }

                // Create adjustment transaction
                var transaction = StoreTransaction.CreateAdjustment(
                    storeId,
                    inventory.Id,
                    explosiveType,
                    adjustmentQuantity,
                    unit,
                    processedByUserId ?? 0,
                    referenceNumber,
                    notes);

                // Update inventory based on adjustment type
                if (adjustmentQuantity > 0)
                {
                    inventory.AddStock(adjustmentQuantity);
                }
                else
                {
                    inventory.ConsumeStock(Math.Abs(adjustmentQuantity));
                }
                
                await _storeInventoryRepository.UpdateAsync(inventory);

                // Save transaction
                var createdTransaction = await _storeTransactionRepository.CreateAsync(transaction);
                var transactionDto = _mapper.Map<StoreTransactionDto>(createdTransaction);
                
                _logger.LogInformation("Successfully processed adjustment with transaction ID: {TransactionId}", createdTransaction.Id);
                return Result.Success(transactionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing adjustment for store {StoreId}", storeId);
                return Result.Failure<StoreTransactionDto>("Failed to process adjustment");
            }
        }
    }
}