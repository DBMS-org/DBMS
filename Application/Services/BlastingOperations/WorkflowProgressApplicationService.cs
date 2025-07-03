using System.Text.Json;
using Application.DTOs.Shared;
using Application.Interfaces.BlastingOperations;
using Application.Utilities;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.BlastingOperations
{
    public class WorkflowProgressApplicationService : IWorkflowProgressService
    {
        private readonly ISiteBlastingRepository _repository;
        private readonly ILogger<WorkflowProgressApplicationService> _logger;

        public WorkflowProgressApplicationService(ISiteBlastingRepository repository, ILogger<WorkflowProgressApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SiteWorkflowProgressDto> GetWorkflowProgressAsync(int projectId, int siteId)
        {
            try
            {
                var data = await _repository.GetWorkflowProgressDataAsync(projectId, siteId);
                if (data?.JsonData != null)
                {
                    var dto = JsonSerializer.Deserialize<SiteWorkflowProgressDto>(data.JsonData);
                    if (dto != null) return dto;
                }

                // default
                return new SiteWorkflowProgressDto
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    SiteName = string.Empty,
                    PatternCreator = new WorkflowStepDto { Completed = false, Progress = 0 },
                    SequenceDesigner = new WorkflowStepDto { Completed = false, Progress = 0 },
                    Simulator = new WorkflowStepDto { Completed = false, Progress = 0 },
                    OverallProgress = 0,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow progress for project {ProjectId}, site {SiteId}", projectId, siteId);
                throw;
            }
        }

        public async Task<SiteWorkflowProgressDto> UpdateWorkflowProgressAsync(int projectId, int siteId, string stepId, bool completed)
        {
            try
            {
                var current = await GetWorkflowProgressAsync(projectId, siteId);

                switch (SafeDataConverter.SafeToLower(stepId))
                {
                    case "pattern_creator":
                    case "drill_pattern":
                        current.PatternCreator.Completed = completed;
                        current.PatternCreator.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                    case "sequence_designer":
                    case "blast_sequence":
                        current.SequenceDesigner.Completed = completed;
                        current.SequenceDesigner.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                    case "simulator":
                    case "operator_completion":
                        current.Simulator.Completed = completed;
                        current.Simulator.LastModified = completed ? DateTime.UtcNow : null;
                        break;
                }

                var entity = new SiteBlastingData
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    DataType = "workflow_progress",
                    JsonData = JsonSerializer.Serialize(current),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.SaveWorkflowProgressDataAsync(entity);
                return current;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow progress for project {ProjectId}, site {SiteId}, step {StepId}", projectId, siteId, stepId);
                throw;
            }
        }

        public async Task<bool> SetOperatorCompletionAsync(int projectId, int siteId, bool completed)
        {
            await UpdateWorkflowProgressAsync(projectId, siteId, "operator_completion", completed);
            return true;
        }
    }
} 