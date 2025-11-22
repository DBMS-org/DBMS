using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.MachineManagement;

namespace Application.Interfaces.MachineManagement
{
    public interface IUsageLogRepository
    {
        Task<MachineUsageLog> CreateAsync(MachineUsageLog log);
        Task<MachineUsageLog?> GetByIdAsync(int id);
        Task<IEnumerable<MachineUsageLog>> GetByMachineIdAsync(int machineId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<MachineUsageLog>> GetByOperatorIdAsync(int operatorId, DateTime? startDate, DateTime? endDate);
        Task<MachineUsageLog?> GetLatestByMachineIdAsync(int machineId);
        Task<MachineUsageLog> UpdateAsync(MachineUsageLog log);
        Task<bool> DeleteAsync(int id);
    }
}
