using Domain.Entities.DrillingOperations;

namespace Domain.Services
{
    public class DrillPointDomainService
    {
        private const double COORDINATE_PRECISION = 0.01; // meters
        private const double GRID_PITCH_MIN_THRESHOLD = 0.5; // meters
        private const double GRID_PITCH_SUPPORT_THRESHOLD = 0.10; // 10%
        
        public bool ValidateCoordinates(double x, double y)
        {
            return !double.IsNaN(x) && !double.IsNaN(y) && 
                   !double.IsInfinity(x) && !double.IsInfinity(y);
        }
        
        public bool ValidateUniqueCoordinates(double x, double y, IEnumerable<DrillPoint> existingPoints)
        {
            return !existingPoints.Any(point => 
                Math.Abs(point.X - x) < COORDINATE_PRECISION && 
                Math.Abs(point.Y - y) < COORDINATE_PRECISION);
        }
        
        public bool ValidateDrillPointCount(int currentCount, int maxPoints)
        {
            return currentCount < maxPoints;
        }
        
        public double GetDistance(DrillPoint a, DrillPoint b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        
        /// <summary>
        /// Robustly estimates grid pitch (spacing & burden) using statistical analysis.
        /// </summary>
        public (double spacing, double burden) CalculateGridPitch(IEnumerable<DrillPoint> drillPoints)
        {
            var points = drillPoints.ToList();
            
            var spacing = EstimatePitch(points.Select(p => p.X));
            var burden = EstimatePitch(points.Select(p => p.Y));
            
            return (spacing, burden);
        }
        
        private double EstimatePitch(IEnumerable<double> coordinates)
        {
            var coords = coordinates.ToList();
            if (coords.Count < 2) return 1.0;
            
            // Sort coordinates ascending
            var sorted = coords.OrderBy(x => x).ToList();
            
            // Build list of deltas, filter out sub-threshold jitter
            var deltas = new List<double>();
            for (int i = 1; i < sorted.Count; i++)
            {
                var delta = Math.Round(sorted[i] - sorted[i - 1], 3);
                if (delta >= GRID_PITCH_MIN_THRESHOLD)
                {
                    deltas.Add(delta);
                }
            }
            
            if (deltas.Count == 0) return 1.0;
            
            // Bucket to 0.1 m resolution and build histogram
            var frequency = new Dictionary<double, int>();
            foreach (var delta in deltas)
            {
                var bucket = Math.Round(delta * 10) / 10;
                frequency[bucket] = frequency.GetValueOrDefault(bucket, 0) + 1;
            }
            
            // Determine mode and its support percentage
            var topEntry = frequency.OrderByDescending(kv => kv.Value).First();
            var support = (double)topEntry.Value / deltas.Count;
            
            if (support >= GRID_PITCH_SUPPORT_THRESHOLD)
            {
                return topEntry.Key;
            }
            
            // Fallback: median of deltas rounded to 0.1 m
            deltas.Sort();
            var median = deltas[deltas.Count / 2];
            return Math.Round(median * 10) / 10;
        }
        
        /// <summary>
        /// Anchors points to origin by translating all points so minimum X,Y becomes 0,0.
        /// </summary>
        public IEnumerable<DrillPoint> AnchorPointsToOrigin(IEnumerable<DrillPoint> points)
        {
            var pointsList = points.ToList();
            if (!pointsList.Any()) return pointsList;
            
            var minX = pointsList.Min(p => p.X);
            var minY = pointsList.Min(p => p.Y);
            
            return pointsList.Select(p => new DrillPoint
            {
                Id = p.Id,
                X = Math.Round(p.X - minX, 2),
                Y = Math.Round(p.Y - minY, 2),
                Depth = p.Depth,
                Spacing = p.Spacing,
                Burden = p.Burden,
                ProjectId = p.ProjectId,
                SiteId = p.SiteId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }
    }
} 