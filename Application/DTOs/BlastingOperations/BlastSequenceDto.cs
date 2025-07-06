using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BlastingOperations
{
    // DTO for blast sequence operations
    public class BlastSequenceDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double DelayBetweenHoles { get; set; }
        public double DelayBetweenRows { get; set; }
        public string SimulationSettingsJson { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
    }

    public class CreateBlastSequenceRequest
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
        
        [Required]
        public double DelayBetweenHoles { get; set; } = 25.0;
        
        [Required]
        public double DelayBetweenRows { get; set; } = 50.0;
        
        public string SimulationSettingsJson { get; set; } = "{}";
    }

    public class UpdateBlastSequenceRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double DelayBetweenHoles { get; set; } = 25.0;
        public double DelayBetweenRows { get; set; } = 50.0;
        public string SimulationSettingsJson { get; set; } = "{}";
    }

    // Note: SimulationSettingsDto removed - these are frontend UI settings
    // Backend only stores the JSON, frontend handles parsing and UI settings
} 
