using Application.DTOs.BlastingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface ISiteBlastingService
    {
        // Site Data Management
        Task<bool> DeleteAllWorkflowDataAsync(int projectId, int siteId);
        Task<bool> HasWorkflowDataAsync(int projectId, int siteId);
        
        // Blast Sequences
        Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId);
        Task<BlastSequenceDto?> GetBlastSequenceAsync(int id);
        Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId);
        Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, UpdateBlastSequenceRequest request, int userId);
        Task<bool> DeleteBlastSequenceAsync(int id);
        
        // Validation
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
        Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId);
    }
} 
