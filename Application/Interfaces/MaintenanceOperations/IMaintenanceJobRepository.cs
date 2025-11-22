using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;

namespace Application.Interfaces.MaintenanceOperations
{
    public interface IMaintenanceJobRepository
    {
        // Basic CRUD operations
        Task<MaintenanceJob?> GetByIdAsync(int id);
        Task<MaintenanceJob> CreateAsync(MaintenanceJob job);
        Task<bool> UpdateAsync(MaintenanceJob job);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Query operations with details
        Task<MaintenanceJob?> GetWithDetailsAsync(int id); // Include Machine, Project, Report, Assignments
        Task<MaintenanceJob?> GetByReportIdAsync(int reportId);

        // Region-based filtering (CRITICAL for security)
        Task<IEnumerable<MaintenanceJob>> GetByEngineerIdAsync(int engineerId);
        Task<IEnumerable<MaintenanceJob>> GetByRegionAsync(int regionId);
        Task<IEnumerable<MaintenanceJob>> GetByEngineerAndRegionAsync(int engineerId, int regionId);

        // Machine and project queries
        Task<IEnumerable<MaintenanceJob>> GetByMachineIdAsync(int machineId);
        Task<IEnumerable<MaintenanceJob>> GetByProjectIdAsync(int projectId);

        // Status and type filtering
        Task<IEnumerable<MaintenanceJob>> GetByStatusAsync(MaintenanceJobStatus status);
        Task<IEnumerable<MaintenanceJob>> GetByTypeAsync(MaintenanceType type);
        Task<IEnumerable<MaintenanceJob>> GetByStatusAndRegionAsync(MaintenanceJobStatus status, int regionId);

        // Date range queries
        Task<IEnumerable<MaintenanceJob>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceJob>> GetByDateRangeAndRegionAsync(DateTime startDate, DateTime endDate, int regionId);

        // Priority and scheduling
        Task<IEnumerable<MaintenanceJob>> GetOverdueJobsAsync();
        Task<IEnumerable<MaintenanceJob>> GetOverdueJobsByRegionAsync(int regionId);
        Task<IEnumerable<MaintenanceJob>> GetJobsByPriorityAsync(MaintenanceType type);
        Task<IEnumerable<MaintenanceJob>> GetScheduledJobsAsync();

        // Workload calculation for assignment
        Task<Dictionary<int, int>> GetEngineerWorkloadByRegionAsync(int regionId);
        Task<int> GetActiveJobCountByEngineerAsync(int engineerId);

        // Bulk operations
        Task<bool> BulkUpdateStatusAsync(IEnumerable<int> jobIds, MaintenanceJobStatus status);
        Task<bool> BulkAssignEngineerAsync(IEnumerable<int> jobIds, int engineerId);

        // Statistics
        Task<int> GetCountAsync();
        Task<int> GetCountByStatusAsync(MaintenanceJobStatus status);
        Task<int> GetCountByEngineerAsync(int engineerId);
        Task<int> GetCountByRegionAsync(int regionId);

        // Maintenance history
        Task<IEnumerable<MaintenanceJob>> GetMaintenanceHistoryByMachineIdAsync(int machineId, int limit = 10);

        // Service alert jobs
        Task<MaintenanceJob?> GetPendingServiceJobByMachineAsync(int machineId, string serviceType);
        Task<bool> CancelPendingServiceJobAsync(int machineId, string serviceType, string reason);

        // Get all jobs with optional filters
        Task<IEnumerable<MaintenanceJob>> GetAllJobsAsync(string? status = null, string? type = null, int? machineId = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
