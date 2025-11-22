using Domain.Common;
using Domain.Entities.MachineManagement;

namespace Domain.Events.MachineManagement
{
    public class UsageLogCreatedEvent : DomainEventBase
    {
        public MachineUsageLog UsageLog { get; }

        public UsageLogCreatedEvent(MachineUsageLog usageLog)
        {
            UsageLog = usageLog;
        }
    }
}
