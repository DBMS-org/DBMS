import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InventoryTransferRequest } from '../../../../core/models/inventory-transfer.model';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { PanelModule } from 'primeng/panel';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-view-details',
  standalone: true,
  imports: [
    CommonModule,
    DialogModule,
    ButtonModule,
    PanelModule,
    TagModule
  ],
  templateUrl: './view-details.component.html',
  styleUrls: ['./view-details.component.scss']
})
export class ViewDetailsComponent {
  @Input() request: InventoryTransferRequest | null = null;
  @Input() isVisible: boolean = false;
  @Output() close = new EventEmitter<void>();

  constructor() { }

  onClose(): void {
    this.close.emit();
  }

  onBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.onClose();
    }
  }

  getStatusClass(status: string): string {
    switch (status?.toUpperCase()) {
      case 'APPROVED':
        return 'bg-success';
      case 'PENDING':
        return 'bg-warning';
      case 'REJECTED':
        return 'bg-danger';
      case 'FULFILLED':
        return 'bg-info';
      case 'IN_PROGRESS':
        return 'bg-primary';
      default:
        return 'bg-secondary';
    }
  }

  getTagSeverity(status: string): 'success' | 'warning' | 'danger' | 'info' | 'secondary' {
    switch (status?.toUpperCase()) {
      case 'APPROVED':
      case 'FULFILLED':
        return 'success';
      case 'PENDING':
        return 'warning';
      case 'REJECTED':
        return 'danger';
      case 'IN_PROGRESS':
        return 'info';
      default:
        return 'secondary';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
}