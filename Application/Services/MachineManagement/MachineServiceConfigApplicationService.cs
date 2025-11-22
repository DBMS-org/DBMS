using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.MachineManagement;
using Application.DTOs.Shared;
using Application.Interfaces.MachineManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.MachineManagement
{
    public class MachineServiceConfigApplicationService : IMachineServiceConfigService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<MachineServiceConfigApplicationService> _logger;

        public MachineServiceConfigApplicationService(
            IMachineRepository machineRepository,
            ILogger<MachineServiceConfigApplicationService> logger)
        {
            _machineRepository = machineRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> UpdateServiceConfigAsync(int machineId, UpdateMachineServiceConfigRequest request)
        {
            try
            {
                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return Result.Failure<bool>("Machine not found");
                }

                machine.ConfigureServiceIntervals(
                    request.EngineServiceInterval,
                    request.DrifterServiceInterval
                );

                await _machineRepository.UpdateAsync(machine);

                _logger.LogInformation("Service configuration updated for machine {MachineId}", machineId);
                return Result<bool>.Success(true);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid service configuration for machine {MachineId}", machineId);
                return Result.Failure<bool>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating service configuration for machine {MachineId}", machineId);
                return Result.Failure<bool>("An error occurred while updating service configuration");
            }
        }

        public async Task<Result<MachineServiceStatusDto>> GetMachineServiceStatusAsync(int machineId)
        {
            try
            {
                var machine = await _machineRepository.GetByIdAsync(machineId);
                if (machine == null)
                {
                    return Result.Failure<MachineServiceStatusDto>("Machine not found");
                }

                var status = new MachineServiceStatusDto
                {
                    MachineId = machine.Id,
                    MachineName = machine.Name,
                    EngineServiceInterval = machine.EngineServiceInterval,
                    CurrentEngineServiceHours = machine.CurrentEngineServiceHours,
                    EngineHoursRemaining = machine.GetEngineHoursUntilService(),
                    LastEngineServiceDate = machine.LastEngineServiceDate,
                    IsEngineServiceDue = machine.IsEngineServiceDue(),
                    DrifterServiceInterval = machine.DrifterServiceInterval,
                    CurrentDrifterServiceHours = machine.CurrentDrifterServiceHours,
                    DrifterHoursRemaining = machine.GetDrifterHoursUntilService(),
                    LastDrifterServiceDate = machine.LastDrifterServiceDate,
                    IsDrifterServiceDue = machine.IsDrifterServiceDue()
                };

                return Result<MachineServiceStatusDto>.Success(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service status for machine {MachineId}", machineId);
                return Result.Failure<MachineServiceStatusDto>("An error occurred while retrieving service status");
            }
        }

        public async Task<Result<IEnumerable<ServiceDueAlertDto>>> GetServiceDueAlertsAsync()
        {
            try
            {
                var machines = await _machineRepository.GetAllAsync();
                var alerts = new List<ServiceDueAlertDto>();

                foreach (var machine in machines)
                {
                    // Check engine service
                    var engineHoursRemaining = machine.GetEngineHoursUntilService();
                    if (engineHoursRemaining <= 100) // Alert threshold
                    {
                        alerts.Add(new ServiceDueAlertDto
                        {
                            MachineId = machine.Id,
                            MachineName = machine.Name,
                            ServiceType = "Engine",
                            HoursRemaining = engineHoursRemaining,
                            IsOverdue = machine.IsEngineServiceDue(),
                            UrgencyLevel = GetUrgencyLevel(engineHoursRemaining, machine.IsEngineServiceDue())
                        });
                    }

                    // Check drifter service (if applicable)
                    if (machine.Type == "Drill Rig" && machine.DrifterServiceInterval.HasValue)
                    {
                        var drifterHoursRemaining = machine.GetDrifterHoursUntilService() ?? 0;
                        if (drifterHoursRemaining <= 50) // Lower threshold for drifter
                        {
                            alerts.Add(new ServiceDueAlertDto
                            {
                                MachineId = machine.Id,
                                MachineName = machine.Name,
                                ServiceType = "Drifter",
                                HoursRemaining = drifterHoursRemaining,
                                IsOverdue = machine.IsDrifterServiceDue(),
                                UrgencyLevel = GetUrgencyLevel(drifterHoursRemaining, machine.IsDrifterServiceDue())
                            });
                        }
                    }
                }

                IEnumerable<ServiceDueAlertDto> orderedAlerts = alerts.OrderBy(a => a.HoursRemaining).ToList();
                return Result<IEnumerable<ServiceDueAlertDto>>.Success(orderedAlerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service due alerts");
                return Result.Failure<IEnumerable<ServiceDueAlertDto>>("An error occurred while retrieving service alerts");
            }
        }

        public async Task<Result<IEnumerable<ServiceDueAlertDto>>> GetServiceDueAlertsByRegionAsync(int regionId)
        {
            try
            {
                var machines = await _machineRepository.GetByRegionAsync(regionId);
                var alerts = new List<ServiceDueAlertDto>();

                foreach (var machine in machines)
                {
                    // Check engine service
                    var engineHoursRemaining = machine.GetEngineHoursUntilService();
                    if (engineHoursRemaining <= 100)
                    {
                        alerts.Add(new ServiceDueAlertDto
                        {
                            MachineId = machine.Id,
                            MachineName = machine.Name,
                            ServiceType = "Engine",
                            HoursRemaining = engineHoursRemaining,
                            IsOverdue = machine.IsEngineServiceDue(),
                            UrgencyLevel = GetUrgencyLevel(engineHoursRemaining, machine.IsEngineServiceDue())
                        });
                    }

                    // Check drifter service
                    if (machine.Type == "Drill Rig" && machine.DrifterServiceInterval.HasValue)
                    {
                        var drifterHoursRemaining = machine.GetDrifterHoursUntilService() ?? 0;
                        if (drifterHoursRemaining <= 50)
                        {
                            alerts.Add(new ServiceDueAlertDto
                            {
                                MachineId = machine.Id,
                                MachineName = machine.Name,
                                ServiceType = "Drifter",
                                HoursRemaining = drifterHoursRemaining,
                                IsOverdue = machine.IsDrifterServiceDue(),
                                UrgencyLevel = GetUrgencyLevel(drifterHoursRemaining, machine.IsDrifterServiceDue())
                            });
                        }
                    }
                }

                IEnumerable<ServiceDueAlertDto> orderedAlerts = alerts.OrderBy(a => a.HoursRemaining).ToList();
                return Result<IEnumerable<ServiceDueAlertDto>>.Success(orderedAlerts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving service due alerts for region {RegionId}", regionId);
                return Result.Failure<IEnumerable<ServiceDueAlertDto>>("An error occurred while retrieving service alerts");
            }
        }

        private string GetUrgencyLevel(decimal hoursRemaining, bool isOverdue)
        {
            if (isOverdue || hoursRemaining <= 0)
                return "RED";
            if (hoursRemaining <= 20)
                return "ORANGE";
            if (hoursRemaining <= 50)
                return "YELLOW";
            return "GREEN";
        }
    }
}
