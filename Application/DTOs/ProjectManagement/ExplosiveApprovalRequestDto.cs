using Application.DTOs.UserManagement;

namespace Application.DTOs.ProjectManagement
{
    public class ExplosiveApprovalRequestDto
    {
        public int Id { get; set; }
        public int ProjectSiteId { get; set; }
        public int RequestedByUserId { get; set; }
        public DateTime ExpectedUsageDate { get; set; }
        public string? Comments { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public string ApprovalType { get; set; } = string.Empty;
        public int? ProcessedByUserId { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? RejectionReason { get; set; }
        public string? AdditionalData { get; set; }
        public decimal? EstimatedDurationHours { get; set; }
        public bool SafetyChecklistCompleted { get; set; }
        public bool EnvironmentalAssessmentCompleted { get; set; }
        public DateTime? BlastingDate { get; set; }
        public string? BlastTiming { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public ProjectSiteDto? ProjectSite { get; set; }
        public UserDto? RequestedByUser { get; set; }
        public UserDto? ProcessedByUser { get; set; }
    }
}