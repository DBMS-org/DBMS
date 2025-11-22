using System;
using Domain.Common;
using Domain.Entities.UserManagement;
using Domain.Events.MachineManagement;

namespace Domain.Entities.MachineManagement
{
    /// <summary>
    /// Represents operator-submitted usage records with start/end hour tracking
    /// </summary>
    public class MachineUsageLog : BaseAuditableEntity
    {
        // Foreign Keys - RELATIONSHIPS
        public int MachineId { get; private set; }
        public int? OperatorId { get; private set; }
        public string SiteEngineer { get; private set; } = string.Empty; // Denormalized for history

        // Navigation Properties - RELATIONSHIPS
        public virtual Machine Machine { get; private set; } = null!;
        public virtual User? Operator { get; private set; }

        // Data Fields
        public DateTime LogDate { get; private set; }
        public decimal EngineHourStart { get; private set; }
        public decimal EngineHourEnd { get; private set; }
        public decimal EngineHoursDelta { get; private set; }  // Calculated
        public decimal? DrifterHourStart { get; private set; }
        public decimal? DrifterHourEnd { get; private set; }
        public decimal? DrifterHoursDelta { get; private set; }  // Calculated
        public decimal IdleHours { get; private set; }
        public decimal WorkingHours { get; private set; }
        public decimal? FuelConsumed { get; private set; }
        public bool HasDowntime { get; private set; }
        public decimal? DowntimeHours { get; private set; }
        public string? BreakdownDescription { get; private set; }
        public string? Remarks { get; private set; }
        public UsageLogStatus Status { get; private set; }
        public int CreatedBy { get; private set; }

        // Private constructor for EF Core
        private MachineUsageLog() { }

        /// <summary>
        /// Factory method to create a new usage log
        /// </summary>
        public static MachineUsageLog Create(
            int machineId,
            int? operatorId,
            string siteEngineer,
            DateTime logDate,
            decimal engineHourStart,
            decimal engineHourEnd,
            decimal? drifterHourStart,
            decimal? drifterHourEnd,
            decimal idleHours,
            decimal workingHours,
            decimal? fuelConsumed,
            bool hasDowntime,
            decimal? downtimeHours,
            string? breakdownDescription,
            string? remarks,
            int createdBy)
        {
            // Validation
            if (engineHourEnd < engineHourStart)
                throw new InvalidOperationException("Engine hour end cannot be less than start");

            if (drifterHourEnd.HasValue && drifterHourStart.HasValue &&
                drifterHourEnd < drifterHourStart)
                throw new InvalidOperationException("Drifter hour end cannot be less than start");

            if (hasDowntime && (downtimeHours == null || downtimeHours <= 0 || string.IsNullOrWhiteSpace(breakdownDescription)))
                throw new InvalidOperationException("Downtime details required when downtime occurred");

            var log = new MachineUsageLog
            {
                MachineId = machineId,
                OperatorId = operatorId,
                SiteEngineer = siteEngineer,
                LogDate = logDate,
                EngineHourStart = engineHourStart,
                EngineHourEnd = engineHourEnd,
                EngineHoursDelta = engineHourEnd - engineHourStart,
                DrifterHourStart = drifterHourStart,
                DrifterHourEnd = drifterHourEnd,
                DrifterHoursDelta = CalculateDelta(drifterHourStart, drifterHourEnd),
                IdleHours = idleHours,
                WorkingHours = workingHours,
                FuelConsumed = fuelConsumed,
                HasDowntime = hasDowntime,
                DowntimeHours = downtimeHours,
                BreakdownDescription = breakdownDescription,
                Remarks = remarks,
                Status = UsageLogStatus.SUBMITTED,
                CreatedBy = createdBy
            };

            // Raise domain event - THIS IS KEY FOR AUTO-INCREMENT
            log.AddDomainEvent(new UsageLogCreatedEvent(log));

            return log;
        }

        /// <summary>
        /// Approve the usage log
        /// </summary>
        public void Approve()
        {
            if (Status == UsageLogStatus.APPROVED)
                throw new InvalidOperationException("Log is already approved");

            Status = UsageLogStatus.APPROVED;
            MarkUpdated();
        }

        /// <summary>
        /// Reject the usage log
        /// </summary>
        public void Reject()
        {
            if (Status == UsageLogStatus.REJECTED)
                throw new InvalidOperationException("Log is already rejected");

            Status = UsageLogStatus.REJECTED;
            MarkUpdated();
        }

        private static decimal? CalculateDelta(decimal? start, decimal? end)
        {
            return (start.HasValue && end.HasValue) ? end - start : null;
        }
    }
}
