using Application.DTOs.BlastingOperations;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.BlastingOperations
{
    public class SiteBlastingApplicationService : ISiteBlastingService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<SiteBlastingApplicationService> _logger;

        public SiteBlastingApplicationService(
            ISiteBlastingRepository repository,
            ILogger<SiteBlastingApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> DeleteAllWorkflowDataAsync(int projectId, int siteId)
        {
            _logger.LogInformation("Deleting all workflow data for project {ProjectId}, site {SiteId}", projectId, siteId);
            return await _repository.DeleteAllWorkflowDataAsync(projectId, siteId);
        }

        public async Task<bool> HasWorkflowDataAsync(int projectId, int siteId)
        {
            return await _repository.HasWorkflowDataAsync(projectId, siteId);
        }

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            return await _repository.ValidateProjectSiteExistsAsync(projectId, siteId);
        }
    }
}