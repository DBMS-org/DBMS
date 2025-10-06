export interface InventoryTransferRequest {
  id: number;
  requestNumber: string;

  // Source & Destination
  centralWarehouseInventoryId: number;
  batchId: string;
  explosiveTypeName: string;
  destinationStoreId: number;
  destinationStoreName: string;

  // Quantity
  requestedQuantity: number;
  approvedQuantity?: number;
  finalQuantity: number;
  unit: string;

  // Status & Workflow
  status: TransferRequestStatus;
  statusName: string;
  requestDate: Date;
  approvedDate?: Date;
  completedDate?: Date;
  requiredByDate?: Date;
  daysUntilRequired?: number;
  isOverdue: boolean;
  isUrgent: boolean;

  // Dispatch/Delivery Tracking
  dispatchDate?: Date;
  truckNumber?: string;
  driverName?: string;
  driverContactNumber?: string;
  dispatchNotes?: string;
  dispatchedByUserId?: number;
  dispatchedByUserName?: string;
  deliveryConfirmedDate?: Date;

  // People
  requestedByUserId: number;
  requestedByUserName: string;
  approvedByUserId?: number;
  approvedByUserName?: string;
  processedByUserId?: number;
  processedByUserName?: string;

  // Notes
  requestNotes?: string;
  approvalNotes?: string;
  rejectionReason?: string;

  // Metadata
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateTransferRequest {
  centralWarehouseInventoryId: number;
  destinationStoreId: number;
  requestedQuantity: number;
  unit: string;
  requiredByDate?: Date;
  requestNotes?: string;
}

export interface ApproveTransferRequest {
  approvedQuantity?: number;
  approvalNotes?: string;
}

export interface RejectTransferRequest {
  reason: string;
}

export interface DispatchTransferRequest {
  truckNumber: string;
  driverName: string;
  driverContactNumber?: string;
  dispatchNotes?: string;
}

export interface TransferRequestFilter {
  pageNumber?: number;
  pageSize?: number;
  status?: TransferRequestStatus;
  destinationStoreId?: number;
  requestedByUserId?: number;
  isOverdue?: boolean;
  isUrgent?: boolean;
  sortBy?: string;
  sortDescending?: boolean;
}

export enum TransferRequestStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected',
  InProgress = 'InProgress',
  Completed = 'Completed',
  Cancelled = 'Cancelled'
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
