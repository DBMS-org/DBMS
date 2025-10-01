using Domain.Common;
using Domain.Entities.StoreManagement.Enums;

namespace Domain.Entities.StoreManagement
{
    public class StoreInventory : BaseAuditableEntity
    {
        public int StoreId { get; private set; }
        public ExplosiveType ExplosiveType { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal ReservedQuantity { get; private set; }
        public string Unit { get; private set; } = string.Empty;
        public decimal MinimumStockLevel { get; private set; }
        public decimal MaximumStockLevel { get; private set; }
        public DateTime? LastRestockedAt { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string? BatchNumber { get; private set; }
        public string? Supplier { get; private set; }

        // Navigation Properties
        public virtual Store Store { get; private set; } = null!;

        // Private constructor for EF Core
        private StoreInventory() { }

        public StoreInventory(
            int storeId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            decimal minimumStockLevel,
            decimal maximumStockLevel)
        {
            ValidateInventoryCreation(quantity, unit, minimumStockLevel, maximumStockLevel);
            
            StoreId = storeId;
            ExplosiveType = explosiveType;
            Quantity = quantity;
            Unit = unit;
            MinimumStockLevel = minimumStockLevel;
            MaximumStockLevel = maximumStockLevel;
            ReservedQuantity = 0;
        }

        public void UpdateQuantity(decimal newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }
            
            if (newQuantity < ReservedQuantity)
            {
                throw new ArgumentException("Available quantity cannot be less than reserved quantity");
            }
            
            Quantity = newQuantity;
        }

        public void AddStock(decimal additionalQuantity, string? batchNumber = null, string? supplier = null, DateTime? expiryDate = null)
        {
            if (additionalQuantity <= 0)
            {
                throw new ArgumentException("Additional quantity must be greater than zero");
            }
            
            Quantity += additionalQuantity;
            LastRestockedAt = DateTime.UtcNow;
            
            if (!string.IsNullOrWhiteSpace(batchNumber))
                BatchNumber = batchNumber;
            
            if (!string.IsNullOrWhiteSpace(supplier))
                Supplier = supplier;
            
            if (expiryDate.HasValue)
                ExpiryDate = expiryDate;
        }

        public void ConsumeStock(decimal consumedQuantity)
        {
            if (consumedQuantity <= 0)
            {
                throw new ArgumentException("Consumed quantity must be greater than zero");
            }
            
            if (consumedQuantity > GetAvailableQuantity())
            {
                throw new InvalidOperationException("Cannot consume more than available quantity");
            }
            
            Quantity -= consumedQuantity;
        }

        public void ReserveStock(decimal reservedQuantity)
        {
            if (reservedQuantity <= 0)
            {
                throw new ArgumentException("Reserved quantity must be greater than zero");
            }
            
            if (reservedQuantity > GetAvailableQuantity())
            {
                throw new InvalidOperationException("Cannot reserve more than available quantity");
            }
            
            ReservedQuantity += reservedQuantity;
        }

        public void ReleaseReservedStock(decimal releasedQuantity)
        {
            if (releasedQuantity <= 0)
            {
                throw new ArgumentException("Released quantity must be greater than zero");
            }
            
            if (releasedQuantity > ReservedQuantity)
            {
                throw new InvalidOperationException("Cannot release more than reserved quantity");
            }
            
            ReservedQuantity -= releasedQuantity;
        }

        public void UpdateStockLevels(decimal minimumStockLevel, decimal maximumStockLevel)
        {
            if (minimumStockLevel < 0)
            {
                throw new ArgumentException("Minimum stock level cannot be negative");
            }
            
            if (maximumStockLevel <= minimumStockLevel)
            {
                throw new ArgumentException("Maximum stock level must be greater than minimum stock level");
            }
            
            MinimumStockLevel = minimumStockLevel;
            MaximumStockLevel = maximumStockLevel;
        }

        public decimal GetAvailableQuantity()
        {
            return Quantity - ReservedQuantity;
        }

        public bool IsLowStock()
        {
            return Quantity <= MinimumStockLevel;
        }

        public bool IsOverStock()
        {
            return Quantity >= MaximumStockLevel;
        }

        public bool IsExpired()
        {
            return ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow;
        }

        public bool IsExpiringSoon(int daysThreshold = 30)
        {
            return ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow.AddDays(daysThreshold);
        }

        private static void ValidateInventoryCreation(decimal quantity, string unit, decimal minimumStockLevel, decimal maximumStockLevel)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
            
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required", nameof(unit));
            
            if (minimumStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative", nameof(minimumStockLevel));
            
            if (maximumStockLevel <= minimumStockLevel)
                throw new ArgumentException("Maximum stock level must be greater than minimum stock level", nameof(maximumStockLevel));
        }
    }
}