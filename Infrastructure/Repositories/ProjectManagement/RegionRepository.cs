using Domain.Entities.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ProjectManagement
{
    public class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _context;

        public RegionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Region>> GetAllActiveRegionsAsync()
        {
            return await _context.Regions
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(int id)
        {
            return await _context.Regions
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<Region?> GetByNameAsync(string name)
        {
            return await _context.Regions
                .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower() && r.IsActive);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<Region> UpdateAsync(Region region)
        {
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
            return region;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null) return false;

            region.Deactivate(); // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 