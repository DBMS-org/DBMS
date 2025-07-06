using Application.DTOs.Shared;
using Domain.Entities.BlastingOperations;

namespace Application.Interfaces.BlastingOperations
{
    public interface IBlastConnectionService
    {
        Task<Result<IEnumerable<BlastConnection>>> GetConnectionsByProjectAndSiteAsync(int projectId, int siteId);
        Task<Result<BlastConnection>> GetConnectionByIdAsync(string id, int projectId, int siteId);
        Task<Result<IEnumerable<BlastConnection>>> GetConnectionsBySequenceAsync(int projectId, int siteId, int sequence);
        Task<Result<BlastConnection>> CreateConnectionAsync(BlastConnection blastConnection);
        Task<Result<BlastConnection>> UpdateConnectionAsync(BlastConnection blastConnection);
        Task<Result> DeleteConnectionAsync(string id, int projectId, int siteId);
        Task<Result<bool>> ConnectionExistsAsync(string id, int projectId, int siteId);
    }
} 