using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class CreateProjectRequest
    {
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Region is required")]
        [StringLength(100, ErrorMessage = "Region cannot exceed 100 characters")]
        public string Region { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Project type is required")]
        [StringLength(100, ErrorMessage = "Project type cannot exceed 100 characters")]
        public string ProjectType { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value")]
        public decimal Budget { get; set; }
        
        public int? AssignedUserId { get; set; }
    }
} 