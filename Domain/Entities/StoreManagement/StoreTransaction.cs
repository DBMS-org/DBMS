using Domain.Common;
using Domain.Entities.StoreManagement.Enums;
using Domain.Entities.UserManagement;

namespace Domain.Entities.StoreManagement
{
    public enum TransactionType
    {
        StockIn = 1,
        StockOut = 2,
        Transfer = 3,
        Adjustment = 4,
        Reservation = 5,
        ReservationRelease = 6
    }

    public class StoreTransaction : BaseAuditableEntity
    {
        public int StoreId { get; private set; }
        public int? StoreInventoryId { get; private set; }
        public ExplosiveType ExplosiveType { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public decimal Quantity { get; private set; }
        public string Unit { get; private set; } = string.Empty;
        public string? ReferenceNumber { get; private set; }
        public string? Notes { get; private set; }
        public int? RelatedStoreId { get; private set; } // For transfers
        public int? ProcessedByUserId { get; private set; }
        public DateTime TransactionDate { get; private set; }

        // Navigation Properties
        public virtual Store Store { get; private set; } = null!;
        public virtual StoreInventory? StoreInventory { get; private set; }
        public virtual Store? RelatedStore { get; private set; }
        public virtual User? ProcessedByUser { get; private set; }

        // Private constructor for EF Core
        private StoreTransaction() { }

        public StoreTransaction(
            int storeId,
            ExplosiveType explosiveType,
            TransactionType transactionType,
            decimal quantity,
            string unit,
            int? processedByUserId = null,
            string? referenceNumber = null,
            string? notes = null)
        {
            ValidateTransaction(quantity, unit);
            
            StoreId = storeId;
            ExplosiveType = explosiveType;
            TransactionType = transactionType;
            Quantity = quantity;
            Unit = unit;
            ProcessedByUserId = processedByUserId;
            ReferenceNumber = referenceNumber;
            Notes = notes;
            TransactionDate = DateTime.UtcNow;
        }

        public static StoreTransaction CreateStockIn(
            int storeId,
            int storeInventoryId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            int processedByUserId,
            string? referenceNumber = null,
            string? notes = null)
        {
            var transaction = new StoreTransaction(storeId, explosiveType, TransactionType.StockIn, quantity, unit, processedByUserId, referenceNumber, notes);
            transaction.StoreInventoryId = storeInventoryId;
            return transaction;
        }

        public static StoreTransaction CreateStockOut(
            int storeId,
            int storeInventoryId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            int processedByUserId,
            string? referenceNumber = null,
            string? notes = null)
        {
            var transaction = new StoreTransaction(storeId, explosiveType, TransactionType.StockOut, quantity, unit, processedByUserId, referenceNumber, notes);
            transaction.StoreInventoryId = storeInventoryId;
            return transaction;
        }

        public static StoreTransaction CreateTransfer(
            int fromStoreId,
            int toStoreId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            int processedByUserId,
            string? referenceNumber = null,
            string? notes = null)
        {
            var transaction = new StoreTransaction(fromStoreId, explosiveType, TransactionType.Transfer, quantity, unit, processedByUserId, referenceNumber, notes);
            transaction.RelatedStoreId = toStoreId;
            return transaction;
        }

        public static StoreTransaction CreateAdjustment(
            int storeId,
            int storeInventoryId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            int processedByUserId,
            string? referenceNumber = null,
            string? notes = null)
        {
            var transaction = new StoreTransaction(storeId, explosiveType, TransactionType.Adjustment, quantity, unit, processedByUserId, referenceNumber, notes);
            transaction.StoreInventoryId = storeInventoryId;
            return transaction;
        }

        public static StoreTransaction CreateReservation(
            int storeId,
            int storeInventoryId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            int processedByUserId,
            string? referenceNumber = null,
            string? notes = null)
        {
            var transaction = new StoreTransaction(storeId, explosiveType, TransactionType.Reservation, quantity, unit, processedByUserId, referenceNumber, notes);
            transaction.StoreInventoryId = storeInventoryId;
            return transaction;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        public void UpdateReferenceNumber(string referenceNumber)
        {
            ReferenceNumber = referenceNumber;
        }

        public bool IsInboundTransaction()
        {
            return TransactionType == TransactionType.StockIn || 
                   (TransactionType == TransactionType.Adjustment && Quantity > 0);
        }

        public bool IsOutboundTransaction()
        {
            return TransactionType == TransactionType.StockOut || 
                   TransactionType == TransactionType.Transfer ||
                   (TransactionType == TransactionType.Adjustment && Quantity < 0);
        }

        private static void ValidateTransaction(decimal quantity, string unit)
        {
            if (quantity == 0)
                throw new ArgumentException("Transaction quantity cannot be zero", nameof(quantity));
            
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required", nameof(unit));
        }
    }
}