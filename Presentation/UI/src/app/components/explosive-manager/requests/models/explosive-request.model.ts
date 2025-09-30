export interface ExplosiveRequest {
  id: string;
  requesterId: string;
  requesterName: string;
  requesterRole: string;
  // Single-item fields (kept for compatibility with existing views)
  explosiveType?: ExplosiveType;
  quantity?: number;
  unit?: string;
  requestDate: Date;
  requiredDate: Date;
  status: RequestStatus;
  approvalDate?: Date;
  approvedBy?: string;
  rejectionReason?: string;
  notes?: string;
  approvedQuantity?: number;
  departureDate?: Date;
  // expectedReceiptDate removed
  storeLocation: string;
  purpose: string;
  // Dispatch-related fields
  dispatchDate?: Date;
  truckNumber?: string;
  driverName?: string;
  routeInformation?: string;
  dispatchNotes?: string;
  dispatchedBy?: string;
  
  // Multi-item requests support
  requestedItems?: RequestItem[];
}

export enum ExplosiveType {
  ANFO = 'ANFO',
  EMULSION = 'EMULSION'
}

export enum RequestStatus {
  PENDING = 'PENDING',
  APPROVED = 'APPROVED',
  REJECTED = 'REJECTED',
  IN_PROGRESS = 'IN_PROGRESS',
  COMPLETED = 'COMPLETED',
  CANCELLED = 'CANCELLED',
  DISPATCHED = 'DISPATCHED'
}

export type ItemDecisionStatus = 'APPROVE' | 'REJECT';

export interface RequestItem {
  explosiveType: ExplosiveType;
  quantity: number;
  unit: string;
  purpose: string;
  specifications?: string;
  // Per-item approval fields (front-end only)
  decision?: ItemDecisionStatus;
  approvedQuantity?: number;
  remarks?: string;
}

export interface RequestFilter {
  explosiveType?: ExplosiveType;
  status?: RequestStatus;
  requesterName?: string;
  dateFrom?: Date;
  dateTo?: Date;
  storeLocation?: string;
}

export interface RequestSearchCriteria {
  searchTerm?: string;
  filters?: RequestFilter;
  sortBy?: 'requestDate' | 'requiredDate' | 'status';
  sortOrder?: 'asc' | 'desc';
  page?: number;
  pageSize?: number;
}