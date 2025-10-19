using Microsoft.EntityFrameworkCore;
using Application.Interfaces.DrillingOperations;
using Domain.Entities.DrillingOperations;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.DrillingOperations
{
    public class DrillPointRepository : IDrillPointRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DrillPointRepository> _logger;

        public DrillPointRepository(ApplicationDbContext context, ILogger<DrillPointRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<DrillPoint> AddAsync(DrillPoint drillPoint)
        {
            _context.DrillPoints.Add(drillPoint);
            await _context.SaveChangesAsync();
            return drillPoint;
        }

        public async Task<DrillPoint?> GetByIdAsync(string id, int projectId, int siteId)
        {
            return await _context.DrillPoints
                .FirstOrDefaultAsync(dp => dp.Id == id && dp.ProjectId == projectId && dp.SiteId == siteId);
        }

        public async Task<List<DrillPoint>> GetAllAsync(int projectId, int siteId)
        {
            return await _context.DrillPoints
                .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId)
                .OrderBy(dp => dp.Id)
                .ToListAsync();
        }

        public async Task<DrillPoint> UpdateAsync(DrillPoint drillPoint)
        {
            _context.DrillPoints.Update(drillPoint);
            await _context.SaveChangesAsync();
            return drillPoint;
        }

        public async Task<bool> DeleteAsync(string id, int projectId, int siteId)
        {
            var drillPoint = await GetByIdAsync(id, projectId, siteId);
            if (drillPoint == null)
                return false;

            _context.DrillPoints.Remove(drillPoint);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllAsync(int projectId, int siteId)
        {
            try
            {
                var drillPoints = await _context.DrillPoints
                    .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId)
                    .ToListAsync();

                if (drillPoints.Any())
                {
                    _context.DrillPoints.RemoveRange(drillPoints);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Deleted {Count} drill points for project {ProjectId}, site {SiteId}", 
                        drillPoints.Count, projectId, siteId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all drill points for project {ProjectId}, site {SiteId}", projectId, siteId);
                return false;
            }
        }

        public async Task<bool> DeleteByProjectSiteAsync(int projectId, int siteId)
        {
            try
            {
                var drillPoints = await _context.DrillPoints
                    .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId)
                    .ToListAsync();

                if (drillPoints.Any())
                {
                    _context.DrillPoints.RemoveRange(drillPoints);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Deleted {Count} drill points for project {ProjectId}, site {SiteId}", 
                        drillPoints.Count, projectId, siteId);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drill points by project/site for project {ProjectId}, site {SiteId}", projectId, siteId);
                return false;
            }
        }

        public async Task<List<DrillPoint>> AddRangeAsync(IEnumerable<DrillPoint> drillPoints)
        {
            var pointsList = drillPoints.ToList();
            _context.DrillPoints.AddRange(pointsList);
            await _context.SaveChangesAsync();
            return pointsList;
        }

        public async Task<bool> ExistsAsync(string id, int projectId, int siteId)
        {
            return await _context.DrillPoints
                .AnyAsync(dp => dp.Id == id && dp.ProjectId == projectId && dp.SiteId == siteId);
        }

        public async Task<bool> ExistsAtCoordinatesAsync(double x, double y, int projectId, int siteId, string? excludePointId = null)
        {
            var query = _context.DrillPoints
                .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId && 
                           Math.Abs(dp.X - x) < 0.01 && Math.Abs(dp.Y - y) < 0.01);

            if (!string.IsNullOrEmpty(excludePointId))
            {
                query = query.Where(dp => dp.Id != excludePointId);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetCountAsync(int projectId, int siteId)
        {
            return await _context.DrillPoints
                .CountAsync(dp => dp.ProjectId == projectId && dp.SiteId == siteId);
        }

        public async Task<List<DrillPoint>> GetByCoordinateRangeAsync(int projectId, int siteId, double minX, double maxX, double minY, double maxY)
        {
            return await _context.DrillPoints
                .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId &&
                           dp.X >= minX && dp.X <= maxX &&
                           dp.Y >= minY && dp.Y <= maxY)
                .OrderBy(dp => dp.X)
                .ThenBy(dp => dp.Y)
                .ToListAsync();
        }

        public async Task<PatternSettings?> GetPatternSettingsAsync(int projectId, int siteId)
        {
            return await _context.PatternSettings
                .FirstOrDefaultAsync(ps => ps.ProjectId == projectId && ps.SiteId == siteId);
        }

        public async Task<PatternSettings> SavePatternSettingsAsync(int projectId, int siteId, PatternSettings settings)
        {
            var existingSettings = await GetPatternSettingsAsync(projectId, siteId);
            
            if (existingSettings != null)
            {
                existingSettings.Spacing = settings.Spacing;
                existingSettings.Burden = settings.Burden;
                existingSettings.Depth = settings.Depth;
                _context.PatternSettings.Update(existingSettings);
            }
            else
            {
                var newSettings = new PatternSettings
                {
                    ProjectId = projectId,
                    SiteId = siteId,
                    Spacing = settings.Spacing,
                    Burden = settings.Burden,
                    Depth = settings.Depth
                };
                _context.PatternSettings.Add(newSettings);
                existingSettings = newSettings;
            }

            await _context.SaveChangesAsync();
            return existingSettings;
        }
    }
} 