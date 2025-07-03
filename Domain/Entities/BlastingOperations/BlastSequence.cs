using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Domain.Entities.DrillingOperations;

namespace Domain.Entities.BlastingOperations
{
    public class BlastSequence
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public int SiteId { get; set; }
        
        public int DrillPatternId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        // Blast connections as JSON
        public string ConnectionsJson { get; set; } = string.Empty;
        
        // Simulation settings as JSON
        public string SimulationSettingsJson { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPattern DrillPattern { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
} 
