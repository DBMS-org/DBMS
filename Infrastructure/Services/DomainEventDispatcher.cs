using Application.Interfaces.Infrastructure;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);
            foreach (var handler in handlers)
            {
                try
                {
                    await ((Task)handlerType.GetMethod("HandleAsync")!
                        .Invoke(handler, new object?[] { domainEvent, cancellationToken })!);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error dispatching domain event {Event}", domainEvent.GetType().Name);
                }
            }
        }
    }

    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
    }
} 