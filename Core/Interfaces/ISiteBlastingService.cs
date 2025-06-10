using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ISiteBlastingService
    {
        // Site blasting data operations (generic JSON storage)
        Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType);
        Task<List<SiteBlastingDataDto>> GetAllSiteDataAsync(int projectId, int siteId);
        Task<SiteBlastingDataDto> SaveSiteDataAsync(CreateSiteBlastingDataRequest request, int userId);
        Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType);
        Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId);
        Task<bool> CleanupSiteDataAsync(CleanupSiteDataRequest request);
        
        // Bulk operations
        Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId);
        Task<Dictionary<string, SiteBlastingDataDto>> GetBulkSiteDataAsync(int projectId, int siteId);
        
        // Drill pattern operations (structured storage)
        Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId);
        Task<DrillPatternDto?> GetDrillPatternAsync(int id);
        Task<DrillPatternDto> CreateDrillPatternAsync(CreateDrillPatternRequest request, int userId);
        Task<DrillPatternDto> UpdateDrillPatternAsync(int id, CreateDrillPatternRequest request, int userId);
        Task<bool> DeleteDrillPatternAsync(int id);
        
        // Blast sequence operations (structured storage)
        Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId);
        Task<BlastSequenceDto?> GetBlastSequenceAsync(int id);
        Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId);
        Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, CreateBlastSequenceRequest request, int userId);
        Task<bool> DeleteBlastSequenceAsync(int id);
        
        // Workflow progress tracking
        Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId);
        Task<SiteWorkflowProgressDto> UpdateWorkflowProgressAsync(int projectId, int siteId, string stepId, bool completed);
        
        // Validation methods
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
        Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId);
        Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId);
    }
} 