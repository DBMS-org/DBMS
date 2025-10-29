using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.MachineManagement;

namespace API.Controllers
{
    [ApiController]
    [Route("api/maintenance")]
    [Authorize]
    public class MaintenanceAnalyticsController : BaseApiController
    {
        private readonly IMaintenanceReportRepository _reportRepository;
        private readonly IMaintenanceJobRepository _jobRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<MaintenanceAnalyticsController> _logger;

        public MaintenanceAnalyticsController(
            IMaintenanceReportRepository reportRepository,
            IMaintenanceJobRepository jobRepository,
            IMachineRepository machineRepository,
            ILogger<MaintenanceAnalyticsController> logger)
        {
            _reportRepository = reportRepository;
            _jobRepository = jobRepository;
            _machineRepository = machineRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all machines with upcoming maintenance due (within next 30 days)
        /// </summary>
        [HttpGet("alerts/service-due")]
        [Authorize(Roles = "Admin,MechanicalEngineer,Manager")]
        public async Task<IActionResult> GetServiceDueAlerts([FromQuery] int daysAhead = 30)
        {
            try
            {
                _logger.LogInformation("Getting service due alerts for next {Days} days", daysAhead);

                var cutoffDate = DateTime.UtcNow.AddDays(daysAhead);

                // This would need a new repository method, but for now we'll return a placeholder
                // In a full implementation, you'd query machines where NextMaintenanceDate <= cutoffDate
                var alerts = new
                {
                    Message = "Service due alerts endpoint",
                    CutoffDate = cutoffDate,
                    DaysAhead = daysAhead
                };

                return Ok(alerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting service due alerts");
                return StatusCode(500, "An error occurred while retrieving service due alerts");
            }
        }

        /// <summary>
        /// Get maintenance history for a specific machine
        /// </summary>
        [HttpGet("machines/{machineId}/history")]
        [Authorize(Roles = "Admin,MechanicalEngineer,Manager,Operator")]
        public async Task<IActionResult> GetMachineHistory(int machineId, [FromQuery] int limit = 20)
        {
            try
            {
                _logger.LogInformation("Getting maintenance history for machine {MachineId}", machineId);

                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return NotFound($"Machine {machineId} not found");
                }

                var reports = await _reportRepository.GetMaintenanceHistoryByMachineIdAsync(machineId, limit);

                var history = new
                {
                    MachineId = machineId,
                    MachineName = machine.Name,
                    LastMaintenanceDate = machine.LastMaintenanceDate,
                    NextMaintenanceDate = machine.NextMaintenanceDate,
                    TotalReports = reports.Count(),
                    Reports = reports.Select(r => new
                    {
                        r.Id,
                        r.TicketId,
                        r.ReportedAt,
                        r.Status,
                        r.Severity,
                        r.ProblemCategory,
                        r.AffectedPart,
                        ResolvedAt = r.ResolvedAt
                    })
                };

                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machine {MachineId} history", machineId);
                return StatusCode(500, "An error occurred while retrieving machine history");
            }
        }

        /// <summary>
        /// Get maintenance jobs within a date range
        /// </summary>
        [HttpGet("jobs/date-range")]
        [Authorize(Roles = "Admin,MechanicalEngineer,Manager")]
        public async Task<IActionResult> GetJobsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? regionId = null)
        {
            try
            {
                _logger.LogInformation("Getting jobs between {StartDate} and {EndDate}", startDate, endDate);

                if (startDate > endDate)
                {
                    return BadRequest("Start date must be before end date");
                }

                // Get reports in date range (jobs are linked to reports)
                var reports = await _reportRepository.GetReportsByDateRangeAsync(startDate, endDate);

                // Filter by region if specified
                if (regionId.HasValue)
                {
                    reports = reports.Where(r => r.Machine.RegionId == regionId.Value);
                }

                var jobs = reports
                    .Where(r => r.MaintenanceJob != null)
                    .Select(r => r.MaintenanceJob)
                    .Select(j => new
                    {
                        j!.Id,
                        j.MachineId,
                        MachineName = j.Machine?.Name,
                        j.Type,
                        j.Status,
                        j.ScheduledDate,
                        j.CompletedDate,
                        j.EstimatedHours,
                        j.ActualHours,
                        j.Reason
                    })
                    .OrderByDescending(j => j.ScheduledDate);

                var summary = new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    RegionId = regionId,
                    TotalJobs = jobs.Count(),
                    CompletedJobs = jobs.Count(j => j.Status == Domain.Entities.MaintenanceOperations.Enums.MaintenanceJobStatus.Completed),
                    InProgressJobs = jobs.Count(j => j.Status == Domain.Entities.MaintenanceOperations.Enums.MaintenanceJobStatus.InProgress),
                    ScheduledJobs = jobs.Count(j => j.Status == Domain.Entities.MaintenanceOperations.Enums.MaintenanceJobStatus.Scheduled),
                    Jobs = jobs
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jobs by date range");
                return StatusCode(500, "An error occurred while retrieving jobs");
            }
        }
    }
}
