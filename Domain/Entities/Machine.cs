namespace Domain.Entities
{
    public class Machine : BaseEntity
    {
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
        
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        
        // JSON field for specifications
        public string? SpecificationsJson { get; set; }
        
        // Foreign key relationships
        public int ProjectId { get; set; }
        public int? OperatorId { get; set; }
        public int? RegionId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual User? Operator { get; set; }
        public virtual Region? Region { get; set; }
        
        // Business logic methods
        public bool IsAvailable() => Status.Equals("Available", StringComparison.OrdinalIgnoreCase);
        
        public bool IsInUse() => Status.Equals("In Use", StringComparison.OrdinalIgnoreCase);
        
        public bool IsUnderMaintenance() => Status.Equals("Under Maintenance", StringComparison.OrdinalIgnoreCase);
        
        public bool RequiresMaintenance()
        {
            return NextMaintenanceDate.HasValue && NextMaintenanceDate.Value <= DateTime.UtcNow;
        }
        
        public void AssignToOperator(int operatorId)
        {
            OperatorId = operatorId;
            Status = "In Use";
            UpdateTimestamp();
        }
        
        public void UnassignOperator()
        {
            OperatorId = null;
            Status = "Available";
            UpdateTimestamp();
        }
        
        public void ScheduleMaintenance(DateTime nextMaintenanceDate)
        {
            NextMaintenanceDate = nextMaintenanceDate;
            UpdateTimestamp();
        }
        
        public void CompleteMaintenance()
        {
            LastMaintenanceDate = DateTime.UtcNow;
            NextMaintenanceDate = DateTime.UtcNow.AddMonths(3); // Default 3 months
            Status = "Available";
            UpdateTimestamp();
        }
        
        public int GetMachineAge()
        {
            if (!ManufacturingYear.HasValue) return 0;
            return DateTime.UtcNow.Year - ManufacturingYear.Value;
        }
    }
} 
