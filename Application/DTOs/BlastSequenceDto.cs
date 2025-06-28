using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    // DTO for blast sequence operations
    public class BlastSequenceDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public int DrillPatternId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ConnectionsJson { get; set; } = string.Empty;
        public string SimulationSettingsJson { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        
        // Related data - only include drill pattern for context
        public DrillPatternDto? DrillPattern { get; set; }
        
        // Note: Connections and SimulationSettings removed - frontend should parse JSON directly
    }

    public class CreateBlastSequenceRequest
    {
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
        
        [Required]
        public string ConnectionsJson { get; set; } = string.Empty;
        
        public string SimulationSettingsJson { get; set; } = string.Empty;
    }

    public class BlastConnectionDto
    {
        public string Id { get; set; } = string.Empty;
        public string FromHoleId { get; set; } = string.Empty;
        public string ToHoleId { get; set; } = string.Empty;
        public string ConnectorType { get; set; } = string.Empty;
        public int Delay { get; set; }
        public int Sequence { get; set; }
    }

    // Note: SimulationSettingsDto removed - these are frontend UI settings
    // Backend only stores the JSON, frontend handles parsing and UI settings
} 
