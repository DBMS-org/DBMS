namespace Application.DTOs.ExplosiveInventory
{
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
