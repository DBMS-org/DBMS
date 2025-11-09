using Domain.Entities.StoreManagement.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Dashboard statistics for central inventory
    /// </summary>
    public class InventoryDashboardDto
    {
        // Summary Statistics
        public int TotalBatches { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public string PrimaryUnit { get; set; } = "kg";

        // By Type
        public Dictionary<ExplosiveType, decimal> QuantityByType { get; set; } = new();
        public Dictionary<ExplosiveType, int> BatchesByType { get; set; } = new();

        // Alerts
        public int ExpiringBatches { get; set; }
        public int ExpiredBatches { get; set; }
        public int QuarantinedBatches { get; set; }
        public int DepletedBatches { get; set; }

        // Transfer Requests
        public int PendingTransferRequests { get; set; }
        public int ApprovedTransferRequests { get; set; }
        public int UrgentTransferRequests { get; set; }
        public int OverdueTransferRequests { get; set; }

        // Recent Activity
        public List<RecentActivityDto> RecentActivities { get; set; } = new();

        // Alerts Detail
        public List<InventoryAlertDto> Alerts { get; set; } = new();
    }

    public class RecentActivityDto
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class InventoryAlertDto
    {
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty; // Critical, Warning, Info
        public string Message { get; set; } = string.Empty;
        public int? InventoryId { get; set; }
        public string? BatchId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
