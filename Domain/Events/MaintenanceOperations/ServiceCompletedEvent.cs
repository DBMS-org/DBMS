using Domain.Common;

namespace Domain.Events.MaintenanceOperations
{
    public class ServiceCompletedEvent : DomainEventBase
    {
        public int MachineId { get; }
        public bool IsEngineServiceCompleted { get; }
        public bool IsDrifterServiceCompleted { get; }

        public ServiceCompletedEvent(int machineId, bool isEngineServiceCompleted = false, bool isDrifterServiceCompleted = false)
        {
            MachineId = machineId;
            IsEngineServiceCompleted = isEngineServiceCompleted;
            IsDrifterServiceCompleted = isDrifterServiceCompleted;
        }
    }
}
