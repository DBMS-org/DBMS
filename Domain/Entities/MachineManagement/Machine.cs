using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Domain.Common;
using Domain.Events.MachineManagement;

namespace Domain.Entities.MachineManagement
{
    public class Machine : BaseAuditableEntity
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

        public MachineStatus Status { get; private set; } = MachineStatus.Available;

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

        // NEW: Service Tracking Properties
        public decimal EngineServiceInterval { get; private set; } = 500m;
        public decimal CurrentEngineServiceHours { get; private set; } = 0m;
        public DateTime? LastEngineServiceDate { get; private set; }
        public decimal? DrifterServiceInterval { get; private set; }
        public decimal? CurrentDrifterServiceHours { get; private set; }
        public DateTime? LastDrifterServiceDate { get; private set; }

        public virtual Project? Project { get; private set; }
        public virtual User? Operator { get; private set; }
        public virtual Region? Region { get; private set; }

        // Computed properties for JSON serialization
        public string? ProjectName => Project?.Name;
        public string? OperatorName => Operator?.Name;
        public string? RegionName => Region?.Name;

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

        /// <summary>
        /// Configure service intervals when creating or editing machine
        /// Called by: Machine Manager when setting up machine
        /// </summary>
        public void ConfigureServiceIntervals(decimal engineInterval, decimal? drifterInterval = null)
        {
            if (engineInterval <= 0)
                throw new InvalidOperationException("Engine service interval must be greater than 0");

            if (Type == "Drill Rig" && drifterInterval == null)
                throw new InvalidOperationException("Drifter service interval required for drill rigs");

            EngineServiceInterval = engineInterval;

            if (Type == "Drill Rig")
            {
                if (drifterInterval <= 0)
                    throw new InvalidOperationException("Drifter service interval must be greater than 0");
                DrifterServiceInterval = drifterInterval;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Increment service hours when usage log is created
        /// Called by: UsageLogCreatedEventHandler
        /// Triggered by: Operator submitting usage log
        /// </summary>
        public void IncrementServiceHours(decimal engineHours, decimal? drifterHours = null)
        {
            CurrentEngineServiceHours += engineHours;

            if (Type == "Drill Rig" && drifterHours.HasValue)
            {
                CurrentDrifterServiceHours = (CurrentDrifterServiceHours ?? 0) + drifterHours.Value;
            }

            MarkUpdated();

            // Raise event if service is now due
            if (IsEngineServiceDue() || IsDrifterServiceDue())
            {
                AddDomainEvent(new ServiceDueEvent(this.Id, this.Name));
            }
        }

        /// <summary>
        /// Reset service hours when maintenance job completed
        /// Called by: ServiceCompletedEventHandler
        /// Triggered by: Mechanical Engineer completing service job
        /// </summary>
        public void ResetServiceHours(bool resetEngine, bool resetDrifter)
        {
            if (resetEngine)
            {
                CurrentEngineServiceHours = 0;
                LastEngineServiceDate = DateTime.UtcNow;
            }

            if (resetDrifter && Type == "Drill Rig")
            {
                CurrentDrifterServiceHours = 0;
                LastDrifterServiceDate = DateTime.UtcNow;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Check if service is due
        /// Called by: Service alert queries
        /// Used by: Machine Manager dashboard
        /// </summary>
        public bool IsEngineServiceDue()
        {
            return CurrentEngineServiceHours >= EngineServiceInterval;
        }

        public bool IsDrifterServiceDue()
        {
            if (Type != "Drill Rig" || !DrifterServiceInterval.HasValue)
                return false;

            return (CurrentDrifterServiceHours ?? 0) >= DrifterServiceInterval.Value;
        }

        /// <summary>
        /// Get hours remaining until service
        /// Called by: Frontend to display progress
        /// Used by: All roles viewing machine details
        /// </summary>
        public decimal GetEngineHoursUntilService()
        {
            return Math.Max(0, EngineServiceInterval - CurrentEngineServiceHours);
        }

        public decimal? GetDrifterHoursUntilService()
        {
            if (Type != "Drill Rig" || !DrifterServiceInterval.HasValue)
                return null;

            return Math.Max(0, DrifterServiceInterval.Value - (CurrentDrifterServiceHours ?? 0));
        }

        /// <summary>
        /// Update last engine service date
        /// Called when manually setting service date during machine creation/editing
        /// </summary>
        public void UpdateLastEngineServiceDate(DateTime date)
        {
            LastEngineServiceDate = date;
            MarkUpdated();
        }

        /// <summary>
        /// Update last drifter service date
        /// Called when manually setting service date during machine creation/editing
        /// </summary>
        public void UpdateLastDrifterServiceDate(DateTime date)
        {
            LastDrifterServiceDate = date;
            MarkUpdated();
        }

        /// <summary>
        /// Update current engine service hours
        /// Called when manually adjusting service hours during machine editing
        /// </summary>
        public void UpdateCurrentEngineServiceHours(decimal hours)
        {
            if (hours < 0)
                throw new InvalidOperationException("Engine service hours cannot be negative");

            CurrentEngineServiceHours = hours;
            MarkUpdated();
        }

        /// <summary>
        /// Update current drifter service hours
        /// Called when manually adjusting service hours during machine editing
        /// </summary>
        public void UpdateCurrentDrifterServiceHours(decimal hours)
        {
            if (hours < 0)
                throw new InvalidOperationException("Drifter service hours cannot be negative");

            CurrentDrifterServiceHours = hours;
            MarkUpdated();
        }
    }
} 
