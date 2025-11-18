using Application.DTOs.MaintenanceOperations;
using Application.DTOs.Shared;
using Application.Interfaces;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.MachineManagement;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.UserManagement;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services.MaintenanceOperations
{
    public class MaintenanceReportApplicationService : IMaintenanceReportService
    {
        private readonly IMaintenanceReportRepository _reportRepository;
        private readonly IMaintenanceJobRepository _jobRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMappingService _mappingService;
        private readonly ILogger<MaintenanceReportApplicationService> _logger;
        private readonly IMaintenanceJobService _jobService;
        private readonly IStatusSynchronizationService _statusSyncService;

        public MaintenanceReportApplicationService(
            IMaintenanceReportRepository reportRepository,
            IMaintenanceJobRepository jobRepository,
            IMachineRepository machineRepository,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IMappingService mappingService,
            ILogger<MaintenanceReportApplicationService> logger,
            IMaintenanceJobService jobService,
            IStatusSynchronizationService statusSyncService)
        {
            _reportRepository = reportRepository;
            _jobRepository = jobRepository;
            _machineRepository = machineRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _mappingService = mappingService;
            _logger = logger;
            _jobService = jobService;
            _statusSyncService = statusSyncService;
        }

        public async Task<Result<MaintenanceReportDto>> SubmitReportAsync(SubmitMaintenanceReportRequest request)
        {
            try
            {
                _logger.LogInformation("Submitting maintenance report for operator {OperatorId} and machine {MachineId}",
                    request.OperatorId, request.MachineId);

                // Create maintenance report from request
                var symptomsJson = request.Symptoms != null ? JsonSerializer.Serialize(request.Symptoms) : null;

                var report = MaintenanceReport.Create(
                    request.OperatorId,
                    request.MachineId,
                    string.Empty, // Will be populated from machine
                    null,
                    null,
                    null,
                    Enum.Parse<MachinePart>(request.AffectedPart),
                    Enum.Parse<ProblemCategory>(request.ProblemCategory),
                    request.CustomDescription,
                    symptomsJson,
                    request.ErrorCodes,
                    Enum.Parse<SeverityLevel>(request.Severity)
                );

                // Save report
                var createdReport = await _reportRepository.CreateAsync(report);

                _logger.LogInformation("Maintenance report created with ticket {TicketId}", createdReport.TicketId);

                // Update machine status based on report severity
                await _statusSyncService.UpdateMachineStatusAsync(
                    request.MachineId,
                    Enum.Parse<SeverityLevel>(request.Severity),
                    ReportStatus.Reported);

                // Auto-create and assign maintenance job
                await _jobService.CreateJobFromReportAsync(createdReport.Id);

                // Notify Mechanical Engineers about new maintenance report
                await NotifyMaintenanceReportCreatedAsync(createdReport, request.OperatorId);

                // Reload report with full details
                var reportWithDetails = await _reportRepository.GetWithDetailsAsync(createdReport.Id);
                var dto = _mappingService.Map<MaintenanceReportDto>(reportWithDetails);

                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting maintenance report");
                return Result.Failure<MaintenanceReportDto>($"Failed to submit report: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportDto>> GetReportByIdAsync(int id)
        {
            try
            {
                var report = await _reportRepository.GetWithDetailsAsync(id);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                var dto = _mappingService.Map<MaintenanceReportDto>(report);
                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting report {ReportId}", id);
                return Result.Failure<MaintenanceReportDto>($"Failed to get report: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportDto>> GetReportByTicketIdAsync(string ticketId)
        {
            try
            {
                var report = await _reportRepository.GetByTicketIdAsync(ticketId);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                var dto = _mappingService.Map<MaintenanceReportDto>(report);
                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting report by ticket {TicketId}", ticketId);
                return Result.Failure<MaintenanceReportDto>($"Failed to get report: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<MaintenanceReportDto>>> GetOperatorReportsAsync(int operatorId)
        {
            try
            {
                var reports = await _reportRepository.GetByOperatorIdAsync(operatorId);
                var dtos = _mappingService.Map<IEnumerable<MaintenanceReportDto>>(reports);
                return Result<IEnumerable<MaintenanceReportDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reports for operator {OperatorId}", operatorId);
                return Result.Failure<IEnumerable<MaintenanceReportDto>>($"Failed to get reports: {ex.Message}");
            }
        }

        public async Task<Result<OperatorMachineDto>> GetOperatorMachineAsync(int operatorId)
        {
            try
            {
                _logger.LogInformation("Getting machine details for operator {OperatorId}", operatorId);

                var machine = await _machineRepository.GetByOperatorIdAsync(operatorId);
                if (machine == null)
                {
                    return Result.Failure<OperatorMachineDto>("No machine assigned to this operator");
                }

                var dto = new OperatorMachineDto
                {
                    Id = machine.Id,
                    Name = machine.Name,
                    Type = machine.Type,
                    Model = machine.Model,
                    Manufacturer = machine.Manufacturer,
                    SerialNumber = machine.SerialNumber,
                    Status = machine.Status.ToString(),
                    CurrentLocation = machine.CurrentLocation,
                    LastMaintenanceDate = machine.LastMaintenanceDate,
                    NextMaintenanceDate = machine.NextMaintenanceDate,
                    ProjectName = machine.Project?.Name,
                    RegionName = machine.Region?.Name
                };

                return Result<OperatorMachineDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machine for operator {OperatorId}", operatorId);
                return Result.Failure<OperatorMachineDto>($"Failed to get machine: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportSummaryDto>> GetOperatorSummaryAsync(int operatorId)
        {
            try
            {
                var reports = await _reportRepository.GetByOperatorIdAsync(operatorId);

                var summary = new MaintenanceReportSummaryDto
                {
                    TotalReports = reports.Count(),
                    PendingReports = reports.Count(r => r.Status == ReportStatus.Reported || r.Status == ReportStatus.Acknowledged),
                    InProgressReports = reports.Count(r => r.Status == ReportStatus.InProgress),
                    ResolvedReports = reports.Count(r => r.Status == ReportStatus.Resolved),
                    ClosedReports = reports.Count(r => r.Status == ReportStatus.Closed),
                    CriticalReports = reports.Count(r => r.Severity == SeverityLevel.Critical && r.Status != ReportStatus.Closed),
                    HighPriorityReports = reports.Count(r => r.Severity == SeverityLevel.High && r.Status != ReportStatus.Closed)
                };

                return Result<MaintenanceReportSummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting summary for operator {OperatorId}", operatorId);
                return Result.Failure<MaintenanceReportSummaryDto>($"Failed to get summary: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportDto>> UpdateReportStatusAsync(int id, UpdateReportStatusRequest request)
        {
            try
            {
                var report = await _reportRepository.GetWithDetailsAsync(id);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                var newStatus = Enum.Parse<ReportStatus>(request.Status);
                var oldStatus = report.Status;
                report.UpdateStatus(newStatus);

                await _reportRepository.UpdateAsync(report);

                // Synchronize machine status based on report status change
                await _statusSyncService.UpdateMachineStatusAsync(report.MachineId, report.Severity, newStatus);

                // Synchronize with job if exists
                await _statusSyncService.SynchronizeReportAndJobAsync(id);

                // Notify operator about status change
                if (oldStatus != newStatus)
                {
                    await NotifyReportStatusChangedAsync(report, oldStatus, newStatus);
                }

                var dto = _mappingService.Map<MaintenanceReportDto>(report);
                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for report {ReportId}", id);
                return Result.Failure<MaintenanceReportDto>($"Failed to update status: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportDto>> CloseReportAsync(int id, int operatorId)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                if (report.OperatorId != operatorId)
                {
                    return Result.Failure<MaintenanceReportDto>("Only the original operator can close this report");
                }

                report.Close();
                await _reportRepository.UpdateAsync(report);

                var dto = _mappingService.Map<MaintenanceReportDto>(report);
                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing report {ReportId}", id);
                return Result.Failure<MaintenanceReportDto>($"Failed to close report: {ex.Message}");
            }
        }

        public async Task<Result<MaintenanceReportDto>> ReopenReportAsync(int id, string reason)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                report.Reopen(reason);
                await _reportRepository.UpdateAsync(report);

                var dto = _mappingService.Map<MaintenanceReportDto>(report);
                return Result<MaintenanceReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reopening report {ReportId}", id);
                return Result.Failure<MaintenanceReportDto>($"Failed to reopen report: {ex.Message}");
            }
        }

        // ===== Private Notification Helper Methods =====

        /// <summary>
        /// Notify Mechanical Engineers when a new maintenance report is created
        /// </summary>
        private async Task NotifyMaintenanceReportCreatedAsync(MaintenanceReport report, int operatorId)
        {
            try
            {
                // Get all Mechanical Engineers
                var mechanicalEngineers = await _userRepository.GetByRoleAndRegionAsync("MechanicalEngineer");

                if (!mechanicalEngineers.Any()) return;

                // Get operator and machine info
                var operator_ = await _userRepository.GetByIdAsync(operatorId);
                var machine = await _machineRepository.GetByIdAsync(report.MachineId);

                var operatorName = operator_?.Name ?? $"Operator #{operatorId}";
                var machineName = machine?.Name ?? $"Machine #{report.MachineId}";

                // Determine priority based on severity
                var priority = report.Severity == SeverityLevel.Critical ? NotificationPriority.Critical :
                               report.Severity == SeverityLevel.High ? NotificationPriority.Urgent :
                               report.Severity == SeverityLevel.Medium ? NotificationPriority.High :
                               NotificationPriority.Normal;

                // Create notifications for all Mechanical Engineers
                var notifications = mechanicalEngineers.Select(engineer =>
                    Notification.Create(
                        userId: engineer.Id,
                        type: NotificationType.MaintenanceReportCreated,
                        title: "New Maintenance Report",
                        message: $"{operatorName} has submitted a {report.Severity} severity maintenance report for {machineName}. Issue: {report.ProblemCategory} - {report.CustomDescription}. Ticket: {report.TicketId}",
                        priority: priority,
                        relatedEntityType: "MaintenanceReport",
                        relatedEntityId: report.Id,
                        actionUrl: $"/mechanical-engineer/reports/{report.Id}"
                    )
                ).ToList();

                await _notificationRepository.CreateBulkAsync(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create notification for maintenance report {ReportId}", report.Id);
                // Don't fail the main operation
            }
        }

        /// <summary>
        /// Notify operator when maintenance report status changes
        /// </summary>
        private async Task NotifyReportStatusChangedAsync(MaintenanceReport report, ReportStatus oldStatus, ReportStatus newStatus)
        {
            try
            {
                // Only notify on significant status changes
                if (newStatus == ReportStatus.Acknowledged ||
                    newStatus == ReportStatus.InProgress ||
                    newStatus == ReportStatus.Resolved)
                {
                    var machine = await _machineRepository.GetByIdAsync(report.MachineId);
                    var machineName = machine?.Name ?? $"Machine #{report.MachineId}";

                    var (notificationType, title, message, priority) = newStatus switch
                    {
                        ReportStatus.Acknowledged => (
                            NotificationType.MaintenanceReportAcknowledged,
                            "Maintenance Report Acknowledged",
                            $"Your maintenance report for {machineName} (Ticket: {report.TicketId}) has been acknowledged by the maintenance team. Work will begin soon.",
                            NotificationPriority.Normal
                        ),
                        ReportStatus.InProgress => (
                            NotificationType.MaintenanceReportInProgress,
                            "Maintenance Work In Progress",
                            $"Maintenance work on {machineName} (Ticket: {report.TicketId}) is now in progress. The team is actively working on resolving the issue.",
                            NotificationPriority.High
                        ),
                        ReportStatus.Resolved => (
                            NotificationType.MaintenanceReportResolved,
                            "Maintenance Report Resolved",
                            $"The maintenance issue on {machineName} (Ticket: {report.TicketId}) has been resolved. Please verify the fix and close the report if satisfied.",
                            NotificationPriority.High
                        ),
                        _ => ((NotificationType)0, "", "", NotificationPriority.Normal)
                    };

                    if (!string.IsNullOrEmpty(title))
                    {
                        var notification = Notification.Create(
                            userId: report.OperatorId,
                            type: notificationType,
                            title: title,
                            message: message,
                            priority: priority,
                            relatedEntityType: "MaintenanceReport",
                            relatedEntityId: report.Id,
                            actionUrl: $"/operator/maintenance-reports/{report.Id}"
                        );

                        await _notificationRepository.CreateAsync(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create status change notification for report {ReportId}", report.Id);
            }
        }
    }
}
