// ===================================================================
// USAGE LOG MODELS - Updated for Backend Integration
// Backend API: /api/usage-logs
// ===================================================================

/**
 * Usage Log DTO - Matches backend UsageLogDto
 */
export interface UsageLogDto {
  id: number;
  machineId: number;
  machineName: string;
  operatorId?: number;
  siteEngineer: string;
  logDate: Date;

  // Start/End tracking
  engineHourStart: number;
  engineHourEnd: number;
  engineHoursDelta: number;
  drifterHourStart?: number;
  drifterHourEnd?: number;
  drifterHoursDelta?: number;

  // Operating hours
  idleHours: number;
  workingHours: number;
  fuelConsumed?: number;

  // Downtime
  hasDowntime: boolean;
  downtimeHours?: number;
  breakdownDescription?: string;

  // Notes
  remarks?: string;

  // Status
  status: string;
  createdAt: Date;
}

/**
 * Create Usage Log Request - Matches backend CreateUsageLogRequest
 */
export interface CreateUsageLogRequest {
  machineId: number;
  logDate: Date;

  // Start/End tracking
  engineHourStart: number;
  engineHourEnd: number;
  drifterHourStart?: number;
  drifterHourEnd?: number;

  // Operating hours
  idleHours: number;
  workingHours: number;
  fuelConsumed?: number;

  // Downtime
  hasDowntime: boolean;
  downtimeHours?: number;
  breakdownDescription?: string;

  // Notes
  remarks?: string;
}

/**
 * Usage Statistics DTO - Matches backend UsageStatisticsDto
 */
export interface UsageStatisticsDto {
  machineId: number;
  machineName: string;
  totalEngineHours: number;
  totalIdleHours: number;
  totalWorkingHours: number;
  totalFuelConsumed: number;
  totalDowntimeHours: number;
  averageDailyHours: number;
  daysWithDowntime: number;
  periodStart: Date;
  periodEnd: Date;
}

/**
 * Operator Machine - Frontend view model
 */
export interface OperatorMachine {
  id: number;
  name: string;
  model: string;
  serialNumber: string;
  rigNo?: string;
  plateNo?: string;
  currentLocation?: string;
  status: string;
  type: string;
  manufacturer: string;
}

/**
 * Usage Log Status Enum
 */
export enum UsageLogStatus {
  SUBMITTED = 'SUBMITTED',
  APPROVED = 'APPROVED',
  REJECTED = 'REJECTED'
}

/**
 * Usage Log Form Data - Internal form state
 */
export interface UsageLogFormData {
  selectedMachineId: number;
  logDate: Date;

  // Start/End tracking
  engineHourStart: number;
  engineHourEnd: number;
  drifterHourStart: number;
  drifterHourEnd: number;

  // Operating hours
  idleHours: number;
  workingHours: number;

  fuelConsumed: number | null;
  hasDowntime: boolean;
  downtimeHours: number;
  breakdownDescription: string;
  remarks: string;
}

/**
 * Usage Log Validation
 */
export interface UsageLogValidation {
  isValid: boolean;
  errors: {
    machineId?: string;
    logDate?: string;
    engineHourStart?: string;
    engineHourEnd?: string;
    drifterHourStart?: string;
    drifterHourEnd?: string;
    idleHours?: string;
    workingHours?: string;
    fuelConsumed?: string;
    downtimeHours?: string;
    breakdownDescription?: string;
  };
}

/**
 * Utility class for usage log calculations
 */
export class UsageLogUtils {
  /**
   * Calculate engine hours delta
   */
  static calculateEngineHoursDelta(start: number, end: number): number {
    return Math.max(0, end - start);
  }

  /**
   * Calculate drifter hours delta
   */
  static calculateDrifterHoursDelta(start?: number, end?: number): number | undefined {
    if (start === undefined || end === undefined) return undefined;
    return Math.max(0, end - start);
  }

  /**
   * Validate hour ranges
   */
  static validateHourRange(start: number, end: number): boolean {
    return end >= start && start >= 0 && end >= 0;
  }

  /**
   * Validate logical hours (idle + working should roughly equal engine hours delta)
   */
  static validateLogicalHours(
    engineDelta: number,
    idleHours: number,
    workingHours: number
  ): boolean {
    const totalOperational = idleHours + workingHours;
    // Allow 0.5 hour tolerance
    return Math.abs(engineDelta - totalOperational) <= 0.5;
  }

  /**
   * Format hours for display
   */
  static formatHours(hours: number): string {
    return hours.toFixed(2);
  }

  /**
   * Convert decimal hours to HH:MM format
   */
  static formatHoursToHHMM(hours: number): string {
    const h = Math.floor(hours);
    const m = Math.round((hours - h) * 60);
    return `${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`;
  }
}
