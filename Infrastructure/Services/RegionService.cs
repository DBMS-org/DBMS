using Application.DTOs;
using Domain.Entities;
using Application.Interfaces;
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

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await _context.Regions
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(int id)
        {
            return await _context.Regions
                .Where(r => r.Id == id && r.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Region?> GetRegionByNameAsync(string name)
        {
            return await _context.Regions
                .Where(r => r.Name.ToLower() == name.ToLower() && r.IsActive)
                .FirstOrDefaultAsync();
        }
    }
} 
