using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.MachineManagement
{
    public class MachineAssignment : BaseAuditableEntity
    {
        public int MachineId { get; set; }
        public int ProjectId { get; set; }
        public int OperatorId { get; set; }
        public string AssignedBy { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public AssignmentStatus Status { get; private set; } = AssignmentStatus.Active;
        public string? Location { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public virtual Machine? Machine { get; private set; }
        public virtual Project? Project { get; private set; }
        public virtual User? Operator { get; private set; }

        private MachineAssignment() { }

        public static MachineAssignment Create(
            int machineId,
            int projectId,
            int operatorId,
            string assignedBy,
            DateTime? expectedReturnDate = null,
            string? location = null,
            string? notes = null)
        {
            return new MachineAssignment
            {
                MachineId = machineId,
                ProjectId = projectId,
                OperatorId = operatorId,
                AssignedBy = assignedBy,
                AssignedDate = DateTime.UtcNow,
                ExpectedReturnDate = expectedReturnDate,
                Status = AssignmentStatus.Active,
                Location = location,
                Notes = notes
            };
        }

        public void Return(DateTime? returnDate = null)
        {
            ActualReturnDate = returnDate ?? DateTime.UtcNow;
            Status = AssignmentStatus.Completed;
            MarkUpdated();
        }

        public void MarkOverdue()
        {
            if (ExpectedReturnDate.HasValue && DateTime.UtcNow > ExpectedReturnDate.Value && Status == AssignmentStatus.Active)
            {
                Status = AssignmentStatus.Overdue;
                MarkUpdated();
            }
        }

        public void Cancel(string? reason = null)
        {
            Status = AssignmentStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = string.IsNullOrEmpty(Notes) ? reason : $"{Notes}\nCancellation reason: {reason}";
            }
            MarkUpdated();
        }

        public void UpdateLocation(string location)
        {
            Location = location;
            MarkUpdated();
        }

        public void AddNotes(string notes)
        {
            Notes = string.IsNullOrEmpty(Notes) ? notes : $"{Notes}\n{notes}";
            MarkUpdated();
        }
    }
}
