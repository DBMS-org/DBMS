import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExplosiveApprovalRequest } from '../../../../core/services/explosive-approval-request.service';

@Component({
  selector: 'app-request-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './request-details.component.html',
  styleUrl: './request-details.component.scss'
})
export class RequestDetailsComponent implements OnInit {
  @Input() request: ExplosiveApprovalRequest | null = null;
  @Input() isVisible: boolean = false;
  @Output() close = new EventEmitter<void>();
  @Output() approve = new EventEmitter<ExplosiveApprovalRequest>();
  @Output() reject = new EventEmitter<ExplosiveApprovalRequest>();

  ngOnInit(): void {
    // Handle escape key to close modal
    document.addEventListener('keydown', this.handleEscapeKey.bind(this));
  }

  ngOnDestroy(): void {
    document.removeEventListener('keydown', this.handleEscapeKey.bind(this));
  }

  private handleEscapeKey(event: KeyboardEvent): void {
    if (event.key === 'Escape' && this.isVisible) {
      this.onClose();
    }
  }

  onClose(): void {
    this.close.emit();
  }

  onApprove(): void {
    if (this.request) {
      this.approve.emit(this.request);
    }
  }

  onReject(): void {
    if (this.request) {
      this.reject.emit(this.request);
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Pending': return 'status-pending';
      case 'Approved': return 'status-approved';
      case 'Rejected': return 'status-rejected';
      case 'Cancelled': return 'status-cancelled';
      case 'Expired': return 'status-expired';
      default: return '';
    }
  }

  getPriorityClass(priority: string): string {
    return `priority-${priority.toLowerCase()}`;
  }

  getApprovalTypeClass(approvalType: string): string {
    return `type-${approvalType.toLowerCase()}`;
  }

  formatDate(dateString: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  formatDateTime(dateString: string): string {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }

  formatDuration(hours: number | undefined): string {
    if (!hours) return 'N/A';
    if (hours < 24) {
      return `${hours} hour${hours !== 1 ? 's' : ''}`;
    }
    const days = Math.floor(hours / 24);
    const remainingHours = hours % 24;
    if (remainingHours === 0) {
      return `${days} day${days !== 1 ? 's' : ''}`;
    }
    return `${days} day${days !== 1 ? 's' : ''} ${remainingHours} hour${remainingHours !== 1 ? 's' : ''}`;
  }

  getBooleanDisplay(value: boolean): string {
    return value ? 'Yes' : 'No';
  }

  getBooleanClass(value: boolean): string {
    return value ? 'boolean-yes' : 'boolean-no';
  }

  onBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }
}