using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    // DTO for workflow progress tracking
    public class SiteWorkflowProgressDto
    {
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public WorkflowStepDto PatternCreator { get; set; } = new();
        public WorkflowStepDto SequenceDesigner { get; set; } = new();
        public WorkflowStepDto Simulator { get; set; } = new();
        public int OverallProgress { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class WorkflowStepDto
    {
        public bool Completed { get; set; }
        public int Progress { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }

    // DTO for bulk operations
    public class BulkSiteDataRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        public string? PatternData { get; set; }
        public string? ConnectionsData { get; set; }
        public string? SimulationSettingsData { get; set; }
        public string? SimulationStateData { get; set; }
    }

    // DTO for data cleanup operations
    public class CleanupSiteDataRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        public List<string> DataTypesToClean { get; set; } = new();
        
        // If empty, cleans all data types
        public bool CleanAll => !DataTypesToClean.Any();
    }
} 
