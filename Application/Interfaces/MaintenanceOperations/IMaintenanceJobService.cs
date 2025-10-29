using Application.DTOs.MaintenanceOperations;
using Application.DTOs.Shared;

namespace Application.Interfaces.MaintenanceOperations
{
    public interface IMaintenanceJobService
    {
        Task<Result<MaintenanceJobDto>> CreateJobFromReportAsync(int reportId);
        Task<Result<MaintenanceJobDto>> CreateManualJobAsync(CreateManualJobRequest request, int createdBy);
        Task<Result<MaintenanceJobDto>> GetJobByIdAsync(int id);
        Task<Result<IEnumerable<MaintenanceJobDto>>> GetEngineerJobsAsync(int engineerId, int? regionId = null);
        Task<Result<MaintenanceJobDto>> UpdateJobStatusAsync(int id, UpdateJobStatusRequest request);
        Task<Result<MaintenanceJobDto>> CompleteJobAsync(int id, CompleteMaintenanceJobRequest request);
        Task<Result<MaintenanceStatsDto>> GetMaintenanceStatsAsync(int? regionId = null);
        Task<Result<IEnumerable<MaintenanceJobDto>>> GetOverdueJobsAsync(int? regionId = null);
        Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> jobIds, string status);
        Task<Result<bool>> BulkAssignEngineerAsync(IEnumerable<int> jobIds, int engineerId);
    }
}
