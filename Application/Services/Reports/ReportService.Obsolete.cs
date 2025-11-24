using Application.DTOs.Reports;
using Domain.Entities.MachineManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Obsolete/Removed reports - Fleet Management, Inventory Status, Operational Efficiency
    /// These were removed in Phase 1 due to insufficient data
    /// Kept for reference and potential future restoration
    /// </summary>
    public partial class ReportService
    {
        // REMOVED IN PHASE 1: Only 2 machines, MachineAssignments table empty
        public async Task<FleetManagementReportDto> GetFleetManagementReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Fleet Management Report");

                var report = new FleetManagementReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate,
                    EndDate = filter?.EndDate
                };

                // Get all machines
                var machinesEnumerable = await _machineRepository.GetAllAsync();
                var machines = machinesEnumerable.ToList();

                // Filter by region if specified
                if (!string.IsNullOrEmpty(filter?.RegionId) && int.TryParse(filter.RegionId, out int regionId))
                {
                    machines = machines.Where(m => m.RegionId == regionId).ToList();
                }

                // Calculate statistics
                report.Statistics = new FleetStatisticsDto
                {
                    TotalMachines = machines.Count,
                    AvailableMachines = machines.Count(m => m.Status == MachineStatus.Available),
                    AssignedMachines = machines.Count(m => m.Status == MachineStatus.Assigned),
                    InMaintenance = machines.Count(m => m.Status == MachineStatus.InMaintenance),
                    OutOfService = machines.Count(m => m.Status == MachineStatus.OutOfService),
                    OverallUtilizationRate = machines.Any() ?
                        (decimal)machines.Count(m => m.Status == MachineStatus.Assigned) / machines.Count * 100 : 0,
                    ServiceComplianceRate = CalculateServiceComplianceRate(machines),
                    PendingAssignmentRequests = 0
                };

                // Status distribution
                report.StatusDistribution = machines
                    .GroupBy(m => m.Status)
                    .Select(g => new StatusDistributionDto
                    {
                        Status = g.Key.ToString(),
                        Count = g.Count(),
                        Percentage = machines.Any() ? (decimal)g.Count() / machines.Count * 100 : 0
                    })
                    .ToList();

                // Machines due for service
                report.MachinesDueForService = GetMachinesDueForService(machines);

                // Regional distribution
                report.RegionalDistribution = GetRegionalFleetDistribution(machines);

                // Utilization by type
                report.UtilizationByType = machines
                    .GroupBy(m => m.Type)
                    .Select(g => new MachineTypeUtilizationDto
                    {
                        MachineType = g.Key,
                        TotalCount = g.Count(),
                        InUseCount = g.Count(m => m.Status == MachineStatus.Assigned),
                        UtilizationRate = g.Any() ? (decimal)g.Count(m => m.Status == MachineStatus.Assigned) / g.Count() * 100 : 0,
                        AverageHoursPerMachine = g.Average(m => m.CurrentEngineServiceHours)
                    })
                    .ToList();

                // Availability trend (placeholder)
                report.AvailabilityTrend = new List<AvailabilityTrendDto>();

                // POPULATE DETAILED DATA ARRAYS
                report.AllMachines = machines.Select(m => new MachineDetailDto
                {
                    MachineId = m.Id,
                    MachineName = m.Name,
                    MachineType = m.Type,
                    Model = m.Model,
                    Manufacturer = m.Manufacturer,
                    SerialNumber = m.SerialNumber,
                    Status = m.Status.ToString(),
                    CurrentEngineServiceHours = m.CurrentEngineServiceHours,
                    EngineServiceInterval = m.EngineServiceInterval,
                    NextEngineServiceDue = m.EngineServiceInterval - m.CurrentEngineServiceHours,
                    CurrentDrifterServiceHours = m.CurrentDrifterServiceHours ?? 0,
                    DrifterServiceInterval = m.DrifterServiceInterval ?? 0,
                    NextDrifterServiceDue = (m.DrifterServiceInterval ?? 0) - (m.CurrentDrifterServiceHours ?? 0),
                    DateAcquired = m.CreatedAt,
                    LastServiceDate = null,
                    AssignedProjectId = m.ProjectId,
                    AssignedProjectName = m.Project?.Name,
                    AssignedOperatorId = m.OperatorId,
                    AssignedOperatorName = m.Operator?.Name,
                    RegionId = m.RegionId,
                    RegionName = m.Region?.Name,
                    Notes = null,
                    CreatedAt = m.CreatedAt
                }).ToList();

                report.CurrentAssignments = new List<MachineAssignmentDetailDto>();
                report.AssignmentRequests = new List<AssignmentRequestDetailDto>();
                report.RecentUsageLogs = new List<UsageLogDetailDto>();
                report.AccessoryInventory = new List<AccessoryDetailDto>();

                _logger.LogInformation("Fleet Management Report generated successfully");
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Fleet Management Report");
                throw;
            }
        }

        // REMOVED IN PHASE 1: StoreInventories table empty
        public async Task<InventoryStatusReportDto> GetInventoryStatusReportAsync(ReportFilterDto? filter = null)
        {
            _logger.LogInformation("Generating Inventory Status Report (removed in Phase 1 - insufficient data)");

            return new InventoryStatusReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                StartDate = filter?.StartDate,
                EndDate = filter?.EndDate,
                Statistics = new InventoryStatisticsDto(),
                StockLevels = new List<MaterialStockDto>(),
                LowStockAlerts = new List<LowStockAlertDto>(),
                ExpiringMaterials = new List<ExpiringMaterialDto>()
            };
        }

        // REMOVED IN PHASE 1: Insufficient data for meaningful metrics
        public async Task<OperationalEfficiencyReportDto> GetOperationalEfficiencyReportAsync(ReportFilterDto? filter = null)
        {
            _logger.LogInformation("Generating Operational Efficiency Report (removed in Phase 1 - insufficient data)");

            return new OperationalEfficiencyReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                StartDate = filter?.StartDate ?? DateTime.UtcNow.AddDays(-30),
                EndDate = filter?.EndDate ?? DateTime.UtcNow
            };
        }

        #region Helper Methods

        private decimal CalculateServiceComplianceRate(List<Machine> machines)
        {
            if (!machines.Any()) return 0;

            var compliantMachines = machines.Count(m =>
                m.CurrentEngineServiceHours < m.EngineServiceInterval ||
                m.Status == MachineStatus.InMaintenance
            );

            return (decimal)compliantMachines / machines.Count * 100;
        }

        private List<MachineDueServiceDto> GetMachinesDueForService(List<Machine> machines)
        {
            return machines
                .Where(m => m.CurrentEngineServiceHours >= m.EngineServiceInterval * 0.9m) // 90% threshold
                .Select(m => new MachineDueServiceDto
                {
                    MachineId = m.Id,
                    MachineName = m.Name,
                    MachineType = m.Type,
                    SerialNumber = m.SerialNumber,
                    CurrentHours = m.CurrentEngineServiceHours,
                    ServiceDueHours = m.EngineServiceInterval,
                    HoursUntilService = m.EngineServiceInterval - m.CurrentEngineServiceHours,
                    EstimatedServiceDate = CalculateEstimatedServiceDate(m),
                    Region = m.RegionName ?? "Unknown",
                    Priority = m.CurrentEngineServiceHours >= m.EngineServiceInterval ? "High" : "Medium"
                })
                .OrderBy(m => m.HoursUntilService)
                .ToList();
        }

        private DateTime? CalculateEstimatedServiceDate(Machine machine)
        {
            var hoursUntilService = machine.EngineServiceInterval - machine.CurrentEngineServiceHours;

            // If service is already due or overdue, return today
            if (hoursUntilService <= 0)
                return DateTime.UtcNow.Date;

            // Estimate based on average usage rate
            const decimal averageHoursPerDay = 8m;
            var daysUntilService = (int)Math.Ceiling(hoursUntilService / averageHoursPerDay);

            return DateTime.UtcNow.Date.AddDays(daysUntilService);
        }

        private List<RegionalFleetDto> GetRegionalFleetDistribution(List<Machine> machines)
        {
            return machines
                .GroupBy(m => m.RegionName ?? m.Region?.Name ?? "Unknown")
                .Select(g => new RegionalFleetDto
                {
                    Region = g.Key,
                    TotalMachines = g.Count(),
                    AvailableMachines = g.Count(m => m.Status == MachineStatus.Available),
                    AssignedMachines = g.Count(m => m.Status == MachineStatus.Assigned),
                    InMaintenance = g.Count(m => m.Status == MachineStatus.InMaintenance),
                    UtilizationRate = g.Any() ? (decimal)g.Count(m => m.Status == MachineStatus.Assigned) / g.Count() * 100 : 0
                })
                .ToList();
        }

        #endregion
    }
}
