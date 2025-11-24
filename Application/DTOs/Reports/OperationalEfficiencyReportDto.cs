namespace Application.DTOs.Reports
{
    public class OperationalEfficiencyReportDto
    {
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Statistics
        public OperationalStatisticsDto Statistics { get; set; } = new();

        // Machine Assignment Metrics
        public MachineAssignmentMetricsDto AssignmentMetrics { get; set; } = new();

        // Project Completion Rate
        public List<ProjectCompletionDto> ProjectCompletion { get; set; } = new();

        // Active Assignments by Region
        public List<RegionalAssignmentDto> RegionalAssignments { get; set; } = new();

        // Request Fulfillment Rates
        public RequestFulfillmentDto RequestFulfillment { get; set; } = new();

        // User Productivity
        public List<UserProductivityDto> UserProductivity { get; set; } = new();

        // Resource Utilization
        public ResourceUtilizationDto ResourceUtilization { get; set; } = new();

        // Workflow Efficiency
        public List<WorkflowEfficiencyDto> WorkflowEfficiency { get; set; } = new();

        // DETAILED DATA ARRAYS
        // All Projects with complete details
        public List<ProjectDetailDto> AllProjects { get; set; } = new();

        // Project Sites
        public List<ProjectSiteDetailDto> AllProjectSites { get; set; } = new();

        // All Users by Role
        public List<UserDetailDto> AllUsers { get; set; } = new();

        // Machine Assignment Requests Workflow
        public List<AssignmentRequestDetailDto> AllAssignmentRequests { get; set; } = new();

        // Resource Allocation Overview
        public List<ResourceAllocationDetailDto> ResourceAllocationDetails { get; set; } = new();
    }

    public class OperationalStatisticsDto
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalSites { get; set; }
        public int ActiveSites { get; set; }
        public int TotalAssignments { get; set; }
        public int ActiveAssignments { get; set; }
        public decimal OverallEfficiencyRate { get; set; }
    }

    public class MachineAssignmentMetricsDto
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public decimal ApprovalRate { get; set; }
        public decimal AverageTurnaroundTime { get; set; } // in hours
        public decimal MedianTurnaroundTime { get; set; } // in hours
        public int AssignmentsThisPeriod { get; set; }
        public int ReturnedThisPeriod { get; set; }
    }

    public class ProjectCompletionDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalSites { get; set; }
        public int CompletedSites { get; set; }
        public decimal CompletionPercentage { get; set; }
        public int DaysActive { get; set; }
        public int MachinesAssigned { get; set; }
    }

    public class RegionalAssignmentDto
    {
        public string Region { get; set; } = string.Empty;
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int MachinesAssigned { get; set; }
        public int Operators { get; set; }
        public decimal UtilizationRate { get; set; }
        public decimal CompletionRate { get; set; }
    }

    public class RequestFulfillmentDto
    {
        public MachineRequestFulfillmentDto MachineRequests { get; set; } = new();
        public TransferRequestFulfillmentDto TransferRequests { get; set; } = new();
        public ExplosiveApprovalFulfillmentDto ExplosiveApprovals { get; set; } = new();
    }

    public class MachineRequestFulfillmentDto
    {
        public int Total { get; set; }
        public int Fulfilled { get; set; }
        public int Pending { get; set; }
        public decimal FulfillmentRate { get; set; }
        public decimal AverageTime { get; set; } // in hours
    }

    public class TransferRequestFulfillmentDto
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int InProgress { get; set; }
        public decimal FulfillmentRate { get; set; }
        public decimal AverageTime { get; set; } // in hours
    }

    public class ExplosiveApprovalFulfillmentDto
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Pending { get; set; }
        public decimal ApprovalRate { get; set; }
        public decimal AverageTime { get; set; } // in hours
    }

    public class UserProductivityDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int ProjectsAssigned { get; set; }
        public int TasksCompleted { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageResponseTime { get; set; } // in hours
        public int ActiveDays { get; set; }
    }

    public class ResourceUtilizationDto
    {
        public decimal MachineUtilization { get; set; }
        public decimal OperatorUtilization { get; set; }
        public decimal EngineerUtilization { get; set; }
        public decimal InventoryUtilization { get; set; }
        public List<ResourceByTypeDto> UtilizationByType { get; set; } = new();
    }

    public class ResourceByTypeDto
    {
        public string ResourceType { get; set; } = string.Empty;
        public int Total { get; set; }
        public int InUse { get; set; }
        public decimal UtilizationRate { get; set; }
    }

    public class WorkflowEfficiencyDto
    {
        public string WorkflowName { get; set; } = string.Empty;
        public int TotalInstances { get; set; }
        public int CompletedInstances { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageCompletionTime { get; set; } // in hours
        public int BottleneckStage { get; set; }
        public string BottleneckDescription { get; set; } = string.Empty;
    }
}
