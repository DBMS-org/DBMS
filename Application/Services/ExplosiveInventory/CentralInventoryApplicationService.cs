using Application.Common;
using Application.DTOs.ExplosiveInventory;
using Application.Interfaces.ExplosiveInventory;
using AutoMapper;
using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement.Enums;
using Domain.Services.ExplosiveInventory;
using FluentValidation;

namespace Application.Services.ExplosiveInventory
{
    /// <summary>
    /// Application service for central warehouse inventory management
    /// </summary>
    public class CentralInventoryApplicationService : ICentralInventoryService
    {
        private readonly ICentralWarehouseInventoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly InventoryValidationDomainService _validationService;
        private readonly IValidator<CreateANFOInventoryRequest> _anfoValidator;
        private readonly IValidator<CreateEmulsionInventoryRequest> _emulsionValidator;

        public CentralInventoryApplicationService(
            ICentralWarehouseInventoryRepository repository,
            IMapper mapper,
            InventoryValidationDomainService validationService,
            IValidator<CreateANFOInventoryRequest> anfoValidator,
            IValidator<CreateEmulsionInventoryRequest> emulsionValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _validationService = validationService;
            _anfoValidator = anfoValidator;
            _emulsionValidator = emulsionValidator;
        }

        // ===== ANFO Operations =====

        public async Task<Result<CentralInventoryDto>> CreateANFOBatchAsync(
            CreateANFOInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            // Validate request
            var validationResult = await _anfoValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<CentralInventoryDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            // Check if batch ID already exists
            if (await _repository.ExistsAsync(request.BatchId, cancellationToken))
                return Result.Failure<CentralInventoryDto>($"Batch ID '{request.BatchId}' already exists");

            // Create inventory entity
            var inventory = new CentralWarehouseInventory(
                request.BatchId,
                ExplosiveType.ANFO,
                request.Quantity,
                request.Unit,
                request.ManufacturingDate,
                request.ExpiryDate,
                request.Supplier,
                request.StorageLocation,
                request.CentralWarehouseStoreId,
                request.ManufacturerBatchNumber
            );

            // Create ANFO technical properties
            var anfoProperties = new ANFOTechnicalProperties(
                inventory.Id, // Will be set after save
                request.Density,
                request.FuelOilContent,
                request.Grade,
                request.StorageTemperature,
                request.StorageHumidity,
                request.FumeClass,
                request.QualityStatus,
                request.MoistureContent,
                request.PrillSize,
                request.DetonationVelocity,
                request.Notes
            );

            // Validate technical properties using domain service
            var techValidation = _validationService.ValidateANFOProperties(anfoProperties);
            if (!techValidation.IsValid)
                return Result.Failure<CentralInventoryDto>(techValidation.ErrorMessage);

            // Set properties
            inventory.SetANFOProperties(anfoProperties);

            // Save to repository
            await _repository.AddAsync(inventory, cancellationToken);

            // Map to DTO
            var dto = _mapper.Map<CentralInventoryDto>(inventory);
            return Result.Success(dto);
        }

