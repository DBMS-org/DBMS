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
        
        // Estimates grid spacing and burden using statistical analysis
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

            var sorted = coords.OrderBy(x => x).ToList();

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

            var frequency = new Dictionary<double, int>();
            foreach (var delta in deltas)
            {
                var bucket = Math.Round(delta * 10) / 10;
                frequency[bucket] = frequency.GetValueOrDefault(bucket, 0) + 1;
            }

            var topEntry = frequency.OrderByDescending(kv => kv.Value).First();
            var support = (double)topEntry.Value / deltas.Count;

            if (support >= GRID_PITCH_SUPPORT_THRESHOLD)
            {
                return topEntry.Key;
            }

            deltas.Sort();
            var median = deltas[deltas.Count / 2];
            return Math.Round(median * 10) / 10;
        }
        
        // Translates all points so minimum X,Y becomes 0,0
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
                Diameter = p.Diameter,
                Stemming = p.Stemming,
                ProjectId = p.ProjectId,
                SiteId = p.SiteId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }
        
        // Calculates optimal stemming (20-30% of depth, minimum equals burden)
        public double CalculateOptimalStemming(double depth, double burden)
        {
            var stemmingByDepth = depth * 0.25;
            var stemmingByBurden = burden;
            var optimalStemming = Math.Max(stemmingByDepth, stemmingByBurden);

            var minStemming = Math.Max(1.0, burden * 0.8);
            var maxStemming = Math.Min(depth * 0.4, 8.0);
            
            return Math.Round(Math.Max(minStemming, Math.Min(optimalStemming, maxStemming)), 1);
        }
        
        // Calculates optimal diameter based on burden and spacing (industry standard: 1/30 to 1/40 of burden)
        public double CalculateOptimalDiameter(double burden, double spacing)
        {
            var patternArea = burden * spacing;
            var diameterByBurden = burden / 35.0;
            var diameterByArea = Math.Sqrt(patternArea) / 20.0;
            var calculatedDiameter = Math.Max(diameterByBurden, diameterByArea);

            var standardSizes = new[] { 0.089, 0.102, 0.115, 0.127, 0.140, 0.152, 0.165, 0.178, 0.191, 0.203, 0.216, 0.229, 0.254, 0.279, 0.305 };
            var optimalDiameter = standardSizes.OrderBy(size => Math.Abs(size - calculatedDiameter)).First();
            
            return optimalDiameter;
        }
        
        // Calculates powder factor (kg explosive per m³ rock)
        public double CalculatePowderFactor(double diameter, double depth, double stemming, double burden, double spacing, double explosiveDensity = 1.2)
        {
            var holeRadius = diameter / 2.0;
            var holeVolume = Math.PI * holeRadius * holeRadius * depth;
            var explosiveLength = Math.Max(0, depth - stemming);
            var explosiveVolume = Math.PI * holeRadius * holeRadius * explosiveLength;
            var explosiveWeight = explosiveVolume * explosiveDensity;
            var rockVolume = burden * spacing * depth;

            return rockVolume > 0 ? explosiveWeight / rockVolume : 0;
        }
        
        // Validates stemming length (must be 0.5-50% of depth, min 60% of burden)
        public bool ValidateStemming(double stemming, double depth, double burden)
        {
            if (stemming < 0 || depth <= 0 || burden <= 0) return false;
            if (stemming > depth * 0.5) return false;
            if (stemming < 0.5) return false;
            if (stemming < burden * 0.6) return false;
            
            return true;
        }
        
        // Validates diameter (must be 1/50 to 1/15 of burden, between 50-500mm)
        public bool ValidateDiameter(double diameter, double burden, double spacing)
        {
            if (diameter <= 0 || burden <= 0 || spacing <= 0) return false;
            if (diameter > burden / 15.0) return false;
            if (diameter < burden / 50.0) return false;
            if (diameter < 0.05 || diameter > 0.5) return false;
            
            return true;
        }
    }
}