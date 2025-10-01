export interface Store {
  id: string;
  storeName: string;
  storeAddress: string;
  storeManagerName: string;
  storeManagerContact: string;
  storeManagerEmail: string;
  explosiveTypesAvailable: ExplosiveType[];
  storageCapacity: number;
  currentOccupancy?: number;
  location: StoreLocation;
  status: StoreStatus;
  isActive: boolean;
}

export interface StoreLocation {
  city: string;
  region: string;
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
  storeManagerEmail: string;
  explosiveTypesAvailable: ExplosiveType[];
  storageCapacity: number;
  location: StoreLocation;
  status: StoreStatus;
}

export interface UpdateStoreRequest extends Partial<CreateStoreRequest> {
  id: string;
}