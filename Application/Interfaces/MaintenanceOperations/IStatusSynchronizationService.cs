using Application.DTOs.Shared;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.MachineManagement;

namespace Application.Interfaces.MaintenanceOperations
{
    /// <summary>
    /// Service responsible for synchronizing statuses between MaintenanceReports, MaintenanceJobs, and Machines
    /// to maintain data consistency across the maintenance workflow
    /// </summary>
    public interface IStatusSynchronizationService
    {
        /// <summary>
        /// Synchronizes the status between a maintenance report and its associated job
        /// </summary>
        Task<Result<bool>> SynchronizeReportAndJobAsync(int reportId);

        /// <summary>
        /// Updates machine status based on maintenance report severity and status
        /// </summary>
        Task<Result<bool>> UpdateMachineStatusAsync(int machineId, SeverityLevel severity, ReportStatus reportStatus);

        /// <summary>
        /// Updates machine status when a job is completed
        /// </summary>
        Task<Result<bool>> HandleJobCompletionAsync(int jobId);

        /// <summary>
        /// Checks if machine can be returned to Available status
        /// (no active HIGH or CRITICAL reports exist)
        /// </summary>
        Task<Result<bool>> CanRestoreMachineStatusAsync(int machineId);

        /// <summary>
        /// Restores machine to Available or Assigned status after all critical issues resolved
        /// </summary>
        Task<Result<bool>> RestoreMachineStatusAsync(int machineId);

        /// <summary>
        /// Updates machine's last maintenance date
        /// </summary>
        Task<Result<bool>> UpdateMachineMaintenanceDateAsync(int machineId, DateTime maintenanceDate);
    }
}
