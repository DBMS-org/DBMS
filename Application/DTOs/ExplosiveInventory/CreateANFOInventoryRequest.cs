using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for creating ANFO inventory batch
    /// </summary>
    public class CreateANFOInventoryRequest
    {
        // Core Information
        public string BatchId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "kg";
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public string? ManufacturerBatchNumber { get; set; }
        public string StorageLocation { get; set; } = string.Empty;
        public int CentralWarehouseStoreId { get; set; }

        // ANFO Technical Properties - Required
        public decimal Density { get; set; }
        public decimal FuelOilContent { get; set; }
        public ANFOGrade Grade { get; set; }
        public decimal StorageTemperature { get; set; }
        public decimal StorageHumidity { get; set; }

        // ANFO Technical Properties - Optional
        public decimal? MoistureContent { get; set; }
        public decimal? PrillSize { get; set; }
        public int? DetonationVelocity { get; set; }

        // Quality Control
        public FumeClass FumeClass { get; set; } = FumeClass.Class1;
        public QualityStatus QualityStatus { get; set; } = QualityStatus.Pending;

        // Additional
        public string? Notes { get; set; }
    }
}
