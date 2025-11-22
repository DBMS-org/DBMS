using Domain.Common;

namespace Domain.Events.MachineManagement
{
    public class ServiceDueEvent : DomainEventBase
    {
        public int MachineId { get; }
        public string MachineName { get; }

        public ServiceDueEvent(int machineId, string machineName)
        {
            MachineId = machineId;
            MachineName = machineName;
        }
    }
}
