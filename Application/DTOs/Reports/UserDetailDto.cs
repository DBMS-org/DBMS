namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed user information for reports
    /// </summary>
    public class UserDetailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty; // Admin, Operator, MechanicalEngineer, etc.
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string Status { get; set; } = string.Empty; // Active, Inactive, Suspended
        public DateTime? HireDate { get; set; }
        public UserCurrentAssignmentsDto CurrentAssignments { get; set; } = new();
        public int ActiveTasksCount { get; set; }
        public int CompletedTasksCount { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class UserCurrentAssignmentsDto
    {
        public List<UserProjectAssignmentDto> ProjectAssignments { get; set; } = new();
        public List<UserMachineAssignmentDto> MachineAssignments { get; set; } = new();
        public List<UserMaintenanceJobDto> MaintenanceJobs { get; set; } = new();
    }

    public class UserProjectAssignmentDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

    public class UserMachineAssignmentDto
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

    public class UserMaintenanceJobDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
