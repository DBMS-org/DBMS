import { ExplosiveType } from './store.model';

export interface StockRequest {
  id: string;
  requesterId: string;
  requesterName: string;
  requesterStoreId: string;
  requesterStoreName: string;
  explosiveManagerId?: string;
  explosiveManagerName?: string;
  requestedItems: StockRequestItem[];
  requestDate: Date;
  requiredDate: Date;
  status: StockRequestStatus;
  priority: RequestPriority;
  justification: string;
  notes?: string;
  approvalDate?: Date;
  rejectionReason?: string;
  fulfillmentDate?: Date;
  createdAt: Date;
  updatedAt: Date;
}

export interface StockRequestItem {
  explosiveType: ExplosiveType;
  requestedQuantity: number;
  unit: string;
  purpose: string;
  specifications?: string;
}

export enum StockRequestStatus {
  PENDING = 'Pending',
  UNDER_REVIEW = 'Under Review',
  APPROVED = 'Approved',
  REJECTED = 'Rejected',
  PARTIALLY_APPROVED = 'Partially Approved',
  IN_PROGRESS = 'In Progress',
  FULFILLED = 'Fulfilled',
  CANCELLED = 'Cancelled'
}

export enum RequestPriority {
  LOW = 'Low',
  MEDIUM = 'Medium',
  HIGH = 'High',
  URGENT = 'Urgent',
  EMERGENCY = 'Emergency'
}

export interface CreateStockRequestRequest {
  requesterStoreId: string;
  requestedItems: StockRequestItem[];
  requiredDate: Date;
  priority: RequestPriority;
  justification: string;
  notes?: string;
}

export interface StockRequestStatistics {
  totalRequests: number;
  pendingRequests: number;
  approvedRequests: number;
  rejectedRequests: number;
  fulfilledRequests: number;
  averageProcessingTime: number;
  requestsByPriority: { [key: string]: number };
  requestsByStatus: { [key: string]: number };
}

export interface StockRequestFilters {
  status?: StockRequestStatus | 'ALL';
  priority?: RequestPriority | 'ALL';
  explosiveType?: ExplosiveType | 'ALL';
  dateRange?: {
    startDate: Date;
    endDate: Date;
  };
  searchTerm?: string;
} 