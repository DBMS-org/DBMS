using Application.DTOs.BlastingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;
using Application.Interfaces.BlastingOperations;

namespace Application.Services.BlastingOperations
{
    /// <summary>
    /// Facade that keeps the original ISiteBlastingService contract intact while delegating the
    /// actual work to smaller, focused services.
    /// </summary>
    public class SiteBlastingApplicationService : ISiteBlastingService
    {
        private readonly ISiteBlastingDataService _dataService;
        private readonly IDrillPatternService _patternService;
        private readonly IBlastSequenceService _sequenceService;
        private readonly IWorkflowProgressService _workflowService;
        private readonly ISiteBlastingRepository _repository; // For direct validation helpers

        public SiteBlastingApplicationService(
            ISiteBlastingDataService dataService,
            IDrillPatternService patternService,
            IBlastSequenceService sequenceService,
            IWorkflowProgressService workflowService,
            ISiteBlastingRepository repository)
        {
            _dataService = dataService;
            _patternService = patternService;
            _sequenceService = sequenceService;
            _workflowService = workflowService;
            _repository = repository;
        }

        #region Site Blasting Data (JSON CRUD)

        public Task<SiteBlastingDataDto?> GetSiteDataAsync(int projectId, int siteId, string dataType) =>
            _dataService.GetSiteDataAsync(projectId, siteId, dataType);

        public Task<List<SiteBlastingDataDto>> GetAllSiteDataAsync(int projectId, int siteId) =>
            _dataService.GetAllSiteDataAsync(projectId, siteId);

        public Task<SiteBlastingDataDto> SaveSiteDataAsync(CreateSiteBlastingDataRequest request, int userId) =>
            _dataService.SaveSiteDataAsync(request, userId);

        public Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType) =>
            _dataService.DeleteSiteDataAsync(projectId, siteId, dataType);

        public Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId) =>
            _dataService.DeleteAllSiteDataAsync(projectId, siteId);

        public Task<bool> CleanupSiteDataAsync(CleanupSiteDataRequest request) =>
            _dataService.CleanupSiteDataAsync(request);

        public Task<bool> SaveBulkSiteDataAsync(BulkSiteDataRequest request, int userId) =>
            _dataService.SaveBulkSiteDataAsync(request, userId);

        public Task<Dictionary<string, SiteBlastingDataDto>> GetBulkSiteDataAsync(int projectId, int siteId) =>
            _dataService.GetBulkSiteDataAsync(projectId, siteId);

        #endregion

        #region Drill Pattern

        public Task<List<DrillPatternDto>> GetDrillPatternsAsync(int projectId, int siteId) =>
            _patternService.GetDrillPatternsAsync(projectId, siteId);

        public Task<DrillPatternDto?> GetDrillPatternAsync(int id) =>
            _patternService.GetDrillPatternAsync(id);

        public Task<DrillPatternDto> CreateDrillPatternAsync(CreateDrillPatternRequest request, int userId) =>
            _patternService.CreateDrillPatternAsync(request, userId);

        public Task<DrillPatternDto> UpdateDrillPatternAsync(int id, CreateDrillPatternRequest request, int userId) =>
            _patternService.UpdateDrillPatternAsync(id, request, userId);

        public Task<bool> DeleteDrillPatternAsync(int id) =>
            _patternService.DeleteDrillPatternAsync(id);

        #endregion

        #region Blast Sequence

        public Task<List<BlastSequenceDto>> GetBlastSequencesAsync(int projectId, int siteId) =>
            _sequenceService.GetBlastSequencesAsync(projectId, siteId);

        public Task<BlastSequenceDto?> GetBlastSequenceAsync(int id) =>
            _sequenceService.GetBlastSequenceAsync(id);

        public Task<BlastSequenceDto> CreateBlastSequenceAsync(CreateBlastSequenceRequest request, int userId) =>
            _sequenceService.CreateBlastSequenceAsync(request, userId);

        public Task<BlastSequenceDto> UpdateBlastSequenceAsync(int id, CreateBlastSequenceRequest request, int userId) =>
            _sequenceService.UpdateBlastSequenceAsync(id, request, userId);

        public Task<bool> DeleteBlastSequenceAsync(int id) =>
            _sequenceService.DeleteBlastSequenceAsync(id);

        #endregion

        #region Workflow Progress

        public Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId) =>
            _workflowService.GetWorkflowProgressAsync(projectId, siteId);

        public Task<SiteWorkflowProgressDto> UpdateWorkflowProgressAsync(int projectId, int siteId, string stepId, bool completed) =>
            _workflowService.UpdateWorkflowProgressAsync(projectId, siteId, stepId, completed);

        public Task<bool> SetOperatorCompletionAsync(int projectId, int siteId, bool completed) =>
            _workflowService.SetOperatorCompletionAsync(projectId, siteId, completed);

        #endregion

        #region Validation

        public Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId) =>
            _repository.ValidateProjectSiteExistsAsync(projectId, siteId);

        public Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId) =>
            _patternService.ValidateDrillPatternOwnershipAsync(patternId, projectId, siteId);

        public Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId) =>
            _sequenceService.ValidateBlastSequenceOwnershipAsync(sequenceId, projectId, siteId);

        #endregion
    }
}