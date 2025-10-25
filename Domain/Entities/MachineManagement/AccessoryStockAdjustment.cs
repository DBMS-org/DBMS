using Domain.Common;

namespace Domain.Entities.MachineManagement
{
    public class AccessoryStockAdjustment : BaseAuditableEntity
    {
        public int AccessoryId { get; private set; }
        public StockAdjustmentType AdjustmentType { get; private set; }
        public int QuantityChanged { get; private set; }
        public int PreviousQuantity { get; private set; }
        public int NewQuantity { get; private set; }
        public StockAdjustmentReason Reason { get; private set; }
        public string? Notes { get; private set; }
        public string AdjustedBy { get; private set; } = string.Empty;
        public DateTime AdjustedDate { get; private set; }

        // Navigation property
        public virtual Accessory? Accessory { get; private set; }

        private AccessoryStockAdjustment() { }

        public static AccessoryStockAdjustment Create(
            int accessoryId,
            StockAdjustmentType adjustmentType,
            int quantityChanged,
            int previousQuantity,
            int newQuantity,
            StockAdjustmentReason reason,
            string adjustedBy,
            string? notes = null)
        {
            return new AccessoryStockAdjustment
            {
                AccessoryId = accessoryId,
                AdjustmentType = adjustmentType,
                QuantityChanged = quantityChanged,
                PreviousQuantity = previousQuantity,
                NewQuantity = newQuantity,
                Reason = reason,
                AdjustedBy = adjustedBy,
                Notes = notes,
                AdjustedDate = DateTime.UtcNow
            };
        }
    }
}
