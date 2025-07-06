using Domain.Entities.BlastingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface IBlastConnectionRepository
    {
        Task<IEnumerable<BlastConnection>> GetByProjectAndSiteAsync(int projectId, int siteId);
        Task<BlastConnection?> GetByIdAsync(string id, int projectId, int siteId);
        Task<IEnumerable<BlastConnection>> GetBySequenceAsync(int projectId, int siteId, int sequence);
        Task<BlastConnection> CreateAsync(BlastConnection blastConnection);
        Task<BlastConnection> UpdateAsync(BlastConnection blastConnection);
        Task DeleteAsync(string id, int projectId, int siteId);
        Task<bool> ExistsAsync(string id, int projectId, int siteId);
    }
} 