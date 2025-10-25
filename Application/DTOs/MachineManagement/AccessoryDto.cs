namespace Application.DTOs.MachineManagement
{
    public class AccessoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public int MinStockLevel { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsLowStock { get; set; }
        public bool IsOutOfStock { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
