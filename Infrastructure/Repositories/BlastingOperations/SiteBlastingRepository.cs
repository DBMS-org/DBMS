using Microsoft.EntityFrameworkCore;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Domain.Entities.DrillingOperations;
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

        // Site Blasting Data Operations
        public async Task<SiteBlastingData?> GetSiteDataAsync(int projectId, int siteId, string dataType)
        {
            return await _context.SiteBlastingData
                .FirstOrDefaultAsync(sd => sd.ProjectId == projectId && sd.SiteId == siteId && sd.DataType == dataType);
        }

        public async Task<List<SiteBlastingData>> GetAllSiteDataAsync(int projectId, int siteId)
        {
            return await _context.SiteBlastingData
                .Where(sd => sd.ProjectId == projectId && sd.SiteId == siteId)
                .ToListAsync();
        }

        public async Task<SiteBlastingData> SaveSiteDataAsync(SiteBlastingData data)
        {
            var existing = await GetSiteDataAsync(data.ProjectId, data.SiteId, data.DataType);

            if (existing != null)
            {
                existing.JsonData = data.JsonData;
                existing.UpdatedAt = DateTime.UtcNow;
                _context.SiteBlastingData.Update(existing);
            }
            else
            {
                data.CreatedAt = DateTime.UtcNow;
                data.UpdatedAt = DateTime.UtcNow;
                await _context.SiteBlastingData.AddAsync(data);
            }

            await _context.SaveChangesAsync();
            return existing ?? data;
        }

        public async Task<SiteBlastingData> UpdateSiteDataAsync(SiteBlastingData data)
        {
            data.UpdatedAt = DateTime.UtcNow;
            _context.SiteBlastingData.Update(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<bool> DeleteSiteDataAsync(int projectId, int siteId, string dataType)
        {
            var data = await GetSiteDataAsync(projectId, siteId, dataType);
            if (data == null) return false;

            _context.SiteBlastingData.Remove(data);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllSiteDataAsync(int projectId, int siteId)
        {
            var allData = await GetAllSiteDataAsync(projectId, siteId);
            if (!allData.Any()) return false;

            _context.SiteBlastingData.RemoveRange(allData);
            await _context.SaveChangesAsync();
            return true;
        }

        // Drill Pattern Operations
        public async Task<List<DrillPattern>> GetDrillPatternsAsync(int projectId, int siteId)
        {
            return await _context.DrillPatterns
                .Where(dp => dp.ProjectId == projectId && dp.SiteId == siteId && dp.IsActive)
                .ToListAsync();
        }

        public async Task<DrillPattern?> GetDrillPatternAsync(int id)
        {
            return await _context.DrillPatterns
                .FirstOrDefaultAsync(dp => dp.Id == id);
        }

        public async Task<DrillPattern> CreateDrillPatternAsync(DrillPattern pattern)
        {
            pattern.CreatedAt = DateTime.UtcNow;
            pattern.UpdatedAt = DateTime.UtcNow;
            await _context.DrillPatterns.AddAsync(pattern);
            await _context.SaveChangesAsync();
            return pattern;
        }

        public async Task<DrillPattern> UpdateDrillPatternAsync(DrillPattern pattern)
        {
            pattern.UpdatedAt = DateTime.UtcNow;
            _context.DrillPatterns.Update(pattern);
            await _context.SaveChangesAsync();
            return pattern;
        }

        public async Task<bool> DeleteDrillPatternAsync(int id)
        {
            var pattern = await GetDrillPatternAsync(id);
            if (pattern == null) return false;

            pattern.IsActive = false;
            _context.DrillPatterns.Update(pattern);
            await _context.SaveChangesAsync();
            return true;
        }

        // Blast Sequence Operations
        public async Task<List<BlastSequence>> GetBlastSequencesAsync(int projectId, int siteId)
        {
            return await _context.BlastSequences
                .Where(bs => bs.ProjectId == projectId && bs.SiteId == siteId && bs.IsActive)
                .ToListAsync();
        }

        public async Task<BlastSequence?> GetBlastSequenceAsync(int id)
        {
            return await _context.BlastSequences
                .FirstOrDefaultAsync(bs => bs.Id == id);
        }

        public async Task<BlastSequence> CreateBlastSequenceAsync(BlastSequence sequence)
        {
            sequence.CreatedAt = DateTime.UtcNow;
            sequence.UpdatedAt = DateTime.UtcNow;
            await _context.BlastSequences.AddAsync(sequence);
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
            var sequence = await GetBlastSequenceAsync(id);
            if (sequence == null) return false;

            sequence.IsActive = false;
            _context.BlastSequences.Update(sequence);
            await _context.SaveChangesAsync();
            return true;
        }

        // Validation Operations
        public async Task<bool> ValidateDrillPatternOwnershipAsync(int patternId, int projectId, int siteId)
        {
            return await _context.DrillPatterns
                .AnyAsync(dp => dp.Id == patternId && dp.ProjectId == projectId && dp.SiteId == siteId);
        }

        public async Task<bool> ValidateBlastSequenceOwnershipAsync(int sequenceId, int projectId, int siteId)
        {
            return await _context.BlastSequences
                .AnyAsync(bs => bs.Id == sequenceId && bs.ProjectId == projectId && bs.SiteId == siteId);
        }

        // Workflow Operations
        public async Task<SiteBlastingData?> GetWorkflowProgressDataAsync(int projectId, int siteId)
        {
            return await GetSiteDataAsync(projectId, siteId, "workflow_progress");
        }

        public async Task<SiteBlastingData> SaveWorkflowProgressDataAsync(SiteBlastingData data)
        {
            data.DataType = "workflow_progress";
            return await SaveSiteDataAsync(data);
        }
    }
} 