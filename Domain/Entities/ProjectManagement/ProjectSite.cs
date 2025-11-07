using Domain.Common;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.ProjectManagement
{
    public class ProjectSite : BaseAuditableEntity, IEntityOwnedByUser
    {
        public int ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Coordinates { get; set; } = string.Empty;

        public ProjectSiteStatus Status { get; set; } = ProjectSiteStatus.Planned;

        public string Description { get; set; } = string.Empty;

        public bool IsPatternApproved { get; set; } = false;
        public bool IsSimulationConfirmed { get; set; } = false;
        public bool IsOperatorCompleted { get; set; } = false;

        /// <summary>
        /// Indicates whether this project site has been marked as completed
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Date and time when the site was marked as completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// User ID of the person who marked the site as completed
        /// </summary>
        public int? CompletedByUserId { get; set; }

        public int OwningUserId => Project?.OwningUserId ?? 0;

        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<ExplosiveApprovalRequest> ExplosiveApprovalRequests { get; set; } = new List<ExplosiveApprovalRequest>();
        public virtual User? CompletedByUser { get; set; }
    }
}
