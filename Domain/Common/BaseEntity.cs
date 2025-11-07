namespace Domain.Common
{
    /// <summary>
    ///     Minimal base entity with identity key and optional domain events support.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        ///     Primary key – use int across the codebase for simplicity.
        /// </summary>
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