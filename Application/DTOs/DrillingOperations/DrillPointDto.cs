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
    }
    
    public class PatternSettingsDto
    {
        [Range(0.1, 50.0)]
        public double Spacing { get; set; } = 3.0;
        
        [Range(0.1, 50.0)]
        public double Burden { get; set; } = 2.5;
        
        [Range(0.1, 100.0)]
        public double Depth { get; set; } = 10.0;
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
        public PatternSettingsDto Settings { get; set; } = new();
    }
    
    public class SavePatternResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? PatternId { get; set; }
    }
} 