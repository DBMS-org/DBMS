export interface ExplosiveRequest {
  id: string;
  requesterId: string;
  requesterName: string;
  requesterRole: string;
  explosiveType: ExplosiveType;
  quantity: number;
  unit: string;
  requestDate: Date;
  requiredDate: Date;
  status: RequestStatus;
  approvalDate?: Date;
  approvedBy?: string;
  rejectionReason?: string;
  notes?: string;

  storeLocation: string;
  purpose: string;
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
  CANCELLED = 'CANCELLED'
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
  sortBy?: 'requestDate' | 'requiredDate' | 'status' | 'quantity';
  sortOrder?: 'asc' | 'desc';
  page?: number;
  pageSize?: number;
}