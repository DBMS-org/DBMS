namespace Core.Entities
{
    public class DrillHole
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Elevation { get; set; }
        public double Depth { get; set; }
        public double Azimuth { get; set; }
        public double Dip { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 