namespace Application.DTOs.Reports
{
    public class ExplosiveWorkflowReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Summary Statistics
        public WorkflowStatisticsDto Statistics { get; set; } = new();

        // Approval Requests Analysis
        public List<ApprovalRequestSummaryDto> ApprovalRequests { get; set; } = new();
        public ApprovalAnalysisDto ApprovalAnalysis { get; set; } = new();

        // Transfer Requests Analysis
        public List<WorkflowTransferRequestDto> TransferRequests { get; set; } = new();
        public TransferAnalysisDto TransferAnalysis { get; set; } = new();

        // Turnaround Time Analysis
        public TurnaroundAnalysisDto TurnaroundAnalysis { get; set; } = new();

        // User/Requester Statistics
        public List<RequesterStatisticsDto> RequesterStatistics { get; set; } = new();
    }

    public class WorkflowStatisticsDto
    {
        // Approval Request Stats
        public int TotalApprovalRequests { get; set; }
        public int PendingApprovals { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int CancelledApprovals { get; set; }
        public int ExpiredApprovals { get; set; }
        public decimal ApprovalRate { get; set; } // Percentage

        // Transfer Request Stats
        public int TotalTransferRequests { get; set; }
        public int PendingTransfers { get; set; }
        public int ApprovedTransfers { get; set; }
        public int RejectedTransfers { get; set; }
        public int InProgressTransfers { get; set; }
        public int CompletedTransfers { get; set; }
        public int CancelledTransfers { get; set; }
        public decimal TransferCompletionRate { get; set; } // Percentage

        // Overall Stats
        public int TotalWorkflowItems { get; set; }
        public int TotalPending { get; set; }
        public int TotalCompleted { get; set; }
        public decimal OverallCompletionRate { get; set; }
    }

    public class ApprovalRequestSummaryDto
    {
        public int Id { get; set; }
        public string ProjectSiteName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string RequestedByUserName { get; set; } = string.Empty;
        public DateTime ExpectedUsageDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string ApprovalType { get; set; } = string.Empty;
        public string? ProcessedByUserName { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? Comments { get; set; }
        public string? RejectionReason { get; set; }
        public bool SafetyChecklistCompleted { get; set; }
        public bool EnvironmentalAssessmentCompleted { get; set; }
        public DateTime? BlastingDate { get; set; }
        public string? BlastTiming { get; set; }
        public int? TurnaroundDays { get; set; }
    }

    public class ApprovalAnalysisDto
    {
        public int TotalRequests { get; set; }
        public Dictionary<string, int> ByStatus { get; set; } = new();
        public Dictionary<string, int> ByPriority { get; set; } = new();
        public Dictionary<string, int> ByApprovalType { get; set; } = new();
        public Dictionary<string, int> ByProjectSite { get; set; } = new();
        public Dictionary<string, int> ByRegion { get; set; } = new();
        public int SafetyChecklistCompletedCount { get; set; }
        public int EnvironmentalAssessmentCompletedCount { get; set; }
        public decimal ComplianceRate { get; set; } // Percentage of requests with checklists completed
        public decimal AverageTurnaroundDays { get; set; }
    }

    public class WorkflowTransferRequestDto
    {
        public int Id { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public string ExplosiveTypeName { get; set; } = string.Empty;
        public string DestinationStoreName { get; set; } = string.Empty;
        public string DestinationRegion { get; set; } = string.Empty;
        public decimal RequestedQuantity { get; set; }
        public decimal? ApprovedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public string RequestedByUserName { get; set; } = string.Empty;
        public string? ApprovedByUserName { get; set; }
        public string? ProcessedByUserName { get; set; }
        public string? TruckNumber { get; set; }
        public string? DriverName { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string? RejectionReason { get; set; }
        public int? TurnaroundDays { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsUrgent { get; set; }
    }

    public class TransferAnalysisDto
    {
        public int TotalRequests { get; set; }
        public Dictionary<string, int> ByStatus { get; set; } = new();
        public Dictionary<string, int> ByExplosiveType { get; set; } = new();
        public Dictionary<string, int> ByDestinationStore { get; set; } = new();
        public Dictionary<string, int> ByDestinationRegion { get; set; } = new();
        public decimal TotalRequestedQuantity { get; set; }
        public decimal TotalApprovedQuantity { get; set; }
        public decimal TotalTransferredQuantity { get; set; }
        public int OverdueCount { get; set; }
        public int UrgentCount { get; set; }
        public decimal AverageTurnaroundDays { get; set; }
        public decimal QuantityFulfillmentRate { get; set; } // Approved/Requested percentage
    }

    public class TurnaroundAnalysisDto
    {
        // Approval Request Turnaround
        public decimal AverageApprovalTurnaroundDays { get; set; }
        public decimal MinApprovalTurnaroundDays { get; set; }
        public decimal MaxApprovalTurnaroundDays { get; set; }
        public int ProcessedApprovalCount { get; set; }

        // Transfer Request Turnaround
        public decimal AverageTransferTurnaroundDays { get; set; }
        public decimal MinTransferTurnaroundDays { get; set; }
        public decimal MaxTransferTurnaroundDays { get; set; }
        public int ProcessedTransferCount { get; set; }

        // Overall Turnaround
        public decimal OverallAverageTurnaroundDays { get; set; }
        public int TotalProcessedRequests { get; set; }
    }

    public class RequesterStatisticsDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public int TotalApprovalRequests { get; set; }
        public int TotalTransferRequests { get; set; }
        public int TotalRequests { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PendingCount { get; set; }
        public decimal ApprovalRate { get; set; } // Percentage
    }
}
