using Application.DTOs.MaintenanceOperations;
using Application.DTOs.Shared;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.Infrastructure;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services.MaintenanceOperations
{
    public class MaintenanceReportApplicationService : IMaintenanceReportService
    {
        private readonly IMaintenanceReportRepository _reportRepository;
        private readonly IMaintenanceJobRepository _jobRepository;
        private readonly IMappingService _mappingService;
        private readonly ILogger<MaintenanceReportApplicationService> _logger;
        private readonly IMaintenanceJobService _jobService;

        public MaintenanceReportApplicationService(
            IMaintenanceReportRepository reportRepository,
            IMaintenanceJobRepository jobRepository,
            IMappingService mappingService,
            ILogger<MaintenanceReportApplicationService> logger,
            IMaintenanceJobService jobService)
        {
            _reportRepository = reportRepository;
            _jobRepository = jobRepository;
            _mappingService = mappingService;
            _logger = logger;
            _jobService = jobService;
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

                // Auto-create and assign maintenance job
                await _jobService.CreateJobFromReportAsync(createdReport.Id);

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
                // This would typically query the machine repository
                // For now, returning a placeholder response
                _logger.LogWarning("GetOperatorMachine not fully implemented");
                return Result.Failure<OperatorMachineDto>("Not implemented");
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
                var report = await _reportRepository.GetByIdAsync(id);
                if (report == null)
                {
                    return Result.Failure<MaintenanceReportDto>("Report not found");
                }

                var newStatus = Enum.Parse<ReportStatus>(request.Status);
                report.UpdateStatus(newStatus);

                await _reportRepository.UpdateAsync(report);

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
    }
}
