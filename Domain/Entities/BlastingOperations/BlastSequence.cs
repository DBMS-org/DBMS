using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.BlastingOperations
{
    public class BlastSequence
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public int SiteId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        // Sequence timing settings
        public double DelayBetweenHoles { get; set; } = 25.0;
        public double DelayBetweenRows { get; set; } = 50.0;
        
        // Simulation settings stored as JSON  
        public string SimulationSettingsJson { get; set; } = "{}";
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
} 
