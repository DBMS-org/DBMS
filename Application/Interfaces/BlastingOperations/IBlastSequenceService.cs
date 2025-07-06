namespace Application.Interfaces.BlastingOperations
{
    using Application.DTOs.BlastingOperations;

    public interface IBlastSequenceService
    {
        Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId);
        Task<BlastSequenceDto?> GetBlastSequenceAsync(int id);
        Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId);
        Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, UpdateBlastSequenceRequest request, int userId);
        Task<bool> DeleteBlastSequenceAsync(int id);
        Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId);
    }
} 