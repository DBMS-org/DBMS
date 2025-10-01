using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Application.Interfaces.StoreManagement;
using AutoMapper;
using Domain.Entities.StoreManagement.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services.StoreManagement
{
    public class StoreInventoryApplicationService : IStoreInventoryService
    {
        private readonly IStoreInventoryRepository _storeInventoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreInventoryApplicationService> _logger;

        public StoreInventoryApplicationService(
            IStoreInventoryRepository storeInventoryRepository,
            IMapper mapper,
            ILogger<StoreInventoryApplicationService> logger)
        {
            _storeInventoryRepository = storeInventoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StoreInventoryDto>>> GetAllInventoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all store inventories");
                var inventories = await _storeInventoryRepository.GetAllAsync();
                var inventoryDtos = _mapper.Map<IEnumerable<StoreInventoryDto>>(inventories);
                
                _logger.LogInformation("Successfully retrieved {Count} store inventories", inventoryDtos.Count());
                return Result.Success(inventoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all store inventories");
                return Result.Failure<IEnumerable<StoreInventoryDto>>("Failed to retrieve store inventories");
            }
        }

        public async Task<Result<StoreInventoryDto>> GetInventoryByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving store inventory with ID: {InventoryId}", id);
                var inventory = await _storeInventoryRepository.GetByIdAsync(id);
                
                if (inventory == null)
                {
                    _logger.LogWarning("Store inventory with ID {InventoryId} not found", id);
                    return Result.Failure<StoreInventoryDto>("Store inventory not found");
                }

                var inventoryDto = _mapper.Map<StoreInventoryDto>(inventory);
                _logger.LogInformation("Successfully retrieved store inventory with ID: {InventoryId}", id);
                return Result.Success(inventoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving store inventory with ID: {InventoryId}", id);
                return Result.Failure<StoreInventoryDto>("Failed to retrieve store inventory");
            }
        }

        public async Task<Result<StoreInventoryDto>> CreateInventoryAsync(CreateStoreInventoryRequest request)
        {
            try
            {
                _logger.LogInformation("Creating new store inventory for store {StoreId} with explosive type {ExplosiveType}", 
                    request.StoreId, request.ExplosiveType);

                // Check if inventory already exists for this store and explosive type
                var existingInventory = await _storeInventoryRepository.GetByStoreAndExplosiveTypeAsync(request.StoreId, request.ExplosiveType);
                if (existingInventory != null)
                {
                    _logger.LogWarning("Inventory already exists for store {StoreId} and explosive type {ExplosiveType}", 
                        request.StoreId, request.ExplosiveType);
                    return Result.Failure<StoreInventoryDto>("Inventory already exists for this store and explosive type");
                }

                var inventory = new Domain.Entities.StoreManagement.StoreInventory(
                    request.StoreId,
                    request.ExplosiveType,
                    request.Quantity,
                    request.Unit,
                    request.MinimumStockLevel,
                    request.MaximumStockLevel);

                var createdInventory = await _storeInventoryRepository.CreateAsync(inventory);
                var inventoryDto = _mapper.Map<StoreInventoryDto>(createdInventory);
                
                _logger.LogInformation("Successfully created store inventory with ID: {InventoryId}", createdInventory.Id);
                return Result.Success(inventoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating store inventory for store {StoreId}", request.StoreId);
                return Result.Failure<StoreInventoryDto>("Failed to create store inventory");
            }
        }

        public async Task<Result> UpdateInventoryQuantityAsync(int id, decimal newQuantity)
        {
            try
            {
                _logger.LogInformation("Updating inventory quantity for ID: {InventoryId} to {NewQuantity}", id, newQuantity);
                
                var inventory = await _storeInventoryRepository.GetByIdAsync(id);
                if (inventory == null)
                {
                    _logger.LogWarning("Store inventory with ID {InventoryId} not found", id);
                    return Result.Failure("Store inventory not found");
                }

                if (newQuantity < 0)
                {
                    _logger.LogWarning("Invalid quantity {Quantity} for inventory {InventoryId}", newQuantity, id);
                    return Result.Failure("Quantity cannot be negative");
                }

                inventory.UpdateQuantity(newQuantity);
                await _storeInventoryRepository.UpdateAsync(inventory);
                
                _logger.LogInformation("Successfully updated inventory quantity for ID: {InventoryId}", id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating inventory quantity for ID: {InventoryId}", id);
                return Result.Failure("Failed to update inventory quantity");
            }
        }

        public async Task<Result> DeleteInventoryAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting store inventory with ID: {InventoryId}", id);
                
                var inventory = await _storeInventoryRepository.GetByIdAsync(id);
                if (inventory == null)
                {
                    _logger.LogWarning("Store inventory with ID {InventoryId} not found", id);
                    return Result.Failure("Store inventory not found");
                }

                await _storeInventoryRepository.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted store inventory with ID: {InventoryId}", id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting store inventory with ID: {InventoryId}", id);
                return Result.Failure("Failed to delete store inventory");
            }
        }

        public async Task<Result<IEnumerable<StoreInventoryDto>>> GetInventoriesByStoreAsync(int storeId)
        {
            try
            {
                _logger.LogInformation("Retrieving inventories for store ID: {StoreId}", storeId);
                
                // Remove StoreExistsAsync validation as it doesn't exist in the repository
                // This validation should be handled by foreign key constraints or at a higher level
                
                var inventories = await _storeInventoryRepository.GetByStoreIdAsync(storeId);
                var inventoryDtos = _mapper.Map<IEnumerable<StoreInventoryDto>>(inventories);
                
                _logger.LogInformation("Successfully retrieved {Count} inventories for store ID: {StoreId}", 
                    inventoryDtos.Count(), storeId);
                return Result.Success(inventoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventories for store ID: {StoreId}", storeId);
                return Result.Failure<IEnumerable<StoreInventoryDto>>("Failed to retrieve store inventories");
            }
        }

        public async Task<Result<IEnumerable<StoreInventoryDto>>> GetInventoriesByExplosiveTypeAsync(ExplosiveType explosiveType)
        {
            try
            {
                _logger.LogInformation("Retrieving inventories for explosive type: {ExplosiveType}", explosiveType);
                
                var inventories = await _storeInventoryRepository.GetByExplosiveTypeAsync(explosiveType);
                var inventoryDtos = _mapper.Map<IEnumerable<StoreInventoryDto>>(inventories);
                
                _logger.LogInformation("Successfully retrieved {Count} inventories for explosive type: {ExplosiveType}", 
                    inventoryDtos.Count(), explosiveType);
                return Result.Success(inventoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inventories for explosive type: {ExplosiveType}", explosiveType);
                return Result.Failure<IEnumerable<StoreInventoryDto>>("Failed to retrieve inventories by explosive type");
            }
        }

        public async Task<Result<IEnumerable<StoreInventoryDto>>> GetLowStockInventoriesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving low stock inventories");
                
                var inventories = await _storeInventoryRepository.GetLowStockInventoriesAsync();
                var inventoryDtos = _mapper.Map<IEnumerable<StoreInventoryDto>>(inventories);
                
                _logger.LogInformation("Successfully retrieved {Count} low stock inventories", inventoryDtos.Count());
                return Result.Success(inventoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving low stock inventories");
                return Result.Failure<IEnumerable<StoreInventoryDto>>("Failed to retrieve low stock inventories");
            }
        }

        public async Task<Result<IEnumerable<StoreInventoryDto>>> GetExpiringInventoriesAsync(int daysAhead = 30)
        {
            try
            {
                _logger.LogInformation("Retrieving inventories expiring within {DaysAhead} days", daysAhead);
                
                var inventories = await _storeInventoryRepository.GetExpiringInventoriesAsync(daysAhead);
                var inventoryDtos = _mapper.Map<IEnumerable<StoreInventoryDto>>(inventories);
                
                _logger.LogInformation("Successfully retrieved {Count} expiring inventories", inventoryDtos.Count());
                return Result.Success(inventoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expiring inventories");
                return Result.Failure<IEnumerable<StoreInventoryDto>>("Failed to retrieve expiring inventories");
            }
        }

        public async Task<Result> AddStockAsync(int inventoryId, decimal quantity, string? batchNumber = null, string? supplier = null, DateTime? expiryDate = null)
        {
            try
            {
                _logger.LogInformation("Adding stock to inventory ID: {InventoryId}, quantity: {Quantity}", inventoryId, quantity);
                
                var inventory = await _storeInventoryRepository.GetByIdAsync(inventoryId);
                if (inventory == null)
                {
                    _logger.LogWarning("Store inventory with ID {InventoryId} not found", inventoryId);
                    return Result.Failure("Store inventory not found");
                }

                if (quantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity {Quantity} for adding stock to inventory {InventoryId}", quantity, inventoryId);
                    return Result.Failure("Quantity must be positive");
                }

                inventory.AddStock(quantity, batchNumber, supplier, expiryDate);
                await _storeInventoryRepository.UpdateAsync(inventory);
                
                _logger.LogInformation("Successfully added {Quantity} stock to inventory ID: {InventoryId}", quantity, inventoryId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding stock to inventory ID: {InventoryId}", inventoryId);
                return Result.Failure("Failed to add stock to inventory");
            }
        }

        public async Task<Result> RemoveStockAsync(int inventoryId, decimal quantity)
        {
            try
            {
                _logger.LogInformation("Removing stock from inventory ID: {InventoryId}, quantity: {Quantity}", inventoryId, quantity);
                
                var inventory = await _storeInventoryRepository.GetByIdAsync(inventoryId);
                if (inventory == null)
                {
                    _logger.LogWarning("Inventory with ID {InventoryId} not found", inventoryId);
                    return Result.Failure("Inventory not found");
                }

                if (inventory.GetAvailableQuantity() < quantity)
                {
                    _logger.LogWarning("Insufficient stock for inventory ID {InventoryId}. Available: {Available}, Required: {Required}", 
                        inventoryId, inventory.GetAvailableQuantity(), quantity);
                    return Result.Failure("Insufficient stock available");
                }

                inventory.ConsumeStock(quantity);
                await _storeInventoryRepository.UpdateAsync(inventory);
                
                _logger.LogInformation("Successfully removed {Quantity} stock from inventory ID: {InventoryId}", quantity, inventoryId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing stock from inventory ID: {InventoryId}", inventoryId);
                return Result.Failure("Failed to remove stock");
            }
        }

        public async Task<Result> ReserveStockAsync(int inventoryId, decimal quantity)
        {
            try
            {
                _logger.LogInformation("Reserving stock for inventory {InventoryId}, quantity {Quantity}", inventoryId, quantity);

                var inventory = await _storeInventoryRepository.GetByIdAsync(inventoryId);
                if (inventory == null)
                {
                    _logger.LogWarning("Inventory with ID {InventoryId} not found", inventoryId);
                    return Result.Failure("Inventory not found");
                }

                // Check if we can reserve the requested quantity
                if (quantity > inventory.GetAvailableQuantity())
                {
                    _logger.LogWarning("Cannot reserve {Quantity} from inventory {InventoryId}. Available: {Available}", 
                        quantity, inventoryId, inventory.GetAvailableQuantity());
                    return Result.Failure("Insufficient available stock for reservation");
                }

                inventory.ReserveStock(quantity);
                await _storeInventoryRepository.UpdateAsync(inventory);
                
                _logger.LogInformation("Successfully reserved {Quantity} stock for inventory {InventoryId}", quantity, inventoryId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reserving stock for inventory {InventoryId}", inventoryId);
                return Result.Failure("Failed to reserve stock");
            }
        }

        public async Task<Result> ReleaseReservedStockAsync(int inventoryId, decimal quantity)
        {
            try
            {
                _logger.LogInformation("Releasing reserved stock for inventory {InventoryId}, quantity {Quantity}", inventoryId, quantity);

                var inventory = await _storeInventoryRepository.GetByIdAsync(inventoryId);
                if (inventory == null)
                {
                    _logger.LogWarning("Inventory with ID {InventoryId} not found", inventoryId);
                    return Result.Failure("Inventory not found");
                }

                // Check if we can release the requested quantity
                if (quantity > inventory.ReservedQuantity)
                {
                    _logger.LogWarning("Cannot release {Quantity} from inventory {InventoryId}. Reserved: {Reserved}", 
                        quantity, inventoryId, inventory.ReservedQuantity);
                    return Result.Failure("Cannot release more than reserved quantity");
                }

                inventory.ReleaseReservedStock(quantity);
                await _storeInventoryRepository.UpdateAsync(inventory);
                
                _logger.LogInformation("Successfully released {Quantity} reserved stock for inventory {InventoryId}", quantity, inventoryId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error releasing reserved stock for inventory {InventoryId}", inventoryId);
                return Result.Failure("Failed to release reserved stock");
            }
        }

        public async Task<Result<decimal>> GetAvailableQuantityAsync(int inventoryId)
        {
            try
            {
                _logger.LogInformation("Getting available quantity for inventory ID: {InventoryId}", inventoryId);
                
                var inventory = await _storeInventoryRepository.GetByIdAsync(inventoryId);
                if (inventory == null)
                {
                    _logger.LogWarning("Store inventory with ID {InventoryId} not found", inventoryId);
                    return Result.Failure<decimal>("Store inventory not found");
                }

                var availableQuantity = inventory.GetAvailableQuantity();
                _logger.LogInformation("Available quantity for inventory ID {InventoryId}: {AvailableQuantity}", inventoryId, availableQuantity);
                return Result.Success(availableQuantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available quantity for inventory ID: {InventoryId}", inventoryId);
                return Result.Failure<decimal>("Failed to get available quantity");
            }
        }
    }
}