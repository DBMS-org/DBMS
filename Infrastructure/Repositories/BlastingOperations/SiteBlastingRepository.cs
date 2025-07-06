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
                // Delete all BlastSequences
                var blastSequences = await _context.BlastSequences
                    .Where(bs => bs.ProjectId == projectId && bs.SiteId == siteId)
                    .ToListAsync();

                // Delete all SiteBlastingData
                var siteData = await _context.SiteBlastingData
                    .Where(sd => sd.ProjectId == projectId && sd.SiteId == siteId)
                    .ToListAsync();

                var hasData = siteData.Any() || blastSequences.Any();

                if (siteData.Any())
                    _context.SiteBlastingData.RemoveRange(siteData);

                if (blastSequences.Any())
                    _context.BlastSequences.RemoveRange(blastSequences);

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
            var hasBlastSequences = await _context.BlastSequences
                .AnyAsync(bs => bs.ProjectId == projectId && bs.SiteId == siteId);
                
            var hasSiteData = await _context.SiteBlastingData
                .AnyAsync(sd => sd.ProjectId == projectId && sd.SiteId == siteId);

            return hasBlastSequences || hasSiteData;
        }

        public async Task<List<BlastSequence>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            return await _context.BlastSequences
                .Where(bs => bs.ProjectId == projectId && bs.SiteId == siteId && bs.IsActive)
                .Include(bs => bs.CreatedBy)
                .OrderBy(bs => bs.CreatedAt)
                .ToListAsync();
        }

        public async Task<BlastSequence?> GetBlastSequenceAsync(int id)
        {
            return await _context.BlastSequences
                .Include(bs => bs.CreatedBy)
                .FirstOrDefaultAsync(bs => bs.Id == id);
        }

        public async Task<BlastSequence> CreateBlastSequenceAsync(BlastSequence sequence)
        {
            _context.BlastSequences.Add(sequence);
            await _context.SaveChangesAsync();
            return sequence;
        }

        public async Task<BlastSequence> UpdateBlastSequenceAsync(BlastSequence sequence)
        {
            sequence.UpdatedAt = DateTime.UtcNow;
            _context.BlastSequences.Update(sequence);
            await _context.SaveChangesAsync();
            return sequence;
        }

        public async Task<bool> DeleteBlastSequenceAsync(int id)
        {
            var sequence = await _context.BlastSequences.FindAsync(id);
            if (sequence == null)
                return false;

            _context.BlastSequences.Remove(sequence);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            return await _context.ProjectSites
                .AnyAsync(ps => ps.ProjectId == projectId && ps.Id == siteId);
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            return await _context.BlastSequences
                .AnyAsync(bs => bs.Id == sequenceId && bs.ProjectId == projectId && bs.SiteId == siteId);
        }
    }
} 