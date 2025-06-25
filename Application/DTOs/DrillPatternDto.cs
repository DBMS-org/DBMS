using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    // DTO for drill pattern operations
    public class DrillPatternDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Spacing { get; set; }
        public double Burden { get; set; }
        public double Depth { get; set; }
        public string DrillPointsJson { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        
        // Note: Parsed DrillPoints and Settings removed - frontend should parse JSON directly
        // This keeps the backend lean and avoids duplicate data
    }

    public class CreateDrillPatternRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0.1, 50.0)]
        public double Spacing { get; set; } = 3.0;
        
        [Range(0.1, 50.0)]
        public double Burden { get; set; } = 2.5;
        
        [Range(0.1, 100.0)]
        public double Depth { get; set; } = 10.0;
        
        [Required]
        public string DrillPointsJson { get; set; } = string.Empty;
    }

    // Note: DrillPointDto and PatternSettingsDto removed - these are frontend concerns
    // Backend stores raw JSON, frontend handles parsing and structure
} 
