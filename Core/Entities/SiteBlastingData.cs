using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class SiteBlastingData
    {
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DataType { get; set; } = string.Empty; // "pattern", "connections", "simulation_settings", "simulation_state"
        
        [Required]
        public string JsonData { get; set; } = string.Empty; // Serialized JSON data
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        public int CreatedByUserId { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        public virtual User CreatedBy { get; set; } = null!;
        
        // Add indexes for better performance
        [NotMapped]
        public string CompositeKey => $"{ProjectId}_{SiteId}_{DataType}";
    }
} 