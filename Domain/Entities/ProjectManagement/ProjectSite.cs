using Domain.Common;
using Domain.Entities.ProjectManagement;

namespace Domain.Entities.ProjectManagement
{
    public class ProjectSite : BaseAuditableEntity
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
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
    }
} 
