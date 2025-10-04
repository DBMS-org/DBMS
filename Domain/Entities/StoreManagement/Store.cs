using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.StoreManagement.Enums;
using Domain.Entities.UserManagement;

namespace Domain.Entities.StoreManagement
{
    public class Store : BaseAuditableEntity
    {
        private readonly List<StoreInventory> _inventories = new();

        public string StoreName { get; private set; } = string.Empty;
        public string StoreAddress { get; private set; } = string.Empty;
        public decimal StorageCapacity { get; private set; }
        public string City { get; private set; } = string.Empty;
        public StoreStatus Status { get; private set; } = StoreStatus.Operational;
        public string AllowedExplosiveTypes { get; private set; } = string.Empty; // Comma-separated: "ANFO,Emulsion"

        // Computed property - calculated from Inventories
        public decimal CurrentOccupancy => Inventories?.Sum(i => i.Quantity) ?? 0;

        // Foreign Keys
        public int RegionId { get; private set; }
        public int? ManagerUserId { get; private set; }

        // Navigation Properties
        public virtual Region Region { get; private set; } = null!;
        public virtual User? ManagerUser { get; private set; }
        
        // Collections
        public virtual IReadOnlyCollection<StoreInventory> Inventories => _inventories.AsReadOnly();

        // Private constructor for EF Core
        private Store() { }

        public Store(
            string storeName,
            string storeAddress,
            decimal storageCapacity,
            string city,
            int regionId,
            string? allowedExplosiveTypes = null)
        {
            ValidateStoreCreation(storeName, storeAddress, storageCapacity, city);

            StoreName = storeName;
            StoreAddress = storeAddress;
            StorageCapacity = storageCapacity;
            City = city;
            RegionId = regionId;
            Status = StoreStatus.Operational;
            AllowedExplosiveTypes = allowedExplosiveTypes ?? string.Empty;
        }

        public void UpdateStoreDetails(
            string storeName,
            string storeAddress,
            decimal storageCapacity,
            string city,
            string? allowedExplosiveTypes = null)
        {
            ValidateStoreCreation(storeName, storeAddress, storageCapacity, city);

            StoreName = storeName;
            StoreAddress = storeAddress;
            StorageCapacity = storageCapacity;
            City = city;
            if (allowedExplosiveTypes != null)
            {
                AllowedExplosiveTypes = allowedExplosiveTypes;
            }
        }

        public void ChangeStatus(StoreStatus newStatus)
        {
            if (Status == StoreStatus.Decommissioned && newStatus != StoreStatus.Decommissioned)
            {
                throw new InvalidOperationException("Cannot change status of a decommissioned store");
            }
            
            Status = newStatus;
        }

        public void AssignManager(int managerUserId)
        {
            ManagerUserId = managerUserId;
        }

        public void RemoveManager()
        {
            ManagerUserId = null;
        }

        public decimal GetUtilizationRate()
        {
            return StorageCapacity > 0 ? (CurrentOccupancy / StorageCapacity) * 100 : 0;
        }

        public bool CanAccommodate(decimal additionalCapacity)
        {
            return (CurrentOccupancy + additionalCapacity) <= StorageCapacity;
        }

        private static void ValidateStoreCreation(
            string storeName,
            string storeAddress,
            decimal storageCapacity,
            string city)
        {
            if (string.IsNullOrWhiteSpace(storeName))
                throw new ArgumentException("Store name is required", nameof(storeName));

            if (string.IsNullOrWhiteSpace(storeAddress))
                throw new ArgumentException("Store address is required", nameof(storeAddress));

            if (storageCapacity <= 0)
                throw new ArgumentException("Storage capacity must be greater than zero", nameof(storageCapacity));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required", nameof(city));
        }
    }
}