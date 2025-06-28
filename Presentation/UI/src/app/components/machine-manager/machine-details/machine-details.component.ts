import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Machine, MachineStatus } from '../../../core/models/machine.model';

@Component({
  selector: 'app-machine-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './machine-details.component.html',
  styleUrls: ['./machine-details.component.scss']
})
export class MachineDetailsComponent {
  @Input() machine!: Machine;
  @Output() close = new EventEmitter<void>();

  onClose(): void {
    this.close.emit();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Available':
        return 'status-available';
      case 'Assigned':
        return 'status-assigned';
      case 'In Maintenance':
        return 'status-maintenance';
      case 'Under Repair':
        return 'status-repair';
      case 'Out of Service':
        return 'status-out-of-service';
      case 'Retired':
        return 'status-retired';
      default:
        return 'status-unknown';
    }
  }

  getStatusIcon(status: string): string {
    switch (status) {
      case 'Available':
        return 'check_circle';
      case 'Assigned':
        return 'assignment';
      case 'In Maintenance':
        return 'build';
      case 'Under Repair':
        return 'warning';
      case 'Out of Service':
        return 'block';
      case 'Retired':
        return 'archive';
      default:
        return 'help';
    }
  }

  formatDate(date: Date | string | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
