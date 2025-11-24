using Application.DTOs.Reports;
using Domain.Entities.MaintenanceOperations.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Maintenance Performance Report implementation
    /// </summary>
    public partial class ReportService
    {
        public async Task<MaintenancePerformanceReportDto> GetMaintenancePerformanceReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Maintenance Performance Report");

                var report = new MaintenancePerformanceReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate ?? DateTime.UtcNow.AddDays(-30),
                    EndDate = filter?.EndDate ?? DateTime.UtcNow,
                    RegionFilter = filter?.RegionId
                };

                // Get jobs using the available method with date range
                var jobsEnumerable = await _maintenanceJobRepository.GetByDateRangeAsync(
                    report.StartDate.Value,
                    report.EndDate.Value
                );
                var jobs = jobsEnumerable.ToList();

                // Filter by region if specified
                if (!string.IsNullOrEmpty(filter?.RegionId) && int.TryParse(filter.RegionId, out int regionId))
                {
                    jobs = jobs.Where(j => j.Machine?.RegionId == regionId).ToList();
                }

                // Calculate statistics
                report.Statistics = new MaintenanceStatisticsDto
                {
                    TotalMaintenanceRecords = jobs.Count,
                    CompletedMaintenance = jobs.Count(j => j.Status == MaintenanceJobStatus.Completed),
                    InProgressMaintenance = jobs.Count(j => j.Status == MaintenanceJobStatus.InProgress),
                    PendingMaintenance = jobs.Count(j => j.Status == MaintenanceJobStatus.Scheduled),
                    AverageCompletionTimeHours = jobs
                        .Where(j => j.ActualHours.HasValue)
                        .Select(j => j.ActualHours.Value)
                        .DefaultIfEmpty(0)
                        .Average(),
                    MaintenanceCompletionRate = jobs.Any() ?
                        (decimal)jobs.Count(j => j.Status == MaintenanceJobStatus.Completed) / jobs.Count * 100 : 0
                };

                // Maintenance type breakdown
                report.MaintenanceTypeBreakdown = jobs
                    .GroupBy(j => j.Type)
                    .Select(g => new MaintenanceTypeBreakdownDto
                    {
                        MaintenanceType = g.Key.ToString(),
                        Count = g.Count(),
                        Percentage = jobs.Any() ? (decimal)g.Count() / jobs.Count * 100 : 0,
                        AverageCompletionTimeHours = g
                            .Where(j => j.ActualHours.HasValue)
                            .Select(j => j.ActualHours.Value)
                            .DefaultIfEmpty(0)
                            .Average()
                    })
                    .ToList();

                // PHASE 2: Maintenance trends removed - insufficient data (only 4 jobs) for meaningful trend analysis
                report.MaintenanceTrends = new List<MaintenanceTrendDto>();

                // Get all users for mechanic names
                var usersEnumerable = await _userRepository.GetAllAsync();
                var users = usersEnumerable.ToList();

                // Top performing mechanics (based on assignments)
                report.TopPerformingMechanics = jobs
                    .Where(j => j.Assignments != null && j.Assignments.Any())
                    .SelectMany(j => j.Assignments.Select(a => new
                    {
                        MechanicId = a.MechanicalEngineerId,
                        Job = j
                    }))
                    .GroupBy(x => x.MechanicId)
                    .Select(g =>
                    {
                        var mechanic = users.FirstOrDefault(u => u.Id == g.Key);
                        return new TopPerformingMechanicDto
                        {
                            MechanicId = g.Key,
                            MechanicName = mechanic?.Name ?? $"Mechanic {g.Key}",
                            TasksCompleted = g.Count(x => x.Job.Status == MaintenanceJobStatus.Completed),
                            AverageCompletionTimeHours = g
                                .Where(x => x.Job.ActualHours.HasValue)
                                .Select(x => x.Job.ActualHours.Value)
                                .DefaultIfEmpty(0)
                                .Average(),
                            CompletionRate = g.Any() ?
                                (decimal)g.Count(x => x.Job.Status == MaintenanceJobStatus.Completed) / g.Count() * 100 : 0,
                            MachinesServiced = g.Select(x => x.Job.MachineId).Distinct().Count()
                        };
                    })
                    .OrderByDescending(m => m.TasksCompleted)
                    .Take(10)
                    .ToList();

                // Critical issues (overdue jobs)
                report.CriticalIssues = jobs
                    .Where(j => j.Status == MaintenanceJobStatus.Overdue)
                    .Select(j => new MaintenanceIssueDto
                    {
                        MaintenanceId = j.Id,
                        MachineIdentifier = j.Machine?.Name ?? "Unknown",
                        IssueDescription = j.Reason,
                        Priority = j.Type == MaintenanceType.Emergency ? "Critical" :
                                  j.Type == MaintenanceType.Corrective ? "High" : "Medium",
                        ReportedDate = j.ScheduledDate,
                        Status = j.Status.ToString(),
                        DaysOpen = (DateTime.UtcNow - j.ScheduledDate).Days
                    })
                    .OrderByDescending(i => i.DaysOpen)
                    .ToList();

                // Regional breakdown
                report.RegionalBreakdown = jobs
                    .Where(j => j.Machine != null)
                    .GroupBy(j => j.Machine!.RegionName ?? "Unknown")
                    .Select(g => new RegionalMaintenanceDto
                    {
                        Region = g.Key,
                        TotalMaintenance = g.Count(),
                        CompletedMaintenance = g.Count(j => j.Status == MaintenanceJobStatus.Completed),
                        CompletionRate = g.Any() ?
                            (decimal)g.Count(j => j.Status == MaintenanceJobStatus.Completed) / g.Count() * 100 : 0
                    })
                    .ToList();

                // POPULATE DETAILED DATA ARRAYS
                // All Maintenance Jobs with complete details
                report.AllMaintenanceJobs = jobs.Select(j => new MaintenanceJobDetailDto
                {
                    MaintenanceJobId = j.Id,
                    JobTitle = $"{j.Type} - {j.Machine?.Name ?? "Unknown"}",
                    MachineId = j.MachineId,
                    MachineName = j.Machine?.Name ?? "Unknown",
                    MachineType = j.Machine?.Type ?? "Unknown",
                    MaintenanceType = j.Type.ToString(),
                    Priority = j.Type == MaintenanceType.Emergency ? "Critical" :
                              j.Type == MaintenanceType.Corrective ? "High" : "Medium",
                    Status = j.Status.ToString(),
                    ProblemDescription = j.Reason,
                    ScheduledDate = j.ScheduledDate,
                    StartDate = null, // Not available in current entity
                    CompletedDate = j.CompletedDate,
                    EstimatedHours = j.EstimatedHours,
                    ActualHours = j.ActualHours,
                    IsEngineServiceCompleted = j.IsEngineServiceCompleted,
                    IsDrifterServiceCompleted = j.IsDrifterServiceCompleted,
                    DrillBitsUsed = j.DrillBitsUsed ?? 0,
                    DrillRodsUsed = j.DrillRodsUsed ?? 0,
                    ShanksUsed = j.ShanksUsed ?? 0,
                    CompletionNotes = null, // Property doesn't exist in MaintenanceJob entity
                    AssignedEngineers = j.Assignments?.Select(a => new EngineerAssignmentDto
                    {
                        EngineerId = a.MechanicalEngineerId,
                        EngineerName = users.FirstOrDefault(u => u.Id == a.MechanicalEngineerId)?.Name ?? "Unknown"
                    }).ToList() ?? new List<EngineerAssignmentDto>(),
                    RegionId = j.Machine?.RegionId,
                    RegionName = j.Machine?.Region?.Name,
                    CreatedAt = j.CreatedAt
                }).ToList();

                // Mechanic/Engineer Performance Details - Filter only Mechanical Engineers
                var engineers = users.Where(u => u.Role.Contains("MechanicalEngineer") || u.Role.Contains("Mechanical Engineer")).ToList();
                report.AllEngineers = engineers.Select(eng => {
                    var engineerJobs = jobs.Where(j => j.Assignments != null && j.Assignments.Any(a => a.MechanicalEngineerId == eng.Id)).ToList();
                    var completedJobs = engineerJobs.Count(j => j.Status == MaintenanceJobStatus.Completed);
                    var jobsWithHours = engineerJobs.Where(j => j.ActualHours.HasValue).ToList();

                    return new EngineerPerformanceDetailDto
                    {
                        EngineerId = eng.Id,
                        EngineerName = eng.Name,
                        Email = eng.Email.Value, // Email is a value object
                        PhoneNumber = eng.OmanPhone ?? eng.CountryPhone,
                        RegionId = null, // User doesn't have RegionId, only Region string
                        RegionName = eng.Region,
                        Status = eng.Status.ToString(),
                        AssignedJobs = engineerJobs.Select(j => new EngineerJobAssignmentDto
                        {
                            JobId = j.Id,
                            JobTitle = $"{j.Type} - {j.Machine?.Name ?? "Unknown"}",
                            MachineId = j.MachineId,
                            MachineName = j.Machine?.Name ?? "Unknown",
                            Status = j.Status.ToString(),
                            AssignedDate = j.CreatedAt,
                            CompletedDate = j.CompletedDate,
                            ActualHours = j.ActualHours
                        }).ToList(),
                        TotalJobsAssigned = engineerJobs.Count,
                        CompletedJobs = completedJobs,
                        InProgressJobs = engineerJobs.Count(j => j.Status == MaintenanceJobStatus.InProgress),
                        OverdueJobs = engineerJobs.Count(j => j.Status == MaintenanceJobStatus.Overdue),
                        AverageCompletionTime = jobsWithHours.Any() ? jobsWithHours.Average(j => j.ActualHours!.Value) : 0,
                        AverageResponseTimeHours = jobsWithHours.Any() ? jobsWithHours.Average(j => j.ActualHours!.Value) : 0,
                        AverageCompletionTimeHours = jobsWithHours.Any() ? jobsWithHours.Average(j => j.ActualHours!.Value) : 0,
                        TotalHoursWorked = jobsWithHours.Sum(j => j.ActualHours!.Value),
                        JobsCompleted = completedJobs,
                        JobsInProgress = engineerJobs.Count(j => j.Status == MaintenanceJobStatus.InProgress),
                        CompletionRate = engineerJobs.Any() ? (decimal)completedJobs / engineerJobs.Count * 100 : 0,
                        PerformanceRating = engineerJobs.Any() ? (decimal)completedJobs / engineerJobs.Count * 100 : 0
                    };
                }).ToList();

                // PHASE 2: Operator Maintenance Reports - Get all reports in date range
                try
                {
                    var operatorReportsEnumerable = await _maintenanceReportRepository.GetReportsByDateRangeAsync(report.StartDate.Value, report.EndDate.Value);
                    var operatorReports = operatorReportsEnumerable.ToList();

                    report.OperatorReports = operatorReports.Select(or => new OperatorMaintenanceReportDetailDto
                    {
                        ReportId = or.Id,
                        MachineId = or.MachineId,
                        MachineName = or.Machine?.Name ?? or.MachineName,
                        MachineType = or.Machine?.Type ?? "Unknown",
                        OperatorId = or.OperatorId,
                        OperatorName = or.Operator?.Name ?? "Unknown",
                        ProblemCategory = or.ProblemCategory.ToString(),
                        IssueDescription = or.CustomDescription,
                        ProblemDescription = or.CustomDescription,
                        Severity = or.Severity.ToString(),
                        Status = or.Status.ToString(),
                        ReportedDate = or.ReportedAt,
                        ReportDate = or.ReportedAt,
                        AssignedEngineerId = or.MechanicalEngineerId,
                        AssignedEngineerName = or.MechanicalEngineerName,
                        RelatedMaintenanceJobId = or.MaintenanceJob?.Id,
                        ResolutionNotes = or.ResolutionNotes,
                        ResolvedDate = or.ResolvedAt
                    }).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not load operator reports for Maintenance Performance Report");
                    report.OperatorReports = new List<OperatorMaintenanceReportDetailDto>();
                }

                // Service History by Machine
                report.MachineServiceHistory = jobs
                    .GroupBy(j => j.MachineId)
                    .Select(g => {
                        var machine = g.First().Machine;
                        var machineJobs = g.OrderByDescending(j => j.ScheduledDate).ToList();
                        var completedJobs = machineJobs.Where(j => j.Status == MaintenanceJobStatus.Completed).ToList();
                        var lastServiceDate = completedJobs.Any() && completedJobs.Any(j => j.CompletedDate.HasValue)
                            ? completedJobs.Where(j => j.CompletedDate.HasValue).Max(j => j.CompletedDate)
                            : null;
                        var totalServiceHours = machineJobs.Where(j => j.ActualHours.HasValue).Sum(j => j.ActualHours.Value);

                        return new MachineServiceHistoryDto
                        {
                            MachineId = g.Key,
                            MachineName = machine?.Name ?? "Unknown",
                            MachineType = machine?.Type ?? "Unknown",
                            LastServiceDate = lastServiceDate,
                            NextServiceDue = lastServiceDate?.AddMonths(3), // Estimate 3 months from last service
                            TotalServices = machineJobs.Count,
                            TotalServicesCompleted = completedJobs.Count,
                            TotalServiceHours = totalServiceHours,
                            AverageServiceTime = machineJobs.Where(j => j.ActualHours.HasValue).Any()
                                ? machineJobs.Where(j => j.ActualHours.HasValue).Average(j => j.ActualHours!.Value)
                                : 0,
                            PreventiveMaintenanceCount = machineJobs.Count(j => j.Type == MaintenanceType.Preventive),
                            CorrectiveMaintenanceCount = machineJobs.Count(j => j.Type == MaintenanceType.Corrective),
                            EmergencyRepairsCount = machineJobs.Count(j => j.Type == MaintenanceType.Emergency),
                            TotalDowntimeHours = totalServiceHours,
                            AverageRepairTime = machineJobs.Where(j => j.ActualHours.HasValue).Any()
                                ? machineJobs.Where(j => j.ActualHours.HasValue).Average(j => j.ActualHours!.Value)
                                : 0,
                            MaterialsConsumed = new MaterialsConsumedDto
                            {
                                TotalDrillBitsUsed = machineJobs.Sum(j => j.DrillBitsUsed ?? 0),
                                TotalDrillRodsUsed = machineJobs.Sum(j => j.DrillRodsUsed ?? 0),
                                TotalShanksUsed = machineJobs.Sum(j => j.ShanksUsed ?? 0)
                            },
                            RecentServices = machineJobs.Take(10).Select(j => new RecentServiceDto
                            {
                                JobId = j.Id,
                                JobTitle = $"{j.Type} - {j.Reason}",
                                ServiceDate = j.CompletedDate ?? j.ScheduledDate,
                                Type = j.Type.ToString(),
                                Status = j.Status.ToString(),
                                CompletionTime = j.ActualHours
                            }).ToList()
                        };
                    }).ToList();

                // Critical & Overdue Issues Detailed (copy of CriticalIssues for consistency)
                report.CriticalIssuesDetailed = report.CriticalIssues;

                _logger.LogInformation("Maintenance Performance Report generated successfully");
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Maintenance Performance Report");
                throw;
            }
        }
    }
}
