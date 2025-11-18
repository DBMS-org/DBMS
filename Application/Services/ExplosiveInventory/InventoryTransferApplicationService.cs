using Application.Common;
using Application.DTOs.ExplosiveInventory;
using Application.Interfaces;
using Application.Interfaces.ExplosiveInventory;
using Application.Interfaces.UserManagement;
using AutoMapper;
using Domain.Entities.ExplosiveInventory;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.Notifications;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Services.ExplosiveInventory
{
    /// <summary>
    /// Application service for inventory transfer request management
    /// </summary>
    public class InventoryTransferApplicationService : IInventoryTransferService
    {
        private readonly IInventoryTransferRequestRepository _transferRepository;
        private readonly ICentralWarehouseInventoryRepository _inventoryRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTransferRequestDto> _createValidator;
        private readonly IValidator<ApproveTransferRequestDto> _approveValidator;
        private readonly IValidator<RejectTransferRequestDto> _rejectValidator;
        private readonly IValidator<DispatchTransferRequestDto> _dispatchValidator;
        private readonly ILogger<InventoryTransferApplicationService> _logger;

        public InventoryTransferApplicationService(
            IInventoryTransferRequestRepository transferRepository,
            ICentralWarehouseInventoryRepository inventoryRepository,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<CreateTransferRequestDto> createValidator,
            IValidator<ApproveTransferRequestDto> approveValidator,
            IValidator<RejectTransferRequestDto> rejectValidator,
            IValidator<DispatchTransferRequestDto> dispatchValidator,
            ILogger<InventoryTransferApplicationService> logger)
        {
            _transferRepository = transferRepository;
            _inventoryRepository = inventoryRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _approveValidator = approveValidator;
            _rejectValidator = rejectValidator;
            _dispatchValidator = dispatchValidator;
            _logger = logger;
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

                // Create notification for Explosive Managers in the region
                await CreateTransferRequestNotificationAsync(transferRequest, requestedByUserId);

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

                // Notify the requester that their request was approved
                await NotifyTransferRequestApprovedAsync(transferRequest);

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

                // Notify the requester that their request was rejected
                await NotifyTransferRequestRejectedAsync(transferRequest);

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

                // Notify destination Store Manager that transfer has been dispatched
                await NotifyTransferDispatchedAsync(transferRequest);

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

                // Notify Explosive Manager that delivery has been confirmed
                await NotifyDeliveryConfirmedAsync(transferRequest);

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

                // Notify relevant parties that transfer has been completed
                await NotifyTransferCompletedAsync(transferRequest);

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

        // ===== Private Notification Helper Methods =====

        /// <summary>
        /// Notify Explosive Managers when a new transfer request is created
        /// </summary>
        private async Task CreateTransferRequestNotificationAsync(InventoryTransferRequest request, int requestedByUserId)
        {
            try
            {
                // Get the requester info
                var requester = await _userRepository.GetByIdAsync(requestedByUserId);
                if (requester == null) return;

                // Get all Explosive Managers in the system (they manage central warehouse)
                var explosiveManagers = await _userRepository.GetByRoleAndRegionAsync("ExplosiveManager");

                if (!explosiveManagers.Any()) return;

                // Determine priority based on urgency
                var priority = request.IsUrgent() ? NotificationPriority.Urgent : NotificationPriority.Normal;

                // Create notifications for all Explosive Managers
                var notifications = explosiveManagers.Select(manager =>
                    Notification.Create(
                        userId: manager.Id,
                        type: NotificationType.TransferRequestCreated,
                        title: "New Inventory Transfer Request",
                        message: $"{requester.Name} has requested {request.RequestedQuantity} {request.Unit} of explosives for transfer to Store #{request.DestinationStoreId}. Request #{request.RequestNumber}.",
                        priority: priority,
                        relatedEntityType: "InventoryTransferRequest",
                        relatedEntityId: request.Id,
                        actionUrl: $"/explosive-manager/requests"
                    )
                ).ToList();

                await _notificationRepository.CreateBulkAsync(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create notification for transfer request {RequestId}", request.Id);
                // Don't fail the main operation
            }
        }

        /// <summary>
        /// Notify the requester when their transfer request is approved
        /// </summary>
        private async Task NotifyTransferRequestApprovedAsync(InventoryTransferRequest request)
        {
            try
            {
                _logger.LogInformation("🔔 Starting notification creation for transfer request {RequestId}", request.Id);

                var requester = await _userRepository.GetByIdAsync(request.RequestedByUserId);
                if (requester == null)
                {
                    _logger.LogWarning("🔔 Requester with ID {RequestedByUserId} not found for transfer request {RequestId}",
                        request.RequestedByUserId, request.Id);
                    return;
                }

                _logger.LogInformation("🔔 Found requester: {RequesterName} (ID: {RequesterId})", requester.Name, requester.Id);

                var approver = request.ApprovedByUserId.HasValue
                    ? await _userRepository.GetByIdAsync(request.ApprovedByUserId.Value)
                    : null;

                var approverName = approver?.Name ?? "Explosive Manager";
                var approvedQty = request.ApprovedQuantity ?? request.RequestedQuantity;

                _logger.LogInformation("🔔 Creating notification for user {UserId}", requester.Id);

                var notification = Notification.Create(
                    userId: requester.Id,
                    type: NotificationType.TransferRequestApproved,
                    title: "Transfer Request Approved",
                    message: $"Your transfer request #{request.RequestNumber} has been approved by {approverName}. Approved quantity: {approvedQty} {request.Unit}. {(!string.IsNullOrEmpty(request.ApprovalNotes) ? $"Notes: {request.ApprovalNotes}" : "")}",
                    priority: NotificationPriority.High,
                    relatedEntityType: "InventoryTransferRequest",
                    relatedEntityId: request.Id,
                    actionUrl: $"/store-manager/request-history/{request.Id}"
                );

                await _notificationRepository.CreateAsync(notification);
                _logger.LogInformation("🔔 Notification created successfully for transfer request {RequestId}", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to create approval notification for transfer request {RequestId}. Error: {ErrorMessage}",
                    request.Id, ex.Message);
                _logger.LogError("❌ Stack trace: {StackTrace}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Notify the requester when their transfer request is rejected
        /// </summary>
        private async Task NotifyTransferRequestRejectedAsync(InventoryTransferRequest request)
        {
            try
            {
                _logger.LogInformation("🔔 Starting rejection notification creation for transfer request {RequestId}", request.Id);

                var requester = await _userRepository.GetByIdAsync(request.RequestedByUserId);
                if (requester == null)
                {
                    _logger.LogWarning("🔔 Requester with ID {RequestedByUserId} not found for transfer request {RequestId}",
                        request.RequestedByUserId, request.Id);
                    return;
                }

                _logger.LogInformation("🔔 Found requester: {RequesterName} (ID: {RequesterId})", requester.Name, requester.Id);

                var notification = Notification.Create(
                    userId: requester.Id,
                    type: NotificationType.TransferRequestRejected,
                    title: "Transfer Request Rejected",
                    message: $"Your transfer request #{request.RequestNumber} has been rejected. Reason: {request.RejectionReason ?? "No reason provided"}",
                    priority: NotificationPriority.High,
                    relatedEntityType: "InventoryTransferRequest",
                    relatedEntityId: request.Id,
                    actionUrl: $"/store-manager/request-history/{request.Id}"
                );

                await _notificationRepository.CreateAsync(notification);
                _logger.LogInformation("🔔 Rejection notification created successfully for transfer request {RequestId}", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to create rejection notification for transfer request {RequestId}. Error: {ErrorMessage}",
                    request.Id, ex.Message);
                _logger.LogError("❌ Stack trace: {StackTrace}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Notify the destination Store Manager when transfer is dispatched
        /// </summary>
        private async Task NotifyTransferDispatchedAsync(InventoryTransferRequest request)
        {
            try
            {
                // Get the requester (destination Store Manager)
                var storeManager = await _userRepository.GetByIdAsync(request.RequestedByUserId);
                if (storeManager == null) return;

                var notification = Notification.Create(
                    userId: storeManager.Id,
                    type: NotificationType.TransferDispatched,
                    title: "Transfer Dispatched",
                    message: $"Your transfer request #{request.RequestNumber} has been dispatched. Truck: {request.TruckNumber}, Driver: {request.DriverName}. {(!string.IsNullOrEmpty(request.DispatchNotes) ? $"Notes: {request.DispatchNotes}" : "")}",
                    priority: NotificationPriority.High,
                    relatedEntityType: "InventoryTransferRequest",
                    relatedEntityId: request.Id,
                    actionUrl: $"/store-manager/request-history/{request.Id}"
                );

                await _notificationRepository.CreateAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create dispatch notification for transfer request {RequestId}", request.Id);
            }
        }

        /// <summary>
        /// Notify relevant parties when transfer is completed
        /// </summary>
        private async Task NotifyTransferCompletedAsync(InventoryTransferRequest request)
        {
            try
            {
                // Notify the requester (Store Manager)
                var requester = await _userRepository.GetByIdAsync(request.RequestedByUserId);
                if (requester == null) return;

                var finalQty = request.GetFinalQuantity();

                var notification = Notification.Create(
                    userId: requester.Id,
                    type: NotificationType.TransferCompleted,
                    title: "Transfer Request Completed",
                    message: $"Transfer request #{request.RequestNumber} has been completed. {finalQty} {request.Unit} of explosives have been added to your store inventory.",
                    priority: NotificationPriority.Normal,
                    relatedEntityType: "InventoryTransferRequest",
                    relatedEntityId: request.Id,
                    actionUrl: $"/store-manager/inventory"
                );

                await _notificationRepository.CreateAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create completion notification for transfer request {RequestId}", request.Id);
            }
        }

        /// <summary>
        /// Notify Explosive Manager when Store Manager confirms delivery receipt
        /// </summary>
        private async Task NotifyDeliveryConfirmedAsync(InventoryTransferRequest request)
        {
            try
            {
                _logger.LogInformation("🔔 Starting delivery confirmation notification for transfer request {RequestId}", request.Id);

                // Get all Explosive Managers in the system (they manage central warehouse)
                var explosiveManagers = await _userRepository.GetByRoleAndRegionAsync("ExplosiveManager");

                if (!explosiveManagers.Any())
                {
                    _logger.LogWarning("🔔 No Explosive Managers found to notify for delivery confirmation");
                    return;
                }

                _logger.LogInformation("🔔 Found {Count} Explosive Managers to notify", explosiveManagers.Count());

                var requester = await _userRepository.GetByIdAsync(request.RequestedByUserId);
                var requesterName = requester?.Name ?? "Store Manager";

                // Create notifications for all Explosive Managers
                var notifications = explosiveManagers.Select(manager =>
                    Notification.Create(
                        userId: manager.Id,
                        type: NotificationType.TransferDeliveryConfirmed,
                        title: "Transfer Delivery Confirmed",
                        message: $"{requesterName} has confirmed receipt of transfer request #{request.RequestNumber}. {request.GetFinalQuantity()} {request.Unit} of explosives delivered to Store #{request.DestinationStoreId}.",
                        priority: NotificationPriority.Normal,
                        relatedEntityType: "InventoryTransferRequest",
                        relatedEntityId: request.Id,
                        actionUrl: $"/explosive-manager/requests"
                    )
                ).ToList();

                await _notificationRepository.CreateBulkAsync(notifications);
                _logger.LogInformation("🔔 Delivery confirmation notification created successfully for transfer request {RequestId}", request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to create delivery confirmation notification for transfer request {RequestId}. Error: {ErrorMessage}",
                    request.Id, ex.Message);
            }
        }
    }
}
