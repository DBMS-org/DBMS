// Enums and Types
export enum MaintenanceType {
  PREVENTIVE = 'PREVENTIVE',
  CORRECTIVE = 'CORRECTIVE',
  PREDICTIVE = 'PREDICTIVE',
  EMERGENCY = 'EMERGENCY'
}

export enum MaintenanceStatus {
  SCHEDULED = 'SCHEDULED',
  IN_PROGRESS = 'IN_PROGRESS',
  COMPLETED = 'COMPLETED',
  CANCELLED = 'CANCELLED',
  OVERDUE = 'OVERDUE'
}

export enum AlertType {
  SERVICE_DUE = 'SERVICE_DUE',
  OVERDUE = 'OVERDUE'
}

export enum Priority {
  HIGH = 'HIGH',
  MEDIUM = 'MEDIUM',
  LOW = 'LOW'
}

// Core Interfaces
export interface MaintenanceJob {
  id: string;
  machineId: string;
  machineName: string;
  serialNumber: string;
  project: string;
  scheduledDate: Date;
  type: MaintenanceType;
  status: MaintenanceStatus;
  assignedTo: string[];
  estimatedHours: number;
  actualHours?: number;
  reason: string;
  observations?: string;
  partsReplaced?: string[];
  attachments?: FileAttachment[];
  createdAt: Date;
  updatedAt: Date;
}

export interface MaintenanceAlert {
  id: string;
  machineId: string;
  machineName: string;
  alertType: AlertType;
  message: string;
  dueDate: Date;
  priority: Priority;
  daysPastDue?: number;
  daysUntilDue?: number;
}

export interface MaintenanceStats {
  totalMachines: number;
  scheduledJobs: number;
  inProgressJobs: number;
  completedJobs: number;
  overdueJobs: number;
  serviceDueAlerts: number;
}

export interface MachineMaintenanceHistory {
  machineId: string;
  machineName: string;
  model: string;
  serialNumber: string;
  currentStatus: string;
  lastServiceDate: Date;
  nextServiceDate: Date;
  engineHours: number;
  serviceHours: number;
  idleHours: number;
  maintenanceRecords: MaintenanceRecord[];
}

export interface MaintenanceRecord {
  id: string;
  date: Date;
  type: MaintenanceType;
  description: string;
  technician: string;
  hoursSpent: number;
  partsUsed?: string[];
  notes?: string;
}

export interface FileAttachment {
  id: string;
  fileName: string;
  fileType: string;
  fileSize: number;
  uploadedAt: Date;
  url: string;
}

export interface ServiceIntervalConfig {
  machineType: string;
  intervalHours?: number;
  intervalMonths?: number;
  alertWindowDays: number;
}

export interface JobFilters {
  dateRange?: { start: Date; end: Date };
  status?: MaintenanceStatus[];
  machineType?: string[];
  project?: string[];
  assignedTo?: string[];
  searchTerm?: string;
}

export interface NotificationPreferences {
  emailNotifications: boolean;
  inAppNotifications: boolean;
  alertWindowDays: number;
  overdueNotifications: boolean;
  
  // Advanced notification settings
  emailFrequency?: 'immediate' | 'daily' | 'weekly';
  quietHoursEnabled?: boolean;
  quietHoursStart?: string;
  quietHoursEnd?: string;
  weekendNotifications?: boolean;

  // Notification type preferences
  serviceDueEmail?: boolean;
  serviceDueInApp?: boolean;
  overdueEmail?: boolean;
  overdueInApp?: boolean;
  jobAssignedEmail?: boolean;
  jobAssignedInApp?: boolean;
  jobCompletedEmail?: boolean;
  jobCompletedInApp?: boolean;
  systemAlertsEmail?: boolean;
  systemAlertsInApp?: boolean;

  // Escalation settings
  escalationEnabled?: boolean;
  escalationDelayHours?: number;
  escalationRecipients?: string[];
}

// Analytics Interfaces
export interface ServiceComplianceData {
  onTime: number;
  overdue: number;
  percentage: number;
}

export interface MTBFMetrics {
  machineType: string;
  mtbfHours: number;
  periodMonths: number;
  failureCount: number;
}

export interface PartsUsageData {
  partName: string;
  usageCount: number;
  totalCost: number;
  machineTypes: string[];
}

export interface UsageMetrics {
  machineId: string;
  engineHours: number;
  idleHours: number;
  serviceHours: number;
  lastUpdated: Date;
}