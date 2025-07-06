using Domain.Entities.BlastingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface ISiteBlastingRepository
    {
        // Site Data Management
        Task<bool> DeleteAllWorkflowDataAsync(int projectId, int siteId);
        Task<bool> HasWorkflowDataAsync(int projectId, int siteId);
        
        // Blast Sequences
        Task<List<BlastSequence>> GetBlastSequencesAsync(int projectId, int siteId);
        Task<BlastSequence?> GetBlastSequenceAsync(int id);
        Task<BlastSequence> CreateBlastSequenceAsync(BlastSequence sequence);
        Task<BlastSequence> UpdateBlastSequenceAsync(BlastSequence sequence);
        Task<bool> DeleteBlastSequenceAsync(int id);
        
        // Validation
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
        Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId);
    }
} 