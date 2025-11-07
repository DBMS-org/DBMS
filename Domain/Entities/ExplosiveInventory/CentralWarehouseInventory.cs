using Domain.Common;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Domain.Entities.ExplosiveInventory
{
    /// <summary>
    /// Represents a batch of explosive materials in the central warehouse
    /// with detailed technical specifications and tracking
    /// </summary>
    public class CentralWarehouseInventory : BaseAuditableEntity
    {
        private readonly List<InventoryTransferRequest> _transferRequests = new();

        // Core Identification
        public string BatchId { get; private set; } = string.Empty;
        public ExplosiveType ExplosiveType { get; private set; }

        // Quantity
        public decimal Quantity { get; private set; }
        public decimal AllocatedQuantity { get; private set; }
        public string Unit { get; private set; } = string.Empty;

        // Dates
        public DateTime ManufacturingDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        // Supplier
        public string Supplier { get; private set; } = string.Empty;
        public string? ManufacturerBatchNumber { get; private set; }

        // Storage
        public string StorageLocation { get; private set; } = string.Empty;

        // Status
        public InventoryStatus Status { get; private set; }

        // Foreign Keys
        public int CentralWarehouseStoreId { get; private set; }

        // Navigation Properties
        public virtual Store CentralWarehouse { get; private set; } = null!;
        public virtual IReadOnlyCollection<InventoryTransferRequest> TransferRequests => _transferRequests.AsReadOnly();

        // Technical Properties (Type-specific - only one will be populated)
        public int? ANFOTechnicalPropertiesId { get; private set; }
        public virtual ANFOTechnicalProperties? ANFOProperties { get; private set; }

        public int? EmulsionTechnicalPropertiesId { get; private set; }
        public virtual EmulsionTechnicalProperties? EmulsionProperties { get; private set; }

        // Computed Properties
        public decimal AvailableQuantity => Quantity - AllocatedQuantity;
        public int DaysUntilExpiry => (ExpiryDate.Date - DateTime.UtcNow.Date).Days;
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsExpiringSoon => DaysUntilExpiry <= 30 && DaysUntilExpiry >= 0;

        // Private constructor for EF Core
        private CentralWarehouseInventory() { }

        // Constructor for creating new inventory
        public CentralWarehouseInventory(
            string batchId,
            ExplosiveType explosiveType,
            decimal quantity,
            string unit,
            DateTime manufacturingDate,
            DateTime expiryDate,
            string supplier,
            string storageLocation,
            int centralWarehouseStoreId,
            string? manufacturerBatchNumber = null)
        {
            ValidateInventoryCreation(batchId, quantity, unit, manufacturingDate, expiryDate, supplier, storageLocation);

            BatchId = batchId;
            ExplosiveType = explosiveType;
            Quantity = quantity;
            AllocatedQuantity = 0;
            Unit = unit;
            ManufacturingDate = manufacturingDate;
            ExpiryDate = expiryDate;
            Supplier = supplier;
            ManufacturerBatchNumber = manufacturerBatchNumber;
            StorageLocation = storageLocation;
            CentralWarehouseStoreId = centralWarehouseStoreId;
            Status = InventoryStatus.Available;
        }

        // Business Methods

        /// <summary>
        /// Allocate quantity for a transfer request
        /// </summary>
        public void AllocateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Allocation quantity must be greater than zero", nameof(quantity));

            if (quantity > AvailableQuantity)
                throw new InvalidOperationException(
                    $"Cannot allocate {quantity} {Unit}. Only {AvailableQuantity} {Unit} available.");

            if (Status == InventoryStatus.Expired)
                throw new InvalidOperationException("Cannot allocate expired inventory");

            if (Status == InventoryStatus.Quarantined)
                throw new InvalidOperationException("Cannot allocate quarantined inventory");

            AllocatedQuantity += quantity;

            // Update status if fully allocated
            if (AvailableQuantity == 0)
                Status = InventoryStatus.Allocated;

            MarkUpdated();
        }

        /// <summary>
        /// Release allocated quantity (when transfer is cancelled or rejected)
        /// </summary>
        public void ReleaseAllocation(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Release quantity must be greater than zero", nameof(quantity));

            if (quantity > AllocatedQuantity)
                throw new InvalidOperationException(
                    $"Cannot release {quantity} {Unit}. Only {AllocatedQuantity} {Unit} is allocated.");

            AllocatedQuantity -= quantity;

            // Update status if allocation is released
            if (Status == InventoryStatus.Allocated && AvailableQuantity > 0)
                Status = InventoryStatus.Available;

            MarkUpdated();
        }

        /// <summary>
        /// Update quantity (stock adjustment or transfer completion)
        /// </summary>
        public void UpdateQuantity(decimal newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(newQuantity));

            if (newQuantity < AllocatedQuantity)
                throw new ArgumentException(
                    $"Quantity cannot be less than allocated quantity ({AllocatedQuantity} {Unit})", nameof(newQuantity));

            Quantity = newQuantity;

            // Update status based on quantity
            if (Quantity == 0)
                Status = InventoryStatus.Depleted;
            else if (Status == InventoryStatus.Depleted)
                Status = InventoryStatus.Available;

            MarkUpdated();
        }

        /// <summary>
        /// Consume quantity (complete a transfer)
        /// </summary>
        public void ConsumeQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Consumption quantity must be greater than zero", nameof(quantity));

            if (quantity > AllocatedQuantity)
                throw new InvalidOperationException(
                    $"Cannot consume {quantity} {Unit}. Only {AllocatedQuantity} {Unit} is allocated.");

            Quantity -= quantity;
            AllocatedQuantity -= quantity;

            // Update status based on quantity
            if (Quantity == 0)
                Status = InventoryStatus.Depleted;
            else if (AllocatedQuantity == 0 && Status == InventoryStatus.Allocated)
                Status = InventoryStatus.Available;

            MarkUpdated();
        }

        /// <summary>
        /// Quarantine batch for quality control investigation
        /// </summary>
        public void QuarantineBatch(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("Quarantine reason is required", nameof(reason));

            if (Status == InventoryStatus.Depleted)
                throw new InvalidOperationException("Cannot quarantine depleted inventory");

            Status = InventoryStatus.Quarantined;
            MarkUpdated();
        }

        /// <summary>
        /// Release from quarantine
        /// </summary>
        public void ReleaseFromQuarantine()
        {
            if (Status != InventoryStatus.Quarantined)
                throw new InvalidOperationException("Inventory is not quarantined");

            if (IsExpired)
            {
                Status = InventoryStatus.Expired;
            }
            else if (Quantity == 0)
            {
                Status = InventoryStatus.Depleted;
            }
            else if (AllocatedQuantity == Quantity)
            {
                Status = InventoryStatus.Allocated;
            }
            else
            {
                Status = InventoryStatus.Available;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Mark batch as expired
        /// </summary>
        public void MarkAsExpired()
        {
            if (Status == InventoryStatus.Depleted)
                throw new InvalidOperationException("Cannot mark depleted inventory as expired");

            Status = InventoryStatus.Expired;
            MarkUpdated();
        }

        /// <summary>
        /// Update storage location
        /// </summary>
        public void UpdateStorageLocation(string newLocation)
        {
            if (string.IsNullOrWhiteSpace(newLocation))
                throw new ArgumentException("Storage location is required", nameof(newLocation));

            StorageLocation = newLocation;
            MarkUpdated();
        }

        /// <summary>
        /// Set ANFO technical properties
        /// </summary>
        public void SetANFOProperties(ANFOTechnicalProperties properties)
        {
            if (ExplosiveType != ExplosiveType.ANFO)
                throw new InvalidOperationException("Cannot set ANFO properties for non-ANFO inventory");

            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            ANFOProperties = properties;
            ANFOTechnicalPropertiesId = properties.Id;
            MarkUpdated();
        }

        /// <summary>
        /// Set Emulsion technical properties
        /// </summary>
        public void SetEmulsionProperties(EmulsionTechnicalProperties properties)
        {
            if (ExplosiveType != ExplosiveType.Emulsion)
                throw new InvalidOperationException("Cannot set Emulsion properties for non-Emulsion inventory");

            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            EmulsionProperties = properties;
            EmulsionTechnicalPropertiesId = properties.Id;
            MarkUpdated();
        }

        /// <summary>
        /// Check if batch can be allocated
        /// </summary>
        public bool CanBeAllocated(decimal quantity)
        {
            return Status == InventoryStatus.Available &&
                   !IsExpired &&
                   quantity <= AvailableQuantity &&
                   quantity > 0;
        }

        // Validation
        private static void ValidateInventoryCreation(
            string batchId,
            decimal quantity,
            string unit,
            DateTime manufacturingDate,
            DateTime expiryDate,
            string supplier,
            string storageLocation)
        {
            if (string.IsNullOrWhiteSpace(batchId))
                throw new ArgumentException("Batch ID is required", nameof(batchId));

            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative", nameof(quantity));

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit is required", nameof(unit));

            if (manufacturingDate > DateTime.UtcNow)
                throw new ArgumentException("Manufacturing date cannot be in the future", nameof(manufacturingDate));

            if (expiryDate <= manufacturingDate)
                throw new ArgumentException("Expiry date must be after manufacturing date", nameof(expiryDate));

            if (string.IsNullOrWhiteSpace(supplier))
                throw new ArgumentException("Supplier is required", nameof(supplier));

            if (string.IsNullOrWhiteSpace(storageLocation))
                throw new ArgumentException("Storage location is required", nameof(storageLocation));
        }
    }
}
