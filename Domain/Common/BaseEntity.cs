namespace Domain.Common
{
    /// <summary>
    /// Base entity supporting domain events
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public List<IDomainEvent> PullDomainEvents()
        {
            var events = _domainEvents.ToList();
            _domainEvents.Clear();
            return events;
        }
    }
} 