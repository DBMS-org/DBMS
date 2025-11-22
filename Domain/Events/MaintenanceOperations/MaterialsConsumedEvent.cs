using Domain.Common;
using Domain.Entities.MaintenanceOperations;

namespace Domain.Events.MaintenanceOperations
{
    public class MaterialsConsumedEvent : DomainEventBase
    {
        public MaintenanceJob Job { get; }

        public MaterialsConsumedEvent(MaintenanceJob job)
        {
            Job = job;
        }
    }
}
