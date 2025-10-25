namespace Application.DTOs.MachineManagement
{
    public class StockAdjustmentHistoryDto
    {
        public int Id { get; set; }
        public string AdjustmentType { get; set; } = string.Empty;
        public int QuantityChanged { get; set; }
        public int PreviousQuantity { get; set; }
        public int NewQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string AdjustedBy { get; set; } = string.Empty;
        public DateTime AdjustedDate { get; set; }
    }
}
