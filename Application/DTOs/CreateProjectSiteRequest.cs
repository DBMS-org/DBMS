using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateProjectSiteRequest
    {
        [Required(ErrorMessage = "Project ID is required")]
        public int ProjectId { get; set; }
        
        [Required(ErrorMessage = "Site name is required")]
        [StringLength(100, ErrorMessage = "Site name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;
        
        public CoordinatesDto? Coordinates { get; set; }
        
        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
} 
