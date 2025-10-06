using Application.Common;
using Application.DTOs.ExplosiveInventory;
using Application.Interfaces.ExplosiveInventory;
using AutoMapper;
using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using FluentValidation;

namespace Application.Services.ExplosiveInventory
{
    /// <summary>
    /// Application service for inventory transfer request management
    /// </summary>
    public class InventoryTransferApplicationService : IInventoryTransferService
    {
        private readonly IInventoryTransferRequestRepository _transferRepository;
        private readonly ICentralWarehouseInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTransferRequestDto> _createValidator;
        private readonly IValidator<ApproveTransferRequestDto> _approveValidator;
        private readonly IValidator<RejectTransferRequestDto> _rejectValidator;
        private readonly IValidator<DispatchTransferRequestDto> _dispatchValidator;

        public InventoryTransferApplicationService(
            IInventoryTransferRequestRepository transferRepository,
            ICentralWarehouseInventoryRepository inventoryRepository,
            IMapper mapper,
            IValidator<CreateTransferRequestDto> createValidator,
            IValidator<ApproveTransferRequestDto> approveValidator,
            IValidator<RejectTransferRequestDto> rejectValidator,
            IValidator<DispatchTransferRequestDto> dispatchValidator)
        {
            _transferRepository = transferRepository;
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _approveValidator = approveValidator;
            _rejectValidator = rejectValidator;
            _dispatchValidator = dispatchValidator;
        }

        // ===== Create & Manage Requests =====

