using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;

namespace Application.Interfaces.MaintenanceOperations
{
    public interface IMaintenanceReportRepository
    {
        // Basic CRUD operations
        Task<MaintenanceReport?> GetByIdAsync(int id);
        Task<MaintenanceReport?> GetByTicketIdAsync(string ticketId);
        Task<MaintenanceReport> CreateAsync(MaintenanceReport report);
        Task<bool> UpdateAsync(MaintenanceReport report);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // Query operations for operators
        Task<IEnumerable<MaintenanceReport>> GetByOperatorIdAsync(int operatorId);
        Task<IEnumerable<MaintenanceReport>> GetByMachineIdAsync(int machineId);
        Task<MaintenanceReport?> GetWithDetailsAsync(int id); // Include Machine, Operator, Engineer

        // Query operations for engineers and admins
        Task<IEnumerable<MaintenanceReport>> GetByStatusAsync(ReportStatus status);
        Task<IEnumerable<MaintenanceReport>> GetBySeverityAsync(SeverityLevel severity);
        Task<IEnumerable<MaintenanceReport>> GetByStatusAndRegionAsync(ReportStatus status, int regionId);
        Task<IEnumerable<MaintenanceReport>> GetUnresolvedReportsAsync();
        Task<IEnumerable<MaintenanceReport>> GetUnresolvedByRegionAsync(int regionId);

        // Statistics and reporting
        Task<int> GetCountAsync();
        Task<int> GetCountByOperatorIdAsync(int operatorId);
        Task<int> GetCountByStatusAsync(ReportStatus status);
        Task<int> GetCountByMachineIdAsync(int machineId);

        // Maintenance history
        Task<IEnumerable<MaintenanceReport>> GetMaintenanceHistoryByMachineIdAsync(int machineId, int limit = 10);

        // Advanced queries
        Task<IEnumerable<MaintenanceReport>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MaintenanceReport>> GetCriticalReportsAsync();
        Task<IEnumerable<MaintenanceReport>> GetOverdueReportsAsync(int hoursThreshold);
    }
}