        public async Task<Result<CentralInventoryDto>> UpdateANFOBatchAsync(
            int id,
            UpdateANFOInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure<CentralInventoryDto>("Inventory batch not found");

            if (inventory.ExplosiveType != ExplosiveType.ANFO)
                return Result.Failure<CentralInventoryDto>("Inventory is not ANFO type");

            if (inventory.ANFOProperties == null)
                return Result.Failure<CentralInventoryDto>("ANFO properties not found");

            try
            {
                // Update quantity if provided
                if (request.Quantity.HasValue)
                    inventory.UpdateQuantity(request.Quantity.Value);

                // Update storage location if provided
                if (!string.IsNullOrWhiteSpace(request.StorageLocation))
                    inventory.UpdateStorageLocation(request.StorageLocation);

                // Update quality parameters if provided
                inventory.ANFOProperties.UpdateQualityParameters(
                    request.Density,
                    request.FuelOilContent,
                    request.MoistureContent,
                    request.PrillSize,
                    request.DetonationVelocity
                );

                // Update storage conditions if provided
                if (request.StorageTemperature.HasValue || request.StorageHumidity.HasValue)
                {
                    inventory.ANFOProperties.UpdateStorageConditions(
                        request.StorageTemperature ?? inventory.ANFOProperties.StorageTemperature,
                        request.StorageHumidity ?? inventory.ANFOProperties.StorageHumidity
                    );
                }

                // Add notes if provided
                if (!string.IsNullOrWhiteSpace(request.Notes))
                    inventory.ANFOProperties.AddNotes(request.Notes);

                // Validate updated properties
                var techValidation = _validationService.ValidateANFOProperties(inventory.ANFOProperties);
                if (!techValidation.IsValid)
                    return Result.Failure<CentralInventoryDto>(techValidation.ErrorMessage);

                await _repository.UpdateAsync(inventory, cancellationToken);

                var dto = _mapper.Map<CentralInventoryDto>(inventory);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<CentralInventoryDto>(ex.Message);
            }
        }

        // ===== Emulsion Operations =====

        public async Task<Result<CentralInventoryDto>> CreateEmulsionBatchAsync(
            CreateEmulsionInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            // Validate request
            var validationResult = await _emulsionValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<CentralInventoryDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            // Check if batch ID already exists
            if (await _repository.ExistsAsync(request.BatchId, cancellationToken))
                return Result.Failure<CentralInventoryDto>($"Batch ID '{request.BatchId}' already exists");

            // Create inventory entity
            var inventory = new CentralWarehouseInventory(
                request.BatchId,
                ExplosiveType.Emulsion,
                request.Quantity,
                request.Unit,
                request.ManufacturingDate,
                request.ExpiryDate,
                request.Supplier,
                request.StorageLocation,
                request.CentralWarehouseStoreId,
                request.ManufacturerBatchNumber
            );

            // Create Emulsion technical properties
            var emulsionProperties = new EmulsionTechnicalProperties(
                inventory.Id,
                request.DensityUnsensitized,
                request.DensitySensitized,
                request.Viscosity,
                request.WaterContent,
                request.pH,
                request.StorageTemperature,
                request.Grade,
                request.Color,
                request.SensitizationType,
                request.FumeClass,
                request.QualityStatus,
                request.DetonationVelocity,
                request.BubbleSize,
                request.ApplicationTemperature,
                request.SensitizerContent,
                request.Notes
            );

            // Validate technical properties
            var techValidation = _validationService.ValidateEmulsionProperties(emulsionProperties);
            if (!techValidation.IsValid)
                return Result.Failure<CentralInventoryDto>(techValidation.ErrorMessage);

            // Set properties
            inventory.SetEmulsionProperties(emulsionProperties);

            // Save to repository
            await _repository.AddAsync(inventory, cancellationToken);

            // Map to DTO
            var dto = _mapper.Map<CentralInventoryDto>(inventory);
            return Result.Success(dto);
        }

