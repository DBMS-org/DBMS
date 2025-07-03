namespace Application.Interfaces.Infrastructure
{
    using Domain.Common;
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
} 