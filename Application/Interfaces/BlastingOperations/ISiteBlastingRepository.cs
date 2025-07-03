using Domain.Entities.BlastingOperations;
using Domain.Entities.DrillingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface ISiteBlastingRepository
    {
        // Site Blasting Data Operations
        Task<SiteBlastingData?> GetSiteDataAsync(int projectId, int siteId, string dataType);
        Task<List<SiteBlastingData>> GetAllSiteDataAsync(int projectId, int siteId);
        Task<SiteBlastingData> SaveSiteDataAsync(SiteBlastingData data);
        Task<SiteBlastingData> UpdateSiteDataAsync(SiteBlastingData data);
        Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType);
        Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId);
        
        // Drill Pattern Operations
        Task<List<DrillPattern>> GetDrillPatternsAsync(int projectId, int siteId);
        Task<DrillPattern?> GetDrillPatternAsync(int id);
        Task<DrillPattern> CreateDrillPatternAsync(DrillPattern pattern);
        Task<DrillPattern> UpdateDrillPatternAsync(DrillPattern pattern);
        Task<bool> DeleteDrillPatternAsync(int id);
        
        // Blast Sequence Operations
        Task<List<BlastSequence>> GetBlastSequencesAsync(int projectId, int siteId);
        Task<BlastSequence?> GetBlastSequenceAsync(int id);
        Task<BlastSequence> CreateBlastSequenceAsync(BlastSequence sequence);
        Task<BlastSequence> UpdateBlastSequenceAsync(BlastSequence sequence);
        Task<bool> DeleteBlastSequenceAsync(int id);
        
        // Validation Operations
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
        Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId);
        Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId);
        
        // Workflow Operations
        Task<SiteBlastingData?> GetWorkflowProgressDataAsync(int projectId, int siteId);
        Task<SiteBlastingData> SaveWorkflowProgressDataAsync(SiteBlastingData data);
    }
} 