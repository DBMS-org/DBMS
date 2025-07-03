namespace Application.Interfaces.BlastingOperations
{
    using Application.DTOs.BlastingOperations;
    using Application.DTOs.Shared;
    using System.Threading;

    public interface ISiteBlastingDataService
    {
        Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType);
        Task<List<SiteBlastingDataDto>> GetAllSiteDataAsync(int projectId, int siteId);
        Task<SiteBlastingDataDto> SaveSiteDataAsync(CreateSiteBlastingDataRequest request, int userId);
        Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType);
        Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId);
        Task<bool> CleanupSiteDataAsync(CleanupSiteDataRequest request);
        Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId, CancellationToken cancellationToken = default);
        Task<Dictionary<string, SiteBlastingDataDto>> GetBulkSiteDataAsync(int projectId, int siteId);
        Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId);
    }
} 