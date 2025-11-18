/**
 * Notification Type Enum
 * Matches backend NotificationType enum values exactly
 * Organized by functional area with numeric ranges
 */
export enum NotificationType {
  // ===== EXPLOSIVE APPROVAL REQUESTS (100-199) =====
  ExplosiveRequestCreated = 100,
  ExplosiveRequestApproved = 101,
  ExplosiveRequestRejected = 102,
  ExplosiveRequestCancelled = 103,
  ExplosiveRequestExpired = 104,
  ExplosiveRequestUpdated = 105,

  // ===== INVENTORY TRANSFER REQUESTS (200-299) =====
  TransferRequestCreated = 200,
  TransferRequestUrgent = 201,
  TransferRequestApproved = 202,
  TransferRequestRejected = 203,
  TransferDispatched = 204,
  TransferCompleted = 205,
  TransferCancelled = 206,
  TransferDeliveryConfirmed = 207,

  // ===== MACHINE ASSIGNMENT REQUESTS (300-399) =====
  MachineRequestCreated = 300,
  MachineRequestApproved = 301,
  MachineRequestRejected = 302,
  MachineRequestPartial = 303,
  MachineRequestCompleted = 304,

  // ===== MACHINE ASSIGNMENTS (400-499) =====
  MachineAssigned = 400,
  MachineAssignmentOverdue = 401,
  MachineReturned = 402,
  MachineAssignmentCancelled = 403,

  // ===== MAINTENANCE REPORTS (500-599) =====
  MaintenanceReportCreated = 500,
  MaintenanceReportAcknowledged = 501,
  MaintenanceReportInProgress = 502,
  MaintenanceReportResolved = 503,
  MaintenanceReportClosed = 504,
  MaintenanceReportReopened = 505,

  // ===== MAINTENANCE JOBS (600-699) =====
  MaintenanceJobCreated = 600,
  MaintenanceJobDueSoon = 601,
  MaintenanceJobOverdue = 602,
  MaintenanceJobStarted = 603,
  MaintenanceJobCompleted = 604,
  MaintenanceJobCancelled = 605,

  // ===== SYSTEM & ADMIN (700-799) =====
  UserAccountCreated = 700,
  UserRoleChanged = 701,
  UserRegionAssigned = 702,
  ProjectCreated = 703,
  ProjectSiteCreated = 704,
  ProjectSiteUpdated = 705,
  BlastSimulationConfirmed = 706,
  PatternApproved = 707,
  PatternApprovalRevoked = 708,

  // ===== BLAST PROPOSALS (800-899) =====
  BlastProposalSubmitted = 800,
  BlastProposalApproved = 801,
  BlastProposalRejected = 802,

  // ===== DRILL DATA (900-999) =====
  DrillDataUploaded = 900,
  DrillDataProcessed = 901,

  // ===== GENERIC (1000+) =====
  System = 1000,
  Info = 1001,
  Warning = 1002,
  Error = 1003
}

/**
 * Helper function to get notification type display name
 */
