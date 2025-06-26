using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class DrillHoleDto
    {
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public string DrillHoleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        // Spatial coordinates
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Elevation { get; set; }
        public double Length { get; set; }
        public double Depth { get; set; }
        public double Azimuth { get; set; }
        public double Dip { get; set; }
        public double ActualDepth { get; set; }
        public double Stemming { get; set; }
        
        // Foreign keys
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        // Navigation property values
        public string? ProjectName { get; set; }
        public string? SiteName { get; set; }
        
        // Base entity properties
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Business logic convenience properties
        public bool IsCompleted => ActualDepth > 0;
        public double CompletionPercentage => Depth <= 0 ? 0 : Math.Min((ActualDepth / Depth) * 100, 100);
        public bool IsWithinTolerance => Math.Abs(ActualDepth - Depth) <= 0.5;
    }
    
    public class CreateDrillHoleRequest
    {
        public int SerialNumber { get; set; }
        public string DrillHoleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Elevation { get; set; }
        public double Length { get; set; }
        public double Depth { get; set; }
        public double Azimuth { get; set; }
        public double Dip { get; set; }
        public double ActualDepth { get; set; }
        public double Stemming { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int SiteId { get; set; }
    }
    
    public class UpdateDrillHoleRequest
    {
        [Required]
        public int Id { get; set; }
        public int SerialNumber { get; set; }
        public string DrillHoleId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Elevation { get; set; }
        public double Length { get; set; }
        public double Depth { get; set; }
        public double Azimuth { get; set; }
        public double Dip { get; set; }
        public double ActualDepth { get; set; }
        public double Stemming { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int SiteId { get; set; }
    }
} 