using Application.DTOs.StoreManagement;
using Application.DTOs.Shared;
using Application.Exceptions;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.StoreManagement;
using Application.Utilities;
using Application.DTOs.Shared;
using AutoMapper;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Application.Services.StoreManagement
{
    public class StoreApplicationService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ILogger<StoreApplicationService> _logger;

        public StoreApplicationService(
            IStoreRepository storeRepository,
            IValidationService validationService,
            IMapper mapper,
            ILogger<StoreApplicationService> logger)
        {
            _storeRepository = storeRepository;
            _validationService = validationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<StoreDto>>> GetStoresByRegionAsync(int regionId)
        {
            try
            {
                _logger.LogInformation("Retrieving stores for region {RegionId}", regionId);

                var stores = await _storeRepository.GetByRegionIdAsync(regionId);
                var storeDtos = _mapper.Map<IEnumerable<StoreDto>>(stores);

                _logger.LogInformation("Successfully retrieved {Count} stores for region {RegionId}", storeDtos.Count(), regionId);
                return Result.Success(storeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving stores for region {RegionId}", regionId);
                return Result.Failure<IEnumerable<StoreDto>>("An error occurred while retrieving stores for the region");
            }
        }

        public async Task<Result<IEnumerable<StoreDto>>> GetAllStoresAsync()
        {
            try
            {
                var stores = await _storeRepository.GetAllAsync();
                var storeDtos = _mapper.Map<IEnumerable<StoreDto>>(stores);
                
                if (!storeDtos.Any())
                {
                    _logger.LogInformation("No stores found in the system");
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved {Count} stores", storeDtos.Count());
                }
                
                return Result.Success(storeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all stores");
                return Result.Failure<IEnumerable<StoreDto>>("An error occurred while retrieving stores");
            }
        }

        public async Task<Result<StoreDto>> GetStoreByIdAsync(int id)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Store not found with ID {StoreId}", id);
                    return Result.Failure<StoreDto>($"Store with ID {id} not found");
                }

                var storeDto = _mapper.Map<StoreDto>(store);
                _logger.LogInformation("Successfully retrieved store {StoreId}", id);
                return Result.Success(storeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting store {StoreId}", id);
                return Result.Failure<StoreDto>("An error occurred while retrieving the store");
            }
        }

        public async Task<Result<StoreDto>> CreateStoreAsync(CreateStoreRequest request)
        {
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    return Result.Failure<StoreDto>(validationResult.Errors);
                }

                var store = new Store(
                    request.StoreName,
                    request.StoreAddress,
                    request.StorageCapacity,
                    request.City,
                    request.RegionId,
                    request.AllowedExplosiveTypes);

                if (request.ManagerUserId.HasValue)
                {
                    store.AssignManager(request.ManagerUserId.Value);
                }

                var createdStore = await _storeRepository.CreateAsync(store);
                var storeWithDetails = await _storeRepository.GetByIdAsync(createdStore.Id);
                var storeDto = _mapper.Map<StoreDto>(storeWithDetails ?? createdStore);
                
                _logger.LogInformation("Successfully created store {StoreName} with ID {StoreId}", request.StoreName, createdStore.Id);
                return Result.Success(storeDto);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for store creation {StoreName}: {Errors}", 
                    request.StoreName, string.Join(", ", ex.ValidationErrors));
                return Result.Failure<StoreDto>(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error creating store {StoreName}", request.StoreName);
                return Result.Failure<StoreDto>("Database error occurred while creating store");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating store {StoreName}", request.StoreName);
                return Result.Failure<StoreDto>(ErrorCodes.Messages.InternalError);
            }
        }

        public async Task<Result> UpdateStoreAsync(int id, UpdateStoreRequest request)
        {
            try
            {
                // Validate input
                var validationResult = await _validationService.ValidateAsync(request);
                if (validationResult.IsFailure)
                {
                    return Result.Failure(validationResult.Errors);
                }

                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Store not found with ID {StoreId}", id);
                    return Result.Failure($"Store with ID {id} not found");
                }

                store.UpdateStoreDetails(
                    request.StoreName,
                    request.StoreAddress,
                    request.StorageCapacity,
                    request.City,
                    request.AllowedExplosiveTypes);

                store.ChangeStatus(request.Status);

                if (request.ManagerUserId.HasValue)
                {
                    store.AssignManager(request.ManagerUserId.Value);
                }
                else
                {
                    store.RemoveManager();
                }

                await _storeRepository.UpdateAsync(store);
                
                _logger.LogInformation("Successfully updated store {StoreId}", id);
                return Result.Success();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for store update {StoreId}: {Errors}", 
                    id, string.Join(", ", ex.ValidationErrors));
                return Result.Failure(ex.ValidationErrors);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error updating store {StoreId}", id);
                return Result.Failure("Database error occurred while updating store");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating store {StoreId}", id);
                return Result.Failure("An error occurred while updating the store");
            }
        }

        public async Task<Result> DeleteStoreAsync(int id)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Store not found with ID {StoreId}", id);
                    return Result.Failure($"Store with ID {id} not found");
                }

                await _storeRepository.DeleteAsync(id);
                
                _logger.LogInformation("Successfully deleted store {StoreId}", id);
                return Result.Success();
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error deleting store {StoreId}", id);
                return Result.Failure("Database error occurred while deleting store");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting store {StoreId}", id);
                return Result.Failure("An error occurred while deleting the store");
            }
        }

        public async Task<Result<IEnumerable<StoreDto>>> SearchStoresAsync(string? storeName = null, string? city = null, string? status = null)
        {
            try
            {
                _logger.LogInformation("Searching stores with criteria - Name: {StoreName}, City: {City}, Status: {Status}", 
                    storeName, city, status);

                StoreStatus? storeStatus = null;
                if (!string.IsNullOrEmpty(status) && Enum.TryParse<StoreStatus>(status, out var parsedStatus))
                {
                    storeStatus = parsedStatus;
                }

                var stores = await _storeRepository.SearchAsync(storeName, city, null, null, storeStatus);

                var storeDtos = _mapper.Map<IEnumerable<StoreDto>>(stores);

                _logger.LogInformation("Successfully found {Count} stores matching search criteria", storeDtos.Count());
                return Result.Success(storeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching stores");
                return Result.Failure<IEnumerable<StoreDto>>("An error occurred while searching stores");
            }
        }

        public async Task<Result<StoreDto>> GetStoreByManagerAsync(int managerUserId)
        {
            try
            {
                _logger.LogInformation("Getting store by manager with ID: {ManagerId}", managerUserId);

                var store = await _storeRepository.GetStoreByManagerAsync(managerUserId);
                if (store == null)
                {
                    _logger.LogWarning("No store found for manager with ID: {ManagerId}", managerUserId);
                    return Result.Failure<StoreDto>($"No store found for manager with ID {managerUserId}");
                }

                var storeDto = _mapper.Map<StoreDto>(store);
                return Result.Success(storeDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store by manager with ID: {ManagerId}", managerUserId);
                return Result.Failure<StoreDto>("An error occurred while retrieving store by manager");
            }
        }

        public async Task<Result> UpdateStoreStatusAsync(int id, StoreStatus status)
        {
            try
            {
                _logger.LogInformation("Updating store status for store ID: {StoreId} to {Status}", id, status);

                var store = await _storeRepository.GetByIdAsync(id);
                if (store == null)
                {
                    _logger.LogWarning("Store not found with ID: {StoreId}", id);
                    return Result.Failure($"Store with ID {id} not found");
                }

                store.ChangeStatus(status);
                var updateResult = await _storeRepository.UpdateAsync(store);

                if (updateResult)
                {
                    _logger.LogInformation("Successfully updated store status for store ID: {StoreId}", id);
                    return Result.Success();
                }
                else
                {
                    _logger.LogWarning("Failed to update store status for store ID: {StoreId}", id);
                    return Result.Failure("Failed to update store status");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating store status for store ID: {StoreId}", id);
                return Result.Failure("An error occurred while updating store status");
            }
        }

        public async Task<Result<decimal>> GetStoreUtilizationAsync(int storeId)
        {
            try
            {
                _logger.LogInformation("Getting store utilization for store ID: {StoreId}", storeId);

                var store = await _storeRepository.GetByIdAsync(storeId);
                if (store == null)
                {
                    _logger.LogWarning("Store not found with ID: {StoreId}", storeId);
                    return Result.Failure<decimal>($"Store with ID {storeId} not found");
                }

                var utilization = store.GetUtilizationRate();

                _logger.LogInformation("Store utilization for store ID: {StoreId} is {Utilization}%", storeId, utilization);
                return Result.Success(utilization);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store utilization for store ID: {StoreId}", storeId);
                return Result.Failure<decimal>("An error occurred while calculating store utilization");
            }
        }

        public async Task<Result<StoreStatisticsDto>> GetStoreStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Getting store statistics");

                var stores = await _storeRepository.GetAllAsync();
                
                var statistics = new StoreStatisticsDto
                {
                    TotalStores = stores.Count(),
                    ActiveStores = stores.Count(s => s.Status == StoreStatus.Operational || s.Status == StoreStatus.UnderMaintenance),
                    InactiveStores = stores.Count(s => s.Status == StoreStatus.TemporarilyClosed || s.Status == StoreStatus.Decommissioned),
                    OperationalStores = stores.Count(s => s.Status == StoreStatus.Operational),
                    MaintenanceStores = stores.Count(s => s.Status == StoreStatus.UnderMaintenance),
                    TotalCapacity = stores.Sum(s => s.StorageCapacity),
                    TotalOccupancy = stores.Sum(s => s.Inventories?.Sum(i => i.Quantity) ?? 0),
                    StoresByRegion = stores.GroupBy(s => s.City)
                                          .ToDictionary(g => g.Key, g => g.Count())
                };

                // Calculate utilization rate
                statistics.UtilizationRate = statistics.TotalCapacity > 0 
                    ? (statistics.TotalOccupancy / statistics.TotalCapacity) * 100 
                    : 0;

                _logger.LogInformation("Store statistics calculated successfully. Total stores: {TotalStores}", statistics.TotalStores);
                return Result.Success(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting store statistics");
                return Result.Failure<StoreStatisticsDto>("An error occurred while calculating store statistics");
            }
        }
    }
}