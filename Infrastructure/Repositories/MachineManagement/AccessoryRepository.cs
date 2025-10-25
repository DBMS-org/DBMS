using Application.Interfaces.MachineManagement;
using Domain.Entities.MachineManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.MachineManagement
{
    public class AccessoryRepository : IAccessoryRepository
    {
        private readonly ApplicationDbContext _context;

        public AccessoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accessory>> GetAllAsync()
        {
            return await _context.Accessories
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Accessory?> GetByIdAsync(int id)
        {
            return await _context.Accessories
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Accessory?> GetByIdWithHistoryAsync(int id)
        {
            return await _context.Accessories
                .Include(a => a.StockAdjustments.OrderByDescending(sa => sa.AdjustedDate))
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsByPartNumberAsync(string partNumber, int? excludeId = null)
        {
            var query = _context.Accessories.Where(a => a.PartNumber == partNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Accessory>> SearchAsync(
            string? searchTerm = null,
            AccessoryCategory? category = null,
            string? supplier = null,
            string? status = null)
        {
            var query = _context.Accessories.AsQueryable();

            // Search term filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(term) ||
                    a.PartNumber.ToLower().Contains(term) ||
                    (a.Description != null && a.Description.ToLower().Contains(term)) ||
                    a.Supplier.ToLower().Contains(term));
            }

            // Category filter
            if (category.HasValue)
            {
                query = query.Where(a => a.Category == category.Value);
            }

            // Supplier filter
            if (!string.IsNullOrWhiteSpace(supplier))
            {
                query = query.Where(a => a.Supplier == supplier);
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                switch (status.ToLower())
                {
                    case "available":
                        query = query.Where(a => a.Quantity > a.MinStockLevel);
                        break;
                    case "low-stock":
                    case "lowstock":
                        query = query.Where(a => a.Quantity <= a.MinStockLevel && a.Quantity > 0);
                        break;
                    case "out-of-stock":
                    case "outofstock":
                        query = query.Where(a => a.Quantity == 0);
                        break;
                }
            }

            return await query.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<Accessory> AddAsync(Accessory accessory)
        {
            await _context.Accessories.AddAsync(accessory);
            await _context.SaveChangesAsync();
            return accessory;
        }

        public async Task UpdateAsync(Accessory accessory)
        {
            _context.Accessories.Update(accessory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Accessory accessory)
        {
            _context.Accessories.Remove(accessory);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Accessories.CountAsync();
        }

        public async Task<int> CountLowStockAsync()
        {
            return await _context.Accessories
                .CountAsync(a => a.Quantity <= a.MinStockLevel && a.Quantity > 0);
        }

        public async Task<int> CountOutOfStockAsync()
        {
            return await _context.Accessories
                .CountAsync(a => a.Quantity == 0);
        }

        public async Task<int> CountAvailableAsync()
        {
            return await _context.Accessories
                .CountAsync(a => a.Quantity > a.MinStockLevel);
        }
    }
}
