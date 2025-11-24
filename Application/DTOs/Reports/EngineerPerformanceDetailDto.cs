namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed engineer performance information for reports
    /// </summary>
    public class EngineerPerformanceDetailDto
    {
        public int EngineerId { get; set; }
        public string EngineerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int? RegionId { get; set; }
        public string? RegionName { get; set; }
        public string Status { get; set; } = string.Empty; // Active, Inactive
        public List<EngineerJobAssignmentDto> AssignedJobs { get; set; } = new();
        public int TotalJobsAssigned { get; set; }
        public int CompletedJobs { get; set; }
        public int JobsCompleted { get; set; } // Alias for frontend compatibility
        public int InProgressJobs { get; set; }
        public int JobsInProgress { get; set; } // Alias for frontend compatibility
        public int OverdueJobs { get; set; }
        public decimal AverageCompletionTime { get; set; } // in hours
        public decimal AverageResponseTimeHours { get; set; } // Alias for frontend compatibility
        public decimal AverageCompletionTimeHours { get; set; } // Alias for frontend compatibility
        public decimal CompletionRate { get; set; } // percentage
        public decimal TotalHoursWorked { get; set; }
        public decimal PerformanceRating { get; set; } // calculated score
    }

    /// <summary>
    /// Job assignment for an engineer
    /// </summary>
    public class EngineerJobAssignmentDto
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal? ActualHours { get; set; }
    }
}
