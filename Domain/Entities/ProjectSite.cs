using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ProjectSite
    {
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Coordinates { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsPatternApproved { get; set; } = false;
        public bool IsSimulationConfirmed { get; set; } = false;
        public bool IsOperatorCompleted { get; set; } = false;
        
        // Navigation properties
        public virtual Project Project { get; set; } = null!;
    }
} 
