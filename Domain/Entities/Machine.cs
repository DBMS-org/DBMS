

namespace Domain.Entities
{
    public class Machine
    {
        public int Id { get; set; }
        


        public string Name { get; set; } = string.Empty;
        


        public string Type { get; set; } = string.Empty;
        


        public string Model { get; set; } = string.Empty;
        


        public string Manufacturer { get; set; } = string.Empty;
        


        public string SerialNumber { get; set; } = string.Empty;
        

        public string? RigNo { get; set; }
        

        public string? PlateNo { get; set; }
        

        public string? ChassisDetails { get; set; }
        
        public int? ManufacturingYear { get; set; }
        


        public string Status { get; set; } = string.Empty;
        

        public string? CurrentLocation { get; set; }
        

        public string? AssignedToProject { get; set; }
        

        public string? AssignedToOperator { get; set; }
        
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        
        // JSON field for specifications
        public string? SpecificationsJson { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign key relationships

        public int ProjectId { get; set; }
        public int? OperatorId { get; set; }
        public int? RegionId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual User? Operator { get; set; }
        public virtual Region? Region { get; set; }
    }
} 
