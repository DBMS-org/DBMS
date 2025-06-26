namespace Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Foreign key properties
        public int? AssignedUserId { get; set; }
        public int? RegionId { get; set; }
        
        // Navigation property values (populated from relationships)
        public string? AssignedUserName { get; set; }
        public string? RegionName { get; set; }
        
        // Related data
        public List<ProjectSiteDto> ProjectSites { get; set; } = new List<ProjectSiteDto>();
        
        // Business logic convenience properties
        public bool IsActive => Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
        public bool IsCompleted => Status.Equals("Completed", StringComparison.OrdinalIgnoreCase);
        public bool IsOverdue => EndDate.HasValue && EndDate.Value < DateTime.UtcNow && !IsCompleted;
    }
} 
