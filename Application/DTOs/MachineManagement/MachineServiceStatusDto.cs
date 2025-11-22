using System;

namespace Application.DTOs.MachineManagement
{
    public class MachineServiceStatusDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;

        // Engine service
        public decimal EngineServiceInterval { get; set; }
        public decimal CurrentEngineServiceHours { get; set; }
        public decimal EngineHoursRemaining { get; set; }
        public DateTime? LastEngineServiceDate { get; set; }
        public bool IsEngineServiceDue { get; set; }

        // Drifter service (if drill rig)
        public decimal? DrifterServiceInterval { get; set; }
        public decimal? CurrentDrifterServiceHours { get; set; }
        public decimal? DrifterHoursRemaining { get; set; }
        public DateTime? LastDrifterServiceDate { get; set; }
        public bool IsDrifterServiceDue { get; set; }
    }
}
