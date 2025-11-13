using Application.DTOs.MaintenanceOperations;
using Application.DTOs.Shared;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.UserManagement;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services.MaintenanceOperations
{
    public class MaintenanceJobApplicationService : IMaintenanceJobService
    {
        private readonly IMaintenanceJobRepository _jobRepository;
        private readonly IMaintenanceReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMappingService _mappingService;
        private readonly ILogger<MaintenanceJobApplicationService> _logger;
        private readonly IStatusSynchronizationService _statusSyncService;

        public MaintenanceJobApplicationService(
            IMaintenanceJobRepository jobRepository,
            IMaintenanceReportRepository reportRepository,
            IUserRepository userRepository,
            IMappingService mappingService,
            ILogger<MaintenanceJobApplicationService> logger,
            IStatusSynchronizationService statusSyncService)
        {
            _jobRepository = jobRepository;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _mappingService = mappingService;
            _logger = logger;
            _statusSyncService = statusSyncService;
        }

        public async Task<Result<MaintenanceJobDto>> CreateJobFromReportAsync(int reportId)
        {
            try
            {
                _logger.LogInformation("Creating maintenance job from report {ReportId}", reportId);

                // Get report with machine details
                var report = await _reportRepository.GetWithDetailsAsync(reportId);
                if (report == null)
                {
                    return Result.Failure<MaintenanceJobDto>("Report not found");
                }

                // Determine maintenance type based on severity
                var maintenanceType = report.Severity switch
                {
                    SeverityLevel.Critical => MaintenanceType.Emergency,
                    SeverityLevel.High => MaintenanceType.Emergency,
                    _ => MaintenanceType.Corrective
                };

                // Estimate hours based on problem category
                var estimatedHours = EstimateHours(report.ProblemCategory, report.Severity);

                // Create the job
                var job = MaintenanceJob.CreateFromReport(
                    report,
                    report.MachineId,
                    report.Machine?.ProjectId,
                    report.OperatorId, // Created by operator
                    estimatedHours
                );

                var createdJob = await _jobRepository.CreateAsync(job);

                _logger.LogInformation("Maintenance job {JobId} created from report {ReportId}", createdJob.Id, reportId);

                // Auto-assign to engineer using round-robin algorithm
                if (report.Machine?.RegionId != null)
                {
                    await AutoAssignJobAsync(createdJob.Id, report.Machine.RegionId.Value, report.Severity);

                    // Acknowledge the report
                    var updatedJob = await _jobRepository.GetWithDetailsAsync(createdJob.Id);
                    if (updatedJob?.Assignments.Any() == true)
                    {
                        var assignment = updatedJob.Assignments.First();
                        report.Acknowledge(
                            assignment.MechanicalEngineerId,
                            assignment.MechanicalEngineer?.Name ?? "Engineer",
                            CalculateEstimatedResponseTime(report.Severity)
                        );
                        await _reportRepository.UpdateAsync(report);
                    }
                }

                // Return job with details
                var jobWithDetails = await _jobRepository.GetWithDetailsAsync(createdJob.Id);
                var jobDto = _mappingService.Map<MaintenanceJobDto>(jobWithDetails);
                return Result<MaintenanceJobDto>.Success(jobDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating job from report {ReportId}", reportId);
                return Result.Failure<MaintenanceJobDto>($"Failed to create job: {ex.Message}");
            }
        }

        private async Task AutoAssignJobAsync(int jobId, int regionId, SeverityLevel severity)
        {
            try
            {
                _logger.LogInformation("Auto-assigning job {JobId} in region {RegionId}", jobId, regionId);

                // Get all active mechanical engineers in the region
                var allEngineers = await _userRepository.GetAllAsync();
                var regionalEngineers = allEngineers
                    .Where(u => u.Role == "MechanicalEngineer" && u.Status.ToString() == "Active")
                    .ToList();

                if (!regionalEngineers.Any())
                {
                    _logger.LogWarning("No mechanical engineers found in region {RegionId}", regionId);
                    return;
                }

                // Get workload for each engineer
                var workload = await _jobRepository.GetEngineerWorkloadByRegionAsync(regionId);

                // Calculate workload score for each engineer
                var engineerScores = new List<(int EngineerId, int Score, DateTime? LastAssigned)>();

                foreach (var engineer in regionalEngineers)
                {
                    var activeJobs = workload.ContainsKey(engineer.Id) ? workload[engineer.Id] : 0;

                    // Score based on number of active jobs
                    // In a more sophisticated version, we could weight by severity
                    var score = activeJobs;

                    engineerScores.Add((engineer.Id, score, null));
                }

                // Find engineer with lowest workload
                var selectedEngineer = engineerScores.OrderBy(e => e.Score).First();

                // Assign the job
                var job = await _jobRepository.GetByIdAsync(jobId);
                if (job != null)
                {
                    job.AssignEngineer(selectedEngineer.EngineerId);
                    await _jobRepository.UpdateAsync(job);

                    _logger.LogInformation("Job {JobId} assigned to engineer {EngineerId}", jobId, selectedEngineer.EngineerId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error auto-assigning job {JobId}", jobId);
                // Don't throw - job creation should succeed even if assignment fails
            }
        }

        private decimal EstimateHours(ProblemCategory category, SeverityLevel severity)
        {
            // Base hours by category
            var baseHours = category switch
            {
                ProblemCategory.DrillBitIssues => 2m,
                ProblemCategory.DrillRodProblems => 3m,
                ProblemCategory.EngineIssues => 8m,
                ProblemCategory.HydraulicProblems => 6m,
                ProblemCategory.ElectricalFaults => 5m,
                ProblemCategory.MechanicalBreakdown => 10m,
                _ => 4m
            };

            // Adjust by severity
            var multiplier = severity switch
            {
                SeverityLevel.Critical => 1.5m,
                SeverityLevel.High => 1.2m,
                SeverityLevel.Medium => 1.0m,
                SeverityLevel.Low => 0.8m,
                _ => 1.0m
            };

            return baseHours * multiplier;
        }

        private string CalculateEstimatedResponseTime(SeverityLevel severity)
        {
            return severity switch
            {
                SeverityLevel.Critical => "Within 2 hours",
                SeverityLevel.High => "Within 4 hours",
                SeverityLevel.Medium => "Within 24 hours",
                SeverityLevel.Low => "Within 72 hours",
                _ => "TBD"
            };
        }

        public async Task<Result<MaintenanceJobDto>> CreateManualJobAsync(CreateManualJobRequest request, int createdBy)
        {
            try
            {
                var maintenanceType = Enum.Parse<MaintenanceType>(request.Type);

                var job = MaintenanceJob.Create(
                    request.MachineId,
                    request.ProjectId,
                    maintenanceType,
                    request.Reason,
                    request.EstimatedHours,
                    createdBy,
                    scheduledDate: request.ScheduledDate
                );

                var createdJob = await _jobRepository.CreateAsync(job);

                if (request.AssignedEngineerId.HasValue)
                {
                    createdJob.AssignEngineer(request.AssignedEngineerId.Value);
                    await _jobRepository.UpdateAsync(createdJob);
                }

                var jobWithDetails = await _jobRepository.GetWithDetailsAsync(createdJob.Id);
                var jobDto = _mappingService.Map<MaintenanceJobDto>(jobWithDetails);
                return Result<MaintenanceJobDto>.Success(jobDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating manual job");
                return Result.Failure<MaintenanceJobDto>($"Failed to create job: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceJobDto>> GetJobByIdAsync(int id)
        {
            try
            {
                var job = await _jobRepository.GetWithDetailsAsync(id);
                if (job == null)
                {
                    return Result.Failure<MaintenanceJobDto>("Job not found");
                }

                var dto = _mappingService.Map<MaintenanceJobDto>(job);
                return Result<MaintenanceJobDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting job {JobId}", id);
                return Result.Failure<MaintenanceJobDto>($"Failed to get job: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<MaintenanceJobDto>>> GetEngineerJobsAsync(int engineerId, int? regionId = null)
        {
            try
            {
                IEnumerable<MaintenanceJob> jobs;

                if (regionId.HasValue)
                {
                    jobs = await _jobRepository.GetByEngineerAndRegionAsync(engineerId, regionId.Value);
                }
                else
                {
                    jobs = await _jobRepository.GetByEngineerIdAsync(engineerId);
                }

                var dtos = _mappingService.Map<IEnumerable<MaintenanceJobDto>>(jobs);
                return Result<IEnumerable<MaintenanceJobDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jobs for engineer {EngineerId}", engineerId);
                return Result.Failure<IEnumerable<MaintenanceJobDto>>($"Failed to get jobs: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceJobDto>> UpdateJobStatusAsync(int id, UpdateJobStatusRequest request)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    return Result.Failure<MaintenanceJobDto>("Job not found");
                }

                var newStatus = Enum.Parse<MaintenanceJobStatus>(request.Status);

                if (newStatus == MaintenanceJobStatus.InProgress)
                {
                    // Only use Start() if job is currently Scheduled
                    if (job.Status == MaintenanceJobStatus.Scheduled)
                    {
                        job.Start();
                    }
                    else if (job.Status != MaintenanceJobStatus.InProgress)
                    {
                        // Allow transition to InProgress from other statuses (except if already InProgress)
                        job.UpdateStatus(newStatus);
                    }
                    // If already InProgress, no action needed
                }
                else
                {
                    job.UpdateStatus(newStatus);
                }

                await _jobRepository.UpdateAsync(job);

                // Update linked report status
                if (job.MaintenanceReportId.HasValue)
                {
                    var report = await _reportRepository.GetByIdAsync(job.MaintenanceReportId.Value);
                    if (report != null)
                    {
                        if (newStatus == MaintenanceJobStatus.InProgress)
                        {
                            report.MarkInProgress();
                        }
                        await _reportRepository.UpdateAsync(report);
                    }
                }

                var jobWithDetails = await _jobRepository.GetWithDetailsAsync(id);
                var dto = _mappingService.Map<MaintenanceJobDto>(jobWithDetails);
                return Result<MaintenanceJobDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job status {JobId}", id);
                return Result.Failure<MaintenanceJobDto>($"Failed to update status: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceJobDto>> CompleteJobAsync(int id, CompleteMaintenanceJobRequest request)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(id);
                if (job == null)
                {
                    return Result.Failure<MaintenanceJobDto>("Job not found");
                }

                var partsJson = request.PartsReplaced != null ? JsonSerializer.Serialize(request.PartsReplaced) : null;

                job.Complete(request.Observations, request.ActualHours, partsJson);
                await _jobRepository.UpdateAsync(job);

                // Update linked report to resolved
                if (job.MaintenanceReportId.HasValue)
                {
                    var report = await _reportRepository.GetByIdAsync(job.MaintenanceReportId.Value);
                    if (report != null)
                    {
                        report.Resolve(request.Observations);
                        await _reportRepository.UpdateAsync(report);
                    }
                }

                // Handle job completion (update machine status, maintenance date, etc.)
                await _statusSyncService.HandleJobCompletionAsync(id);

                var jobWithDetails = await _jobRepository.GetWithDetailsAsync(id);
                var dto = _mappingService.Map<MaintenanceJobDto>(jobWithDetails);
                return Result<MaintenanceJobDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing job {JobId}", id);
                return Result.Failure<MaintenanceJobDto>($"Failed to complete job: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceStatsDto>> GetMaintenanceStatsAsync(int? regionId = null)
        {
            try
            {
                // Implementation would query various statistics
                var stats = new MaintenanceStatsDto();
                return Result<MaintenanceStatsDto>.Success(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance stats");
                return Result.Failure<MaintenanceStatsDto>($"Failed to get stats: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<MaintenanceJobDto>>> GetOverdueJobsAsync(int? regionId = null)
        {
            try
            {
                IEnumerable<MaintenanceJob> jobs;

                if (regionId.HasValue)
                {
                    jobs = await _jobRepository.GetOverdueJobsByRegionAsync(regionId.Value);
                }
                else
                {
                    jobs = await _jobRepository.GetOverdueJobsAsync();
                }

                var dtos = _mappingService.Map<IEnumerable<MaintenanceJobDto>>(jobs);
                return Result<IEnumerable<MaintenanceJobDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue jobs");
                return Result.Failure<IEnumerable<MaintenanceJobDto>>($"Failed to get overdue jobs: {ex.Message}");
            }
        }

        public async Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> jobIds, string status)
        {
            try
            {
                var jobStatus = Enum.Parse<MaintenanceJobStatus>(status);
                var result = await _jobRepository.BulkUpdateStatusAsync(jobIds, jobStatus);
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating job status");
                return Result.Failure<bool>($"Failed to update jobs: {ex.Message}");
            }
        }

        public async Task<Result<bool>> BulkAssignEngineerAsync(IEnumerable<int> jobIds, int engineerId)
        {
            try
            {
                var result = await _jobRepository.BulkAssignEngineerAsync(jobIds, engineerId);
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk assigning engineer");
                return Result.Failure<bool>($"Failed to assign engineer: {ex.Message}");
            }
        }
    }
}
