using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Domain.Common;

namespace Domain.Entities.MachineManagement
{
    public class Machine : BaseAuditableEntity
    {
        // Identity comes from BaseEntity (Id)
        public string Name { get; set; } = string.Empty;
        
        public string Type { get; set; } = string.Empty;
        
        public string Model { get; set; } = string.Empty;
        
        public string Manufacturer { get; set; } = string.Empty;
        
        public string SerialNumber { get; set; } = string.Empty;
        
        public string? RigNo { get; set; }
        
        public string? PlateNo { get; set; }
        
        public string? ChassisDetails { get; set; }
        
        public int? ManufacturingYear { get; set; }
        
        public MachineStatus Status { get; private set; } = MachineStatus.Available;
        
        public string? CurrentLocation { get; set; }
        
        public string? Location { get; set; }
        
        public string? AssignedToProject { get; set; }
        
        public string? AssignedToOperator { get; set; }
        
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        
        // JSON field for specifications
        public string? SpecificationsJson { get; set; }
        
        // Foreign key relationships
        public int? ProjectId { get; set; }
        public int? OperatorId { get; set; }
        public int? RegionId { get; set; }
        
        // Navigation properties
        public virtual Project? Project { get; private set; }
        public virtual User? Operator { get; private set; }
        public virtual Region? Region { get; private set; }

        private Machine() { }

        public static Machine Create(string name, string type, string model, string manufacturer, string serialNumber, int? projectId = null)
        {
            return new Machine
            {
                Name = name,
                Type = type,
                Model = model,
                Manufacturer = manufacturer,
                SerialNumber = serialNumber,
                ProjectId = projectId,
                Status = MachineStatus.Available
            };
        }

        public void AssignOperator(int operatorId)
        {
            OperatorId = operatorId;
            MarkUpdated();
        }

        public void ChangeStatus(MachineStatus status)
        {
            Status = status;
            MarkUpdated();
        }

        public void AssignToProject(string projectName)
        {
            AssignedToProject = projectName;
            MarkUpdated();
            AddDomainEvent(new Domain.Events.MachineAssignedToProjectEvent(Id, projectName));
        }
    }
} 
