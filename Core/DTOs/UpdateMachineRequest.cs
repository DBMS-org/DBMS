using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class UpdateMachineRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? RigNo { get; set; }
        
        [MaxLength(50)]
        public string? PlateNo { get; set; }
        
        [MaxLength(500)]
        public string? ChassisDetails { get; set; }
        
        [Range(1900, 2100)]
        public int? ManufacturingYear { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        public int? ProjectId { get; set; }
        public int? OperatorId { get; set; }
        public int? RegionId { get; set; }
        
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        
        public MachineSpecificationsDto? Specifications { get; set; }
    }
} 