export function getNotificationTypeDisplayName(type: NotificationType): string {
  switch (type) {
    // Explosive Approval Requests
    case NotificationType.ExplosiveRequestCreated:
      return 'Explosive Request Created';
    case NotificationType.ExplosiveRequestApproved:
      return 'Explosive Request Approved';
    case NotificationType.ExplosiveRequestRejected:
      return 'Explosive Request Rejected';
    case NotificationType.ExplosiveRequestCancelled:
      return 'Explosive Request Cancelled';
    case NotificationType.ExplosiveRequestExpired:
      return 'Explosive Request Expired';
    case NotificationType.ExplosiveRequestUpdated:
      return 'Explosive Request Updated';

    // Transfer Requests
    case NotificationType.TransferRequestCreated:
      return 'Transfer Request Created';
    case NotificationType.TransferRequestUrgent:
      return 'Urgent Transfer Request';
    case NotificationType.TransferRequestApproved:
      return 'Transfer Request Approved';
    case NotificationType.TransferRequestRejected:
      return 'Transfer Request Rejected';
    case NotificationType.TransferDispatched:
      return 'Transfer Dispatched';
    case NotificationType.TransferCompleted:
      return 'Transfer Completed';
    case NotificationType.TransferCancelled:
      return 'Transfer Cancelled';
    case NotificationType.TransferDeliveryConfirmed:
      return 'Delivery Confirmed';

    // Machine Requests
    case NotificationType.MachineRequestCreated:
      return 'Machine Request Created';
    case NotificationType.MachineRequestApproved:
      return 'Machine Request Approved';
    case NotificationType.MachineRequestRejected:
      return 'Machine Request Rejected';
    case NotificationType.MachineRequestPartial:
      return 'Machine Request Partially Fulfilled';
    case NotificationType.MachineRequestCompleted:
      return 'Machine Request Completed';

    // Machine Assignments
    case NotificationType.MachineAssigned:
      return 'Machine Assigned';
    case NotificationType.MachineAssignmentOverdue:
      return 'Machine Assignment Overdue';
    case NotificationType.MachineReturned:
      return 'Machine Returned';
    case NotificationType.MachineAssignmentCancelled:
      return 'Machine Assignment Cancelled';

    // Maintenance Reports
    case NotificationType.MaintenanceReportCreated:
      return 'Maintenance Report Created';
    case NotificationType.MaintenanceReportAcknowledged:
      return 'Maintenance Report Acknowledged';
    case NotificationType.MaintenanceReportInProgress:
      return 'Maintenance In Progress';
    case NotificationType.MaintenanceReportResolved:
      return 'Maintenance Report Resolved';
    case NotificationType.MaintenanceReportClosed:
      return 'Maintenance Report Closed';
    case NotificationType.MaintenanceReportReopened:
      return 'Maintenance Report Reopened';

    // Maintenance Jobs
    case NotificationType.MaintenanceJobCreated:
      return 'Maintenance Job Created';
    case NotificationType.MaintenanceJobDueSoon:
      return 'Maintenance Job Due Soon';
    case NotificationType.MaintenanceJobOverdue:
      return 'Maintenance Job Overdue';
    case NotificationType.MaintenanceJobStarted:
      return 'Maintenance Job Started';
    case NotificationType.MaintenanceJobCompleted:
      return 'Maintenance Job Completed';
    case NotificationType.MaintenanceJobCancelled:
      return 'Maintenance Job Cancelled';

    // System/Admin
    case NotificationType.UserAccountCreated:
      return 'User Account Created';
    case NotificationType.UserRoleChanged:
      return 'User Role Changed';
    case NotificationType.UserRegionAssigned:
      return 'User Region Assigned';
    case NotificationType.ProjectCreated:
      return 'Project Created';
    case NotificationType.ProjectSiteCreated:
      return 'Project Site Created';
    case NotificationType.ProjectSiteUpdated:
      return 'Project Site Updated';
    case NotificationType.BlastSimulationConfirmed:
      return 'Blast Simulation Confirmed';
    case NotificationType.PatternApproved:
      return 'Pattern Approved';
    case NotificationType.PatternApprovalRevoked:
      return 'Pattern Approval Revoked';

    // Blast Proposals
    case NotificationType.BlastProposalSubmitted:
      return 'Blast Proposal Submitted';
    case NotificationType.BlastProposalApproved:
      return 'Blast Proposal Approved';
    case NotificationType.BlastProposalRejected:
      return 'Blast Proposal Rejected';

    // Drill Data
    case NotificationType.DrillDataUploaded:
      return 'Drill Data Uploaded';
    case NotificationType.DrillDataProcessed:
      return 'Drill Data Processed';

    // Generic
    case NotificationType.System:
      return 'System Notification';
    case NotificationType.Info:
      return 'Information';
    case NotificationType.Warning:
      return 'Warning';
    case NotificationType.Error:
      return 'Error';

    default:
      return 'Notification';
  }
}

/**
 * Helper function to get notification type category
 */
export function getNotificationTypeCategory(type: NotificationType): string {
  if (type >= 100 && type < 200) return 'Explosive Requests';
  if (type >= 200 && type < 300) return 'Transfer Requests';
  if (type >= 300 && type < 400) return 'Machine Requests';
  if (type >= 400 && type < 500) return 'Machine Assignments';
  if (type >= 500 && type < 600) return 'Maintenance Reports';
  if (type >= 600 && type < 700) return 'Maintenance Jobs';
  if (type >= 700 && type < 800) return 'System & Admin';
  if (type >= 800 && type < 900) return 'Blast Proposals';
  if (type >= 900 && type < 1000) return 'Drill Data';
  if (type >= 1000) return 'General';
  return 'Unknown';
}

/**
 * Helper function to get icon for notification type
 */
export function getNotificationTypeIcon(type: NotificationType): string {
  if (type >= 100 && type < 200) return 'whatshot'; // Explosive
  if (type >= 200 && type < 300) return 'local_shipping'; // Transfer
  if (type >= 300 && type < 400) return 'assignment'; // Machine Request
  if (type >= 400 && type < 500) return 'build_circle'; // Machine Assignment
  if (type >= 500 && type < 600) return 'report_problem'; // Maintenance Report
  if (type >= 600 && type < 700) return 'construction'; // Maintenance Job
  if (type >= 700 && type < 800) return 'admin_panel_settings'; // System/Admin
  if (type >= 800 && type < 900) return 'science'; // Blast Proposals
  if (type >= 900 && type < 1000) return 'analytics'; // Drill Data

  // Generic icons
  switch (type) {
    case NotificationType.System:
      return 'info';
    case NotificationType.Info:
      return 'info_outline';
    case NotificationType.Warning:
      return 'warning';
    case NotificationType.Error:
      return 'error';
    default:
      return 'notifications';
  }
}
