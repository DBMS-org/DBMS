using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BlastingOperations
{
    // Main DTO for site blasting data operations
    public class SiteBlastingDataDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string DataType { get; set; } = string.Empty;
        public string JsonData { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
    }

    // DTO for creating/updating site blasting data
    public class CreateSiteBlastingDataRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string DataType { get; set; } = string.Empty;
        
        [Required]
        public string JsonData { get; set; } = string.Empty;
    }
} 
