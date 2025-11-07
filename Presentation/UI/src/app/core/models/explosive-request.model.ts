export interface ExplosiveRequest {
  id: string;
  storeManager: string;
  location: string;
  explosiveType: string;
  quantity: number;
  requiredDate: Date;
  projectReference: string;
  justification: string;
  status: 'PENDING' | 'APPROVED' | 'REJECTED';
  approvedQuantity?: number;
  departureDate?: Date;
  rejectionReason?: string;
  approvalComments?: string;
}

export interface RequestSearchCriteria {
  startDate?: Date;
  endDate?: Date;
  status?: string;
  explosiveType?: string;
  location?: string;
}