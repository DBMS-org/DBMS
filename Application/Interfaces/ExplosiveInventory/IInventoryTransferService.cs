using Application.Common;
using Application.DTOs.ExplosiveInventory;

namespace Application.Interfaces.ExplosiveInventory
{
    /// <summary>
    /// Service interface for inventory transfer request management
    /// </summary>
    public interface IInventoryTransferService
    {
        // ===== Create & Manage Requests =====
        Task<Result<TransferRequestDto>> CreateTransferRequestAsync(CreateTransferRequestDto request, int requestedByUserId, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> ApproveTransferRequestAsync(int requestId, ApproveTransferRequestDto approval, int approvedByUserId, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> RejectTransferRequestAsync(int requestId, RejectTransferRequestDto rejection, int rejectedByUserId, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> DispatchTransferRequestAsync(int requestId, DispatchTransferRequestDto dispatch, int dispatchedByUserId, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> ConfirmDeliveryAsync(int requestId, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> CompleteTransferRequestAsync(int requestId, int processedByUserId, CancellationToken cancellationToken = default);
        Task<Result> CancelTransferRequestAsync(int requestId, string reason, CancellationToken cancellationToken = default);

        // ===== Query Requests =====
        Task<Result<PagedList<TransferRequestDto>>> GetAllRequestsAsync(TransferRequestFilterDto filter, CancellationToken cancellationToken = default);
        Task<Result<TransferRequestDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<List<TransferRequestDto>>> GetPendingRequestsAsync(CancellationToken cancellationToken = default);
        Task<Result<List<TransferRequestDto>>> GetUrgentRequestsAsync(CancellationToken cancellationToken = default);
        Task<Result<List<TransferRequestDto>>> GetOverdueRequestsAsync(CancellationToken cancellationToken = default);
        Task<Result<List<TransferRequestDto>>> GetByDestinationStoreAsync(int storeId, CancellationToken cancellationToken = default);
        Task<Result<List<TransferRequestDto>>> GetByUserAsync(int userId, CancellationToken cancellationToken = default);
    }
}
