export interface ApprovalForm {
  approvedQuantity: number;
  departureDate: Date;
  // expectedReceiptDate removed
  comments: string;
}

export interface RejectionForm {
  reason: string;
  comments: string;
}

export interface PendingForm {
  comments: string;
}