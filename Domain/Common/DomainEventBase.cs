using System;

namespace Domain.Common
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
} 