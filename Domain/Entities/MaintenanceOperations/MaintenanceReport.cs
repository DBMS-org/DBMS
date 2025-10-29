using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.MachineManagement;
using Domain.Entities.UserManagement;

namespace Domain.Entities.MaintenanceOperations
{
    /// <summary>
    /// Represents a maintenance problem report submitted by an operator
    /// </summary>
    public class MaintenanceReport : BaseAuditableEntity
    {
        // Unique identifier for tracking
        public string TicketId { get; private set; } = string.Empty;

        // Operator who reported the issue
        public int OperatorId { get; private set; }

        // Machine being reported
        public int MachineId { get; private set; }

        // Denormalized machine data for historical reference
        public string MachineName { get; private set; } = string.Empty;
        public string? MachineModel { get; private set; }
        public string? SerialNumber { get; private set; }
        public string? Location { get; private set; }

        // Problem details
        public MachinePart AffectedPart { get; private set; }
        public ProblemCategory ProblemCategory { get; private set; }
        public string CustomDescription { get; private set; } = string.Empty;

        // Diagnostic information (JSON serialized arrays)
        public string? Symptoms { get; private set; }  // JSON array of selected symptoms
        public string? ErrorCodes { get; private set; }
        public string? RecentMaintenanceHistory { get; private set; }

        // Severity and status
        public SeverityLevel Severity { get; private set; }
        public ReportStatus Status { get; private set; }

        // Timestamps for tracking
        public DateTime ReportedAt { get; private set; }
        public DateTime? AcknowledgedAt { get; private set; }
        public DateTime? InProgressAt { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }

        // Assigned mechanical engineer
        public int? MechanicalEngineerId { get; private set; }
        public string? MechanicalEngineerName { get; private set; }

        // Resolution information
        public string? ResolutionNotes { get; private set; }
        public string? EstimatedResponseTime { get; private set; }

        // Navigation properties
        public virtual User Operator { get; private set; } = null!;
        public virtual Machine Machine { get; private set; } = null!;
        public virtual User? MechanicalEngineer { get; private set; }
        public virtual MaintenanceJob? MaintenanceJob { get; private set; }

        // Private constructor for EF Core
        private MaintenanceReport() { }

        /// <summary>
        /// Creates a new maintenance report
        /// </summary>
        public static MaintenanceReport Create(
            int operatorId,
            int machineId,
            string machineName,
            string? machineModel,
            string? serialNumber,
            string? location,
            MachinePart affectedPart,
            ProblemCategory problemCategory,
            string customDescription,
            string? symptoms,
            string? errorCodes,
            SeverityLevel severity)
        {
            var report = new MaintenanceReport
            {
                TicketId = GenerateTicketId(),
                OperatorId = operatorId,
                MachineId = machineId,
                MachineName = machineName,
                MachineModel = machineModel,
                SerialNumber = serialNumber,
                Location = location,
                AffectedPart = affectedPart,
                ProblemCategory = problemCategory,
                CustomDescription = customDescription,
                Symptoms = symptoms,
                ErrorCodes = errorCodes,
                Severity = severity,
                Status = ReportStatus.Reported,
                ReportedAt = DateTime.UtcNow
            };

            return report;
        }

        /// <summary>
        /// Generates a unique ticket ID in format: MR-YYYYMMDD-####
        /// </summary>
        private static string GenerateTicketId()
        {
            var dateStr = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = new Random();
            var sequence = random.Next(1000, 9999);
            return $"MR-{dateStr}-{sequence}";
        }

        /// <summary>
        /// Acknowledges the report and assigns a mechanical engineer
        /// </summary>
        public void Acknowledge(int mechanicalEngineerId, string mechanicalEngineerName, string? estimatedResponseTime = null)
        {
            if (Status != ReportStatus.Reported)
                throw new InvalidOperationException("Only reported issues can be acknowledged.");

            Status = ReportStatus.Acknowledged;
            AcknowledgedAt = DateTime.UtcNow;
            MechanicalEngineerId = mechanicalEngineerId;
            MechanicalEngineerName = mechanicalEngineerName;
            EstimatedResponseTime = estimatedResponseTime;
            MarkUpdated();
        }

        /// <summary>
        /// Marks the report as in progress
        /// </summary>
        public void MarkInProgress()
        {
            if (Status != ReportStatus.Acknowledged && Status != ReportStatus.Reported)
                throw new InvalidOperationException("Only acknowledged or reported issues can be marked in progress.");

            Status = ReportStatus.InProgress;
            InProgressAt = DateTime.UtcNow;
            MarkUpdated();
        }

        /// <summary>
        /// Marks the report as resolved with engineer's notes
        /// </summary>
        public void Resolve(string resolutionNotes)
        {
            if (Status != ReportStatus.InProgress)
                throw new InvalidOperationException("Only in-progress issues can be resolved.");

            if (string.IsNullOrWhiteSpace(resolutionNotes))
                throw new ArgumentException("Resolution notes are required.", nameof(resolutionNotes));

            Status = ReportStatus.Resolved;
            ResolvedAt = DateTime.UtcNow;
            ResolutionNotes = resolutionNotes;
            MarkUpdated();
        }

        /// <summary>
        /// Closes the report after operator verification
        /// </summary>
        public void Close()
        {
            if (Status != ReportStatus.Resolved)
                throw new InvalidOperationException("Only resolved issues can be closed.");

            Status = ReportStatus.Closed;
            ClosedAt = DateTime.UtcNow;
            MarkUpdated();
        }

        /// <summary>
        /// Reopens a resolved report if operator rejects the fix
        /// </summary>
        public void Reopen(string reason)
        {
            if (Status != ReportStatus.Resolved)
                throw new InvalidOperationException("Only resolved issues can be reopened.");

            Status = ReportStatus.InProgress;
            ResolutionNotes = $"{ResolutionNotes}\n\n[REOPENED] {DateTime.UtcNow:yyyy-MM-dd HH:mm}: {reason}";
            MarkUpdated();
        }

        /// <summary>
        /// Updates the status of the report
        /// </summary>
        public void UpdateStatus(ReportStatus newStatus)
        {
            Status = newStatus;

            switch (newStatus)
            {
                case ReportStatus.Acknowledged when AcknowledgedAt == null:
                    AcknowledgedAt = DateTime.UtcNow;
                    break;
                case ReportStatus.InProgress when InProgressAt == null:
                    InProgressAt = DateTime.UtcNow;
                    break;
                case ReportStatus.Resolved when ResolvedAt == null:
                    ResolvedAt = DateTime.UtcNow;
                    break;
                case ReportStatus.Closed when ClosedAt == null:
                    ClosedAt = DateTime.UtcNow;
                    break;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Calculates the total time from report to closure
        /// </summary>
        public TimeSpan? GetTotalResolutionTime()
        {
            if (ClosedAt.HasValue)
                return ClosedAt.Value - ReportedAt;

            return null;
        }

        /// <summary>
        /// Calculates the time from acknowledgment to resolution
        /// </summary>
        public TimeSpan? GetWorkTime()
        {
            if (ResolvedAt.HasValue && AcknowledgedAt.HasValue)
                return ResolvedAt.Value - AcknowledgedAt.Value;

            return null;
        }
    }
}
