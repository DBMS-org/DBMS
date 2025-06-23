using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        
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
        
        public int? ManufacturingYear { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string? CurrentLocation { get; set; }
        
        [MaxLength(100)]
        public string? AssignedToProject { get; set; }
        
        [MaxLength(100)]
        public string? AssignedToOperator { get; set; }
        
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        
        // JSON field for specifications
        public string? SpecificationsJson { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign key relationships
        public int? ProjectId { get; set; }
        public int? OperatorId { get; set; }
        
        // Navigation properties
        public virtual Project? Project { get; set; }
        public virtual User? Operator { get; set; }
    }
} 