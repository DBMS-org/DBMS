using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.BlastingOperations
{
    public class BlastConnectionRepository : IBlastConnectionRepository
    {
        private readonly ApplicationDbContext _context;

        public BlastConnectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlastConnection>> GetByProjectAndSiteAsync(int projectId, int siteId)
        {
            return await _context.BlastConnections
                .Where(bc => bc.ProjectId == projectId && bc.SiteId == siteId)
                .Include(bc => bc.Point1DrillPoint)
                .Include(bc => bc.Point2DrillPoint)
                .Include(bc => bc.Project)
                .Include(bc => bc.Site)
                .OrderBy(bc => bc.Sequence)
                .ToListAsync();
        }

        public async Task<BlastConnection?> GetByIdAsync(string id, int projectId, int siteId)
        {
            return await _context.BlastConnections
                .Where(bc => bc.Id == id && bc.ProjectId == projectId && bc.SiteId == siteId)
                .Include(bc => bc.Point1DrillPoint)
                .Include(bc => bc.Point2DrillPoint)
                .Include(bc => bc.Project)
                .Include(bc => bc.Site)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BlastConnection>> GetBySequenceAsync(int projectId, int siteId, int sequence)
        {
            return await _context.BlastConnections
                .Where(bc => bc.ProjectId == projectId && bc.SiteId == siteId && bc.Sequence == sequence)
                .Include(bc => bc.Point1DrillPoint)
                .Include(bc => bc.Point2DrillPoint)
                .Include(bc => bc.Project)
                .Include(bc => bc.Site)
                .ToListAsync();
        }

        public async Task<BlastConnection> CreateAsync(BlastConnection blastConnection)
        {
            blastConnection.CreatedAt = DateTime.UtcNow;
            blastConnection.UpdatedAt = DateTime.UtcNow;
            
            _context.BlastConnections.Add(blastConnection);
            await _context.SaveChangesAsync();
            
            return blastConnection;
        }

        public async Task<BlastConnection> UpdateAsync(BlastConnection blastConnection)
        {
            blastConnection.UpdatedAt = DateTime.UtcNow;
            
            _context.BlastConnections.Update(blastConnection);
            await _context.SaveChangesAsync();
            
            return blastConnection;
        }

        public async Task DeleteAsync(string id, int projectId, int siteId)
        {
            var blastConnection = await GetByIdAsync(id, projectId, siteId);
            if (blastConnection != null)
            {
                _context.BlastConnections.Remove(blastConnection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string id, int projectId, int siteId)
        {
            return await _context.BlastConnections
                .AnyAsync(bc => bc.Id == id && bc.ProjectId == projectId && bc.SiteId == siteId);
        }
    }
} 