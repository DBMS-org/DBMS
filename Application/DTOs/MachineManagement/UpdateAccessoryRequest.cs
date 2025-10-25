using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.MachineManagement
{
    public class UpdateAccessoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int MinStockLevel { get; set; }

        [Required]
        [MaxLength(200)]
        public string Supplier { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Location { get; set; }
    }
}
