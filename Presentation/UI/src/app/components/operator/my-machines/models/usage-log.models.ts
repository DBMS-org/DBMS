export interface MachineUsageLog {
  id?: string;
  machineId: string;
  machineName: string;
  operatorId: string;
  logDate: Date;

  engineHours: string;
  idleHours: string;
  workingHours: string;

  fuelConsumed?: number;

  hasDowntime: boolean;
  downtimeHours?: string;
  breakdownDescription?: string;

  remarks?: string;
  attachments?: UsageLogAttachment[];

  createdAt: Date;
  updatedAt: Date;
  status: UsageLogStatus;
}

export interface UsageLogAttachment {
  id: string;
  fileName: string;
  fileType: string;
  fileSize: number;
  uploadedAt: Date;
  url?: string;
  description?: string;
}

export interface CreateUsageLogRequest {
  machineId: string;
  logDate: string; // ISO date string
  engineHours: string;
  idleHours: string;
  workingHours: string;
  fuelConsumed?: number;
  hasDowntime: boolean;
  downtimeHours?: string;
  breakdownDescription?: string;
  remarks?: string;
  attachments?: File[];
}

export interface OperatorMachine {
  id: string;
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

export enum UsageLogStatus {
  DRAFT = 'DRAFT',
  SUBMITTED = 'SUBMITTED',
  APPROVED = 'APPROVED',
  REJECTED = 'REJECTED'
}

export interface UsageLogFormData {
  selectedMachineId: string;
  logDate: Date;
  engineHours: string;
  idleHours: string;
  workingHours: string;
  fuelConsumed: number | null;
  hasDowntime: boolean;
  downtimeHours: string;
  breakdownDescription: string;
  remarks: string;
  attachments: File[];
}

export interface UsageLogValidation {
  isValid: boolean;
  errors: {
    machineId?: string;
    logDate?: string;
    engineHours?: string;
    idleHours?: string;
    workingHours?: string;
    fuelConsumed?: string;
    downtimeHours?: string;
    breakdownDescription?: string;
    attachments?: string;
  };
}

export interface TimeComponents {
  hours: number;
  minutes: number;
}

export class UsageLogUtils {
  static formatTimeToHHMM(hours: number, minutes: number): string {
    const h = Math.floor(hours).toString().padStart(2, '0');
    const m = Math.floor(minutes).toString().padStart(2, '0');
    return `${h}:${m}`;
  }
  
  static parseTimeFromHHMM(timeString: string): TimeComponents {
    const [hours, minutes] = timeString.split(':').map(Number);
    return { hours: hours || 0, minutes: minutes || 0 };
  }
  
  static convertTimeToDecimal(timeString: string): number {
    const { hours, minutes } = this.parseTimeFromHHMM(timeString);
    return hours + (minutes / 60);
  }
  
  static validateTimeFormat(timeString: string): boolean {
    const timeRegex = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/;
    return timeRegex.test(timeString);
  }
  
  static validateLogicalHours(engineHours: string, idleHours: string, workingHours: string): boolean {
    const engine = this.convertTimeToDecimal(engineHours);
    const idle = this.convertTimeToDecimal(idleHours);
    const working = this.convertTimeToDecimal(workingHours);

    const totalOperational = idle + working;
    return Math.abs(engine - totalOperational) <= 0.5;
  }
}