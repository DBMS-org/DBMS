using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.StoreManagement.Enums;
using Domain.Entities.UserManagement;

namespace Domain.Entities.StoreManagement
{
    public class Store : BaseAuditableEntity
    {
        private readonly List<ExplosiveType> _explosiveTypesAvailable = new();
        private readonly List<StoreInventory> _inventories = new();

        public string StoreName { get; private set; } = string.Empty;
        public string StoreAddress { get; private set; } = string.Empty;
        public string StoreManagerName { get; private set; } = string.Empty;
        public string StoreManagerContact { get; private set; } = string.Empty;
        public string StoreManagerEmail { get; private set; } = string.Empty;
        public decimal StorageCapacity { get; private set; }
        public decimal CurrentOccupancy { get; private set; }
        public string City { get; private set; } = string.Empty;
        public StoreStatus Status { get; private set; } = StoreStatus.Operational;
        
        // Foreign Keys
        public int RegionId { get; private set; }
        public int? ProjectId { get; private set; }
        public int? ManagerUserId { get; private set; }

        // Navigation Properties
        public virtual Region Region { get; private set; } = null!;
        public virtual Project? Project { get; private set; }
        public virtual User? ManagerUser { get; private set; }
        
        // Collections
        public virtual IReadOnlyCollection<ExplosiveType> ExplosiveTypesAvailable => _explosiveTypesAvailable.AsReadOnly();
        public virtual IReadOnlyCollection<StoreInventory> Inventories => _inventories.AsReadOnly();

        // Private constructor for EF Core
        private Store() { }

        public Store(
            string storeName,
            string storeAddress,
            string storeManagerName,
            string storeManagerContact,
            string storeManagerEmail,
            decimal storageCapacity,
            string city,
            int regionId)
        {
            ValidateStoreCreation(storeName, storeAddress, storeManagerName, storeManagerContact, storeManagerEmail, storageCapacity, city);
            
            StoreName = storeName;
            StoreAddress = storeAddress;
            StoreManagerName = storeManagerName;
            StoreManagerContact = storeManagerContact;
            StoreManagerEmail = storeManagerEmail;
            StorageCapacity = storageCapacity;
            City = city;
            RegionId = regionId;
            CurrentOccupancy = 0;
            Status = StoreStatus.Operational;
        }

        public void UpdateStoreDetails(
            string storeName,
            string storeAddress,
            string storeManagerName,
            string storeManagerContact,
            string storeManagerEmail,
            decimal storageCapacity,
            string city)
        {
            ValidateStoreCreation(storeName, storeAddress, storeManagerName, storeManagerContact, storeManagerEmail, storageCapacity, city);
            
            StoreName = storeName;
            StoreAddress = storeAddress;
            StoreManagerName = storeManagerName;
            StoreManagerContact = storeManagerContact;
            StoreManagerEmail = storeManagerEmail;
            StorageCapacity = storageCapacity;
            City = city;
        }

        public void ChangeStatus(StoreStatus newStatus)
        {
            if (Status == StoreStatus.Decommissioned && newStatus != StoreStatus.Decommissioned)
            {
                throw new InvalidOperationException("Cannot change status of a decommissioned store");
            }
            
            Status = newStatus;
        }

        public void AssignToProject(int projectId)
        {
            if (Status != StoreStatus.Operational)
            {
                throw new InvalidOperationException("Can only assign operational stores to projects");
            }
            
            ProjectId = projectId;
        }

        public void RemoveFromProject()
        {
            ProjectId = null;
        }

        public void AssignManager(int managerUserId)
        {
            ManagerUserId = managerUserId;
        }

        public void AddExplosiveType(ExplosiveType explosiveType)
        {
            if (!_explosiveTypesAvailable.Contains(explosiveType))
            {
                _explosiveTypesAvailable.Add(explosiveType);
            }
        }

        public void RemoveExplosiveType(ExplosiveType explosiveType)
        {
            _explosiveTypesAvailable.Remove(explosiveType);
        }

        public void UpdateOccupancy(decimal newOccupancy)
        {
            if (newOccupancy < 0)
            {
                throw new ArgumentException("Occupancy cannot be negative");
            }
            
            if (newOccupancy > StorageCapacity)
            {
                throw new ArgumentException("Occupancy cannot exceed storage capacity");
            }
            
            CurrentOccupancy = newOccupancy;
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
            string storeManagerName,
            string storeManagerContact,
            string storeManagerEmail,
            decimal storageCapacity,
            string city)
        {
            if (string.IsNullOrWhiteSpace(storeName))
                throw new ArgumentException("Store name is required", nameof(storeName));
            
            if (string.IsNullOrWhiteSpace(storeAddress))
                throw new ArgumentException("Store address is required", nameof(storeAddress));
            
            if (string.IsNullOrWhiteSpace(storeManagerName))
                throw new ArgumentException("Store manager name is required", nameof(storeManagerName));
            
            if (string.IsNullOrWhiteSpace(storeManagerContact))
                throw new ArgumentException("Store manager contact is required", nameof(storeManagerContact));
            
            if (string.IsNullOrWhiteSpace(storeManagerEmail))
                throw new ArgumentException("Store manager email is required", nameof(storeManagerEmail));
            
            if (storageCapacity <= 0)
                throw new ArgumentException("Storage capacity must be greater than zero", nameof(storageCapacity));
            
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required", nameof(city));
        }
    }
}