        public async Task<Result<CentralInventoryDto>> UpdateEmulsionBatchAsync(
            int id,
            UpdateEmulsionInventoryRequest request,
            CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure<CentralInventoryDto>("Inventory batch not found");

            if (inventory.ExplosiveType != ExplosiveType.Emulsion)
                return Result.Failure<CentralInventoryDto>("Inventory is not Emulsion type");

            if (inventory.EmulsionProperties == null)
                return Result.Failure<CentralInventoryDto>("Emulsion properties not found");

            try
            {
                // Update quantity if provided
                if (request.Quantity.HasValue)
                    inventory.UpdateQuantity(request.Quantity.Value);

                // Update storage location if provided
                if (!string.IsNullOrWhiteSpace(request.StorageLocation))
                    inventory.UpdateStorageLocation(request.StorageLocation);

                // Update density if provided
                if (request.DensityUnsensitized.HasValue || request.DensitySensitized.HasValue)
                {
                    inventory.EmulsionProperties.UpdateDensity(
                        request.DensityUnsensitized ?? inventory.EmulsionProperties.DensityUnsensitized,
                        request.DensitySensitized ?? inventory.EmulsionProperties.DensitySensitized
                    );
                }

                // Update rheology if provided
                if (request.Viscosity.HasValue || request.WaterContent.HasValue || request.pH.HasValue)
                {
                    inventory.EmulsionProperties.UpdateRheology(
                        request.Viscosity ?? inventory.EmulsionProperties.Viscosity,
                        request.WaterContent ?? inventory.EmulsionProperties.WaterContent,
                        request.pH ?? inventory.EmulsionProperties.pH
                    );
                }

                // Update storage conditions if provided
                if (request.StorageTemperature.HasValue || request.ApplicationTemperature.HasValue)
                {
                    inventory.EmulsionProperties.UpdateStorageConditions(
                        request.StorageTemperature ?? inventory.EmulsionProperties.StorageTemperature,
                        request.ApplicationTemperature
                    );
                }

                // Update performance if provided
                if (request.DetonationVelocity.HasValue || request.BubbleSize.HasValue)
                {
                    inventory.EmulsionProperties.UpdatePerformance(
                        request.DetonationVelocity,
                        request.BubbleSize
                    );
                }

                // Add notes if provided
                if (!string.IsNullOrWhiteSpace(request.Notes))
                    inventory.EmulsionProperties.AddNotes(request.Notes);

                // Validate updated properties
                var techValidation = _validationService.ValidateEmulsionProperties(inventory.EmulsionProperties);
                if (!techValidation.IsValid)
                    return Result.Failure<CentralInventoryDto>(techValidation.ErrorMessage);

                await _repository.UpdateAsync(inventory, cancellationToken);

                var dto = _mapper.Map<CentralInventoryDto>(inventory);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<CentralInventoryDto>(ex.Message);
            }
        }

        // ===== Query Operations =====

        public async Task<Result<PagedList<CentralInventoryDto>>> GetAllInventoryAsync(
            InventoryFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(
                filter.PageNumber,
                filter.PageSize,
                filter.ExplosiveType,
                filter.Status,
                filter.Supplier,
                filter.BatchId,
                filter.IsExpired,
                filter.IsExpiringSoon,
                filter.ManufacturingDateFrom,
                filter.ManufacturingDateTo,
                filter.ExpiryDateFrom,
                filter.ExpiryDateTo,
                filter.SortBy,
                filter.SortDescending,
                cancellationToken
            );

            var dtos = _mapper.Map<List<CentralInventoryDto>>(items);
            var pagedList = new PagedList<CentralInventoryDto>(dtos, totalCount, filter.PageNumber, filter.PageSize);

            return Result.Success(pagedList);
        }

        public async Task<Result<CentralInventoryDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure<CentralInventoryDto>("Inventory batch not found");

            var dto = _mapper.Map<CentralInventoryDto>(inventory);
            return Result.Success(dto);
        }

        public async Task<Result<CentralInventoryDto>> GetByBatchIdAsync(string batchId, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByBatchIdAsync(batchId, cancellationToken);
            if (inventory == null)
                return Result.Failure<CentralInventoryDto>($"Inventory batch '{batchId}' not found");

            var dto = _mapper.Map<CentralInventoryDto>(inventory);
            return Result.Success(dto);
        }

