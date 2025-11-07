using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BlastingOperations
{
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
