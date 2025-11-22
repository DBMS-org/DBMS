namespace Application.DTOs.MachineManagement
{
    public class ServiceDueAlertDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;  // "Engine" or "Drifter"
        public decimal HoursRemaining { get; set; }
        public bool IsOverdue { get; set; }
        public string UrgencyLevel { get; set; } = string.Empty;  // "GREEN", "YELLOW", "ORANGE", "RED"
    }
}
