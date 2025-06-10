using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class BlastSequence
    {
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        public int DrillPatternId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        // Blast connections as JSON
        [Required]
        public string ConnectionsJson { get; set; } = string.Empty;
        
        // Simulation settings as JSON
        public string SimulationSettingsJson { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual DrillPattern DrillPattern { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
    }
} 