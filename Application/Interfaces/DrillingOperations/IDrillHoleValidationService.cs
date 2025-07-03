using Domain.Entities.DrillingOperations;

namespace Application.Interfaces.DrillingOperations
{
    public interface IDrillHoleValidationService
    {
        void ValidateDrillHole(DrillHole drillHole);
        bool IsValidCoordinate(double coordinate);
        bool IsValidDepth(double depth);
        bool IsValidAzimuth(double? azimuth);
        bool IsValidDip(double? dip);
    }
} 