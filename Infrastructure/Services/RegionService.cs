using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class RegionService : IRegionService
    {
        private readonly ApplicationDbContext _context;

        public RegionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegionDto>> GetAllRegionsAsync()
        {
            return await _context.Regions
                .Where(r => r.IsActive)
                .Select(r => new RegionDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<RegionDto?> GetRegionByIdAsync(int id)
        {
            var region = await _context.Regions
                .Where(r => r.Id == id && r.IsActive)
                .FirstOrDefaultAsync();

            if (region == null) return null;

            return new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                IsActive = region.IsActive,
                CreatedAt = region.CreatedAt,
                UpdatedAt = region.UpdatedAt
            };
        }

        public async Task<RegionDto?> GetRegionByNameAsync(string name)
        {
            var region = await _context.Regions
                .Where(r => r.Name.ToLower() == name.ToLower() && r.IsActive)
                .FirstOrDefaultAsync();

            if (region == null) return null;

            return new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                IsActive = region.IsActive,
                CreatedAt = region.CreatedAt,
                UpdatedAt = region.UpdatedAt
            };
        }
    }
} 