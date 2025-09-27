export interface ApprovalForm {
  approvedQuantity: number;
  departureDate: Date;
  expectedReceiptDate: Date;
  comments: string;
}

export interface RejectionForm {
  reason: string;
  comments: string;
}

export interface PendingForm {
  comments: string;
}