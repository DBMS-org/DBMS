using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DrillHoleRepository : IDrillHoleRepository
    {
        private readonly ApplicationDbContext _context;

        public DrillHoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DrillHole>> GetAllAsync()
        {
            return await _context.DrillHoles
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<DrillHole?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _context.DrillHoles
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DrillHole> AddAsync(DrillHole drillHole)
        {
            if (drillHole == null)
                throw new ArgumentNullException(nameof(drillHole));

            _context.DrillHoles.Add(drillHole);
            await _context.SaveChangesAsync();
            return drillHole;
        }

        public async Task UpdateAsync(DrillHole drillHole)
        {
            if (drillHole == null)
                throw new ArgumentNullException(nameof(drillHole));

            _context.DrillHoles.Update(drillHole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            var drillHole = await _context.DrillHoles
                .FirstOrDefaultAsync(d => d.Id == id);
            
            if (drillHole != null)
            {
                _context.DrillHoles.Remove(drillHole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return await _context.DrillHoles
                .AnyAsync(d => d.Id == id);
        }

        public async Task AddRangeAsync(IEnumerable<DrillHole> drillHoles)
        {
            if (drillHoles == null || !drillHoles.Any())
                return;

            await _context.DrillHoles.AddRangeAsync(drillHoles);
            await _context.SaveChangesAsync();
        }

        public async Task ClearAllAsync()
        {
            var drillHoles = await _context.DrillHoles.ToListAsync();
            if (drillHoles.Any())
            {
                _context.DrillHoles.RemoveRange(drillHoles);
                await _context.SaveChangesAsync();
            }
        }

        // Additional helper methods for better performance and functionality
        public async Task<IEnumerable<DrillHole>> GetByProjectIdAsync(int projectId)
        {
            return await _context.DrillHoles
                .Where(d => d.ProjectId == projectId)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<DrillHole>> GetBySiteIdAsync(int projectId, int siteId)
        {
            return await _context.DrillHoles
                .Where(d => d.ProjectId == projectId && d.SiteId == siteId)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task DeleteByProjectIdAsync(int projectId)
        {
            var drillHoles = await _context.DrillHoles
                .Where(d => d.ProjectId == projectId)
                .ToListAsync();
            
            if (drillHoles.Any())
            {
                _context.DrillHoles.RemoveRange(drillHoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBySiteIdAsync(int projectId, int siteId)
        {
            var drillHoles = await _context.DrillHoles
                .Where(d => d.ProjectId == projectId && d.SiteId == siteId)
                .ToListAsync();
            
            if (drillHoles.Any())
            {
                _context.DrillHoles.RemoveRange(drillHoles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.DrillHoles.CountAsync();
        }

        public async Task<int> GetCountByProjectIdAsync(int projectId)
        {
            return await _context.DrillHoles
                .CountAsync(d => d.ProjectId == projectId);
        }

        public async Task<int> GetCountBySiteIdAsync(int projectId, int siteId)
        {
            return await _context.DrillHoles
                .CountAsync(d => d.ProjectId == projectId && d.SiteId == siteId);
        }
    }
} 
