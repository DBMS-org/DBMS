using System;
using Domain.Common;
using Domain.Entities.UserManagement;

namespace Domain.Entities.MaintenanceOperations
{
    /// <summary>
    /// Junction entity representing the many-to-many relationship between maintenance jobs and mechanical engineers
    /// </summary>
    public class MaintenanceJobAssignment : BaseEntity
    {
        public int MaintenanceJobId { get; set; }
        public int MechanicalEngineerId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual MaintenanceJob MaintenanceJob { get; set; } = null!;
        public virtual User MechanicalEngineer { get; set; } = null!;
    }
}
