using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.MachineManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.MaintenanceOperations
{
    /// <summary>
    /// Represents a maintenance job to be performed by mechanical engineers
    /// </summary>
    public class MaintenanceJob : BaseAuditableEntity
    {
        // Machine being serviced
        public int MachineId { get; private set; }

        // Associated project (for business context)
        public int? ProjectId { get; private set; }

        // Associated maintenance report (nullable for manual/preventive jobs)
        public int? MaintenanceReportId { get; private set; }

        // Job classification
        public MaintenanceType Type { get; private set; }
        public MaintenanceJobStatus Status { get; private set; }

        // Scheduling
        public DateTime ScheduledDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public DateTime? InProgressAt { get; private set; }

        // Time estimation and tracking
        public decimal EstimatedHours { get; private set; }
        public decimal? ActualHours { get; private set; }

        // Job details
        public string Reason { get; private set; } = string.Empty;
        public string? Observations { get; private set; }

        // Parts replaced (JSON serialized array)
        public string? PartsReplaced { get; private set; }

        // Creator of the job
        public int CreatedBy { get; private set; }

        // Navigation properties
        public virtual Machine Machine { get; private set; } = null!;
        public virtual Project? Project { get; private set; }
        public virtual MaintenanceReport? MaintenanceReport { get; private set; }
        public virtual User Creator { get; private set; } = null!;
        public virtual ICollection<MaintenanceJobAssignment> Assignments { get; private set; } = new List<MaintenanceJobAssignment>();

        // Private constructor for EF Core
        private MaintenanceJob() { }

        /// <summary>
        /// Creates a new maintenance job
        /// </summary>
        public static MaintenanceJob Create(
            int machineId,
            int? projectId,
            MaintenanceType type,
            string reason,
            decimal estimatedHours,
            int createdBy,
            int? maintenanceReportId = null,
            DateTime? scheduledDate = null)
        {
            var job = new MaintenanceJob
            {
                MachineId = machineId,
                ProjectId = projectId,
                MaintenanceReportId = maintenanceReportId,
                Type = type,
                Status = MaintenanceJobStatus.Scheduled,
                Reason = reason,
                EstimatedHours = estimatedHours,
                ScheduledDate = scheduledDate ?? DateTime.UtcNow,
                CreatedBy = createdBy
            };

            return job;
        }

        /// <summary>
        /// Creates a maintenance job from a maintenance report
        /// </summary>
        public static MaintenanceJob CreateFromReport(
            MaintenanceReport report,
            int machineId,
            int? projectId,
            int createdBy,
            decimal estimatedHours)
        {
            // Determine maintenance type based on severity
            var maintenanceType = report.Severity switch
            {
                SeverityLevel.Critical => MaintenanceType.Emergency,
                SeverityLevel.High => MaintenanceType.Emergency,
                SeverityLevel.Medium => MaintenanceType.Corrective,
                SeverityLevel.Low => MaintenanceType.Corrective,
                _ => MaintenanceType.Corrective
            };

            var job = new MaintenanceJob
            {
                MachineId = machineId,
                ProjectId = projectId,
                MaintenanceReportId = report.Id,
                Type = maintenanceType,
                Status = MaintenanceJobStatus.Scheduled,
                Reason = $"{report.ProblemCategory}: {report.CustomDescription}",
                EstimatedHours = estimatedHours,
                ScheduledDate = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            return job;
        }

        /// <summary>
        /// Assigns a mechanical engineer to this job
        /// </summary>
        public void AssignEngineer(int engineerId, DateTime? assignedAt = null)
        {
            var assignment = new MaintenanceJobAssignment
            {
                MaintenanceJobId = this.Id,
                MechanicalEngineerId = engineerId,
                AssignedAt = assignedAt ?? DateTime.UtcNow
            };

            Assignments.Add(assignment);
            MarkUpdated();
        }

        /// <summary>
        /// Starts the maintenance job
        /// </summary>
        public void Start()
        {
            if (Status != MaintenanceJobStatus.Scheduled)
                throw new InvalidOperationException("Only scheduled jobs can be started.");

            Status = MaintenanceJobStatus.InProgress;
            InProgressAt = DateTime.UtcNow;
            MarkUpdated();
        }

        /// <summary>
        /// Completes the maintenance job
        /// </summary>
        public void Complete(string observations, decimal actualHours, string? partsReplaced = null)
        {
            if (Status != MaintenanceJobStatus.InProgress)
                throw new InvalidOperationException("Only in-progress jobs can be completed.");

            if (string.IsNullOrWhiteSpace(observations))
                throw new ArgumentException("Observations are required to complete a job.", nameof(observations));

            if (actualHours <= 0)
                throw new ArgumentException("Actual hours must be greater than zero.", nameof(actualHours));

            Status = MaintenanceJobStatus.Completed;
            CompletedDate = DateTime.UtcNow;
            Observations = observations;
            ActualHours = actualHours;
            PartsReplaced = partsReplaced;
            MarkUpdated();
        }

        /// <summary>
        /// Cancels the maintenance job
        /// </summary>
        public void Cancel(string reason)
        {
            if (Status == MaintenanceJobStatus.Completed)
                throw new InvalidOperationException("Completed jobs cannot be cancelled.");

            Status = MaintenanceJobStatus.Cancelled;
            Observations = $"[CANCELLED] {DateTime.UtcNow:yyyy-MM-dd HH:mm}: {reason}";
            MarkUpdated();
        }

        /// <summary>
        /// Updates the job status
        /// </summary>
        public void UpdateStatus(MaintenanceJobStatus newStatus)
        {
            Status = newStatus;

            switch (newStatus)
            {
                case MaintenanceJobStatus.InProgress when InProgressAt == null:
                    InProgressAt = DateTime.UtcNow;
                    break;
                case MaintenanceJobStatus.Completed when CompletedDate == null:
                    CompletedDate = DateTime.UtcNow;
                    break;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Checks if the job is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return Status != MaintenanceJobStatus.Completed
                && Status != MaintenanceJobStatus.Cancelled
                && ScheduledDate < DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the job as overdue
        /// </summary>
        public void MarkOverdue()
        {
            if (IsOverdue() && Status != MaintenanceJobStatus.Overdue)
            {
                Status = MaintenanceJobStatus.Overdue;
                MarkUpdated();
            }
        }

        /// <summary>
        /// Calculates the actual time spent on the job
        /// </summary>
        public TimeSpan? GetActualWorkTime()
        {
            if (CompletedDate.HasValue && InProgressAt.HasValue)
                return CompletedDate.Value - InProgressAt.Value;

            return null;
        }

        /// <summary>
        /// Gets the variance between estimated and actual hours
        /// </summary>
        public decimal? GetHoursVariance()
        {
            if (ActualHours.HasValue)
                return ActualHours.Value - EstimatedHours;

            return null;
        }
    }
}
