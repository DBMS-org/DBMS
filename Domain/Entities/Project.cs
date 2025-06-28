using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Region { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public int? AssignedUserId { get; set; }
        public int? RegionId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual User? AssignedUser { get; set; }
        public virtual Region? RegionNavigation { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; } = new List<ProjectSite>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
    }
} 
