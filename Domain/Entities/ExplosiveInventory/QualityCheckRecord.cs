using Domain.Common;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.UserManagement;

namespace Domain.Entities.ExplosiveInventory
{
    /// <summary>
    /// Records quality control inspections for inventory batches
    /// Provides audit trail for quality management
    /// </summary>
    public class QualityCheckRecord : BaseAuditableEntity
    {
        public int CentralWarehouseInventoryId { get; private set; }
        public DateTime CheckDate { get; private set; }
        public int CheckedByUserId { get; private set; }

        public QualityStatus Status { get; private set; }
        public string CheckType { get; private set; } = string.Empty;  // Routine, Investigation, Pre-Transfer, Expiry
        public string? Findings { get; private set; }
        public string? ActionTaken { get; private set; }
        public bool RequiresFollowUp { get; private set; }
        public DateTime? FollowUpDate { get; private set; }

        // Navigation Properties
        public virtual CentralWarehouseInventory Inventory { get; private set; } = null!;
        public virtual User CheckedByUser { get; private set; } = null!;

        // Private constructor for EF Core
        private QualityCheckRecord() { }

        // Constructor
        public QualityCheckRecord(
            int centralWarehouseInventoryId,
            int checkedByUserId,
            string checkType,
            QualityStatus status,
            string? findings = null,
            string? actionTaken = null,
            bool requiresFollowUp = false,
            DateTime? followUpDate = null)
        {
            ValidateQualityCheck(checkType);

            CentralWarehouseInventoryId = centralWarehouseInventoryId;
            CheckedByUserId = checkedByUserId;
            CheckDate = DateTime.UtcNow;
            CheckType = checkType;
            Status = status;
            Findings = findings;
            ActionTaken = actionTaken;
            RequiresFollowUp = requiresFollowUp;
            FollowUpDate = followUpDate;
        }

        // Business Methods

        /// <summary>
        /// Update findings
        /// </summary>
        public void UpdateFindings(string findings)
        {
            Findings = findings;
            MarkUpdated();
        }

        /// <summary>
        /// Record action taken
        /// </summary>
        public void RecordAction(string action)
        {
            ActionTaken = string.IsNullOrWhiteSpace(ActionTaken)
                ? action
                : $"{ActionTaken}\n{DateTime.UtcNow:yyyy-MM-dd HH:mm}: {action}";

            MarkUpdated();
        }

        /// <summary>
        /// Mark as requiring follow-up
        /// </summary>
        public void MarkForFollowUp(DateTime followUpDate, string reason)
        {
            RequiresFollowUp = true;
            FollowUpDate = followUpDate;
            RecordAction($"Follow-up required by {followUpDate:yyyy-MM-dd}: {reason}");
        }

        /// <summary>
        /// Complete follow-up
        /// </summary>
        public void CompleteFollowUp(string outcome)
        {
            RequiresFollowUp = false;
            RecordAction($"Follow-up completed: {outcome}");
        }

        /// <summary>
        /// Update quality status
        /// </summary>
        public void UpdateStatus(QualityStatus newStatus, string reason)
        {
            Status = newStatus;
            RecordAction($"Status changed to {newStatus}: {reason}");
        }

        // Static factory methods for common check types
        public static QualityCheckRecord CreateRoutineCheck(
            int inventoryId,
            int checkedByUserId,
            QualityStatus status,
            string? findings = null)
        {
            return new QualityCheckRecord(
                inventoryId,
                checkedByUserId,
                "Routine",
                status,
                findings);
        }

        public static QualityCheckRecord CreateInvestigationCheck(
            int inventoryId,
            int checkedByUserId,
            QualityStatus status,
            string findings,
            string actionTaken)
        {
            return new QualityCheckRecord(
                inventoryId,
                checkedByUserId,
                "Investigation",
                status,
                findings,
                actionTaken);
        }

        public static QualityCheckRecord CreatePreTransferCheck(
            int inventoryId,
            int checkedByUserId,
            QualityStatus status)
        {
            return new QualityCheckRecord(
                inventoryId,
                checkedByUserId,
                "Pre-Transfer",
                status,
                "Pre-transfer quality verification");
        }

        public static QualityCheckRecord CreateExpiryCheck(
            int inventoryId,
            int checkedByUserId,
            QualityStatus status,
            int daysUntilExpiry)
        {
            return new QualityCheckRecord(
                inventoryId,
                checkedByUserId,
                "Expiry",
                status,
                $"Expiry check: {daysUntilExpiry} days remaining");
        }

        // Validation
        private static void ValidateQualityCheck(string checkType)
        {
            if (string.IsNullOrWhiteSpace(checkType))
                throw new ArgumentException("Check type is required", nameof(checkType));

            var validCheckTypes = new[] { "Routine", "Investigation", "Pre-Transfer", "Expiry", "Custom" };
            if (!validCheckTypes.Contains(checkType))
                throw new ArgumentException(
                    $"Invalid check type. Must be one of: {string.Join(", ", validCheckTypes)}",
                    nameof(checkType));
        }
    }
}
