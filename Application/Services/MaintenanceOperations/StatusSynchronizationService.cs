using Application.DTOs.Shared;
using Application.Interfaces.MachineManagement;
using Application.Interfaces.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.MachineManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.MaintenanceOperations
{
    public class StatusSynchronizationService : IStatusSynchronizationService
    {
        private readonly IMaintenanceReportRepository _reportRepository;
        private readonly IMaintenanceJobRepository _jobRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<StatusSynchronizationService> _logger;

        public StatusSynchronizationService(
            IMaintenanceReportRepository reportRepository,
            IMaintenanceJobRepository jobRepository,
            IMachineRepository machineRepository,
            ILogger<StatusSynchronizationService> logger)
        {
            _reportRepository = reportRepository;
            _jobRepository = jobRepository;
            _machineRepository = machineRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> SynchronizeReportAndJobAsync(int reportId)
        {
            try
            {
                _logger.LogInformation("Synchronizing report {ReportId} with associated job", reportId);

                var report = await _reportRepository.GetWithDetailsAsync(reportId);
                if (report == null)
                {
                    return Result.Failure<bool>("Report not found");
                }

                if (report.MaintenanceJob != null)
                {
                    var job = report.MaintenanceJob;

                    // Sync report status to job status
                    var jobStatusUpdated = false;
                    switch (report.Status)
                    {
                        case ReportStatus.Acknowledged:
                            if (job.Status == MaintenanceJobStatus.Scheduled)
                            {
                                // Job remains scheduled after acknowledgment
                                jobStatusUpdated = false;
                            }
                            break;

                        case ReportStatus.InProgress:
                            if (job.Status != MaintenanceJobStatus.InProgress && job.Status != MaintenanceJobStatus.Completed)
                            {
                                job.Start();
                                jobStatusUpdated = true;
                            }
                            break;

                        case ReportStatus.Resolved:
                            if (job.Status != MaintenanceJobStatus.Completed)
                            {
                                // Note: Job completion should be done via CompleteJobAsync with observations
                                _logger.LogWarning("Report {ReportId} marked as resolved but job {JobId} not completed", reportId, job.Id);
                            }
                            break;
                    }

                    if (jobStatusUpdated)
                    {
                        await _jobRepository.UpdateAsync(job);
                    }
                }

                _logger.LogInformation("Successfully synchronized report {ReportId} with job", reportId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing report {ReportId} with job", reportId);
                return Result.Failure<bool>($"Failed to synchronize: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMachineStatusAsync(int machineId, SeverityLevel severity, ReportStatus reportStatus)
        {
            try
            {
                _logger.LogInformation("Updating machine {MachineId} status based on severity {Severity} and report status {ReportStatus}",
                    machineId, severity, reportStatus);

                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return Result.Failure<bool>("Machine not found");
                }

                var originalStatus = machine.Status;

                if (reportStatus == ReportStatus.Reported || reportStatus == ReportStatus.Acknowledged || reportStatus == ReportStatus.InProgress)
                {
                    // Active maintenance scenario
                    if (severity == SeverityLevel.Critical || severity == SeverityLevel.High)
                    {
                        // Critical or High severity - machine should be in maintenance
                        if (machine.Status != MachineStatus.InMaintenance && machine.Status != MachineStatus.OutOfService)
                        {
                            machine.ChangeStatus(MachineStatus.InMaintenance);
                            _logger.LogInformation("Machine {MachineId} status changed from {OldStatus} to InMaintenance due to {Severity} severity report",
                                machineId, originalStatus, severity);
                        }
                    }
                }
                else if (reportStatus == ReportStatus.Resolved || reportStatus == ReportStatus.Closed)
                {
                    var canRestore = await CanRestoreMachineStatusAsync(machineId);
                    if (canRestore.IsSuccess && canRestore.Value)
                    {
                        await RestoreMachineStatusAsync(machineId);
                    }
                }

                if (machine.Status != originalStatus)
                {
                    await _machineRepository.UpdateAsync(machine);
                    _logger.LogInformation("Machine {MachineId} status updated successfully", machineId);
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating machine {MachineId} status", machineId);
                return Result.Failure<bool>($"Failed to update machine status: {ex.Message}");
            }
        }

        public async Task<Result<bool>> HandleJobCompletionAsync(int jobId)
        {
            try
            {
                _logger.LogInformation("Handling job completion for job {JobId}", jobId);

                var job = await _jobRepository.GetWithDetailsAsync(jobId);
                if (job == null)
                {
                    return Result.Failure<bool>("Job not found");
                }

                if (job.MaintenanceReport != null)
                {
                    if (job.MaintenanceReport.Status != ReportStatus.Resolved && job.MaintenanceReport.Status != ReportStatus.Closed)
                    {
                        job.MaintenanceReport.Resolve(job.Observations ?? "Maintenance job completed");
                        await _reportRepository.UpdateAsync(job.MaintenanceReport);
                        _logger.LogInformation("Report {ReportId} marked as resolved after job {JobId} completion", job.MaintenanceReport.Id, jobId);
                    }

                    await UpdateMachineStatusAsync(job.MachineId, job.MaintenanceReport.Severity, ReportStatus.Resolved);
                }

                await UpdateMachineMaintenanceDateAsync(job.MachineId, DateTime.UtcNow);

                _logger.LogInformation("Successfully handled job {JobId} completion", jobId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling job {JobId} completion", jobId);
                return Result.Failure<bool>($"Failed to handle job completion: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CanRestoreMachineStatusAsync(int machineId)
        {
            try
            {
                _logger.LogInformation("Checking if machine {MachineId} can be restored to normal status", machineId);

                var reports = await _reportRepository.GetByMachineIdAsync(machineId);

                var hasActiveHighSeverityReports = reports.Any(r =>
                    (r.Severity == SeverityLevel.Critical || r.Severity == SeverityLevel.High) &&
                    (r.Status != ReportStatus.Resolved && r.Status != ReportStatus.Closed) &&
                    r.IsActive);

                if (hasActiveHighSeverityReports)
                {
                    _logger.LogInformation("Machine {MachineId} cannot be restored - has active high severity reports", machineId);
                    return Result<bool>.Success(false);
                }

                _logger.LogInformation("Machine {MachineId} can be restored to normal status", machineId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if machine {MachineId} can be restored", machineId);
                return Result.Failure<bool>($"Failed to check machine status: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RestoreMachineStatusAsync(int machineId)
        {
            try
            {
                _logger.LogInformation("Restoring machine {MachineId} to normal operational status", machineId);

                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return Result.Failure<bool>("Machine not found");
                }

                // Determine appropriate status based on assignment
                var restoredStatus = machine.OperatorId.HasValue ? MachineStatus.Assigned : MachineStatus.Available;

                if (machine.Status != restoredStatus)
                {
                    var originalStatus = machine.Status;
                    machine.ChangeStatus(restoredStatus);
                    await _machineRepository.UpdateAsync(machine);

                    _logger.LogInformation("Machine {MachineId} status restored from {OldStatus} to {NewStatus}",
                        machineId, originalStatus, restoredStatus);
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring machine {MachineId} status", machineId);
                return Result.Failure<bool>($"Failed to restore machine status: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateMachineMaintenanceDateAsync(int machineId, DateTime maintenanceDate)
        {
            try
            {
                _logger.LogInformation("Updating machine {MachineId} last maintenance date to {Date}", machineId, maintenanceDate);

                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return Result.Failure<bool>("Machine not found");
                }

                machine.LastMaintenanceDate = maintenanceDate;

                // Calculate next maintenance date (e.g., 6 months from now)
                machine.NextMaintenanceDate = maintenanceDate.AddMonths(6);

                await _machineRepository.UpdateAsync(machine);

                _logger.LogInformation("Machine {MachineId} maintenance dates updated successfully", machineId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating machine {MachineId} maintenance date", machineId);
                return Result.Failure<bool>($"Failed to update maintenance date: {ex.Message}");
            }
        }
    }
}
