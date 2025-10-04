export interface Store {
  id: number;
  storeName: string;
  storeAddress: string;
  storageCapacity: number;
  currentOccupancy: number;
  city: string;
  status: StoreStatus;
  allowedExplosiveTypes?: string; // Comma-separated: "ANFO,Emulsion"
  regionId: number;
  managerUserId?: number;
  createdAt: Date;
  updatedAt: Date;

  // Additional properties for display
  regionName?: string;
  managerUserName?: string;
  managerUserEmail?: string;
  managerUserContact?: string;
  inventoryItemsCount?: number;
  utilizationPercentage?: number;
}

export enum ExplosiveType {
  ANFO = 1,
  Emulsion = 2,
  Dynamite = 3,
  BlastingCaps = 4,
  DetonatingCord = 5,
  Primer = 6,
  Booster = 7,
  ShapedCharges = 8
}

export enum StoreStatus {
  Operational = 1,
  UnderMaintenance = 2,
  TemporarilyClosed = 3,
  InspectionRequired = 4,
  Decommissioned = 5
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
  regionId?: number | 'ALL';
  city?: string | 'ALL';
  storeManager?: string | 'ALL';
  searchTerm?: string;
}

export interface CreateStoreRequest {
  storeName: string;
  storeAddress: string;
  storageCapacity: number;
  city: string;
  allowedExplosiveTypes?: string; // Comma-separated: "ANFO,Emulsion"
  regionId: number;
  managerUserId?: number;
}

export interface UpdateStoreRequest {
  storeName: string;
  storeAddress: string;
  storageCapacity: number;
  city: string;
  allowedExplosiveTypes?: string; // Comma-separated: "ANFO,Emulsion"
  status: StoreStatus;
  managerUserId?: number;
}