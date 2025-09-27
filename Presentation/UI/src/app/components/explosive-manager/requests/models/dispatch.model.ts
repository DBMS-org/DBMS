export interface DispatchForm {
  truckNumber: string;
  dispatchDate: Date;
  driverName?: string;
  routeInformation?: string;
  dispatchNotes?: string;
}

export interface DispatchData extends DispatchForm {
  requestId: string;
  dispatchedBy: string;
  dispatchTimestamp: Date;
}