// Enums for maintenance reporting
export enum MachinePart {
  DRILL_BIT = 'DRILL_BIT',
  DRILL_ROD = 'DRILL_ROD',
  SHANK = 'SHANK',
  ENGINE = 'ENGINE',
  HYDRAULIC_SYSTEM = 'HYDRAULIC_SYSTEM',
  ELECTRICAL_SYSTEM = 'ELECTRICAL_SYSTEM',
  MECHANICAL_COMPONENTS = 'MECHANICAL_COMPONENTS',
  OTHER = 'OTHER'
}

export enum ProblemCategory {
  ENGINE_ISSUES = 'ENGINE_ISSUES',
  HYDRAULIC_PROBLEMS = 'HYDRAULIC_PROBLEMS',
  ELECTRICAL_FAULTS = 'ELECTRICAL_FAULTS',
  MECHANICAL_BREAKDOWN = 'MECHANICAL_BREAKDOWN',
  DRILL_BIT_ISSUES = 'DRILL_BIT_ISSUES',
  DRILL_ROD_PROBLEMS = 'DRILL_ROD_PROBLEMS',
  OTHER = 'OTHER'
}

export enum SeverityLevel {
  CRITICAL = 'CRITICAL',    // Machine Down
  HIGH = 'HIGH',           // Performance Issues
  MEDIUM = 'MEDIUM',       // Minor Issues
  LOW = 'LOW'              // Maintenance Needed
}

export enum ReportStatus {
  REPORTED = 'REPORTED',
  ACKNOWLEDGED = 'ACKNOWLEDGED',
  IN_PROGRESS = 'IN_PROGRESS',
  RESOLVED = 'RESOLVED',
  CLOSED = 'CLOSED'
}

export enum MachineStatus {
  OPERATIONAL = 'OPERATIONAL',
  UNDER_MAINTENANCE = 'UNDER_MAINTENANCE',
  DOWN_FOR_REPAIR = 'DOWN_FOR_REPAIR',
  OFFLINE = 'OFFLINE'
}

// Core interfaces
export interface ProblemReport {
  id: string;
  operatorId: string;
  operatorName: string;
  operatorEmail?: string;
  operatorPhone?: string;
  machineId: string;
  machineName: string;
  machineModel: string;
  serialNumber: string;
  location: string;
  projectName?: string;
  projectId?: string;
  regionName?: string;
  regionId?: string;
  affectedPart: MachinePart;
  problemCategory: ProblemCategory;
  customDescription: string;
  symptoms: string[];
  errorCodes?: string;
  recentMaintenanceHistory?: string;
  severity: SeverityLevel;
  status: ReportStatus;
  ticketId: string;
  reportedAt: Date;
  acknowledgedAt?: Date;
  inProgressAt?: Date;
  resolvedAt?: Date;
  closedAt?: Date;
  mechanicalEngineerId?: string;
  mechanicalEngineerName?: string;
  mechanicalEngineerEmail?: string;
  mechanicalEngineerPhone?: string;
  resolutionNotes?: string;
  estimatedResponseTime?: string;
}

export interface OperatorMachine {
  id: string;
  name: string;
  model: string;
  serialNumber: string;
  currentLocation: string;
  status: MachineStatus;
  assignedOperatorId: string;
}

export interface ProblemReportSummary {
  totalReports: number;
  reportedToday: number;
  inProgressReports: number;
  resolvedThisWeek: number;
  averageResponseTime: string;
}

export interface ReportFilters {
  status?: ReportStatus[];
  dateRange?: { start: Date; end: Date };
  severity?: SeverityLevel[];
  searchTerm?: string;
}

// Request/Response DTOs for API communication
export interface CreateProblemReportRequest {
  machineId: string;
  machineName: string;
  machineModel: string;
  serialNumber: string;
  location: string;
  affectedPart: MachinePart;
  problemCategory: ProblemCategory;
  customDescription: string;
  symptoms: string[];
  errorCodes?: string;
  recentMaintenanceHistory?: string;
  severity: SeverityLevel;
}

export interface UpdateReportStatusRequest {
  status: ReportStatus;
  resolutionNotes?: string;
  estimatedResponseTime?: string;
}

// Display helper interfaces
export interface MachinePartOption {
  value: MachinePart;
  label: string;
  icon: string;
}

export interface ProblemCategoryOption {
  value: ProblemCategory;
  label: string;
  icon: string;
}

