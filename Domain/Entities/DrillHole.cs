namespace Domain.Entities
{
    public class DrillHole : BaseEntity
    {
        public int SerialNumber { get; set; }
        public string DrillHoleId { get; set; } = string.Empty; // Renamed from Id to avoid conflict
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
        
        // Navigation properties (previously missing)
        public virtual Project Project { get; set; } = null!;
        public virtual ProjectSite Site { get; set; } = null!;
        
        // Business logic methods
        public bool IsCompleted() => ActualDepth > 0;
        
        public double GetCompletionPercentage()
        {
            if (Depth <= 0) return 0;
            return Math.Min((ActualDepth / Depth) * 100, 100);
        }
        
        public bool IsWithinTolerance(double tolerance = 0.5)
        {
            return Math.Abs(ActualDepth - Depth) <= tolerance;
        }
        
        public void UpdateActualDepth(double actualDepth)
        {
            ActualDepth = actualDepth;
            UpdateTimestamp();
        }
    }
} 
