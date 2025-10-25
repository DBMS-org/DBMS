using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class StockAdjustmentRequest
    {
        [Required]
        public string Type { get; set; } = string.Empty; // "Add", "Remove", "Set"

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty; // "Purchase", "Usage", "Damaged", etc.

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
