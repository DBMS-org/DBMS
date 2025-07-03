using Domain.Entities.DrillingOperations;
using Application.Interfaces.DrillingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.DrillingOperations
{
    public class DrillHoleValidationService : IDrillHoleValidationService
    {
        private readonly ILogger<DrillHoleValidationService> _logger;

        public DrillHoleValidationService(ILogger<DrillHoleValidationService> logger)
        {
            _logger = logger;
        }

        public void ValidateDrillHole(DrillHole drillHole)
        {
            if (drillHole == null)
            {
                _logger.LogError("Validation failed: DrillHole is null");
                throw new ArgumentNullException(nameof(drillHole));
            }

            _logger.LogDebug("Validating drill hole with ID: {DrillHoleId}", drillHole.Id);

            if (string.IsNullOrWhiteSpace(drillHole.Id))
            {
                _logger.LogError("Validation failed: DrillHole ID cannot be null or empty");
                throw new ArgumentException("DrillHole ID cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(drillHole.Name))
            {
                _logger.LogInformation("DrillHole name is empty, using ID as name for drill hole: {DrillHoleId}", drillHole.Id);
                drillHole.Name = drillHole.Id; // Use ID as name if name is empty
            }

            if (!IsValidDepth(drillHole.Depth))
            {
                _logger.LogError("Validation failed: DrillHole Depth cannot be negative for drill hole: {DrillHoleId}", drillHole.Id);
                throw new ArgumentException("DrillHole Depth cannot be negative");
            }

            if (!IsValidDepth(drillHole.Length))
            {
                _logger.LogError("Validation failed: DrillHole Length cannot be negative for drill hole: {DrillHoleId}", drillHole.Id);
                throw new ArgumentException("DrillHole Length cannot be negative");
            }

            if (!IsValidDepth(drillHole.ActualDepth))
            {
                _logger.LogError("Validation failed: DrillHole Actual Depth cannot be negative for drill hole: {DrillHoleId}", drillHole.Id);
                throw new ArgumentException("DrillHole Actual Depth cannot be negative");
            }

            if (!IsValidDepth(drillHole.Stemming))
            {
                _logger.LogError("Validation failed: DrillHole Stemming cannot be negative for drill hole: {DrillHoleId}", drillHole.Id);
                throw new ArgumentException("DrillHole Stemming cannot be negative");
            }

            // More lenient validation for blast hole data - handle nullable values
            if (!IsValidAzimuth(drillHole.Azimuth))
            {
                _logger.LogWarning("Invalid azimuth value {Azimuth} for drill hole {DrillHoleId}, clamping to valid range", drillHole.Azimuth, drillHole.Id);
                drillHole.Azimuth = ClampAzimuth(drillHole.Azimuth);
            }

            if (!IsValidDip(drillHole.Dip))
            {
                _logger.LogWarning("Invalid dip value {Dip} for drill hole {DrillHoleId}, clamping to valid range", drillHole.Dip, drillHole.Id);
                drillHole.Dip = ClampDip(drillHole.Dip);
            }

            _logger.LogDebug("Validation completed successfully for drill hole: {DrillHoleId}", drillHole.Id);
        }

        public bool IsValidCoordinate(double coordinate)
        {
            // Basic coordinate validation - could be extended with more specific rules
            return !double.IsNaN(coordinate) && !double.IsInfinity(coordinate);
        }

        public bool IsValidDepth(double depth)
        {
            return depth >= 0 && !double.IsNaN(depth) && !double.IsInfinity(depth);
        }

        public bool IsValidAzimuth(double? azimuth)
        {
            if (!azimuth.HasValue)
                return true; // Nullable azimuth is valid

            return azimuth.Value >= 0 && azimuth.Value <= 360 && 
                   !double.IsNaN(azimuth.Value) && !double.IsInfinity(azimuth.Value);
        }

        public bool IsValidDip(double? dip)
        {
            if (!dip.HasValue)
                return true; // Nullable dip is valid

            return dip.Value >= -90 && dip.Value <= 90 && 
                   !double.IsNaN(dip.Value) && !double.IsInfinity(dip.Value);
        }

        private static double? ClampAzimuth(double? azimuth)
        {
            if (!azimuth.HasValue)
                return null;

            if (double.IsNaN(azimuth.Value) || double.IsInfinity(azimuth.Value))
                return null;

            return Math.Max(0, Math.Min(360, azimuth.Value));
        }

        private static double? ClampDip(double? dip)
        {
            if (!dip.HasValue)
                return null;

            if (double.IsNaN(dip.Value) || double.IsInfinity(dip.Value))
                return null;

            return Math.Max(-90, Math.Min(90, dip.Value));
        }
    }
} 