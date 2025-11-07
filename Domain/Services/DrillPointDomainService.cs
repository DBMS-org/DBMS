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
                Diameter = p.Diameter,
                Stemming = p.Stemming,
                ProjectId = p.ProjectId,
                SiteId = p.SiteId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });
        }
        
        /// <summary>
        /// Calculates optimal stemming length based on hole depth and burden.
        /// Uses industry standard of 20-30% of hole depth, with minimum safety requirements.
        /// </summary>
        public double CalculateOptimalStemming(double depth, double burden)
        {
            // Industry standard: stemming should be 20-30% of hole depth
            var stemmingByDepth = depth * 0.25; // 25% as middle ground
            
            // Alternative calculation: stemming should be at least equal to burden
            var stemmingByBurden = burden;
            
            // Use the larger of the two calculations for safety
            var optimalStemming = Math.Max(stemmingByDepth, stemmingByBurden);
            
            // Apply practical constraints
            var minStemming = Math.Max(1.0, burden * 0.8); // Minimum 1m or 80% of burden
            var maxStemming = Math.Min(depth * 0.4, 8.0); // Maximum 40% of depth or 8m
            
            return Math.Round(Math.Max(minStemming, Math.Min(optimalStemming, maxStemming)), 1);
        }
        
        /// <summary>
        /// Calculates optimal hole diameter based on burden, spacing, and explosive requirements.
        /// Uses industry standards for blast hole diameter selection.
        /// </summary>
        public double CalculateOptimalDiameter(double burden, double spacing)
        {
            // Calculate pattern area per hole
            var patternArea = burden * spacing;
            
            // Industry rule: diameter should be approximately 1/30 to 1/40 of burden
            var diameterByBurden = burden / 35.0; // Use middle value
            
            // Alternative calculation based on pattern density
            var diameterByArea = Math.Sqrt(patternArea) / 20.0;
            
            // Use the larger of the two for adequate fragmentation
            var calculatedDiameter = Math.Max(diameterByBurden, diameterByArea);
            
            // Round to standard drill bit sizes (in meters)
            var standardSizes = new[] { 0.089, 0.102, 0.115, 0.127, 0.140, 0.152, 0.165, 0.178, 0.191, 0.203, 0.216, 0.229, 0.254, 0.279, 0.305 };
            
            // Find the closest standard size
            var optimalDiameter = standardSizes.OrderBy(size => Math.Abs(size - calculatedDiameter)).First();
            
            return optimalDiameter;
        }
        
        /// <summary>
        /// Calculates powder factor (kg of explosive per cubic meter of rock) based on pattern parameters.
        /// </summary>
        public double CalculatePowderFactor(double diameter, double depth, double stemming, double burden, double spacing, double explosiveDensity = 1.2)
        {
            // Calculate hole volume (cylindrical)
            var holeRadius = diameter / 2.0;
            var holeVolume = Math.PI * holeRadius * holeRadius * depth;
            
            // Calculate explosive column length (depth minus stemming)
            var explosiveLength = Math.Max(0, depth - stemming);
            
            // Calculate explosive volume
            var explosiveVolume = Math.PI * holeRadius * holeRadius * explosiveLength;
            
            // Calculate explosive weight (kg)
            var explosiveWeight = explosiveVolume * explosiveDensity;
            
            // Calculate rock volume per hole (burden × spacing × depth)
            var rockVolume = burden * spacing * depth;
            
            // Calculate powder factor (kg/m³)
            return rockVolume > 0 ? explosiveWeight / rockVolume : 0;
        }
        
        /// <summary>
        /// Validates that stemming length is appropriate for the given hole parameters.
        /// </summary>
        public bool ValidateStemming(double stemming, double depth, double burden)
        {
            if (stemming < 0 || depth <= 0 || burden <= 0) return false;
            
            // Stemming should not exceed 50% of hole depth
            if (stemming > depth * 0.5) return false;
            
            // Stemming should be at least 0.5m for safety
            if (stemming < 0.5) return false;
            
            // Stemming should not be less than 60% of burden for proper confinement
            if (stemming < burden * 0.6) return false;
            
            return true;
        }
        
        /// <summary>
        /// Validates that diameter is appropriate for the given pattern parameters.
        /// </summary>
        public bool ValidateDiameter(double diameter, double burden, double spacing)
        {
            if (diameter <= 0 || burden <= 0 || spacing <= 0) return false;
            
            // Diameter should not exceed 1/15 of burden (too large)
            if (diameter > burden / 15.0) return false;
            
            // Diameter should not be less than 1/50 of burden (too small)
            if (diameter < burden / 50.0) return false;
            
            // Practical limits: 50mm to 500mm
            if (diameter < 0.05 || diameter > 0.5) return false;
            
            return true;
        }
    }
}