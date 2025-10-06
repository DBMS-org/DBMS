using Domain.Common;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.ProjectManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.ExplosiveInventory
{
    /// <summary>
    /// Represents a request to transfer explosive materials from central warehouse to a store
    /// Implements approval workflow managed by Explosive Manager with dispatch tracking
    /// </summary>
    public class InventoryTransferRequest : BaseAuditableEntity
    {
        // Core Info
        public string RequestNumber { get; private set; } = string.Empty;

        // Source & Destination
        public int CentralWarehouseInventoryId { get; private set; }
        public int DestinationStoreId { get; private set; }

        // Quantity
        public decimal RequestedQuantity { get; private set; }
        public decimal? ApprovedQuantity { get; private set; }
        public string Unit { get; private set; } = string.Empty;

        // Status & Workflow
        public TransferRequestStatus Status { get; private set; }
        public DateTime RequestDate { get; private set; }
        public DateTime? ApprovedDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public DateTime? RequiredByDate { get; private set; }

        // People
        public int RequestedByUserId { get; private set; }
        public int? ApprovedByUserId { get; private set; }
        public int? ProcessedByUserId { get; private set; }

        // Dispatch/Delivery Tracking
        public DateTime? DispatchDate { get; private set; }
        public string? TruckNumber { get; private set; }
        public string? DriverName { get; private set; }
        public string? DriverContactNumber { get; private set; }
        public string? DispatchNotes { get; private set; }
        public int? DispatchedByUserId { get; private set; }
        public DateTime? DeliveryConfirmedDate { get; private set; }

        // Notes
        public string? RequestNotes { get; private set; }
        public string? ApprovalNotes { get; private set; }
        public string? RejectionReason { get; private set; }

        // Transaction Reference
        public int? CompletedTransactionId { get; private set; }

        // Navigation Properties
        public virtual CentralWarehouseInventory CentralInventory { get; private set; } = null!;
        public virtual Store DestinationStore { get; private set; } = null!;
        public virtual User RequestedByUser { get; private set; } = null!;
        public virtual User? ApprovedByUser { get; private set; }
        public virtual User? ProcessedByUser { get; private set; }
        public virtual User? DispatchedByUser { get; private set; }
        public virtual StoreTransaction? CompletedTransaction { get; private set; }

        // Private constructor for EF Core
        private InventoryTransferRequest() { }

        // Constructor
        public InventoryTransferRequest(
            string requestNumber,
            int centralWarehouseInventoryId,
            int destinationStoreId,
            decimal requestedQuantity,
            string unit,
            int requestedByUserId,
            DateTime? requiredByDate = null,
            string? requestNotes = null)
        {
            ValidateTransferRequest(requestNumber, requestedQuantity, unit);

            RequestNumber = requestNumber;
            CentralWarehouseInventoryId = centralWarehouseInventoryId;
            DestinationStoreId = destinationStoreId;
            RequestedQuantity = requestedQuantity;
            Unit = unit;
            RequestedByUserId = requestedByUserId;
            RequiredByDate = requiredByDate;
            RequestNotes = requestNotes;

            Status = TransferRequestStatus.Pending;
            RequestDate = DateTime.UtcNow;
        }

        // Business Methods

        /// <summary>
        /// Approve the transfer request (by Explosive Manager)
        /// </summary>
        public void Approve(int approvedByUserId, decimal? approvedQuantity = null, string? approvalNotes = null)
        {
            if (!CanBeApproved())
                throw new InvalidOperationException(
                    $"Transfer request cannot be approved. Current status: {Status}");

            // Use requested quantity if approved quantity not specified
            var quantityToApprove = approvedQuantity ?? RequestedQuantity;

            if (quantityToApprove <= 0)
                throw new ArgumentException("Approved quantity must be greater than zero", nameof(approvedQuantity));

            if (quantityToApprove > RequestedQuantity)
                throw new ArgumentException(
                    "Approved quantity cannot exceed requested quantity", nameof(approvedQuantity));

            ApprovedQuantity = quantityToApprove;
            ApprovedByUserId = approvedByUserId;
            ApprovedDate = DateTime.UtcNow;
            ApprovalNotes = approvalNotes;
            Status = TransferRequestStatus.Approved;

            MarkUpdated();
        }

        /// <summary>
        /// Reject the transfer request (by Explosive Manager)
        /// </summary>
        public void Reject(int rejectedByUserId, string reason)
        {
            if (!CanBeRejected())
                throw new InvalidOperationException(
                    $"Transfer request cannot be rejected. Current status: {Status}");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Rejection reason is required", nameof(reason));

            ApprovedByUserId = rejectedByUserId;
            ApprovedDate = DateTime.UtcNow;
            RejectionReason = reason;
            Status = TransferRequestStatus.Rejected;

            MarkUpdated();
        }

        /// <summary>
        /// Mark request as in progress (transfer being fulfilled)
        /// </summary>
        public void MarkInProgress(int processedByUserId)
        {
            if (Status != TransferRequestStatus.Approved)
                throw new InvalidOperationException("Only approved requests can be marked as in progress");

            ProcessedByUserId = processedByUserId;
            Status = TransferRequestStatus.InProgress;

            MarkUpdated();
        }

        /// <summary>
        /// Dispatch the approved transfer request
        /// </summary>
        public void Dispatch(
            int dispatchedByUserId,
            string truckNumber,
            string driverName,
            string? driverContactNumber = null,
            string? dispatchNotes = null)
        {
            if (Status != TransferRequestStatus.Approved)
                throw new InvalidOperationException("Only approved requests can be dispatched");

            if (string.IsNullOrWhiteSpace(truckNumber))
                throw new ArgumentException("Truck number is required", nameof(truckNumber));

            if (string.IsNullOrWhiteSpace(driverName))
                throw new ArgumentException("Driver name is required", nameof(driverName));

            DispatchedByUserId = dispatchedByUserId;
            DispatchDate = DateTime.UtcNow;
            TruckNumber = truckNumber;
            DriverName = driverName;
            DriverContactNumber = driverContactNumber;
            DispatchNotes = dispatchNotes;
            Status = TransferRequestStatus.InProgress;

            MarkUpdated();
        }

        /// <summary>
        /// Confirm delivery of dispatched transfer
        /// </summary>
        public void ConfirmDelivery()
        {
            if (Status != TransferRequestStatus.InProgress)
                throw new InvalidOperationException("Only in-progress requests can have delivery confirmed");

            if (!DispatchDate.HasValue)
                throw new InvalidOperationException("Request must be dispatched before delivery can be confirmed");

            DeliveryConfirmedDate = DateTime.UtcNow;

            MarkUpdated();
        }

        /// <summary>
        /// Complete the transfer request
        /// </summary>
        public void Complete(int processedByUserId, int transactionId)
        {
            if (Status != TransferRequestStatus.Approved && Status != TransferRequestStatus.InProgress)
                throw new InvalidOperationException(
                    "Only approved or in-progress requests can be completed");

            ProcessedByUserId = processedByUserId;
            CompletedTransactionId = transactionId;
            CompletedDate = DateTime.UtcNow;
            Status = TransferRequestStatus.Completed;

            MarkUpdated();
        }

        /// <summary>
        /// Cancel the transfer request
        /// </summary>
        public void Cancel(string reason)
        {
            if (Status == TransferRequestStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed transfer request");

            if (Status == TransferRequestStatus.Cancelled)
                throw new InvalidOperationException("Transfer request is already cancelled");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Cancellation reason is required", nameof(reason));

            RejectionReason = reason;
            Status = TransferRequestStatus.Cancelled;

            MarkUpdated();
        }

        /// <summary>
        /// Update request notes
        /// </summary>
        public void UpdateRequestNotes(string notes)
        {
            if (Status != TransferRequestStatus.Pending)
                throw new InvalidOperationException("Can only update notes on pending requests");

            RequestNotes = notes;
            MarkUpdated();
        }

        /// <summary>
        /// Update required by date
        /// </summary>
        public void UpdateRequiredByDate(DateTime? requiredByDate)
        {
            if (Status == TransferRequestStatus.Completed || Status == TransferRequestStatus.Cancelled)
                throw new InvalidOperationException("Cannot update required date on completed or cancelled requests");

            RequiredByDate = requiredByDate;
            MarkUpdated();
        }

        /// <summary>
        /// Check if request can be approved
        /// </summary>
        public bool CanBeApproved()
        {
            return Status == TransferRequestStatus.Pending;
        }

        /// <summary>
        /// Check if request can be rejected
        /// </summary>
        public bool CanBeRejected()
        {
            return Status == TransferRequestStatus.Pending;
        }

        /// <summary>
        /// Check if request can be cancelled
        /// </summary>
        public bool CanBeCancelled()
        {
            return Status != TransferRequestStatus.Completed &&
                   Status != TransferRequestStatus.Cancelled;
        }

        /// <summary>
        /// Check if request is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return RequiredByDate.HasValue &&
                   DateTime.UtcNow > RequiredByDate.Value &&
                   Status != TransferRequestStatus.Completed &&
                   Status != TransferRequestStatus.Cancelled &&
                   Status != TransferRequestStatus.Rejected;
        }

        /// <summary>
        /// Check if request is urgent (required within 7 days)
        /// </summary>
        public bool IsUrgent()
        {
            return RequiredByDate.HasValue &&
                   (RequiredByDate.Value - DateTime.UtcNow).TotalDays <= 7 &&
                   Status == TransferRequestStatus.Pending;
        }

        /// <summary>
        /// Get the final quantity to transfer (approved or requested)
        /// </summary>
        public decimal GetFinalQuantity()
        {
            return ApprovedQuantity ?? RequestedQuantity;
        }

        /// <summary>
        /// Get days until required
        /// </summary>
        public int? GetDaysUntilRequired()
        {
            if (!RequiredByDate.HasValue)
                return null;

            return (RequiredByDate.Value.Date - DateTime.UtcNow.Date).Days;
        }

        // Static factory method for generating request numbers
        public static string GenerateRequestNumber()
        {
            var year = DateTime.UtcNow.Year;
            var timestamp = DateTime.UtcNow.ToString("MMddHHmmss");
            return $"TR-{year}-{timestamp}";
        }

        // Validation
        private static void ValidateTransferRequest(
            string requestNumber,
            decimal requestedQuantity,
            string unit)
        {
            if (string.IsNullOrWhiteSpace(requestNumber))
                throw new ArgumentException("Request number is required", nameof(requestNumber));

            if (requestedQuantity <= 0)
                throw new ArgumentException("Requested quantity must be greater than zero", nameof(requestedQuantity));

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required", nameof(unit));
        }
    }
}
