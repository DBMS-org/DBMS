export interface Machine {
  id: number;
  name: string;
  type: MachineType;
  model: string;
  manufacturer: string;
  serialNumber: string;
  rigNo?: string;
  plateNo?: string;
  chassisDetails?: string;
  manufacturingYear?: number;
  status: MachineStatus;
  currentLocation?: string;
  assignedToProject?: string;
  assignedToOperator?: string;
  lastMaintenanceDate?: Date;
  nextMaintenanceDate?: Date;
  specifications?: MachineSpecifications;
  createdAt: Date;
  updatedAt: Date;
  projectId?: number;
  operatorId?: number;
  projectName?: string;
  operatorName?: string;
}

export interface MachineSpecifications {
  power?: string;
  weight?: string;
  dimensions?: string;
  capacity?: string;
  operatingTemperature?: string;
  fuelType?: string;
  maxOperatingDepth?: string;
  drillingDiameter?: string;
  additionalFeatures?: string[];
}

export interface CreateMachineRequest {
  name: string;
  type: string;
  model: string;
  manufacturer: string;
  serialNumber: string;
  rigNo?: string;
  plateNo?: string;
  chassisDetails?: string;
  manufacturingYear?: number;
  status: string;
  currentLocation?: string;
  projectId?: number;
  operatorId?: number;
  specifications?: MachineSpecifications;
}

export interface UpdateMachineRequest {
  name: string;
  type: string;
  model: string;
  manufacturer: string;
  serialNumber: string;
  rigNo?: string;
  plateNo?: string;
  chassisDetails?: string;
  manufacturingYear?: number;
  status: string;
  currentLocation?: string;
  projectId?: number;
  operatorId?: number;
  lastMaintenanceDate?: Date;
  nextMaintenanceDate?: Date;
  specifications?: MachineSpecifications;
}

export interface MachineAssignmentRequest {
  id: string;
  projectId: string;
  machineType: MachineType;
  quantity: number;
  requestedBy: string;
  requestedDate: Date;
  status: AssignmentRequestStatus;
  urgency: RequestUrgency;
  detailsOrExplanation?: string;
  approvedBy?: string;
  approvedDate?: Date;
  assignedMachines?: string[];
  comments?: string;
  expectedUsageDuration?: string;
  expectedReturnDate?: Date;
}

export interface MachineAssignment {
  id: string;
  machineId: string;
  projectId: string;
  operatorId: string;
  assignedBy: string;
  assignedDate: Date;
  expectedReturnDate?: Date;
  actualReturnDate?: Date;
  status: AssignmentStatus;
  location?: string;
  notes?: string;
}

export enum MachineType {
  DRILL_RIG = 'Drill Rig',
  EXCAVATOR = 'Excavator',
  LOADER = 'Loader',
  TRUCK = 'Truck',
  COMPRESSOR = 'Compressor',
  GENERATOR = 'Generator',
  CRANE = 'Crane',
  BULLDOZER = 'Bulldozer',
  GRADER = 'Grader',
  OTHER = 'Other'
}

export enum MachineStatus {
  AVAILABLE = 'Available',
  ASSIGNED = 'Assigned',
  IN_MAINTENANCE = 'In Maintenance',
  OUT_OF_SERVICE = 'Out of Service',
  UNDER_REPAIR = 'Under Repair',
  RETIRED = 'Retired'
}

export enum AssignmentRequestStatus {
  PENDING = 'Pending',
  APPROVED = 'Approved',
  REJECTED = 'Rejected',
  PARTIALLY_FULFILLED = 'Partially Fulfilled',
  COMPLETED = 'Completed',
  CANCELLED = 'Cancelled'
}

export enum AssignmentStatus {
  ACTIVE = 'Active',
  COMPLETED = 'Completed',
  OVERDUE = 'Overdue',
  CANCELLED = 'Cancelled'
}

export enum RequestUrgency {
  LOW = 'Low',
  MEDIUM = 'Medium',
  HIGH = 'High',
  CRITICAL = 'Critical'
} 