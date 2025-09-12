export interface Store {
  id: string;
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail?: string;
  explosiveTypesAvailable: ExplosiveType[];
  storageCapacity: number;
  currentOccupancy?: number;
  status: StoreStatus;
  isActive: boolean;
  location: StoreLocation;
  securityLevel: SecurityLevel;
  lastInspectionDate?: Date;
  nextInspectionDate?: Date;
  createdAt: Date;
  updatedAt: Date;
  createdBy: string;
  updatedBy: string;
}

export interface StoreLocation {
  city: string;
  region: string;
  coordinates?: {
    latitude: number;
    longitude: number;
  };
}

export interface StoreInventoryRecord {
  id: string;
  storeId: string;
  explosiveType: ExplosiveType;
  quantity: number;
  unit: string;
  batchNumber: string;
  manufacturingDate: Date;
  expiryDate: Date;
  supplier: string;
  lastUpdated: Date;
}

export enum StoreType {
  MAIN_WAREHOUSE = 'Main Warehouse',
  FIELD_STORAGE = 'Field Storage',
  TEMPORARY_STORAGE = 'Temporary Storage',
  DISTRIBUTION_CENTER = 'Distribution Center',
  MOBILE_STORAGE = 'Mobile Storage'
}

export enum ExplosiveType {
  ANFO = 'ANFO',
  EMULSION = 'Emulsion',
  DYNAMITE = 'Dynamite',
  BLASTING_CAPS = 'Blasting Caps',
  DETONATING_CORD = 'Detonating Cord',
  PRIMER = 'Primer',
  BOOSTER = 'Booster',
  SHAPED_CHARGES = 'Shaped Charges'
}

export enum StoreStatus {
  OPERATIONAL = 'Operational',
  UNDER_MAINTENANCE = 'Under Maintenance',
  TEMPORARILY_CLOSED = 'Temporarily Closed',
  INSPECTION_REQUIRED = 'Inspection Required',
  DECOMMISSIONED = 'Decommissioned'
}

export enum SecurityLevel {
  LEVEL_1 = 'Level 1 - Basic',
  LEVEL_2 = 'Level 2 - Standard',
  LEVEL_3 = 'Level 3 - High',
  LEVEL_4 = 'Level 4 - Maximum'
}

export interface StoreStatistics {
  totalStores: number;
  activeStores: number;
  inactiveStores: number;
  operationalStores: number;
  maintenanceStores: number;
  totalCapacity: number;
  totalOccupancy: number;
  utilizationRate: number;
  storesByRegion: { [key: string]: number };
}

export interface StoreFilters {
  status?: StoreStatus | 'ALL';
  location?: string | 'ALL';
  storeManager?: string | 'ALL';
  isActive?: boolean | null;
  searchTerm?: string;
}

export interface CreateStoreRequest {
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail?: string;
  explosiveTypesAvailable: ExplosiveType[];
  storageCapacity: number;
  location: StoreLocation;
  securityLevel: SecurityLevel;
}

export interface UpdateStoreRequest extends Partial<CreateStoreRequest> {
  id: string;
}