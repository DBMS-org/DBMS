using Domain.Common;

namespace Domain.Entities.MachineManagement
{
    public class Accessory : BaseAuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public AccessoryCategory Category { get; private set; }
        public string PartNumber { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public int Quantity { get; private set; }
        public AccessoryUnit Unit { get; private set; }
        public int MinStockLevel { get; private set; }
        public string Supplier { get; private set; } = string.Empty;
        public string? Location { get; private set; }

        // Navigation properties
        public virtual ICollection<AccessoryStockAdjustment> StockAdjustments { get; private set; } = new List<AccessoryStockAdjustment>();

        private Accessory() { }

        public static Accessory Create(
            string name,
            AccessoryCategory category,
            string partNumber,
            int quantity,
            AccessoryUnit unit,
            int minStockLevel,
            string supplier,
            string? description = null,
            string? location = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentException("Part number cannot be empty", nameof(partNumber));

            if (string.IsNullOrWhiteSpace(supplier))
                throw new ArgumentException("Supplier cannot be empty", nameof(supplier));

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

            if (minStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative", nameof(minStockLevel));

            return new Accessory
            {
                Name = name,
                Category = category,
                PartNumber = partNumber,
                Quantity = quantity,
                Unit = unit,
                MinStockLevel = minStockLevel,
                Supplier = supplier,
                Description = description,
                Location = location
            };
        }

        public void Update(
            string name,
            AccessoryCategory category,
            string partNumber,
            AccessoryUnit unit,
            int minStockLevel,
            string supplier,
            string? description,
            string? location)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(partNumber))
                throw new ArgumentException("Part number cannot be empty", nameof(partNumber));

            if (string.IsNullOrWhiteSpace(supplier))
                throw new ArgumentException("Supplier cannot be empty", nameof(supplier));

            if (minStockLevel < 0)
                throw new ArgumentException("Minimum stock level cannot be negative", nameof(minStockLevel));

            Name = name;
            Category = category;
            PartNumber = partNumber;
            Unit = unit;
            MinStockLevel = minStockLevel;
            Supplier = supplier;
            Description = description;
            Location = location;
            MarkUpdated();
        }

        public void AdjustStock(int newQuantity, StockAdjustmentType adjustmentType, StockAdjustmentReason reason, string adjustedBy, string? notes = null)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

            var previousQuantity = Quantity;
            var quantityChanged = Math.Abs(newQuantity - previousQuantity);

            Quantity = newQuantity;
            MarkUpdated();

            var adjustment = AccessoryStockAdjustment.Create(
                Id,
                adjustmentType,
                quantityChanged,
                previousQuantity,
                newQuantity,
                reason,
                adjustedBy,
                notes);

            StockAdjustments.Add(adjustment);
        }

        public bool IsLowStock() => Quantity <= MinStockLevel && Quantity > 0;

        public bool IsOutOfStock() => Quantity == 0;

        public string GetStockStatus()
        {
            if (IsOutOfStock()) return "Out of Stock";
            if (IsLowStock()) return "Low Stock";
            return "Available";
        }
    }
}
