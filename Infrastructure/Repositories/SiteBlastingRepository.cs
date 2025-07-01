using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class SiteBlastingRepository : ISiteBlastingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SiteBlastingRepository> _logger;

        public SiteBlastingRepository(ApplicationDbContext context, ILogger<SiteBlastingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Site Blasting Data Operations

        public async Task<SiteBlastingData?> GetSiteDataAsync(int projectId, int siteId, string dataType)
        {
            return await _context.SiteBlastingData
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && 
                                       s.SiteId == siteId && 
                                       s.DataType == dataType);
        }

        public async Task<List<SiteBlastingData>> GetAllSiteDataAsync(int projectId, int siteId)
        {
            return await _context.SiteBlastingData
                .Include(s => s.CreatedBy)
                .Where(s => s.ProjectId == projectId && s.SiteId == siteId)
                .ToListAsync();
        }

        public async Task<SiteBlastingData> SaveSiteDataAsync(SiteBlastingData data)
        {
            _context.SiteBlastingData.Add(data);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(data)
                .Reference(s => s.CreatedBy)
                .LoadAsync();
                
            return data;
        }

        public async Task<SiteBlastingData> UpdateSiteDataAsync(SiteBlastingData data)
        {
            _context.SiteBlastingData.Update(data);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(data)
                .Reference(s => s.CreatedBy)
                .LoadAsync();
                
            return data;
        }

        public async Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType)
        {
            var data = await _context.SiteBlastingData
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && 
                                       s.SiteId == siteId && 
                                       s.DataType == dataType);

            if (data == null) return false;

            _context.SiteBlastingData.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId)
        {
            // Delete all SiteBlastingData
            var siteData = await _context.SiteBlastingData
                .Where(s => s.ProjectId == projectId && s.SiteId == siteId)
                .ToListAsync();

            // Delete all DrillPatterns
            var drillPatterns = await _context.DrillPatterns
                .Where(p => p.ProjectId == projectId && p.SiteId == siteId)
                .ToListAsync();

            // Delete all BlastSequences
            var blastSequences = await _context.BlastSequences
                .Where(b => b.ProjectId == projectId && b.SiteId == siteId)
                .ToListAsync();

            var hasData = siteData.Any() || drillPatterns.Any() || blastSequences.Any();

            if (hasData)
            {
                if (siteData.Any())
                    _context.SiteBlastingData.RemoveRange(siteData);
                    
                if (drillPatterns.Any())
                    _context.DrillPatterns.RemoveRange(drillPatterns);
                    
                if (blastSequences.Any())
                    _context.BlastSequences.RemoveRange(blastSequences);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        #endregion

        #region Drill Pattern Operations

        public async Task<List<DrillPattern>> GetDrillPatternsAsync(int projectId, int siteId)
        {
            return await _context.DrillPatterns
                .Include(p => p.CreatedBy)
                .Where(p => p.ProjectId == projectId && p.SiteId == siteId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<DrillPattern?> GetDrillPatternAsync(int id)
        {
            return await _context.DrillPatterns
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<DrillPattern> CreateDrillPatternAsync(DrillPattern pattern)
        {
            _context.DrillPatterns.Add(pattern);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(pattern)
                .Reference(p => p.CreatedBy)
                .LoadAsync();
                
            return pattern;
        }

        public async Task<DrillPattern> UpdateDrillPatternAsync(DrillPattern pattern)
        {
            _context.DrillPatterns.Update(pattern);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(pattern)
                .Reference(p => p.CreatedBy)
                .LoadAsync();
                
            return pattern;
        }

        public async Task<bool> DeleteDrillPatternAsync(int id)
        {
            // Retrieve the drill pattern together with any related blast sequences
            var pattern = await _context.DrillPatterns
                .Include(p => p.BlastSequences)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pattern == null) return false;

            // Remove any blast sequences that reference this pattern first to avoid
            // FK constraint violations (DrillPatternId is configured with DeleteBehavior.Restrict)
            if (pattern.BlastSequences != null && pattern.BlastSequences.Count > 0)
            {
                _context.BlastSequences.RemoveRange(pattern.BlastSequences);
            }

            _context.DrillPatterns.Remove(pattern);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Blast Sequence Operations

        public async Task<List<BlastSequence>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            return await _context.BlastSequences
                .Include(b => b.CreatedBy)
                .Where(b => b.ProjectId == projectId && b.SiteId == siteId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<BlastSequence?> GetBlastSequenceAsync(int id)
        {
            return await _context.BlastSequences
                .Include(b => b.CreatedBy)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlastSequence> CreateBlastSequenceAsync(BlastSequence sequence)
        {
            _context.BlastSequences.Add(sequence);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(sequence)
                .Reference(b => b.CreatedBy)
                .LoadAsync();
                
            return sequence;
        }

        public async Task<BlastSequence> UpdateBlastSequenceAsync(BlastSequence sequence)
        {
            _context.BlastSequences.Update(sequence);
            await _context.SaveChangesAsync();
            
            // Load navigation properties
            await _context.Entry(sequence)
                .Reference(b => b.CreatedBy)
                .LoadAsync();
                
            return sequence;
        }

        public async Task<bool> DeleteBlastSequenceAsync(int id)
        {
            var sequence = await _context.BlastSequences.FindAsync(id);
            if (sequence == null) return false;

            _context.BlastSequences.Remove(sequence);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Validation Operations

        public async Task<bool> ValidateProjectSiteExistsAsync(int projectId, int siteId)
        {
            return await _context.ProjectSites
                .AnyAsync(ps => ps.ProjectId == projectId && ps.Id == siteId);
        }

        public async Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId)
        {
            return await _context.DrillPatterns
                .AnyAsync(p => p.Id == patternId && p.ProjectId == projectId && p.SiteId == siteId);
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            return await _context.BlastSequences
                .AnyAsync(b => b.Id == sequenceId && b.ProjectId == projectId && b.SiteId == siteId);
        }

        #endregion

        #region Workflow Operations

        public async Task<SiteBlastingData?> GetWorkflowProgressDataAsync(int projectId, int siteId)
        {
            return await _context.SiteBlastingData
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.ProjectId == projectId && 
                                       s.SiteId == siteId && 
                                       s.DataType == "workflow_progress");
        }

        public async Task<SiteBlastingData> SaveWorkflowProgressDataAsync(SiteBlastingData data)
        {
            var existingData = await _context.SiteBlastingData
                .FirstOrDefaultAsync(s => s.ProjectId == data.ProjectId && 
                                       s.SiteId == data.SiteId && 
                                       s.DataType == data.DataType);

            if (existingData != null)
            {
                existingData.JsonData = data.JsonData;
                existingData.UpdatedAt = DateTime.UtcNow;
                _context.SiteBlastingData.Update(existingData);
                await _context.SaveChangesAsync();
                return existingData;
            }
            else
            {
                return await SaveSiteDataAsync(data);
            }
        }

        #endregion
    }
} 