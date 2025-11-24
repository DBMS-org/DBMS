using Application.Interfaces.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.MaintenanceOperations
{
    public class MaintenanceJobRepository : IMaintenanceJobRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MaintenanceJobRepository> _logger;

        public MaintenanceJobRepository(
            ApplicationDbContext context,
            ILogger<MaintenanceJobRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MaintenanceJob?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.MaintenanceJobs.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job {JobId} from database", id);
                throw;
            }
        }

        public async Task<MaintenanceJob?> GetWithDetailsAsync(int id)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Include(j => j.Project)
                    .Include(j => j.MaintenanceReport)
                    .Include(j => j.Assignments)
                        .ThenInclude(a => a.MechanicalEngineer)
                    .Include(j => j.Creator)
                    .FirstOrDefaultAsync(j => j.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job {JobId} with details from database", id);
                throw;
            }
        }

        public async Task<MaintenanceJob?> GetByReportIdAsync(int reportId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .FirstOrDefaultAsync(j => j.MaintenanceReportId == reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job for report {ReportId} from database", reportId);
                throw;
            }
        }

        public async Task<MaintenanceJob> CreateAsync(MaintenanceJob job)
        {
            try
            {
                _context.MaintenanceJobs.Add(job);
                await _context.SaveChangesAsync();
                return job;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating maintenance job in database");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(MaintenanceJob job)
        {
            try
            {
                _context.MaintenanceJobs.Update(job);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating maintenance job {JobId} in database", job.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var job = await _context.MaintenanceJobs.FindAsync(id);
                if (job == null)
                    return false;

                job.Deactivate(); // Soft delete
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting maintenance job {JobId} from database", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .AnyAsync(j => j.Id == id && j.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if maintenance job {JobId} exists in database", id);
                throw;
            }
        }

        // CRITICAL: Region-based filtering for security
        public async Task<IEnumerable<MaintenanceJob>> GetByEngineerIdAsync(int engineerId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Include(j => j.Project)
                    .Include(j => j.Assignments)
                    .Where(j => j.Assignments.Any(a => a.MechanicalEngineerId == engineerId) && j.IsActive)
                    .OrderByDescending(j => j.Type) // Emergency first
                    .ThenBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for engineer {EngineerId} from database", engineerId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByRegionAsync(int regionId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Include(j => j.Project)
                    .Where(j => j.Machine.RegionId == regionId && j.IsActive)
                    .OrderByDescending(j => j.Type)
                    .ThenBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByEngineerAndRegionAsync(int engineerId, int regionId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Include(j => j.Project)
                    .Include(j => j.MaintenanceReport)
                    .Include(j => j.Assignments)
                    .Where(j => j.Assignments.Any(a => a.MechanicalEngineerId == engineerId)
                        && j.Machine.RegionId == regionId
                        && j.IsActive)
                    .OrderByDescending(j => j.Type)
                    .ThenBy(j => j.Status)
                    .ThenBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for engineer {EngineerId} in region {RegionId} from database", engineerId, regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByMachineIdAsync(int machineId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.MachineId == machineId && j.IsActive)
                    .OrderByDescending(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for machine {MachineId} from database", machineId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByProjectIdAsync(int projectId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.ProjectId == projectId && j.IsActive)
                    .OrderByDescending(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for project {ProjectId} from database", projectId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByStatusAsync(MaintenanceJobStatus status)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.Status == status && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs with status {Status} from database", status);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByTypeAsync(MaintenanceType type)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.Type == type && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs with type {Type} from database", type);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByStatusAndRegionAsync(MaintenanceJobStatus status, int regionId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Where(j => j.Status == status && j.Machine.RegionId == regionId && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs with status {Status} for region {RegionId} from database", status, regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                        .ThenInclude(m => m.Region)
                    .Include(j => j.Assignments)
                        .ThenInclude(a => a.MechanicalEngineer)
                    .Include(j => j.Project)
                    .Include(j => j.MaintenanceReport)
                    .Include(j => j.Creator)
                    .Where(j => j.ScheduledDate >= startDate && j.ScheduledDate <= endDate && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for date range from database");
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetByDateRangeAndRegionAsync(DateTime startDate, DateTime endDate, int regionId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Where(j => j.ScheduledDate >= startDate
                        && j.ScheduledDate <= endDate
                        && j.Machine.RegionId == regionId
                        && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs for date range and region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetOverdueJobsAsync()
        {
            try
            {
                var now = DateTime.UtcNow;
                return await _context.MaintenanceJobs
                    .Where(j => j.ScheduledDate < now
                        && j.Status != MaintenanceJobStatus.Completed
                        && j.Status != MaintenanceJobStatus.Cancelled
                        && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue maintenance jobs from database");
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetOverdueJobsByRegionAsync(int regionId)
        {
            try
            {
                var now = DateTime.UtcNow;
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Where(j => j.ScheduledDate < now
                        && j.Machine.RegionId == regionId
                        && j.Status != MaintenanceJobStatus.Completed
                        && j.Status != MaintenanceJobStatus.Cancelled
                        && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting overdue maintenance jobs for region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetJobsByPriorityAsync(MaintenanceType type)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.Type == type && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance jobs by priority {Type} from database", type);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetScheduledJobsAsync()
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Where(j => j.Status == MaintenanceJobStatus.Scheduled && j.IsActive)
                    .OrderBy(j => j.ScheduledDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scheduled maintenance jobs from database");
                throw;
            }
        }

        // Workload calculation for round-robin assignment
        public async Task<Dictionary<int, int>> GetEngineerWorkloadByRegionAsync(int regionId)
        {
            try
            {
                var workload = await _context.MaintenanceJobAssignments
                    .Include(a => a.MaintenanceJob)
                        .ThenInclude(j => j.Machine)
                    .Where(a => a.MaintenanceJob.Machine.RegionId == regionId
                        && (a.MaintenanceJob.Status == MaintenanceJobStatus.Scheduled
                            || a.MaintenanceJob.Status == MaintenanceJobStatus.InProgress)
                        && a.MaintenanceJob.IsActive)
                    .GroupBy(a => a.MechanicalEngineerId)
                    .Select(g => new { EngineerId = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.EngineerId, x => x.Count);

                return workload;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating engineer workload for region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<int> GetActiveJobCountByEngineerAsync(int engineerId)
        {
            try
            {
                return await _context.MaintenanceJobAssignments
                    .Include(a => a.MaintenanceJob)
                    .CountAsync(a => a.MechanicalEngineerId == engineerId
                        && (a.MaintenanceJob.Status == MaintenanceJobStatus.Scheduled
                            || a.MaintenanceJob.Status == MaintenanceJobStatus.InProgress)
                        && a.MaintenanceJob.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active job count for engineer {EngineerId} from database", engineerId);
                throw;
            }
        }

        public async Task<bool> BulkUpdateStatusAsync(IEnumerable<int> jobIds, MaintenanceJobStatus status)
        {
            try
            {
                var jobs = await _context.MaintenanceJobs
                    .Where(j => jobIds.Contains(j.Id))
                    .ToListAsync();

                foreach (var job in jobs)
                {
                    job.UpdateStatus(status);
                }

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating job status in database");
                throw;
            }
        }

        public async Task<bool> BulkAssignEngineerAsync(IEnumerable<int> jobIds, int engineerId)
        {
            try
            {
                var jobs = await _context.MaintenanceJobs
                    .Include(j => j.Assignments)
                    .Where(j => jobIds.Contains(j.Id))
                    .ToListAsync();

                foreach (var job in jobs)
                {
                    // Remove existing assignments
                    _context.MaintenanceJobAssignments.RemoveRange(job.Assignments);

                    // Add new assignment
                    job.AssignEngineer(engineerId);
                }

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk assigning engineer {EngineerId} in database", engineerId);
                throw;
            }
        }

        public async Task<int> GetCountAsync()
        {
            try
            {
                return await _context.MaintenanceJobs
                    .CountAsync(j => j.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job count from database");
                throw;
            }
        }

        public async Task<int> GetCountByStatusAsync(MaintenanceJobStatus status)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .CountAsync(j => j.Status == status && j.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job count for status {Status} from database", status);
                throw;
            }
        }

        public async Task<int> GetCountByEngineerAsync(int engineerId)
        {
            try
            {
                return await _context.MaintenanceJobAssignments
                    .Include(a => a.MaintenanceJob)
                    .CountAsync(a => a.MechanicalEngineerId == engineerId && a.MaintenanceJob.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job count for engineer {EngineerId} from database", engineerId);
                throw;
            }
        }

        public async Task<int> GetCountByRegionAsync(int regionId)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .CountAsync(j => j.Machine.RegionId == regionId && j.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance job count for region {RegionId} from database", regionId);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetMaintenanceHistoryByMachineIdAsync(int machineId, int limit = 10)
        {
            try
            {
                return await _context.MaintenanceJobs
                    .Include(j => j.Assignments)
                        .ThenInclude(a => a.MechanicalEngineer)
                    .Where(j => j.MachineId == machineId && j.IsActive)
                    .OrderByDescending(j => j.CompletedDate ?? j.ScheduledDate)
                    .Take(limit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting maintenance history for machine {MachineId} from database", machineId);
                throw;
            }
        }

        public async Task<MaintenanceJob?> GetPendingServiceJobByMachineAsync(int machineId, string serviceType)
        {
            try
            {
                // Find an auto-generated service job that is still pending (Scheduled or InProgress)
                return await _context.MaintenanceJobs
                    .Where(j => j.MachineId == machineId
                        && j.IsAutoGenerated
                        && j.Reason.Contains(serviceType == "EngineService" ? "Engine service due" : "Drifter service due")
                        && (j.Status == MaintenanceJobStatus.Scheduled || j.Status == MaintenanceJobStatus.InProgress)
                        && j.IsActive)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending service job for machine {MachineId} and type {ServiceType}", machineId, serviceType);
                throw;
            }
        }

        public async Task<bool> CancelPendingServiceJobAsync(int machineId, string serviceType, string reason)
        {
            try
            {
                var job = await GetPendingServiceJobByMachineAsync(machineId, serviceType);
                if (job == null)
                {
                    return false;
                }

                job.Cancel(reason);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling pending service job for machine {MachineId} and type {ServiceType}", machineId, serviceType);
                throw;
            }
        }

        public async Task<IEnumerable<MaintenanceJob>> GetAllJobsAsync(string? status = null, string? type = null, int? machineId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = _context.MaintenanceJobs
                    .Include(j => j.Machine)
                    .Include(j => j.Project)
                    .Include(j => j.Assignments)
                        .ThenInclude(a => a.MechanicalEngineer)
                    .Include(j => j.MaintenanceReport)
                    .Where(j => j.IsActive)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<MaintenanceJobStatus>(status, true, out var statusEnum))
                {
                    query = query.Where(j => j.Status == statusEnum);
                }

                if (!string.IsNullOrEmpty(type) && Enum.TryParse<MaintenanceType>(type, true, out var typeEnum))
                {
                    query = query.Where(j => j.Type == typeEnum);
                }

                if (machineId.HasValue)
                {
                    query = query.Where(j => j.MachineId == machineId.Value);
                }

                if (startDate.HasValue)
                {
                    query = query.Where(j => j.ScheduledDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(j => j.ScheduledDate <= endDate.Value);
                }

                return await query.OrderByDescending(j => j.ScheduledDate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all maintenance jobs");
                throw;
            }
        }
    }
}
