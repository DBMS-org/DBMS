using Microsoft.EntityFrameworkCore;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Infrastructure.Data;

namespace Infrastructure.Repositories.BlastingOperations
{
    public class SiteBlastingRepository : ISiteBlastingRepository
    {
        private readonly ApplicationDbContext _context;

        public SiteBlastingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAllWorkflowDataAsync(int projectId, int siteId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Delete all SiteBlastingData
                var siteData = await _context.SiteBlastingData
                    .Where(sd => sd.ProjectId == projectId && sd.SiteId == siteId)
                    .ToListAsync();

                var hasData = siteData.Any();

                if (siteData.Any())
                    _context.SiteBlastingData.RemoveRange(siteData);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return hasData;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> HasWorkflowDataAsync(int projectId, int siteId)
        {
            return await _context.SiteBlastingData
                .AnyAsync(sd => sd.ProjectId == projectId && sd.SiteId == siteId);
        }

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            return await _context.ProjectSites
                .AnyAsync(ps => ps.ProjectId == projectId && ps.Id == siteId);
        }
    }
} 