namespace Application.Interfaces.BlastingOperations
{
    using Application.DTOs.DrillingOperations;

    public interface IDrillPatternService
    {
        Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId);
        Task<DrillPatternDto?> GetDrillPatternAsync(int id);
        Task<DrillPatternDto> CreateDrillPatternAsync(CreateDrillPatternRequest request, int userId);
        Task<DrillPatternDto> UpdateDrillPatternAsync(int id, CreateDrillPatternRequest request, int userId);
        Task<bool> DeleteDrillPatternAsync(int id);
        Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId);
    }
} 