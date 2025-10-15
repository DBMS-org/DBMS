using Domain.Common;
using Domain.Entities.UserManagement;

namespace Domain.Entities.ProjectManagement
{
    public class ExplosiveApprovalRequest : BaseAuditableEntity
    {
        public int Id { get; set; }
        
        /// <summary>
        /// The project site for which explosive approval is requested
        /// </summary>
        public int ProjectSiteId { get; set; }
        
        /// <summary>
        /// The user who submitted the approval request
        /// </summary>
        public int RequestedByUserId { get; set; }
        
        /// <summary>
        /// Expected date when explosives will be used
        /// </summary>
        public DateTime ExpectedUsageDate { get; set; }
        
        /// <summary>
        /// Additional comments or notes for the approval request
        /// </summary>
        public string? Comments { get; set; }
        
        /// <summary>
        /// Current status of the approval request
        /// </summary>
        public ExplosiveApprovalStatus Status { get; set; } = ExplosiveApprovalStatus.Pending;
        
        /// <summary>
        /// Priority level of the request (for future workflow management)
        /// </summary>
        public RequestPriority Priority { get; set; } = RequestPriority.Normal;
        
        /// <summary>
        /// Type of explosive approval being requested (extensible for different approval types)
        /// </summary>
        public ExplosiveApprovalType ApprovalType { get; set; } = ExplosiveApprovalType.Standard;
        
        /// <summary>
        /// User who approved/rejected the request (nullable until processed)
        /// </summary>
        public int? ProcessedByUserId { get; set; }
        
        /// <summary>
        /// Date when the request was processed (approved/rejected)
        /// </summary>
        public DateTime? ProcessedAt { get; set; }
        
        /// <summary>
        /// Reason for rejection (if applicable)
        /// </summary>
        public string? RejectionReason { get; set; }
        
        /// <summary>
        /// JSON field for storing additional metadata and future extensibility
        /// This allows adding new properties without schema changes
        /// </summary>
        public string? AdditionalData { get; set; }
        
        /// <summary>
        /// Estimated duration of explosive usage in hours (for scheduling)
        /// </summary>
        public decimal? EstimatedDurationHours { get; set; }
        
        /// <summary>
        /// Safety compliance checklist completion status
        /// </summary>
        public bool SafetyChecklistCompleted { get; set; } = false;
        
        /// <summary>
        /// Environmental impact assessment completion status
        /// </summary>
        public bool EnvironmentalAssessmentCompleted { get; set; } = false;

        /// <summary>
        /// Specific date when the blasting operation will occur
        /// Optional when creating request, can be updated later
        /// Required before approval by Store Manager
        /// </summary>
        public DateTime? BlastingDate { get; set; }

        /// <summary>
        /// Specific time when the blasting operation will occur
        /// Format: "HH:mm" (24-hour format)
        /// Optional when creating request, can be updated later
        /// Required before approval by Store Manager
        /// </summary>
        public string? BlastTiming { get; set; }

        // Navigation properties
        public virtual ProjectSite ProjectSite { get; set; } = null!;
        public virtual User RequestedByUser { get; set; } = null!;
        public virtual User? ProcessedByUser { get; set; }
    }
    
    public enum ExplosiveApprovalStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3,
        Expired = 4
    }
    
    public enum RequestPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
    }
    
    public enum ExplosiveApprovalType
    {
        Standard = 0,
        Emergency = 1,
        Maintenance = 2,
        Testing = 3,
        Research = 4
    }
}