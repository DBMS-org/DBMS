namespace Application.DTOs.MachineManagement
{
    public class MachineAssignmentDto
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string MachineSerialNumber { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int OperatorId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public string AssignedBy { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
