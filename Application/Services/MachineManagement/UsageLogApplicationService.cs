using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.MachineManagement;
using Application.DTOs.Shared;
using Application.Interfaces.MachineManagement;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.Infrastructure;
using Application.Interfaces.UserManagement;
using Domain.Entities.MachineManagement;
using Domain.Entities.MaintenanceOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.MachineManagement
{
    public class UsageLogApplicationService : IUsageLogService
    {
        private readonly IUsageLogRepository _usageLogRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMaintenanceJobRepository _maintenanceJobRepository;
        private readonly IMappingService _mappingService;
        private readonly ILogger<UsageLogApplicationService> _logger;

        public UsageLogApplicationService(
            IUsageLogRepository usageLogRepository,
            IMachineRepository machineRepository,
            IUserRepository userRepository,
            IMaintenanceJobRepository maintenanceJobRepository,
            IMappingService mappingService,
            ILogger<UsageLogApplicationService> logger)
        {
            _usageLogRepository = usageLogRepository;
            _machineRepository = machineRepository;
            _userRepository = userRepository;
            _maintenanceJobRepository = maintenanceJobRepository;
            _mappingService = mappingService;
            _logger = logger;
        }

        public async Task<Result<UsageLogDto>> CreateUsageLogAsync(CreateUsageLogRequest request, int operatorId)
        {
            try
            {
                _logger.LogInformation("Creating usage log for machine {MachineId} by operator {OperatorId}",
                    request.MachineId, operatorId);

                // Validate machine exists
                var machine = await _machineRepository.GetByIdAsync(request.MachineId);
                if (machine == null)
                {
                    return Result.Failure<UsageLogDto>("Machine not found");
                }

                // Get operator info for site engineer field
                var operatorUser = await _userRepository.GetByIdAsync(operatorId);
                if (operatorUser == null)
                {
                    return Result.Failure<UsageLogDto>("Operator not found");
                }

                // Create usage log using domain factory method
                var usageLog = MachineUsageLog.Create(
                    machineId: request.MachineId,
                    operatorId: operatorId,
                    siteEngineer: operatorUser.Name,
                    logDate: request.LogDate,
                    engineHourStart: request.EngineHourStart,
                    engineHourEnd: request.EngineHourEnd,
                    drifterHourStart: request.DrifterHourStart,
                    drifterHourEnd: request.DrifterHourEnd,
                    idleHours: request.IdleHours,
                    workingHours: request.WorkingHours,
                    fuelConsumed: request.FuelConsumed,
                    hasDowntime: request.HasDowntime,
                    downtimeHours: request.DowntimeHours,
                    breakdownDescription: request.BreakdownDescription,
                    remarks: request.Remarks,
                    createdBy: operatorId
                );

                // Save usage log
                var createdLog = await _usageLogRepository.CreateAsync(usageLog);

                // Calculate deltas and update machine service hours
                var engineHoursDelta = request.EngineHourEnd - request.EngineHourStart;
                decimal? drifterHoursDelta = null;
                if (request.DrifterHourStart.HasValue && request.DrifterHourEnd.HasValue)
                {
                    drifterHoursDelta = request.DrifterHourEnd.Value - request.DrifterHourStart.Value;
                }

                machine.IncrementServiceHours(engineHoursDelta, drifterHoursDelta);
                await _machineRepository.UpdateAsync(machine);

                _logger.LogInformation("Usage log {LogId} created successfully for machine {MachineId}. Service hours updated: Engine +{EngineHours}, Drifter +{DrifterHours}",
                    createdLog.Id, request.MachineId, engineHoursDelta, drifterHoursDelta);

                // Check and create service alert jobs if thresholds are reached
                await CheckAndCreateServiceAlerts(machine, operatorId);

                // Map to DTO
                var dto = MapToDto(createdLog);
                return Result<UsageLogDto>.Success(dto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation failed for usage log creation");
                return Result.Failure<UsageLogDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating usage log for machine {MachineId}", request.MachineId);
                return Result.Failure<UsageLogDto>("An error occurred while creating the usage log");
            }
        }

        public async Task<Result<UsageLogDto>> GetUsageLogByIdAsync(int id)
        {
            try
            {
                var log = await _usageLogRepository.GetByIdAsync(id);
                if (log == null)
                {
                    return Result.Failure<UsageLogDto>("Usage log not found");
                }

                var dto = MapToDto(log);
                return Result<UsageLogDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage log {LogId}", id);
                return Result.Failure<UsageLogDto>("An error occurred while retrieving the usage log");
            }
        }

        public async Task<Result<IEnumerable<UsageLogDto>>> GetUsageLogsByMachineAsync(
            int machineId,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                var logs = await _usageLogRepository.GetByMachineIdAsync(machineId, startDate, endDate);
                IEnumerable<UsageLogDto> dtos = logs.Select(MapToDto).ToList();
                return Result<IEnumerable<UsageLogDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage logs for machine {MachineId}", machineId);
                return Result.Failure<IEnumerable<UsageLogDto>>("An error occurred while retrieving usage logs");
            }
        }

        public async Task<Result<IEnumerable<UsageLogDto>>> GetUsageLogsByOperatorAsync(
            int operatorId,
            DateTime? startDate,
            DateTime? endDate)
        {
            try
            {
                var logs = await _usageLogRepository.GetByOperatorIdAsync(operatorId, startDate, endDate);
                IEnumerable<UsageLogDto> dtos = logs.Select(MapToDto).ToList();
                return Result<IEnumerable<UsageLogDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving usage logs for operator {OperatorId}", operatorId);
                return Result.Failure<IEnumerable<UsageLogDto>>("An error occurred while retrieving usage logs");
            }
        }

        public async Task<Result<UsageLogDto>> GetLatestUsageLogByMachineAsync(int machineId)
        {
            try
            {
                var log = await _usageLogRepository.GetLatestByMachineIdAsync(machineId);
                if (log == null)
                {
                    return Result.Failure<UsageLogDto>("No usage logs found for this machine");
                }

                var dto = MapToDto(log);
                return Result<UsageLogDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest usage log for machine {MachineId}", machineId);
                return Result.Failure<UsageLogDto>("An error occurred while retrieving the latest usage log");
            }
        }

        public async Task<Result<UsageStatisticsDto>> GetUsageStatisticsAsync(int machineId, int days = 30)
        {
            try
            {
                // Use end of today to include all logs from today
                var endDate = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
                var startDate = DateTime.UtcNow.Date.AddDays(-days);

                var logs = await _usageLogRepository.GetByMachineIdAsync(machineId, startDate, endDate);
                var logsList = logs.ToList();

                if (!logsList.Any())
                {
                    return Result.Failure<UsageStatisticsDto>("No usage data found for the specified period");
                }

                var machine = await _machineRepository.GetByIdAsync(machineId);

                _logger.LogInformation("Found {Count} usage logs for machine {MachineId} between {Start} and {End}",
                    logsList.Count, machineId, startDate, endDate);

                var statistics = new UsageStatisticsDto
                {
                    MachineId = machineId,
                    MachineName = machine?.Name ?? "Unknown",
                    TotalEngineHours = logsList.Sum(l => l.EngineHoursDelta),
                    TotalIdleHours = logsList.Sum(l => l.IdleHours),
                    TotalWorkingHours = logsList.Sum(l => l.WorkingHours),
                    TotalFuelConsumed = logsList.Sum(l => l.FuelConsumed ?? 0),
                    TotalDowntimeHours = logsList.Sum(l => l.DowntimeHours ?? 0),
                    AverageDailyHours = logsList.Any() ? logsList.Average(l => l.EngineHoursDelta) : 0,
                    DaysWithDowntime = logsList.Count(l => l.HasDowntime),
                    PeriodStart = startDate,
                    PeriodEnd = DateTime.UtcNow.Date
                };

                return Result<UsageStatisticsDto>.Success(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating usage statistics for machine {MachineId}", machineId);
                return Result.Failure<UsageStatisticsDto>("An error occurred while calculating usage statistics");
            }
        }

        public async Task<Result<bool>> ApproveUsageLogAsync(int logId, int approvedBy)
        {
            try
            {
                var log = await _usageLogRepository.GetByIdAsync(logId);
                if (log == null)
                {
                    return Result.Failure<bool>("Usage log not found");
                }

                log.Approve();
                await _usageLogRepository.UpdateAsync(log);

                _logger.LogInformation("Usage log {LogId} approved by user {UserId}", logId, approvedBy);
                return Result<bool>.Success(true);
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure<bool>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving usage log {LogId}", logId);
                return Result.Failure<bool>("An error occurred while approving the usage log");
            }
        }

        public async Task<Result<bool>> RejectUsageLogAsync(int logId, int rejectedBy)
        {
            try
            {
                var log = await _usageLogRepository.GetByIdAsync(logId);
                if (log == null)
                {
                    return Result.Failure<bool>("Usage log not found");
                }

                log.Reject();
                await _usageLogRepository.UpdateAsync(log);

                _logger.LogInformation("Usage log {LogId} rejected by user {UserId}", logId, rejectedBy);
                return Result<bool>.Success(true);
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure<bool>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting usage log {LogId}", logId);
                return Result.Failure<bool>("An error occurred while rejecting the usage log");
            }
        }

        private UsageLogDto MapToDto(MachineUsageLog log)
        {
            return new UsageLogDto
            {
                Id = log.Id,
                MachineId = log.MachineId,
                MachineName = log.Machine?.Name ?? "Unknown",
                OperatorId = log.OperatorId,
                SiteEngineer = log.SiteEngineer,
                LogDate = log.LogDate,
                EngineHourStart = log.EngineHourStart,
                EngineHourEnd = log.EngineHourEnd,
                EngineHoursDelta = log.EngineHoursDelta,
                DrifterHourStart = log.DrifterHourStart,
                DrifterHourEnd = log.DrifterHourEnd,
                DrifterHoursDelta = log.DrifterHoursDelta,
                IdleHours = log.IdleHours,
                WorkingHours = log.WorkingHours,
                FuelConsumed = log.FuelConsumed,
                HasDowntime = log.HasDowntime,
                DowntimeHours = log.DowntimeHours,
                BreakdownDescription = log.BreakdownDescription,
                Remarks = log.Remarks,
                Status = log.Status.ToString(),
                CreatedAt = log.CreatedAt
            };
        }

        /// <summary>
        /// Checks service hour thresholds and creates/updates service alert jobs
        /// Thresholds: 70% = Medium, 90% = High, 100% = Critical
        /// If a higher threshold is reached, previous pending job is cancelled and new one created
        /// </summary>
        private async Task CheckAndCreateServiceAlerts(Machine machine, int createdBy)
        {
            try
            {
                // Check Engine Service threshold
                if (machine.EngineServiceInterval > 0)
                {
                    var engineProgress = (machine.CurrentEngineServiceHours / machine.EngineServiceInterval) * 100;
                    await CheckAndCreateServiceAlert(machine, "EngineService", engineProgress,
                        machine.CurrentEngineServiceHours, machine.EngineServiceInterval, createdBy);
                }

                // Check Drifter Service threshold (only for drill rigs)
                if (machine.DrifterServiceInterval.HasValue && machine.DrifterServiceInterval.Value > 0)
                {
                    var currentDrifterHours = machine.CurrentDrifterServiceHours ?? 0;
                    var drifterProgress = (currentDrifterHours / machine.DrifterServiceInterval.Value) * 100;
                    await CheckAndCreateServiceAlert(machine, "DrifterService", drifterProgress,
                        currentDrifterHours, machine.DrifterServiceInterval.Value, createdBy);
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail usage log creation if alert creation fails
                _logger.LogWarning(ex, "Failed to create service alerts for machine {MachineId}", machine.Id);
            }
        }

        private async Task CheckAndCreateServiceAlert(Machine machine, string serviceType, decimal progressPercent,
            decimal currentHours, decimal intervalHours, int createdBy)
        {
            // Determine alert level based on progress
            string? newAlertLevel = null;
            if (progressPercent >= 100)
                newAlertLevel = "Critical";
            else if (progressPercent >= 90)
                newAlertLevel = "High";
            else if (progressPercent >= 70)
                newAlertLevel = "Medium";

            if (newAlertLevel == null)
            {
                // No threshold reached, nothing to do
                return;
            }

            // Check for existing pending service job
            var existingJob = await _maintenanceJobRepository.GetPendingServiceJobByMachineAsync(machine.Id, serviceType);

            if (existingJob != null)
            {
                // Check if we need to upgrade priority
                var existingLevel = existingJob.ServiceAlertLevel;
                var shouldUpgrade = ShouldUpgradeAlertLevel(existingLevel, newAlertLevel);

                if (!shouldUpgrade)
                {
                    // Same or higher priority job already exists
                    _logger.LogDebug("Service alert job already exists for machine {MachineId}, service type {ServiceType} with level {Level}",
                        machine.Id, serviceType, existingLevel);
                    return;
                }

                // Cancel existing job and create new one with higher priority
                await _maintenanceJobRepository.CancelPendingServiceJobAsync(
                    machine.Id, serviceType, $"Superseded by {newAlertLevel} priority alert");

                _logger.LogInformation("Cancelled {ServiceType} service job for machine {MachineId} - upgrading from {OldLevel} to {NewLevel}",
                    serviceType, machine.Id, existingLevel, newAlertLevel);
            }

            // Create new service alert job
            var job = MaintenanceJob.CreateServiceAlert(
                machineId: machine.Id,
                projectId: machine.ProjectId,
                serviceType: serviceType,
                alertLevel: newAlertLevel,
                currentHours: currentHours,
                intervalHours: intervalHours,
                createdBy: createdBy
            );

            await _maintenanceJobRepository.CreateAsync(job);

            // Auto-assign to a mechanical engineer in the same region as the machine
            await AutoAssignEngineerToJob(job, machine);

            _logger.LogInformation("Created {AlertLevel} priority {ServiceType} service alert for machine {MachineId}. Progress: {Progress:F0}%",
                newAlertLevel, serviceType, machine.Id, progressPercent);
        }

        private bool ShouldUpgradeAlertLevel(string? existingLevel, string newLevel)
        {
            var levelOrder = new Dictionary<string, int>
            {
                { "Medium", 1 },
                { "High", 2 },
                { "Critical", 3 }
            };

            if (string.IsNullOrEmpty(existingLevel))
                return true;

            var existingPriority = levelOrder.GetValueOrDefault(existingLevel, 0);
            var newPriority = levelOrder.GetValueOrDefault(newLevel, 0);

            return newPriority > existingPriority;
        }

        /// <summary>
        /// Auto-assigns the job to a mechanical engineer in the same region as the machine
        /// </summary>
        private async Task AutoAssignEngineerToJob(MaintenanceJob job, Machine machine)
        {
            try
            {
                // Get the machine's region name
                var regionName = machine.Region?.Name;
                if (string.IsNullOrEmpty(regionName))
                {
                    _logger.LogWarning("Machine {MachineId} has no region assigned. Cannot auto-assign engineer.", machine.Id);
                    return;
                }

                // Find mechanical engineers in the same region
                var engineers = await _userRepository.GetByRoleAndRegionAsync("MechanicalEngineer", regionName);
                var engineer = engineers.FirstOrDefault();

                if (engineer == null)
                {
                    _logger.LogWarning("No mechanical engineer found in region {Region} for machine {MachineId}. Job {JobId} left unassigned.",
                        regionName, machine.Id, job.Id);
                    return;
                }

                // Assign the engineer to the job
                job.AssignEngineer(engineer.Id);
                await _maintenanceJobRepository.UpdateAsync(job);

                _logger.LogInformation("Auto-assigned job {JobId} to mechanical engineer {EngineerName} (ID: {EngineerId}) in region {Region}",
                    job.Id, engineer.Name, engineer.Id, regionName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to auto-assign engineer to job {JobId}", job.Id);
            }
        }
    }
}
