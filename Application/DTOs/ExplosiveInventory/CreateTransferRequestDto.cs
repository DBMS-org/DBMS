namespace Application.DTOs.ExplosiveInventory
{
    /// <summary>
    /// Request DTO for creating transfer request
    /// </summary>
    public class CreateTransferRequestDto
    {
        public int CentralWarehouseInventoryId { get; set; }
        public int DestinationStoreId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public string Unit { get; set; } = "kg";
        public DateTime? RequiredByDate { get; set; }
        public string? RequestNotes { get; set; }
    }
}
