using Application.DTOs.Reports;
using Domain.Entities.MachineManagement;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Regional Performance Report implementation
    /// </summary>
    public partial class ReportService
    {
        public async Task<RegionalPerformanceReportDto> GetRegionalPerformanceReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Regional Performance Report");

                var report = new RegionalPerformanceReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate ?? DateTime.UtcNow.AddDays(-30),
                    EndDate = filter?.EndDate ?? DateTime.UtcNow
                };

                // Get all data
                var projectsEnumerable = await _projectRepository.GetAllAsync();
                var projects = projectsEnumerable.ToList();

                var machinesEnumerable = await _machineRepository.GetAllAsync();
                var machines = machinesEnumerable.ToList();

                var usersEnumerable = await _userRepository.GetAllAsync();
                var users = usersEnumerable.ToList();

                var jobsEnumerable = await _maintenanceJobRepository.GetByDateRangeAsync(
                    report.StartDate.Value,
                    report.EndDate.Value
                );
                var maintenanceJobs = jobsEnumerable.ToList();

                // Group by regions from projects
                var allRegions = projects.Where(p => p.RegionNavigation != null)
                                    .Select(p => p.RegionNavigation!)
                                    .ToList();

                var regions = allRegions
                                    .GroupBy(r => r.Id)
                                    .Select(g => g.First())
                                    .ToList();

                // Regional comparison
                report.RegionalComparison = regions.Select(region =>
                {
                    var regProjects = projects.Where(p => p.RegionId == region.Id).ToList();
                    var completedProjects = regProjects.Count(p => p.Status == ProjectStatus.Completed);
                    var regionMachines = machines.Where(m => m.RegionId == region.Id).ToList();
                    var assignedMachines = regionMachines.Count(m => m.Status == MachineStatus.Assigned);
                    var regionUsers = users.Where(u => u.Region == region.Name).ToList();

                    return new RegionalComparisonDto
                    {
                        Region = region.Name,
                        TotalProjects = regProjects.Count,
                        ActiveProjects = regProjects.Count(p => p.Status == ProjectStatus.Active),
                        TotalMachines = regionMachines.Count,
                        AssignedMachines = assignedMachines,
                        TotalUsers = regionUsers.Count,
                        ActiveUsers = regionUsers.Count(u => u.Status == UserStatus.Active),
                        MachineUtilization = regionMachines.Any()
                            ? (decimal)assignedMachines / regionMachines.Count * 100
                            : 0,
                        ProjectCompletionRate = regProjects.Any()
                            ? (decimal)completedProjects / regProjects.Count * 100
                            : 0,
                        OverallEfficiency = CalculateRegionalEfficiency(regProjects, regionMachines, assignedMachines)
                    };
                }).ToList();

                // PHASE 2: Machine distribution removed - only 2 machines insufficient for meaningful distribution analysis
                report.MachineDistribution = new List<RegionalMachineDistributionDto>();

                // PHASE 2: Maintenance stats removed - limited maintenance data (only 4 jobs)
                report.MaintenanceStats = new List<RegionalMaintenanceStatsDto>();

                // PHASE 2: Regional KPIs removed - too few entities per region for meaningful performance metrics
                report.RegionalKPIs = new List<RegionalKPIDto>();

                // PHASE 2: Top performing regions removed - insufficient data for ranking
                report.TopPerformingRegions = new List<TopRegionDto>();

                // PHASE 2: Resource allocation removed - minimal resources (2 machines, few users) insufficient for allocation analysis
                report.ResourceAllocation = new List<RegionalResourceDto>();

                // Service alert trends - placeholder
                report.ServiceAlertTrends = new List<RegionalServiceAlertDto>();

                // POPULATE DETAILED DATA ARRAYS

                // All Regions with complete details
                var stores = (await _storeRepository.GetAllAsync()).ToList();
                report.AllRegions = regions.Select(region =>
                {
                    var regionProjects = projects.Where(p => p.RegionId == region.Id).ToList();
                    var regionMachines = machines.Count(m => m.RegionId == region.Id);
                    var regionUsers = users.Count(u => u.Region == region.Name);
                    var regionStores = stores.Count(s => s.RegionId == region.Id);

                    return new RegionDetailDto
                    {
                        RegionId = region.Id,
                        RegionName = region.Name,
                        Description = null, // Not available in entity
                        ManagerId = null, // Not available in entity
                        ManagerName = null,
                        TotalProjects = regionProjects.Count,
                        ActiveProjects = regionProjects.Count(p => p.Status == ProjectStatus.Active),
                        CompletedProjects = regionProjects.Count(p => p.Status == ProjectStatus.Completed),
                        TotalMachines = regionMachines,
                        TotalUsers = regionUsers,
                        TotalStores = regionStores,
                        Geography = null, // Not available in entity
                        EstablishedDate = null, // Not available in entity
                        Status = "Active" // Assuming all regions are active
                    };
                }).ToList();

                // Regional Resource Breakdown - detailed resource lists per region
                report.RegionalResources = regions.Select(region =>
                {
                    var regionMachines = machines.Where(m => m.RegionId == region.Id).ToList();
                    var regionUsers = users.Where(u => u.Region == region.Name).ToList();
                    var regionProjects = projects.Where(p => p.RegionId == region.Id).ToList();
                    var regionStores = stores.Where(s => s.RegionId == region.Id).ToList();
                    var regionMachineIds = regionMachines.Select(m => m.Id).ToList();
                    var regionJobs = maintenanceJobs.Where(j => regionMachineIds.Contains(j.MachineId)).ToList();

                    return new RegionalResourceBreakdownDto
                    {
                        RegionId = region.Id,
                        RegionName = region.Name,
                        Machines = regionMachines.Select(m => new RegionMachineDto
                        {
                            MachineId = m.Id,
                            MachineName = m.Name,
                            MachineType = m.Type,
                            Status = m.Status.ToString(),
                            CurrentAssignment = m.Project?.Name
                        }).ToList(),
                        Users = regionUsers.Select(u => new RegionUserDto
                        {
                            UserId = u.Id,
                            UserName = u.Name,
                            Role = u.Role,
                            Status = u.Status.ToString(),
                            CurrentAssignments = new List<string>() // Would need assignment tracking
                        }).ToList(),
                        Projects = regionProjects.Select(p => new RegionProjectDto
                        {
                            ProjectId = p.Id,
                            ProjectName = p.Name,
                            Status = p.Status.ToString(),
                            CompletionPercentage = CalculateProjectCompletionPercentage(p)
                        }).ToList(),
                        Stores = regionStores.Select(s => new RegionStoreDto
                        {
                            StoreId = s.Id,
                            StoreName = s.StoreName,
                            TotalBatches = s.Inventories?.Count ?? 0,
                            LowStockItems = s.Inventories?.Count(i => i.IsLowStock()) ?? 0
                        }).ToList(),
                        MaintenanceJobs = regionJobs.Select(j => new RegionMaintenanceJobDto
                        {
                            JobId = j.Id,
                            JobTitle = j.Reason,
                            MachineId = j.MachineId,
                            MachineName = j.Machine?.Name ?? "Unknown",
                            Status = j.Status.ToString(),
                            Priority = j.ServiceAlertLevel ?? "Normal"
                        }).ToList(),
                        PerformanceMetrics = new RegionalPerformanceMetricsDto
                        {
                            ProjectSuccessRate = regionProjects.Any() ? (decimal)regionProjects.Count(p => p.Status == ProjectStatus.Completed) / regionProjects.Count * 100 : 0,
                            MachineUptime = regionMachines.Any() ? (decimal)(regionMachines.Count - regionMachines.Count(m => m.Status == MachineStatus.OutOfService)) / regionMachines.Count * 100 : 0,
                            MaintenanceEfficiency = regionJobs.Any() ? (decimal)regionJobs.Count(j => j.Status == MaintenanceJobStatus.Completed) / regionJobs.Count * 100 : 0,
                            ResourceUtilization = regionMachines.Any() ? (decimal)regionMachines.Count(m => m.Status == MachineStatus.Assigned) / regionMachines.Count * 100 : 0
                        }
                    };
                }).ToList();

                // Cross-Regional Transfers - Using inventory transfer requests as proxy
                var transferRequests = (await _transferRequestRepository.GetAllAsync()).ToList();
                report.CrossRegionalTransfers = transferRequests
                    .Where(t => t.DestinationStore != null)
                    .Select(t => new CrossRegionalTransferDto
                    {
                        TransferId = t.Id,
                        FromRegionId = 0, // Central warehouse doesn't have region
                        FromRegionName = "Central Warehouse",
                        FromStoreId = 0,
                        FromStoreName = "Central Warehouse",
                        ToRegionId = t.DestinationStore.RegionId,
                        ToRegionName = t.DestinationStore.Region?.Name ?? "Unknown",
                        ToStoreId = t.DestinationStoreId,
                        ToStoreName = t.DestinationStore.StoreName,
                        MaterialType = t.CentralInventory?.ExplosiveType.ToString() ?? "Unknown",
                        Quantity = t.RequestedQuantity,
                        Unit = t.Unit,
                        Status = t.Status.ToString(),
                        RequestDate = t.RequestDate,
                        CompletedDate = t.CompletedDate,
                        TransferTime = t.CompletedDate.HasValue && t.RequestDate != default ?
                            (decimal)(t.CompletedDate.Value - t.RequestDate).TotalHours : null,
                        RequestedBy = t.RequestedByUser?.Name ?? "Unknown",
                        ApprovedBy = t.ApprovedByUser?.Name
                    }).ToList();

                _logger.LogInformation("Regional Performance Report generated successfully");
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Regional Performance Report");
                throw;
            }
        }

        private decimal CalculateRegionalEfficiency(List<Project> projects, List<Machine> machines, int assignedMachines)
        {
            if (!projects.Any() || !machines.Any()) return 0;

            var completionRate = (decimal)projects.Count(p => p.Status == ProjectStatus.Completed) / projects.Count * 100;
            var utilizationRate = (decimal)assignedMachines / machines.Count * 100;

            return (completionRate + utilizationRate) / 2;
        }

        private List<string> GetRegionalStrengths(RegionalKPIDto kpi)
        {
            var strengths = new List<string>();

            if (kpi.OperationalUptime >= 90) strengths.Add("Excellent operational uptime");
            if (kpi.MachineAvailability >= 80) strengths.Add("High machine availability");
            if (kpi.ResourceUtilization >= 75) strengths.Add("Strong resource utilization");
            if (kpi.MaintenanceCompliance >= 85) strengths.Add("Good maintenance compliance");

            return strengths.Any() ? strengths : new List<string> { "Steady performance" };
        }

        private List<string> GetRegionalImprovementAreas(RegionalKPIDto kpi)
        {
            var areas = new List<string>();

            if (kpi.OperationalUptime < 75) areas.Add("Operational uptime needs improvement");
            if (kpi.MachineAvailability < 60) areas.Add("Low machine availability");
            if (kpi.ResourceUtilization < 50) areas.Add("Underutilized resources");
            if (kpi.MaintenanceCompliance < 70) areas.Add("Maintenance compliance below target");

            return areas.Any() ? areas : new List<string> { "Continue current practices" };
        }

        private string GetTopPerformanceMetric(RegionalKPIDto kpi)
        {
            var metrics = new Dictionary<string, decimal>
            {
                { "Operational Uptime", kpi.OperationalUptime },
                { "Machine Availability", kpi.MachineAvailability },
                { "Resource Utilization", kpi.ResourceUtilization },
                { "Maintenance Compliance", kpi.MaintenanceCompliance }
            };

            return metrics.OrderByDescending(m => m.Value).First().Key;
        }

        private decimal CalculateProjectCompletionPercentage(Project project)
        {
            // This is a simplified calculation - you may want to enhance this based on your business logic
            if (project.Status == ProjectStatus.Completed) return 100;
            if (project.Status == ProjectStatus.Active) return 50; // Assume midway for active projects
            return 0;
        }
    }
}
