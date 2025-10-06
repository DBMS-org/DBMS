using Domain.Entities.ExplosiveInventory.Enums;

namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for creating Emulsion inventory batch
    /// </summary>
    public class CreateEmulsionInventoryRequest
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

        // Emulsion Technical Properties - Required
        public decimal DensityUnsensitized { get; set; }
        public decimal DensitySensitized { get; set; }
        public int Viscosity { get; set; }
        public decimal WaterContent { get; set; }
        public decimal pH { get; set; }
        public decimal StorageTemperature { get; set; }
        public EmulsionGrade Grade { get; set; }
        public string Color { get; set; } = string.Empty;
        public SensitizationType SensitizationType { get; set; }

        // Emulsion Technical Properties - Optional
        public int? DetonationVelocity { get; set; }
        public int? BubbleSize { get; set; }
        public decimal? ApplicationTemperature { get; set; }
        public decimal? SensitizerContent { get; set; }

        // Quality Control
        public FumeClass FumeClass { get; set; } = FumeClass.Class1;
        public QualityStatus QualityStatus { get; set; } = QualityStatus.Pending;

        // Additional
        public string? Notes { get; set; }
    }
}
