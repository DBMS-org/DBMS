namespace Application.DTOs.Reports
{
    public class InventoryStatusReportDto
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Statistics
        public InventoryStatisticsDto Statistics { get; set; } = new();

        // Stock Levels by Material Type
        public List<MaterialStockDto> StockLevels { get; set; } = new();

        // Low Stock Alerts
        public List<LowStockAlertDto> LowStockAlerts { get; set; } = new();

        // Expiring Materials
        public List<ExpiringMaterialDto> ExpiringMaterials { get; set; } = new();

        // Inventory Turnover
        public List<InventoryTurnoverDto> TurnoverRates { get; set; } = new();

        // Transfer Request Status
        public TransferRequestSummaryDto TransferRequests { get; set; } = new();

        // Regional Inventory Distribution
        public List<RegionalInventoryDto> RegionalDistribution { get; set; } = new();

        // Accessories Inventory
        public AccessoriesInventoryDto AccessoriesInventory { get; set; } = new();

        // DETAILED DATA ARRAYS
        // All Inventory Batches with complete details
        public List<InventoryBatchDetailDto> AllInventoryBatches { get; set; } = new();

        // Store Details
        public List<StoreDetailDto> AllStores { get; set; } = new();

        // All Transfer Requests
        public List<TransferRequestDetailDto> AllTransferRequests { get; set; } = new();

        // Store Transactions History
        public List<StoreTransactionDetailDto> AllTransactions { get; set; } = new();

        // Explosive Approval Requests
        public List<ExplosiveApprovalDetailDto> ExplosiveApprovals { get; set; } = new();
    }

    public class InventoryStatisticsDto
    {
        public int TotalBatches { get; set; }
        public int AvailableBatches { get; set; }
        public int QuarantinedBatches { get; set; }
        public int ExpiredBatches { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int PendingTransfers { get; set; }
    }

    public class MaterialStockDto
    {
        public string MaterialType { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int BatchCount { get; set; }
        public decimal AverageAge { get; set; } // in days
        public string Status { get; set; } = string.Empty;
    }

    public class LowStockAlertDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinimumStock { get; set; }
        public decimal ReorderPoint { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int DaysUntilStockout { get; set; }
        public string Priority { get; set; } = string.Empty;
    }

    public class ExpiringMaterialDto
    {
        public string BatchId { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string StorageLocation { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
    }

    public class InventoryTurnoverDto
    {
        public string MaterialType { get; set; } = string.Empty;
        public decimal TurnoverRate { get; set; }
        public decimal AverageDaysInStock { get; set; }
        public decimal MonthlyConsumption { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class TransferRequestSummaryDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int DispatchedRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int UrgentRequests { get; set; }
        public int OverdueRequests { get; set; }
        public decimal AverageFulfillmentTime { get; set; } // in hours
    }

    public class RegionalInventoryDto
    {
        public string Region { get; set; } = string.Empty;
        public int TotalBatches { get; set; }
        public int StoreCount { get; set; }
        public List<MaterialStockDto> StockByType { get; set; } = new();
    }

    public class AccessoriesInventoryDto
    {
        public int TotalAccessories { get; set; }
        public int AvailableCount { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public List<AccessoryStockDto> TopLowStockItems { get; set; } = new();
    }

    public class AccessoryStockDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
