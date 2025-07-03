using Domain.Events;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class MachineAssignedLoggingHandler : IDomainEventHandler<MachineAssignedToProjectEvent>
    {
        private readonly ILogger<MachineAssignedLoggingHandler> _logger;

        public MachineAssignedLoggingHandler(ILogger<MachineAssignedLoggingHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(MachineAssignedToProjectEvent domainEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Machine {MachineId} assigned to project {Project} at {Time}", domainEvent.MachineId, domainEvent.Project, domainEvent.OccurredOn);
            return Task.CompletedTask;
        }
    }
} 