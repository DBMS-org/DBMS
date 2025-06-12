namespace Core.Entities
{
    public class DrillHole
    {
        public int SerialNumber { get; set; }
        public string Id { get; set; } = string.Empty;
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
        
        // Project and Site context
        public int ProjectId { get; set; }
        public int SiteId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 