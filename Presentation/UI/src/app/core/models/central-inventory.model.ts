export interface CentralInventory {
  id: number;
  batchId: string;
  explosiveType: ExplosiveType;
  quantity: number;
  allocatedQuantity: number;
  availableQuantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;
  manufacturerBatchNumber?: string;
  storageLocation: string;
  status: InventoryStatus;
  centralWarehouseStoreId: number;
  centralWarehouseStoreName?: string;
  anfoProperties?: ANFOTechnicalProperties;
  emulsionProperties?: EmulsionTechnicalProperties;
  createdAt: Date;
  updatedAt: Date;
  isActive: boolean;
}

export interface ANFOTechnicalProperties {
  id: number;
  centralWarehouseInventoryId: number;
  density: number;
  fuelOilContent: number;
  moistureContent?: number;
  prillSize?: number;
  detonationVelocity?: number;
  grade: ANFOGrade;
  storageTemperature: number;
  storageHumidity: number;
  fumeClass: FumeClass;
  qualityCheckDate?: Date;
  qualityStatus: QualityStatus;
  waterResistance: string;
  notes?: string;
}

export interface EmulsionTechnicalProperties {
  id: number;
  centralWarehouseInventoryId: number;
  densityUnsensitized: number;
  densitySensitized: number;
  viscosity: number;
  waterContent: number;
  pH: number;
  detonationVelocity?: number;
  bubbleSize?: number;
  storageTemperature: number;
  applicationTemperature?: number;
  grade: EmulsionGrade;
  color: string;
  sensitizationType: SensitizationType;
  sensitizerContent?: number;
  fumeClass: FumeClass;
  qualityCheckDate?: Date;
  qualityStatus: QualityStatus;
  phaseSeparation?: boolean;
  crystallization?: boolean;
  colorConsistency?: boolean;
  waterResistance: string;
  notes?: string;
}

export interface CreateANFOInventoryRequest {
  batchId: string;
  quantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;
  manufacturerBatchNumber?: string;
  storageLocation: string;
  centralWarehouseStoreId: number;
  density: number;
  fuelOilContent: number;
  moistureContent?: number;
  prillSize?: number;
  detonationVelocity?: number;
  grade: ANFOGrade;
  storageTemperature: number;
  storageHumidity: number;
  fumeClass: FumeClass;
  qualityStatus: QualityStatus;
  waterResistance?: string;
  notes?: string;
}

export interface CreateEmulsionInventoryRequest {
  batchId: string;
  quantity: number;
  unit: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;
  manufacturerBatchNumber?: string;
  storageLocation: string;
  centralWarehouseStoreId: number;
  densityUnsensitized: number;
  densitySensitized: number;
  viscosity: number;
  waterContent: number;
  pH: number;
  detonationVelocity?: number;
  bubbleSize?: number;
  storageTemperature: number;
  applicationTemperature?: number;
  grade: EmulsionGrade;
  color: string;
  sensitizationType: SensitizationType;
  sensitizerContent?: number;
  fumeClass: FumeClass;
  qualityStatus: QualityStatus;
  waterResistance?: string;
  notes?: string;
}

export interface UpdateANFOInventoryRequest {
  quantity?: number;
  storageLocation?: string;
  density?: number;
  fuelOilContent?: number;
  moistureContent?: number;
  storageTemperature?: number;
  storageHumidity?: number;
  qualityStatus?: QualityStatus;
  notes?: string;
}

export interface UpdateEmulsionInventoryRequest {
  quantity?: number;
  storageLocation?: string;
  densityUnsensitized?: number;
  densitySensitized?: number;
  viscosity?: number;
  waterContent?: number;
  pH?: number;
  storageTemperature?: number;
  applicationTemperature?: number;
  qualityStatus?: QualityStatus;
  notes?: string;
}

export interface InventoryDashboard {
  totalBatches: number;
  totalQuantity: number;
  availableQuantity: number;
  allocatedQuantity: number;
  primaryUnit: string;
  quantityByType: {
    ANFO: number;
    Emulsion: number;
  };
  batchesByType: {
    ANFO: number;
    Emulsion: number;
  };
  expiringBatches: number;
  expiredBatches: number;
  quarantinedBatches: number;
  depletedBatches: number;
  pendingTransferRequests: number;
  approvedTransferRequests: number;
  urgentTransferRequests: number;
  overdueTransferRequests: number;
  recentActivities: any[];
  alerts: any[];
}

export interface InventoryFilter {
  pageNumber?: number;
  pageSize?: number;
  explosiveType?: ExplosiveType;
  status?: InventoryStatus;
  supplier?: string;
  batchId?: string;
  isExpiringSoon?: boolean;
  sortBy?: string;
  sortDescending?: boolean;
}

export interface PagedList<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export enum ExplosiveType {
  ANFO = 'ANFO',
  Emulsion = 'Emulsion'
}

export enum InventoryStatus {
  Available = 'Available',
  Allocated = 'Allocated',
  Expired = 'Expired',
  Quarantined = 'Quarantined',
  Depleted = 'Depleted'
}

export enum ANFOGrade {
  TGAN = 'TGAN',
  Standard = 'Standard'
}

export enum EmulsionGrade {
  Standard = 'Standard',
  HighDensity = 'HighDensity',
  LowDensity = 'LowDensity',
  WaterResistant = 'WaterResistant'
}

export enum SensitizationType {
  Chemical = 'Chemical',
  Physical = 'Physical',
  Hybrid = 'Hybrid'
}

export enum FumeClass {
  Class1 = 'Class1',
  Class2 = 'Class2',
  Class3 = 'Class3'
}

export enum QualityStatus {
  Approved = 'Approved',
  Pending = 'Pending',
  Rejected = 'Rejected'
}
