namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed accessory inventory information for reports
    /// </summary>
    public class AccessoryDetailDto
    {
        public int AccessoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // DrillBit, DrillRod, Shank
        public string? SubCategory { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string Status { get; set; } = string.Empty; // InStock, LowStock, OutOfStock
        public DateTime? LastRestocked { get; set; }
    }
}
