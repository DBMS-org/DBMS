using Domain.Common;

namespace Domain.Events
{
    public class MachineAssignedToProjectEvent : DomainEventBase
    {
        public int MachineId { get; }
        public string Project { get; }
        public MachineAssignedToProjectEvent(int machineId, string project)
        {
            MachineId = machineId;
            Project = project;
        }
    }
} 