namespace Application.DTOs.MachineManagement
{
    public class MachineDto
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
        public string? Location { get; set; }
        public string? AssignedToProject { get; set; }
        public string? AssignedToOperator { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? SpecificationsJson { get; set; }
        public int? ProjectId { get; set; }
        public int? OperatorId { get; set; }
        public int? RegionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public string? ProjectName { get; set; }
        public string? OperatorName { get; set; }
        public string? RegionName { get; set; }
    }
} 