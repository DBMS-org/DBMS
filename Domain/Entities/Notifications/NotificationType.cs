namespace Domain.Entities.Notifications
{
    /// <summary>
    /// Types of notifications that can be sent to users
    /// Grouped by functional area for better organization
    /// </summary>
    public enum NotificationType
    {
        // ===== EXPLOSIVE APPROVAL REQUESTS (100-199) =====

        /// <summary>
        /// New explosive approval request created (notifies Store Manager)
        /// </summary>
        ExplosiveRequestCreated = 100,

        /// <summary>
        /// Explosive approval request approved (notifies Blasting Engineer)
        /// </summary>
        ExplosiveRequestApproved = 101,

        /// <summary>
        /// Explosive approval request rejected (notifies Blasting Engineer)
        /// </summary>
        ExplosiveRequestRejected = 102,

        /// <summary>
        /// Explosive approval request cancelled (notifies Store Manager)
        /// </summary>
        ExplosiveRequestCancelled = 103,

        /// <summary>
        /// Explosive approval request expired (notifies Blasting Engineer)
        /// </summary>
        ExplosiveRequestExpired = 104,

        /// <summary>
        /// Explosive approval request updated (notifies Store Manager)
        /// </summary>
        ExplosiveRequestUpdated = 105,

        // ===== INVENTORY TRANSFER REQUESTS (200-299) =====

        /// <summary>
        /// New transfer request created (notifies Explosive Manager)
        /// </summary>
        TransferRequestCreated = 200,

        /// <summary>
        /// Transfer request is urgent (notifies Explosive Manager)
        /// </summary>
        TransferRequestUrgent = 201,

        /// <summary>
        /// Transfer request approved (notifies Store Manager)
        /// </summary>
        TransferRequestApproved = 202,

        /// <summary>
        /// Transfer request rejected (notifies Store Manager)
        /// </summary>
        TransferRequestRejected = 203,

        /// <summary>
        /// Transfer dispatched (notifies Store Manager)
        /// </summary>
        TransferDispatched = 204,

        /// <summary>
        /// Transfer completed (notifies Store Manager)
        /// </summary>
        TransferCompleted = 205,

        /// <summary>
        /// Transfer request cancelled (notifies Store Manager and Explosive Manager)
        /// </summary>
        TransferCancelled = 206,

        /// <summary>
        /// Transfer delivery confirmed by Store Manager (notifies Explosive Manager)
        /// </summary>
        TransferDeliveryConfirmed = 207,

        // ===== MACHINE ASSIGNMENT REQUESTS (300-399) =====

        /// <summary>
        /// New machine assignment request created (notifies Machine Manager)
        /// </summary>
        MachineRequestCreated = 300,

        /// <summary>
        /// Machine assignment request approved (notifies requester)
        /// </summary>
        MachineRequestApproved = 301,

        /// <summary>
        /// Machine assignment request rejected (notifies requester)
        /// </summary>
        MachineRequestRejected = 302,

        /// <summary>
        /// Machine assignment partially fulfilled (notifies requester)
        /// </summary>
        MachineRequestPartial = 303,

        /// <summary>
        /// Machine assignment request completed (notifies Machine Manager)
        /// </summary>
        MachineRequestCompleted = 304,

        // ===== MACHINE ASSIGNMENTS (400-499) =====

        /// <summary>
        /// Machine assigned to operator (notifies Operator)
        /// </summary>
        MachineAssigned = 400,

        /// <summary>
        /// Machine assignment overdue (notifies Operator and Machine Manager)
        /// </summary>
        MachineAssignmentOverdue = 401,

        /// <summary>
        /// Machine returned (notifies Machine Manager)
        /// </summary>
        MachineReturned = 402,

        /// <summary>
        /// Machine assignment cancelled (notifies Operator)
        /// </summary>
        MachineAssignmentCancelled = 403,

        // ===== MAINTENANCE REPORTS (500-599) =====

        /// <summary>
        /// New maintenance report created (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceReportCreated = 500,

        /// <summary>
        /// Maintenance report acknowledged (notifies Operator)
        /// </summary>
        MaintenanceReportAcknowledged = 501,

        /// <summary>
        /// Maintenance report in progress (notifies Operator)
        /// </summary>
        MaintenanceReportInProgress = 502,

        /// <summary>
        /// Maintenance report resolved (notifies Operator)
        /// </summary>
        MaintenanceReportResolved = 503,

        /// <summary>
        /// Maintenance report closed (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceReportClosed = 504,

        /// <summary>
        /// Maintenance report reopened (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceReportReopened = 505,

        // ===== MAINTENANCE JOBS (600-699) =====

        /// <summary>
        /// New maintenance job created (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceJobCreated = 600,

        /// <summary>
        /// Maintenance job due soon (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceJobDueSoon = 601,

        /// <summary>
        /// Maintenance job overdue (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceJobOverdue = 602,

        /// <summary>
        /// Maintenance job started (notifies creator)
        /// </summary>
        MaintenanceJobStarted = 603,

        /// <summary>
        /// Maintenance job completed (notifies creator and Machine Manager)
        /// </summary>
        MaintenanceJobCompleted = 604,

        /// <summary>
        /// Maintenance job cancelled (notifies Mechanical Engineer)
        /// </summary>
        MaintenanceJobCancelled = 605,

        // ===== SYSTEM & ADMIN (700-799) =====

        /// <summary>
        /// User account created (notifies new user)
        /// </summary>
        UserAccountCreated = 700,

        /// <summary>
        /// User role changed (notifies user)
        /// </summary>
        UserRoleChanged = 701,

        /// <summary>
        /// User assigned to region (notifies user)
        /// </summary>
        UserRegionAssigned = 702,

        /// <summary>
        /// New project created (notifies Blasting Engineers in region)
        /// </summary>
        ProjectCreated = 703,

        /// <summary>
        /// New project site created (notifies assigned Blasting Engineer)
        /// </summary>
        ProjectSiteCreated = 704,

        /// <summary>
        /// Project site updated (notifies relevant stakeholders)
        /// </summary>
        ProjectSiteUpdated = 705,

        /// <summary>
        /// Blast simulation confirmed (notifies Admin for final approval)
        /// </summary>
        BlastSimulationConfirmed = 706,

        /// <summary>
        /// Pattern approved for drilling (notifies Operators)
        /// </summary>
        PatternApproved = 707,

        /// <summary>
        /// Pattern approval revoked (notifies Operators)
        /// </summary>
        PatternApprovalRevoked = 708,

        // ===== GENERIC (1000+) =====

        /// <summary>
        /// Generic system notification
        /// </summary>
        System = 1000,

        /// <summary>
        /// Informational notification
        /// </summary>
        Info = 1001,

        /// <summary>
        /// Warning notification
        /// </summary>
        Warning = 1002,

        /// <summary>
        /// Error notification
        /// </summary>
        Error = 1003
    }
}
