using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.MachineManagement;
using Application.DTOs.Shared;

namespace Application.Interfaces.MachineManagement
{
    public interface IUsageLogService
    {
        // CREATE - Called by: Operator
        Task<Result<UsageLogDto>> CreateUsageLogAsync(CreateUsageLogRequest request, int operatorId);

        // READ - Called by: All roles (filtered by role)
        Task<Result<UsageLogDto>> GetUsageLogByIdAsync(int id);
        Task<Result<IEnumerable<UsageLogDto>>> GetUsageLogsByMachineAsync(
            int machineId,
            DateTime? startDate,
            DateTime? endDate);
        Task<Result<IEnumerable<UsageLogDto>>> GetUsageLogsByOperatorAsync(
            int operatorId,
            DateTime? startDate,
            DateTime? endDate);
        Task<Result<UsageLogDto>> GetLatestUsageLogByMachineAsync(int machineId);

        // STATISTICS - Called by: Operator (own machine), Machine Manager (all machines)
        Task<Result<UsageStatisticsDto>> GetUsageStatisticsAsync(int machineId, int days = 30);

        // APPROVAL - Called by: Machine Manager
        Task<Result<bool>> ApproveUsageLogAsync(int logId, int approvedBy);
        Task<Result<bool>> RejectUsageLogAsync(int logId, int rejectedBy);
    }
}
