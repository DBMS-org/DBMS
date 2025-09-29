import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExplosiveApprovalRequest } from '../../../../core/services/explosive-approval-request.service';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../../core/services/explosive-calculations.service';

@Component({
  selector: 'app-request-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './request-details.component.html',
  styleUrl: './request-details.component.scss'
})
export class RequestDetailsComponent implements OnInit, OnChanges {
  @Input() request: ExplosiveApprovalRequest | null = null;
  @Input() isVisible: boolean = false;
  @Output() close = new EventEmitter<void>();
  @Output() approve = new EventEmitter<ExplosiveApprovalRequest>();
  @Output() reject = new EventEmitter<ExplosiveApprovalRequest>();

  // Explosive calculations data
  explosiveCalculations: ExplosiveCalculationResultDto[] = [];
  totalAnfo: number = 0;
  totalEmulsion: number = 0;
  isLoadingCalculations: boolean = false;

  constructor(private explosiveCalculationsService: ExplosiveCalculationsService) {}

  ngOnInit(): void {
    // Handle escape key to close modal
    document.addEventListener('keydown', this.handleEscapeKey.bind(this));
    this.loadExplosiveCalculations();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['request'] && this.request) {
      this.loadExplosiveCalculations();
    }
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

  private loadExplosiveCalculations(): void {
    if (!this.request?.projectSite?.id) {
      this.resetCalculations();
      return;
    }

    this.isLoadingCalculations = true;
    
    this.explosiveCalculationsService.getByProjectAndSite(
      this.request.projectSite.project.id,
      this.request.projectSite.id
    ).subscribe({
      next: (calculations) => {
        this.explosiveCalculations = calculations;
        this.calculateTotals();
        this.isLoadingCalculations = false;
      },
      error: (error) => {
        console.error('Error loading explosive calculations:', error);
        this.resetCalculations();
        this.isLoadingCalculations = false;
      }
    });
  }

  private calculateTotals(): void {
    this.totalAnfo = this.explosiveCalculations.reduce((sum, calc) => sum + (calc.totalAnfo || 0), 0);
    this.totalEmulsion = this.explosiveCalculations.reduce((sum, calc) => sum + (calc.totalEmulsion || 0), 0);
  }

  private resetCalculations(): void {
    this.explosiveCalculations = [];
    this.totalAnfo = 0;
    this.totalEmulsion = 0;
    this.isLoadingCalculations = false;
  }
}