import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExplosiveApprovalRequest } from '../../../../core/services/explosive-approval-request.service';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../../core/services/explosive-calculations.service';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { TagModule } from 'primeng/tag';
import { TableModule } from 'primeng/table';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-request-details',
  standalone: true,
  imports: [
    CommonModule,
    DialogModule,
    ButtonModule,
    PanelModule,
    TagModule,
    TableModule,
    TooltipModule
  ],
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

  getStatusSeverity(status: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (status) {
      case 'Pending': return 'warning';
      case 'Approved': return 'success';
      case 'Rejected': return 'danger';
      case 'Cancelled': return 'secondary';
      case 'Expired': return 'contrast';
      default: return 'info';
    }
  }

  getPrioritySeverity(priority: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (priority.toLowerCase()) {
      case 'high': return 'danger';
      case 'medium': return 'warning';
      case 'low': return 'info';
      default: return 'secondary';
    }
  }

  getApprovalTypeSeverity(approvalType: string): 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined {
    switch (approvalType.toLowerCase()) {
      case 'normal': return 'info';
      case 'urgent': return 'danger';
      case 'emergency': return 'contrast';
      default: return 'secondary';
    }
  }

  /**
   * Check if the request can be approved
   * Requirements:
   * - Request status must be 'Pending'
   * - Blasting date must be specified
   * - Blast timing must be specified
   */
  canApprove(): boolean {
    if (!this.request || this.request.status !== 'Pending') {
      return false;
    }

    // Both blasting date and timing must be specified
    return !!(this.request.blastingDate && this.request.blastTiming);
  }

  /**
   * Get the message explaining why approval is blocked
   */
  getApprovalBlockedMessage(): string {
    if (!this.request) {
      return 'Request data not available';
    }

    if (this.request.status !== 'Pending') {
      return `Request status is ${this.request.status}`;
    }

    const missingDate = !this.request.blastingDate;
    const missingTiming = !this.request.blastTiming;

    if (missingDate && missingTiming) {
      return 'Cannot approve: Blasting Date and Blast Timing not specified by Blasting Engineer';
    } else if (missingDate) {
      return 'Cannot approve: Blasting Date not specified by Blasting Engineer';
    } else if (missingTiming) {
      return 'Cannot approve: Blast Timing not specified by Blasting Engineer';
    }

    return 'Approval requirements met';
  }
}