export interface SeverityLevelOption {
  value: SeverityLevel;
  label: string;
  description: string;
  color: string;
}

export interface ReportStatusOption {
  value: ReportStatus;
  label: string;
  color: string;
  icon: string;
}

// Common symptoms for checkboxes
export interface SymptomOption {
  value: string;
  label: string;
  category?: ProblemCategory;
}

// Constants for UI display
export const MACHINE_PART_OPTIONS: MachinePartOption[] = [
  { value: MachinePart.DRILL_BIT, label: 'Drill Bit', icon: 'construction' },
  { value: MachinePart.DRILL_ROD, label: 'Drill Rod', icon: 'linear_scale' },
  { value: MachinePart.SHANK, label: 'Shank', icon: 'hardware' },
  { value: MachinePart.ENGINE, label: 'Engine', icon: 'settings' },
  { value: MachinePart.HYDRAULIC_SYSTEM, label: 'Hydraulic System', icon: 'water_drop' },
  { value: MachinePart.ELECTRICAL_SYSTEM, label: 'Electrical System', icon: 'electrical_services' },
  { value: MachinePart.MECHANICAL_COMPONENTS, label: 'Mechanical Components', icon: 'precision_manufacturing' },
  { value: MachinePart.OTHER, label: 'Other', icon: 'help_outline' }
];

export const PROBLEM_CATEGORY_OPTIONS: ProblemCategoryOption[] = [
  { value: ProblemCategory.ENGINE_ISSUES, label: 'Engine Issues', icon: 'settings' },
  { value: ProblemCategory.HYDRAULIC_PROBLEMS, label: 'Hydraulic Problems', icon: 'water_drop' },
  { value: ProblemCategory.ELECTRICAL_FAULTS, label: 'Electrical Faults', icon: 'electrical_services' },
  { value: ProblemCategory.MECHANICAL_BREAKDOWN, label: 'Mechanical Breakdown', icon: 'build' },
  { value: ProblemCategory.DRILL_BIT_ISSUES, label: 'Drill Bit Issues', icon: 'construction' },
  { value: ProblemCategory.DRILL_ROD_PROBLEMS, label: 'Drill Rod Problems', icon: 'linear_scale' },
  { value: ProblemCategory.OTHER, label: 'Other', icon: 'help_outline' }
];

export const SEVERITY_LEVEL_OPTIONS: SeverityLevelOption[] = [
  { 
    value: SeverityLevel.CRITICAL, 
    label: 'Critical', 
    description: 'Machine Down - Immediate attention required',
    color: '#f44336'
  },
  { 
    value: SeverityLevel.HIGH, 
    label: 'High', 
    description: 'Performance Issues - Significant impact on operations',
    color: '#ff9800'
  },
  { 
    value: SeverityLevel.MEDIUM, 
    label: 'Medium', 
    description: 'Minor Issues - Some impact on efficiency',
    color: '#ffeb3b'
  },
  { 
    value: SeverityLevel.LOW, 
    label: 'Low', 
    description: 'Maintenance Needed - Preventive action recommended',
    color: '#4caf50'
  }
];

export const REPORT_STATUS_OPTIONS: ReportStatusOption[] = [
  { value: ReportStatus.REPORTED, label: 'Reported', color: '#2196f3', icon: 'report' },
  { value: ReportStatus.ACKNOWLEDGED, label: 'Acknowledged', color: '#ff9800', icon: 'visibility' },
  { value: ReportStatus.IN_PROGRESS, label: 'In Progress', color: '#9c27b0', icon: 'build' },
  { value: ReportStatus.RESOLVED, label: 'Resolved', color: '#4caf50', icon: 'check_circle' },
  { value: ReportStatus.CLOSED, label: 'Closed', color: '#757575', icon: 'archive' }
];

export const COMMON_SYMPTOMS: SymptomOption[] = [
  { value: 'unusual_noise', label: 'Unusual Noise' },
  { value: 'overheating', label: 'Overheating' },
  { value: 'fluid_leaks', label: 'Fluid Leaks' },
  { value: 'performance_drop', label: 'Performance Drop' },
  { value: 'error_messages', label: 'Error Messages' },
  { value: 'vibration', label: 'Excessive Vibration' },
  { value: 'smoke', label: 'Smoke or Burning Smell' },
  { value: 'power_loss', label: 'Power Loss' },
  { value: 'irregular_operation', label: 'Irregular Operation' },
  { value: 'component_damage', label: 'Visible Component Damage' }
];