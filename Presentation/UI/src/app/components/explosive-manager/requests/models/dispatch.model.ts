export interface DispatchForm {
  truckNumber: string;
  dispatchDate: Date;
  driverName?: string;
  routeInformation?: string;
  dispatchNotes?: string;
  // Front-end only: per-item approved quantities and remarks sent along with dispatch
  itemDecisions?: ItemDecision[];
}

export interface DispatchData extends DispatchForm {
  requestId: string;
  dispatchedBy: string;
  dispatchTimestamp: Date;
}

// Removed decision field as per updated UI (no approve/reject)
export interface ItemDecision {
  index: number; // index of the item in requestedItems
  approvedQuantity?: number; // must be <= requested quantity
  remarks?: string;
}