using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.DrillingOperations
{
    public class DrillHoleDto
    {
        public int? SerialNumber { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Elevation { get; set; }
        public double Length { get; set; }
        public double Depth { get; set; }

        // Nullable for 2D/3D compatibility
        public double? Azimuth { get; set; }
        public double? Dip { get; set; }

        public double ActualDepth { get; set; }
        public double Stemming { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int SiteId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool Has3DData => Azimuth.HasValue && Dip.HasValue;
        public bool RequiresFallbackTo2D => !Has3DData;
    }

    public class CreateDrillHoleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public double Easting { get; set; }
        
        [Required]
        public double Northing { get; set; }
        
        [Required]
        public double Elevation { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public double Depth { get; set; }
        
        public double? Azimuth { get; set; }
        public double? Dip { get; set; }
        
        [Required]
        public double ActualDepth { get; set; }
        
        [Required]
        public double Stemming { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        public int SiteId { get; set; }
    }

    public class UpdateDrillHoleRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public double Easting { get; set; }
        
        [Required]
        public double Northing { get; set; }
        
        [Required]
        public double Elevation { get; set; }
        
        [Required]
        public double Length { get; set; }
        
        [Required]
        public double Depth { get; set; }
        
        public double? Azimuth { get; set; }
        public double? Dip { get; set; }
        
        [Required]
        public double ActualDepth { get; set; }
        
        [Required]
        public double Stemming { get; set; }
    }
}