        public async Task<Result<TransferRequestDto>> CreateTransferRequestAsync(
            CreateTransferRequestDto request,
            int requestedByUserId,
            CancellationToken cancellationToken = default)
        {
            // Validate request
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<TransferRequestDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            // Check if inventory exists and has sufficient quantity
            var inventory = await _inventoryRepository.GetByIdAsync(request.CentralWarehouseInventoryId, cancellationToken);
            if (inventory == null)
                return Result.Failure<TransferRequestDto>("Inventory batch not found");

            if (!inventory.CanBeAllocated(request.RequestedQuantity))
                return Result.Failure<TransferRequestDto>(
                    $"Insufficient available quantity. Available: {inventory.AvailableQuantity} {inventory.Unit}, Requested: {request.RequestedQuantity} {request.Unit}");

            // Generate request number
            var requestNumber = InventoryTransferRequest.GenerateRequestNumber();

            // Create transfer request
            var transferRequest = new InventoryTransferRequest(
                requestNumber,
                request.CentralWarehouseInventoryId,
                request.DestinationStoreId,
                request.RequestedQuantity,
                request.Unit,
                requestedByUserId,
                request.RequiredByDate,
                request.RequestNotes
            );

            // Allocate quantity in inventory
            try
            {
                inventory.AllocateQuantity(request.RequestedQuantity);
                await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                await _transferRepository.AddAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result<TransferRequestDto>> ApproveTransferRequestAsync(
            int requestId,
            ApproveTransferRequestDto approval,
            int approvedByUserId,
            CancellationToken cancellationToken = default)
        {
            // Validate
            var validationResult = await _approveValidator.ValidateAsync(approval, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<TransferRequestDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            try
            {
                // If approved quantity differs from requested, need to adjust allocation
                if (approval.ApprovedQuantity.HasValue && approval.ApprovedQuantity.Value != transferRequest.RequestedQuantity)
                {
                    var inventory = await _inventoryRepository.GetByIdAsync(transferRequest.CentralWarehouseInventoryId, cancellationToken);
                    if (inventory != null)
                    {
                        var difference = transferRequest.RequestedQuantity - approval.ApprovedQuantity.Value;
                        if (difference > 0)
                        {
                            // Release excess allocation
                            inventory.ReleaseAllocation(difference);
                            await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                        }
                    }
                }

                transferRequest.Approve(approvedByUserId, approval.ApprovedQuantity, approval.ApprovalNotes);
                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result<TransferRequestDto>> RejectTransferRequestAsync(
            int requestId,
            RejectTransferRequestDto rejection,
            int rejectedByUserId,
            CancellationToken cancellationToken = default)
        {
            var validationResult = await _rejectValidator.ValidateAsync(rejection, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<TransferRequestDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            try
            {
                // Release allocated quantity
                var inventory = await _inventoryRepository.GetByIdAsync(transferRequest.CentralWarehouseInventoryId, cancellationToken);
                if (inventory != null)
                {
                    inventory.ReleaseAllocation(transferRequest.RequestedQuantity);
                    await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                }

                transferRequest.Reject(rejectedByUserId, rejection.RejectionReason);
                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result<TransferRequestDto>> DispatchTransferRequestAsync(
            int requestId,
            DispatchTransferRequestDto dispatch,
            int dispatchedByUserId,
            CancellationToken cancellationToken = default)
        {
            // Validate
            var validationResult = await _dispatchValidator.ValidateAsync(dispatch, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure<TransferRequestDto>(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            try
            {
                transferRequest.Dispatch(
                    dispatchedByUserId,
                    dispatch.TruckNumber,
                    dispatch.DriverName,
                    dispatch.DriverContactNumber,
                    dispatch.DispatchNotes
                );

                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result<TransferRequestDto>> ConfirmDeliveryAsync(int requestId, CancellationToken cancellationToken = default)
        {
            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            try
            {
                transferRequest.ConfirmDelivery();
                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result<TransferRequestDto>> CompleteTransferRequestAsync(
            int requestId,
            int processedByUserId,
            CancellationToken cancellationToken = default)
        {
            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            try
            {
                // Consume quantity from inventory
                var inventory = await _inventoryRepository.GetByIdAsync(transferRequest.CentralWarehouseInventoryId, cancellationToken);
                if (inventory != null)
                {
                    var finalQuantity = transferRequest.GetFinalQuantity();
                    inventory.ConsumeQuantity(finalQuantity);
                    await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                }

                // Note: In real implementation, create StoreTransaction here
                // For now, passing 0 as transaction ID
                transferRequest.Complete(processedByUserId, 0);
                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                var dto = _mapper.Map<TransferRequestDto>(transferRequest);
                return Result.Success(dto);
            }
            catch (Exception ex)
            {
                return Result.Failure<TransferRequestDto>(ex.Message);
            }
        }

        public async Task<Result> CancelTransferRequestAsync(int requestId, string reason, CancellationToken cancellationToken = default)
        {
            var transferRequest = await _transferRepository.GetByIdAsync(requestId, cancellationToken);
            if (transferRequest == null)
                return Result.Failure("Transfer request not found");

            try
            {
                // Release allocated quantity
                var inventory = await _inventoryRepository.GetByIdAsync(transferRequest.CentralWarehouseInventoryId, cancellationToken);
                if (inventory != null && transferRequest.Status == TransferRequestStatus.Pending)
                {
                    inventory.ReleaseAllocation(transferRequest.RequestedQuantity);
                    await _inventoryRepository.UpdateAsync(inventory, cancellationToken);
                }

                transferRequest.Cancel(reason);
                await _transferRepository.UpdateAsync(transferRequest, cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        // ===== Query Requests =====

        public async Task<Result<PagedList<TransferRequestDto>>> GetAllRequestsAsync(
            TransferRequestFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _transferRepository.GetPagedAsync(
                filter.PageNumber,
                filter.PageSize,
                filter.Status,
                filter.DestinationStoreId,
                filter.RequestedByUserId,
                filter.IsOverdue,
                filter.IsUrgent,
                filter.RequestDateFrom,
                filter.RequestDateTo,
                filter.RequiredByDateFrom,
                filter.RequiredByDateTo,
                filter.SortBy,
                filter.SortDescending,
                cancellationToken
            );

            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            var pagedList = new PagedList<TransferRequestDto>(dtos, totalCount, filter.PageNumber, filter.PageSize);

            return Result.Success(pagedList);
        }

        public async Task<Result<TransferRequestDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var transferRequest = await _transferRepository.GetByIdAsync(id, cancellationToken);
            if (transferRequest == null)
                return Result.Failure<TransferRequestDto>("Transfer request not found");

            var dto = _mapper.Map<TransferRequestDto>(transferRequest);
            return Result.Success(dto);
        }

        public async Task<Result<List<TransferRequestDto>>> GetPendingRequestsAsync(CancellationToken cancellationToken = default)
        {
            var items = await _transferRepository.GetPendingRequestsAsync(cancellationToken);
            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<TransferRequestDto>>> GetUrgentRequestsAsync(CancellationToken cancellationToken = default)
        {
            var items = await _transferRepository.GetUrgentRequestsAsync(cancellationToken);
            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<TransferRequestDto>>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default)
        {
            var items = await _transferRepository.GetOverdueRequestsAsync(cancellationToken);
            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<TransferRequestDto>>> GetByDestinationStoreAsync(int storeId, CancellationToken cancellationToken = default)
        {
            var items = await _transferRepository.GetByDestinationStoreAsync(storeId, cancellationToken);
            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            return Result.Success(dtos);
        }

        public async Task<Result<List<TransferRequestDto>>> GetByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            var items = await _transferRepository.GetByUserAsync(userId, cancellationToken);
            var dtos = _mapper.Map<List<TransferRequestDto>>(items);
            return Result.Success(dtos);
        }
    }
}
