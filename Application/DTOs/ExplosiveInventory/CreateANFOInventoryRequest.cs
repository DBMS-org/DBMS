using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for creating ANFO inventory batch
    /// </summary>
    public class CreateANFOInventoryRequest
    {
        public string BatchId { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "kg";
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public string? ManufacturerBatchNumber { get; set; }
        public string StorageLocation { get; set; } = string.Empty;
        public int CentralWarehouseStoreId { get; set; }

        public decimal Density { get; set; }
        public decimal FuelOilContent { get; set; }
        public ANFOGrade Grade { get; set; }
        public decimal StorageTemperature { get; set; }
        public decimal StorageHumidity { get; set; }

        public decimal? MoistureContent { get; set; }
        public decimal? PrillSize { get; set; }
        public int? DetonationVelocity { get; set; }

        public FumeClass FumeClass { get; set; } = FumeClass.Class1;
        public QualityStatus QualityStatus { get; set; } = QualityStatus.Pending;

        public string? Notes { get; set; }
    }
}
