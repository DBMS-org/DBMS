using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.MachineManagement;
using Domain.Events.MachineManagement;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Handlers.MachineManagement
{
    /// <summary>
    /// Handles UsageLogCreatedEvent - Auto-increments machine service hours
    /// Triggered by: Operator submitting usage log
    /// Impact: Updates machine's CurrentEngineServiceHours and CurrentDrifterServiceHours
    /// </summary>
    public class UsageLogCreatedEventHandler : IDomainEventHandler<UsageLogCreatedEvent>
    {
        private readonly IMachineRepository _machineRepository;
        private readonly ILogger<UsageLogCreatedEventHandler> _logger;

        public UsageLogCreatedEventHandler(
            IMachineRepository machineRepository,
            ILogger<UsageLogCreatedEventHandler> logger)
        {
            _machineRepository = machineRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UsageLogCreatedEvent domainEvent, CancellationToken cancellationToken = default)
        {
            // Service hour increment is now handled directly in UsageLogApplicationService
            // This handler is kept for logging purposes only
            var log = domainEvent.UsageLog;
            _logger.LogInformation(
                "UsageLogCreatedEvent received for machine {MachineId}, Log {LogId}",
                log.MachineId, log.Id);
            await Task.CompletedTask;
        }
    }
}
