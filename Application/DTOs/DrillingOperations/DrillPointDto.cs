using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.DrillingOperations
{
    public class DrillPointDto
    {
        public string Id { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Depth { get; set; }
        public double Spacing { get; set; }
        public double Burden { get; set; }
        public double Diameter { get; set; } = 0.15;
        public double Stemming { get; set; } = 2.0;
        public double Subdrill { get; set; } = 0.0;
        public double Volume { get; set; } = 0.0;
        public double ANFO { get; set; } = 0.0;
        public double Emulsion { get; set; } = 0.0;
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    public class CreateDrillPointRequest
    {
        [Required]
        public double X { get; set; }
        
        [Required]
        public double Y { get; set; }
        
        [Range(0.1, 100.0)]
        public double Depth { get; set; } = 10.0;
        
        [Range(0.1, 50.0)]
        public double Spacing { get; set; } = 3.0;
        
        [Range(0.1, 50.0)]
        public double Burden { get; set; } = 2.5;
        
        [Range(0.05, 1.0)]
        public double Diameter { get; set; } = 0.89;
        
        [Range(0.5, 10.0)]
        public double Stemming { get; set; } = 3.0;
        
        [Range(0.0, 5.0)]
        public double Subdrill { get; set; } = 0.0;
        
        [Range(0.0, 1000.0)]
        public double Volume { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double ANFO { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double Emulsion { get; set; } = 0.0;
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
    }
    
    public class UpdateDrillPointPositionRequest
    {
        [Required]
        public string PointId { get; set; } = string.Empty;
        
        [Required]
        public double X { get; set; }
        
        [Required]
        public double Y { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
    }
    
    public class PatternSettingsDto
    {
        [Range(0.1, 50.0)]
        public double Spacing { get; set; } = 3.0;
        
        [Range(0.1, 50.0)]
        public double Burden { get; set; } = 2.5;
        
        [Range(0.1, 100.0)]
        public double Depth { get; set; } = 10.0;
        
        [Range(0.05, 1.0)]
        public double Diameter { get; set; } = 0.89;
        
        [Range(0.5, 10.0)]
        public double Stemming { get; set; } = 3.0;
        
        [Range(0.0, 5.0)]
        public double Subdrill { get; set; } = 0.0;
        
        [Range(0.0, 1000.0)]
        public double Volume { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double ANFO { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double Emulsion { get; set; } = 0.0;
    }
    
    public class PatternDataDto
    {
        public List<DrillPointDto> DrillPoints { get; set; } = new();
        public PatternSettingsDto Settings { get; set; } = new();
    }
    
    public class ProcessCsvDataRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        public List<DrillHoleDataDto> CsvData { get; set; } = new();
    }
    
    public class DrillHoleDataDto
    {
        public int? Id { get; set; }
        public double? Easting { get; set; }
        public double? Northing { get; set; }
        public double? Depth { get; set; }
        public double? Length { get; set; }
    }
    
    public class SavePatternRequest
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Required]
        public List<DrillPointDto> DrillPoints { get; set; } = new();
        
        [Required]
        public UnifiedPatternSettingsDto Settings { get; set; } = new();
    }
    
    public class UnifiedPatternSettingsDto
    {
        public string Name { get; set; } = "Default Pattern";
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
        
        [Range(0.1, 50.0)]
        public double Spacing { get; set; } = 3.0;
        
        [Range(0.1, 50.0)]
        public double Burden { get; set; } = 2.5;
        
        [Range(0.1, 100.0)]
        public double Depth { get; set; } = 10.0;
        
        [Range(0.05, 1.0)]
        public double Diameter { get; set; } = 0.89;
        
        [Range(0.5, 10.0)]
        public double Stemming { get; set; } = 3.0;
        
        [Range(0.0, 5.0)]
        public double Subdrill { get; set; } = 0.0;
        
        [Range(0.0, 1000.0)]
        public double Volume { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double ANFO { get; set; } = 0.0;
        
        [Range(0.0, 500.0)]
        public double Emulsion { get; set; } = 0.0;
    }
    
    public class SavePatternResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? PatternId { get; set; }
    }
}