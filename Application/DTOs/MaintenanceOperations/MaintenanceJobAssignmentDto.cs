namespace Application.DTOs.MaintenanceOperations
{
    public class MaintenanceJobAssignmentDto
    {
        public int Id { get; set; }
        public int MaintenanceJobId { get; set; }
        public int MechanicalEngineerId { get; set; }
        public string MechanicalEngineerName { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
    }
}
