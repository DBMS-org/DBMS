using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.ProjectManagement
{
    public class UpdateProjectSiteRequest
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        public CoordinatesDto? Coordinates { get; set; }
    }
} 