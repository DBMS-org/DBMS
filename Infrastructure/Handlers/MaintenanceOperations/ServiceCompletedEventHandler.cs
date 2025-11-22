using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.MachineManagement;
using Domain.Events.MaintenanceOperations;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Handlers.MaintenanceOperations
{
    /// <summary>
    /// Handles ServiceCompletedEvent - Resets machine service hours
    /// Triggered by: Mechanical Engineer completing service maintenance job
    /// Impact: Resets machine's CurrentEngineServiceHours and/or CurrentDrifterServiceHours
    /// </summary>
    public class ServiceCompletedEventHandler : IDomainEventHandler<ServiceCompletedEvent>
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<ServiceCompletedEventHandler> _logger;

        public ServiceCompletedEventHandler(
            IMachineRepository machineRepository,
            ILogger<ServiceCompletedEventHandler> logger)
        {
            _machineRepository = machineRepository;
            _logger = logger;
        }

        public async Task HandleAsync(ServiceCompletedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Processing ServiceCompletedEvent for machine {MachineId}. Engine: {EngineService}, Drifter: {DrifterService}",
                domainEvent.MachineId,
                domainEvent.IsEngineServiceCompleted,
                domainEvent.IsDrifterServiceCompleted);

            try
            {
                // Get the machine
                var machine = await _machineRepository.GetByIdAsync(domainEvent.MachineId);
                if (machine == null)
                {
                    _logger.LogWarning("Machine {MachineId} not found for service completion",
                        domainEvent.MachineId);
                    return;
                }

                // Only reset if at least one service type is specified
                if (!domainEvent.IsEngineServiceCompleted && !domainEvent.IsDrifterServiceCompleted)
                {
                    _logger.LogDebug("No specific service completion flags set for machine {MachineId}, skipping reset",
                        domainEvent.MachineId);
                    return;
                }

                // Reset only the specified service hours
                machine.ResetServiceHours(
                    resetEngine: domainEvent.IsEngineServiceCompleted,
                    resetDrifter: domainEvent.IsDrifterServiceCompleted
                );

                await _machineRepository.UpdateAsync(machine);

                _logger.LogInformation(
                    "Machine {MachineId} service hours reset. Engine: {EngineReset}, Drifter: {DrifterReset}",
                    machine.Id,
                    domainEvent.IsEngineServiceCompleted,
                    domainEvent.IsDrifterServiceCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling ServiceCompletedEvent for machine {MachineId}",
                    domainEvent.MachineId);
                throw;
            }
        }
    }
}
