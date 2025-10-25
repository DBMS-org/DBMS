using Domain.Common;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.MachineManagement
{
    public class MachineAssignmentRequest : BaseAuditableEntity
    {
        public int ProjectId { get; set; }
        public string MachineType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
        public AssignmentRequestStatus Status { get; private set; } = AssignmentRequestStatus.Pending;
        public RequestUrgency Urgency { get; set; } = RequestUrgency.Medium;
        public string? DetailsOrExplanation { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? AssignedMachinesJson { get; set; } // JSON array of machine IDs
        public string? Comments { get; set; }
        public string? ExpectedUsageDuration { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }

        // Navigation properties
        public virtual Project? Project { get; private set; }

        private MachineAssignmentRequest() { }

        public static MachineAssignmentRequest Create(
            int projectId,
            string machineType,
            int quantity,
            string requestedBy,
            RequestUrgency urgency,
            string? detailsOrExplanation = null,
            string? expectedUsageDuration = null,
            DateTime? expectedReturnDate = null)
        {
            return new MachineAssignmentRequest
            {
                ProjectId = projectId,
                MachineType = machineType,
                Quantity = quantity,
                RequestedBy = requestedBy,
                RequestedDate = DateTime.UtcNow,
                Status = AssignmentRequestStatus.Pending,
                Urgency = urgency,
                DetailsOrExplanation = detailsOrExplanation,
                ExpectedUsageDuration = expectedUsageDuration,
                ExpectedReturnDate = expectedReturnDate
            };
        }

        public void Approve(string approvedBy, string[] assignedMachineIds, string? comments = null)
        {
            Status = assignedMachineIds.Length >= Quantity
                ? AssignmentRequestStatus.Approved
                : AssignmentRequestStatus.PartiallyFulfilled;
            ApprovedBy = approvedBy;
            ApprovedDate = DateTime.UtcNow;
            AssignedMachinesJson = System.Text.Json.JsonSerializer.Serialize(assignedMachineIds);
            Comments = comments;
            MarkUpdated();
        }

        public void Reject(string rejectedBy, string comments)
        {
            Status = AssignmentRequestStatus.Rejected;
            ApprovedBy = rejectedBy; // Store who rejected it
            ApprovedDate = DateTime.UtcNow;
            Comments = comments;
            MarkUpdated();
        }

        public void Complete()
        {
            Status = AssignmentRequestStatus.Completed;
            MarkUpdated();
        }

        public void Cancel(string? reason = null)
        {
            Status = AssignmentRequestStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                Comments = string.IsNullOrEmpty(Comments) ? reason : $"{Comments}\nCancellation reason: {reason}";
            }
            MarkUpdated();
        }
    }
}