        public async Task<Result<List<CentralInventoryDto>>> GetByExplosiveTypeAsync(
            ExplosiveType type,
            CancellationToken cancellationToken = default)
        {
            var items = await _repository.GetByExplosiveTypeAsync(type, cancellationToken);
            var dtos = _mapper.Map<List<CentralInventoryDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<CentralInventoryDto>>> GetExpiringBatchesAsync(
            int daysThreshold = 30,
            CancellationToken cancellationToken = default)
        {
            var items = await _repository.GetExpiringBatchesAsync(daysThreshold, cancellationToken);
            var dtos = _mapper.Map<List<CentralInventoryDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<CentralInventoryDto>>> GetExpiredBatchesAsync(CancellationToken cancellationToken = default)
        {
            var items = await _repository.GetExpiredBatchesAsync(cancellationToken);
            var dtos = _mapper.Map<List<CentralInventoryDto>>(items);
            return Result.Success(dtos);
        }

        // ===== Status Management =====

        public async Task<Result> QuarantineBatchAsync(int id, string reason, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure("Inventory batch not found");

            try
            {
                inventory.QuarantineBatch(reason);
                await _repository.UpdateAsync(inventory, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> ReleaseFromQuarantineAsync(int id, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure("Inventory batch not found");

            try
            {
                inventory.ReleaseFromQuarantine();
                await _repository.UpdateAsync(inventory, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> MarkAsExpiredAsync(int id, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure("Inventory batch not found");

            try
            {
                inventory.MarkAsExpired();
                await _repository.UpdateAsync(inventory, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateStorageLocationAsync(int id, string newLocation, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure("Inventory batch not found");

            try
            {
                inventory.UpdateStorageLocation(newLocation);
                await _repository.UpdateAsync(inventory, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        // ===== Dashboard =====

        public async Task<Result<InventoryDashboardDto>> GetDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            var allInventory = await _repository.GetAllAsync(cancellationToken);

            var dashboard = new InventoryDashboardDto
            {
                TotalBatches = allInventory.Count,
                TotalQuantity = allInventory.Sum(i => i.Quantity),
                AvailableQuantity = allInventory.Sum(i => i.AvailableQuantity),
                AllocatedQuantity = allInventory.Sum(i => i.AllocatedQuantity),
                ExpiringBatches = allInventory.Count(i => i.IsExpiringSoon),
                ExpiredBatches = allInventory.Count(i => i.IsExpired),
                QuarantinedBatches = allInventory.Count(i => i.Status == InventoryStatus.Quarantined),
                DepletedBatches = allInventory.Count(i => i.Status == InventoryStatus.Depleted)
            };

            // Quantity by type
            foreach (var group in allInventory.GroupBy(i => i.ExplosiveType))
            {
                dashboard.QuantityByType[group.Key] = group.Sum(i => i.Quantity);
                dashboard.BatchesByType[group.Key] = group.Count();
            }

            // Generate alerts
            foreach (var inventory in allInventory)
            {
                if (inventory.IsExpired)
                {
                    dashboard.Alerts.Add(new InventoryAlertDto
                    {
                        AlertType = "Expired",
                        Severity = "Critical",
                        Message = $"Batch {inventory.BatchId} has expired",
                        InventoryId = inventory.Id,
                        BatchId = inventory.BatchId,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else if (inventory.IsExpiringSoon)
                {
                    dashboard.Alerts.Add(new InventoryAlertDto
                    {
                        AlertType = "ExpiringSoon",
                        Severity = "Warning",
                        Message = $"Batch {inventory.BatchId} expires in {inventory.DaysUntilExpiry} days",
                        InventoryId = inventory.Id,
                        BatchId = inventory.BatchId,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (inventory.Status == InventoryStatus.Quarantined)
                {
                    dashboard.Alerts.Add(new InventoryAlertDto
                    {
                        AlertType = "Quarantined",
                        Severity = "Warning",
                        Message = $"Batch {inventory.BatchId} is quarantined",
                        InventoryId = inventory.Id,
                        BatchId = inventory.BatchId,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            return Result.Success(dashboard);
        }

        // ===== Delete =====

        public async Task<Result> DeleteBatchAsync(int id, CancellationToken cancellationToken = default)
        {
            var inventory = await _repository.GetByIdAsync(id, cancellationToken);
            if (inventory == null)
                return Result.Failure("Inventory batch not found");

            if (inventory.AllocatedQuantity > 0)
                return Result.Failure("Cannot delete batch with allocated quantity");

            try
            {
                await _repository.DeleteAsync(inventory, cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
