using Microsoft.EntityFrameworkCore;
using Application.Interfaces.DrillingOperations;
using Domain.Entities.DrillingOperations;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.DrillingOperations
{
    public class ExplosiveCalculationResultRepository : IExplosiveCalculationResultRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExplosiveCalculationResultRepository> _logger;

        public ExplosiveCalculationResultRepository(
            ApplicationDbContext context, 
            ILogger<ExplosiveCalculationResultRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ExplosiveCalculationResult> AddAsync(ExplosiveCalculationResult calculationResult)
        {
            try
            {
                _context.ExplosiveCalculationResults.Add(calculationResult);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Added explosive calculation result with ID: {Id}", calculationResult.Id);
                return calculationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding explosive calculation result");
                throw;
            }
        }

        public async Task<ExplosiveCalculationResult?> GetByIdAsync(int id)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Project)
                .Include(e => e.Site)
                .Include(e => e.PatternSettings)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ExplosiveCalculationResult?> GetByCalculationIdAsync(string calculationId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Project)
                .Include(e => e.Site)
                .Include(e => e.PatternSettings)
                .FirstOrDefaultAsync(e => e.CalculationId == calculationId);
        }

        public async Task<List<ExplosiveCalculationResult>> GetAllAsync(int projectId, int siteId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Project)
                .Include(e => e.Site)
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId && e.SiteId == siteId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<ExplosiveCalculationResult> UpdateAsync(ExplosiveCalculationResult calculationResult)
        {
            try
            {
                calculationResult.MarkUpdated();
                _context.ExplosiveCalculationResults.Update(calculationResult);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated explosive calculation result with ID: {Id}", calculationResult.Id);
                return calculationResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating explosive calculation result with ID: {Id}", calculationResult.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var calculationResult = await _context.ExplosiveCalculationResults.FindAsync(id);
                if (calculationResult == null)
                {
                    return false;
                }

                _context.ExplosiveCalculationResults.Remove(calculationResult);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted explosive calculation result with ID: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation result with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteByCalculationIdAsync(string calculationId)
        {
            try
            {
                var calculationResult = await _context.ExplosiveCalculationResults
                    .FirstOrDefaultAsync(e => e.CalculationId == calculationId);
                    
                if (calculationResult == null)
                {
                    return false;
                }

                _context.ExplosiveCalculationResults.Remove(calculationResult);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted explosive calculation result with CalculationId: {CalculationId}", calculationId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation result with CalculationId: {CalculationId}", calculationId);
                throw;
            }
        }

        public async Task<bool> DeleteAllAsync(int projectId, int siteId)
        {
            try
            {
                var calculationResults = await _context.ExplosiveCalculationResults
                    .Where(e => e.ProjectId == projectId && e.SiteId == siteId)
                    .ToListAsync();

                if (!calculationResults.Any())
                {
                    return false;
                }

                _context.ExplosiveCalculationResults.RemoveRange(calculationResults);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted {Count} explosive calculation results for Project: {ProjectId}, Site: {SiteId}", 
                    calculationResults.Count, projectId, siteId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive calculation results for Project: {ProjectId}, Site: {SiteId}", 
                    projectId, siteId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ExplosiveCalculationResults
                .AnyAsync(e => e.Id == id);
        }

        public async Task<bool> ExistsByCalculationIdAsync(string calculationId)
        {
            return await _context.ExplosiveCalculationResults
                .AnyAsync(e => e.CalculationId == calculationId);
        }

        public async Task<int> GetCountAsync(int projectId, int siteId)
        {
            return await _context.ExplosiveCalculationResults
                .CountAsync(e => e.ProjectId == projectId && e.SiteId == siteId);
        }

        public async Task<List<ExplosiveCalculationResult>> GetByProjectAsync(int projectId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Site)
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ExplosiveCalculationResult>> GetBySiteAsync(int projectId, int siteId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId && e.SiteId == siteId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ExplosiveCalculationResult>> GetByPatternSettingsAsync(int patternSettingsId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Project)
                .Include(e => e.Site)
                .Where(e => e.PatternSettingsId == patternSettingsId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ExplosiveCalculationResult>> GetByUserAsync(int userId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.Project)
                .Include(e => e.Site)
                .Include(e => e.PatternSettings)
                .Where(e => e.OwningUserId == userId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<ExplosiveCalculationResult?> GetLatestByProjectSiteAsync(int projectId, int siteId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId && e.SiteId == siteId)
                .OrderByDescending(e => e.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ExplosiveCalculationResult>> GetByDateRangeAsync(int projectId, int siteId, DateTime startDate, DateTime endDate)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId && 
                           e.SiteId == siteId && 
                           e.CreatedAt >= startDate && 
                           e.CreatedAt <= endDate)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ExplosiveCalculationResult>> GetWithDrillPointsAsync(int projectId, int siteId)
        {
            return await _context.ExplosiveCalculationResults
                .Include(e => e.PatternSettings)
                .Where(e => e.ProjectId == projectId && e.SiteId == siteId)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }


    }
}