using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;

namespace Domain.Services
{
    public class StoreInventoryDomainService
    {
        private const decimal SAFETY_STOCK_MULTIPLIER = 1.2m;
        private const int DEFAULT_EXPIRY_WARNING_DAYS = 30;
        private const decimal UTILIZATION_WARNING_THRESHOLD = 0.85m; // 85%
        private const decimal UTILIZATION_CRITICAL_THRESHOLD = 0.95m; // 95%

        /// <summary>
        /// Validates if a store can accommodate additional inventory
        /// </summary>
        public bool CanAccommodateInventory(Store store, decimal additionalQuantity)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            return store.CanAccommodate(additionalQuantity);
        }

        /// <summary>
        /// Calculates optimal stock levels based on historical consumption and safety requirements
        /// </summary>
        public (decimal minimumLevel, decimal maximumLevel) CalculateOptimalStockLevels(
            decimal averageMonthlyConsumption,
            int leadTimeDays,
            decimal safetyStockMultiplier = SAFETY_STOCK_MULTIPLIER)
        {
            if (averageMonthlyConsumption < 0)
                throw new ArgumentException("Average monthly consumption cannot be negative");

            if (leadTimeDays < 0)
                throw new ArgumentException("Lead time cannot be negative");

            var dailyConsumption = averageMonthlyConsumption / 30m;
            var leadTimeConsumption = dailyConsumption * leadTimeDays;
            var safetyStock = leadTimeConsumption * safetyStockMultiplier;

            var minimumLevel = leadTimeConsumption + safetyStock;
            var maximumLevel = minimumLevel * 3m; // 3 months of minimum stock

            return (minimumLevel, maximumLevel);
        }

        /// <summary>
        /// Validates if a transfer between stores is feasible
        /// </summary>
        public bool ValidateStoreTransfer(
            Store sourceStore,
            Store destinationStore,
            ExplosiveType explosiveType,
            decimal quantity)
        {
            if (sourceStore == null || destinationStore == null)
                return false;

            if (sourceStore.Id == destinationStore.Id)
                return false;

            if (sourceStore.Status != StoreStatus.Operational || 
                destinationStore.Status != StoreStatus.Operational)
                return false;

            // Check if source store has the explosive type available in inventory
            if (!sourceStore.Inventories.Any(i => i.ExplosiveType == explosiveType))
                return false;

            // Check if destination store can handle this explosive type (has inventory for it)
            if (!destinationStore.Inventories.Any(i => i.ExplosiveType == explosiveType))
                return false;

            // Check if destination store can accommodate the quantity
            return destinationStore.CanAccommodate(quantity);
        }

        /// <summary>
        /// Determines if inventory requires immediate attention based on various factors
        /// </summary>
        public InventoryAlert GetInventoryAlert(StoreInventory inventory)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            var alerts = new List<AlertType>();

            // Check stock levels
            if (inventory.IsLowStock())
                alerts.Add(AlertType.LowStock);

            if (inventory.IsOverStock())
                alerts.Add(AlertType.OverStock);

            // Check expiry
            if (inventory.IsExpired())
                alerts.Add(AlertType.Expired);
            else if (inventory.IsExpiringSoon(DEFAULT_EXPIRY_WARNING_DAYS))
                alerts.Add(AlertType.ExpiringSoon);

            // Check if no available stock due to reservations
            if (inventory.GetAvailableQuantity() <= 0 && inventory.Quantity > 0)
                alerts.Add(AlertType.FullyReserved);

            return new InventoryAlert
            {
                InventoryId = inventory.Id,
                ExplosiveType = inventory.ExplosiveType,
                Alerts = alerts,
                CurrentQuantity = inventory.Quantity,
                AvailableQuantity = inventory.GetAvailableQuantity(),
                ReservedQuantity = inventory.ReservedQuantity
            };
        }

        /// <summary>
        /// Calculates store utilization metrics
        /// </summary>
        public StoreUtilizationMetrics CalculateStoreUtilization(Store store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var utilizationRate = store.GetUtilizationRate();
            var availableCapacity = store.StorageCapacity - store.CurrentOccupancy;

            var status = utilizationRate switch
            {
                >= UTILIZATION_CRITICAL_THRESHOLD * 100 => UtilizationStatus.Critical,
                >= UTILIZATION_WARNING_THRESHOLD * 100 => UtilizationStatus.Warning,
                _ => UtilizationStatus.Normal
            };

            return new StoreUtilizationMetrics
            {
                StoreId = store.Id,
                StoreName = store.StoreName,
                TotalCapacity = store.StorageCapacity,
                CurrentOccupancy = store.CurrentOccupancy,
                AvailableCapacity = availableCapacity,
                UtilizationRate = utilizationRate,
                Status = status
            };
        }

        /// <summary>
        /// Validates explosive type compatibility for store operations
        /// </summary>
        public bool ValidateExplosiveTypeCompatibility(
            ExplosiveType primaryType,
            ExplosiveType secondaryType)
        {
            // Define incompatible combinations based on safety regulations
            var incompatibleCombinations = new Dictionary<ExplosiveType, ExplosiveType[]>
            {
                { ExplosiveType.ANFO, new[] { ExplosiveType.DetonatingCord } },
                { ExplosiveType.Dynamite, new[] { ExplosiveType.BlastingCaps } }
            };

            if (incompatibleCombinations.TryGetValue(primaryType, out var incompatibleTypes))
            {
                return !incompatibleTypes.Contains(secondaryType);
            }

            return true; // Default to compatible if no specific rules defined
        }
    }

    public class InventoryAlert
    {
        public int InventoryId { get; set; }
        public ExplosiveType ExplosiveType { get; set; }
        public List<AlertType> Alerts { get; set; } = new();
        public decimal CurrentQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
    }

    public enum AlertType
    {
        LowStock,
        OverStock,
        Expired,
        ExpiringSoon,
        FullyReserved
    }

    public class StoreUtilizationMetrics
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public decimal TotalCapacity { get; set; }
        public decimal CurrentOccupancy { get; set; }
        public decimal AvailableCapacity { get; set; }
        public decimal UtilizationRate { get; set; }
        public UtilizationStatus Status { get; set; }
    }

    public enum UtilizationStatus
    {
        Normal,
        Warning,
        Critical
    }
}