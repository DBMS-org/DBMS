using Domain.Entities.MachineManagement;

namespace Application.DTOs.MachineManagement
{
    public class MachineAssignmentRequestDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string MachineType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Urgency { get; set; } = string.Empty;
        public string? DetailsOrExplanation { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public List<int>? AssignedMachines { get; set; }
        public string? Comments { get; set; }
        public string? ExpectedUsageDuration { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
