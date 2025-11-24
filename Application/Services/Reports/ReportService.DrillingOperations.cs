using Application.DTOs.Reports;
using Domain.Entities.BlastingOperations;
using Domain.Entities.DrillingOperations;
using Domain.Entities.ProjectManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Drilling Operations Report implementation - PHASE 3
    /// </summary>
    public partial class ReportService
    {
        public async Task<DrillingOperationsReportDto> GetDrillingOperationsReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Drilling Operations Report");

                var report = new DrillingOperationsReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate,
                    EndDate = filter?.EndDate,
                    RegionFilter = filter?.RegionId
                };

                // Get all drilling data
                var drillHolesEnumerable = await _drillHoleRepository.GetAllAsync();
                var drillHoles = drillHolesEnumerable.ToList();

                // Get all projects to iterate through
                var allProjects = await _projectRepository.GetAllAsync();
                var projects = allProjects.ToList();

                // Collect all drill points, blast connections, and explosive calculations
                var drillPoints = new List<DrillPoint>();
                var blastConnections = new List<BlastConnection>();
                var explosiveCalcs = new List<ExplosiveCalculationResult>();
                var projectSites = new List<ProjectSite>();

                foreach (var project in projects)
                {
                    var sites = await _projectRepository.GetProjectSitesAsync(project.Id);
                    foreach (var site in sites)
                    {
                        projectSites.Add(site);

                        // Get drill points for this project/site
                        var sitePoints = await _drillPointRepository.GetAllAsync(project.Id, site.Id);
                        drillPoints.AddRange(sitePoints);

                        // Get blast connections for this project/site
                        var siteConnections = await _blastConnectionRepository.GetByProjectAndSiteAsync(project.Id, site.Id);
                        blastConnections.AddRange(siteConnections);

                        // Get explosive calculations for this project/site
                        var siteCalcs = await _explosiveCalculationResultRepository.GetAllAsync(project.Id, site.Id);
                        explosiveCalcs.AddRange(siteCalcs);
                    }
                }

                // Calculate summary statistics
                report.Statistics = new DrillingStatisticsDto
                {
                    TotalDrillHoles = drillHoles.Count,
                    TotalDrillPoints = drillPoints.Count,
                    TotalBlastConnections = blastConnections.Count,
                    TotalProjectSites = projectSites.Count,
                    AverageDrillDepth = drillHoles.Any() ? (decimal)drillHoles.Average(h => h.Depth) : 0,
                    TotalDrillingMeters = (decimal)drillHoles.Sum(h => h.Depth),
                    ActivePatterns = drillPoints.GroupBy(p => p.SiteId).Count()
                };

                // Drilling by Project Site
                report.DrillingByProjectSite = projectSites.Select(site =>
                {
                    var siteHoles = drillHoles.Where(h => h.SiteId == site.Id).ToList();
                    var sitePoints = drillPoints.Where(p => p.SiteId == site.Id).ToList();
                    var siteConnections = blastConnections.Where(c => c.SiteId == site.Id).ToList();

                    return new ProjectSiteDrillingDto
                    {
                        ProjectSiteId = site.Id,
                        ProjectSiteName = site.Name,
                        ProjectName = site.Project?.Name ?? "Unknown",
                        DrillHoleCount = siteHoles.Count,
                        DrillPointCount = sitePoints.Count,
                        TotalDepth = (decimal)siteHoles.Sum(h => h.Depth),
                        AverageDepth = siteHoles.Any() ? (decimal)siteHoles.Average(h => h.Depth) : 0,
                        BlastConnectionCount = siteConnections.Count,
                        Status = site.Status.ToString()
                    };
                }).ToList();

                // Drilling by Region
                var regionGroups = projectSites
                    .Where(s => !string.IsNullOrEmpty(s.Project?.Region))
                    .GroupBy(s => s.Project.Region);

                report.DrillingByRegion = regionGroups.Select(regionGroup =>
                {
                    var regionSiteIds = regionGroup.Select(s => s.Id).ToList();
                    var regionHoles = drillHoles.Where(h => regionSiteIds.Contains(h.SiteId)).ToList();
                    var regionPoints = drillPoints.Where(p => regionSiteIds.Contains(p.SiteId)).ToList();

                    return new RegionalDrillingDto
                    {
                        RegionName = regionGroup.Key,
                        TotalDrillHoles = regionHoles.Count,
                        TotalDrillPoints = regionPoints.Count,
                        ProjectSiteCount = regionGroup.Count(),
                        TotalDepth = (decimal)regionHoles.Sum(h => h.Depth),
                        AverageDepth = regionHoles.Any() ? (decimal)regionHoles.Average(h => h.Depth) : 0
                    };
                }).ToList();

                // Blast Connections Summary
                report.BlastConnections = blastConnections.Select(conn =>
                {
                    var projectSite = projectSites.FirstOrDefault(s => s.Id == conn.SiteId);

                    return new BlastConnectionSummaryDto
                    {
                        ConnectionId = conn.Id,
                        ConnectorType = conn.ConnectorType.ToString(),
                        Point1DrillPointId = conn.Point1DrillPointId,
                        Point2DrillPointId = conn.Point2DrillPointId,
                        Delay = conn.Delay,
                        Sequence = conn.Sequence,
                        ProjectSiteName = projectSite?.Name ?? "Unknown"
                    };
                }).ToList();

                // Explosive Calculations Summary
                report.ExplosiveCalculations = explosiveCalcs.Select(calc =>
                {
                    var projectSite = projectSites.FirstOrDefault(s => s.Id == calc.SiteId);

                    return new ExplosiveCalculationSummaryDto
                    {
                        CalculationId = calc.CalculationId,
                        ProjectSiteName = projectSite?.Name ?? "Unknown",
                        TotalAnfo = (decimal)calc.TotalAnfo,
                        TotalEmulsion = (decimal)calc.TotalEmulsion,
                        TotalExplosive = (decimal)(calc.TotalAnfo + calc.TotalEmulsion),
                        NumberOfFilledHoles = calc.NumberOfFilledHoles,
                        CalculationDate = calc.CreatedAt
                    };
                }).ToList();

                // All Drill Holes Detail
                report.AllDrillHoles = drillHoles.Select(hole =>
                {
                    var projectSite = projectSites.FirstOrDefault(s => s.Id == hole.SiteId);

                    return new DrillHoleDetailDto
                    {
                        HoleId = hole.Id,
                        HoleName = hole.Name,
                        Depth = (decimal)hole.Depth,
                        Easting = (decimal)hole.Easting,
                        Northing = (decimal)hole.Northing,
                        Elevation = (decimal)hole.Elevation,
                        Azimuth = hole.Azimuth.HasValue ? (decimal)hole.Azimuth.Value : null,
                        Dip = hole.Dip.HasValue ? (decimal)hole.Dip.Value : null,
                        ProjectSiteName = projectSite?.Name ?? "Unknown",
                        CreatedDate = hole.CreatedAt
                    };
                }).OrderByDescending(h => h.CreatedDate).ToList();

                // All Drill Points Detail
                report.AllDrillPoints = drillPoints.Select(point =>
                {
                    var projectSite = projectSites.FirstOrDefault(s => s.Id == point.SiteId);

                    return new DrillPointDetailDto
                    {
                        PointId = point.Id,
                        X = (decimal)point.X,
                        Y = (decimal)point.Y,
                        Depth = (decimal)point.Depth,
                        Diameter = (decimal)point.Diameter,
                        ProjectSiteName = projectSite?.Name ?? "Unknown",
                        CreatedDate = point.CreatedAt
                    };
                }).OrderByDescending(p => p.CreatedDate).ToList();

                _logger.LogInformation("Drilling Operations Report generated successfully with {DrillHoleCount} drill holes and {DrillPointCount} drill points",
                    drillHoles.Count, drillPoints.Count);

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Drilling Operations Report");
                throw;
            }
        }
    }
}
