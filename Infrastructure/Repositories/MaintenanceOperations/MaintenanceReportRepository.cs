using Application.Interfaces.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.MaintenanceOperations
{
    public class MaintenanceReportRepository : IMaintenanceReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenanceReportRepository> _logger;

        public MaintenanceReportRepository(
            ApplicationDbContext context,
            ILogger<MaintenanceReportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MaintenanceReport?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.MaintenanceReports.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report {ReportId} from database", id);
                throw;
            }
        }

        public async Task<MaintenanceReport?> GetByTicketIdAsync(string ticketId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .FirstOrDefaultAsync(r => r.TicketId == ticketId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report by ticket {TicketId} from database", ticketId);
                throw;
            }
        }

        public async Task<MaintenanceReport?> GetWithDetailsAsync(int id)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Include(r => r.Machine)
                    .Include(r => r.Operator)
                    .Include(r => r.MechanicalEngineer)
                    .Include(r => r.MaintenanceJob)
                    .FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report {ReportId} with details from database", id);
                throw;
            }
        }

        public async Task<MaintenanceReport> CreateAsync(MaintenanceReport report)
        {
            try
            {
                _context.MaintenanceReports.Add(report);
                await _context.SaveChangesAsync();
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating maintenance report {TicketId} in database", report.TicketId);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(MaintenanceReport report)
        {
            try
            {
                _context.MaintenanceReports.Update(report);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating maintenance report {ReportId} in database", report.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var report = await _context.MaintenanceReports.FindAsync(id);
                if (report == null)
                    return false;

                report.Deactivate(); // Soft delete
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting maintenance report {ReportId} from database", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.MaintenanceReports
                    .AnyAsync(r => r.Id == id && r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if maintenance report {ReportId} exists in database", id);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetByOperatorIdAsync(int operatorId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.OperatorId == operatorId && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports for operator {OperatorId} from database", operatorId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetByMachineIdAsync(int machineId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.MachineId == machineId && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports for machine {MachineId} from database", machineId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetByStatusAsync(ReportStatus status)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.Status == status && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports with status {Status} from database", status);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetBySeverityAsync(SeverityLevel severity)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.Severity == severity && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports with severity {Severity} from database", severity);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetByStatusAndRegionAsync(ReportStatus status, int regionId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Include(r => r.Machine)
                    .Where(r => r.Status == status && r.Machine.RegionId == regionId && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports with status {Status} for region {RegionId} from database", status, regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetUnresolvedReportsAsync()
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.Status != ReportStatus.Resolved && r.Status != ReportStatus.Closed && r.IsActive)
                    .OrderBy(r => r.Severity) // Critical first
                    .ThenBy(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unresolved maintenance reports from database");
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetUnresolvedByRegionAsync(int regionId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Include(r => r.Machine)
                    .Where(r => r.Machine.RegionId == regionId
                        && r.Status != ReportStatus.Resolved
                        && r.Status != ReportStatus.Closed
                        && r.IsActive)
                    .OrderBy(r => r.Severity)
                    .ThenBy(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unresolved maintenance reports for region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<int> GetCountAsync()
        {
            try
            {
                return await _context.MaintenanceReports
                    .CountAsync(r => r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report count from database");
                throw;
            }
        }

        public async Task<int> GetCountByOperatorIdAsync(int operatorId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .CountAsync(r => r.OperatorId == operatorId && r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report count for operator {OperatorId} from database", operatorId);
                throw;
            }
        }

        public async Task<int> GetCountByStatusAsync(ReportStatus status)
        {
            try
            {
                return await _context.MaintenanceReports
                    .CountAsync(r => r.Status == status && r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report count for status {Status} from database", status);
                throw;
            }
        }

        public async Task<int> GetCountByMachineIdAsync(int machineId)
        {
            try
            {
                return await _context.MaintenanceReports
                    .CountAsync(r => r.MachineId == machineId && r.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance report count for machine {MachineId} from database", machineId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetMaintenanceHistoryByMachineIdAsync(int machineId, int limit = 10)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.MachineId == machineId && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance history for machine {MachineId} from database", machineId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.ReportedAt >= startDate && r.ReportedAt <= endDate && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance reports for date range from database");
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetCriticalReportsAsync()
        {
            try
            {
                return await _context.MaintenanceReports
                    .Where(r => r.Severity == SeverityLevel.Critical
                        && r.Status != ReportStatus.Closed
                        && r.IsActive)
                    .OrderByDescending(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting critical maintenance reports from database");
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceReport>> GetOverdueReportsAsync(int hoursThreshold)
        {
            try
            {
                var threshold = DateTime.UtcNow.AddHours(-hoursThreshold);
                return await _context.MaintenanceReports
                    .Where(r => r.ReportedAt < threshold
                        && r.Status != ReportStatus.Resolved
                        && r.Status != ReportStatus.Closed
                        && r.IsActive)
                    .OrderBy(r => r.ReportedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue maintenance reports from database");
                throw;
            }
        }
    }
}
