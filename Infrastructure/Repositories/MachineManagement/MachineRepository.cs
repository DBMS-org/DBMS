using Application.Interfaces.MachineManagement;
using Domain.Entities.MachineManagement;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.MachineManagement
{
    public class MachineRepository : IMachineRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MachineRepository> _logger;

        public MachineRepository(
            ApplicationDbContext context,
            ILogger<MachineRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Machine?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Machines
                    .Include(m => m.Operator)
                    .Include(m => m.Project)
                    .Include(m => m.Region)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machine {MachineId} from database", id);
                throw;
            }
        }

        public async Task<Machine?> GetByOperatorIdAsync(int operatorId)
        {
            try
            {
                return await _context.Machines
                    .Include(m => m.Operator)
                    .Include(m => m.Project)
                    .Include(m => m.Region)
                    .FirstOrDefaultAsync(m => m.OperatorId == operatorId && m.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machine for operator {OperatorId} from database", operatorId);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Machine machine)
        {
            try
            {
                _context.Machines.Update(machine);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating machine {MachineId} in database", machine.Id);
                throw;
            }
        }

        public async Task<IEnumerable<Machine>> GetByOperatorIdsAsync(IEnumerable<int> operatorIds)
        {
            try
            {
                return await _context.Machines
                    .Where(m => m.OperatorId.HasValue && operatorIds.Contains(m.OperatorId.Value) && m.IsActive)
                    .Include(m => m.Operator)
                    .Include(m => m.Project)
                    .Include(m => m.Region)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting machines for multiple operators from database");
                throw;
            }
        }
    }
}
