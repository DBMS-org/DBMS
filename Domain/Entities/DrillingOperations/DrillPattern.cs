using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Domain.Entities.BlastingOperations;

namespace Domain.Entities.DrillingOperations
{
    public class DrillPattern
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public int SiteId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        // Pattern settings - stored separately for easy querying and business logic
        // These represent the default/template values for the pattern
        public double Spacing { get; set; } = 3.0;
        public double Burden { get; set; } = 2.5;
        public double Depth { get; set; } = 10.0;
        
        // Drill points as JSON
        public string DrillPointsJson { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();
    }
} 
