namespace Application.DTOs.Reports
{
    /// <summary>
    /// Detailed machine assignment request information for reports
    /// </summary>
    public class AssignmentRequestDetailDto
    {
        public int RequestId { get; set; }
        public int RequesterId { get; set; }
        public string RequesterName { get; set; } = string.Empty;
        public string RequesterRole { get; set; } = string.Empty;
        public string RequestedMachineType { get; set; } = string.Empty;
        public int? RequestedMachineId { get; set; }
        public string? RequestedMachineName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending, Approved, Rejected
        public DateTime RequestDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public string? Justification { get; set; }
        public int? ApprovedById { get; set; }
        public string? ApproverName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? ResponseNotes { get; set; }
        public string? RejectionReason { get; set; }
    }
}
