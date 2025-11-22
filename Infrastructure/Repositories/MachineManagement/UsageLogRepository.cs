using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.MachineManagement;
using Domain.Entities.MachineManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.MachineManagement
{
    public class UsageLogRepository : IUsageLogRepository
    {
        private readonly ApplicationDbContext _context;

        public UsageLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MachineUsageLog> CreateAsync(MachineUsageLog log)
        {
            await _context.MachineUsageLogs.AddAsync(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<MachineUsageLog?> GetByIdAsync(int id)
        {
            return await _context.MachineUsageLogs
                .Include(l => l.Machine)
                .Include(l => l.Operator)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<MachineUsageLog>> GetByMachineIdAsync(
            int machineId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.MachineUsageLogs
                .Include(l => l.Machine)
                .Include(l => l.Operator)
                .Where(l => l.MachineId == machineId);

            if (startDate.HasValue)
            {
                query = query.Where(l => l.LogDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.LogDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MachineUsageLog>> GetByOperatorIdAsync(
            int operatorId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.MachineUsageLogs
                .Include(l => l.Machine)
                .Include(l => l.Operator)
                .Where(l => l.OperatorId == operatorId);

            if (startDate.HasValue)
            {
                query = query.Where(l => l.LogDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.LogDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();
        }

        public async Task<MachineUsageLog?> GetLatestByMachineIdAsync(int machineId)
        {
            return await _context.MachineUsageLogs
                .Include(l => l.Machine)
                .Include(l => l.Operator)
                .Where(l => l.MachineId == machineId)
                .OrderByDescending(l => l.LogDate)
                .FirstOrDefaultAsync();
        }

        public async Task<MachineUsageLog> UpdateAsync(MachineUsageLog log)
        {
            _context.MachineUsageLogs.Update(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _context.MachineUsageLogs.FindAsync(id);
            if (log == null)
                return false;

            _context.MachineUsageLogs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
