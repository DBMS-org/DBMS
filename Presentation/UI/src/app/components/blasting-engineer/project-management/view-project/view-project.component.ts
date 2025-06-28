import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';

@Component({
  selector: 'app-view-project',
  imports: [CommonModule],
  templateUrl: './view-project.component.html',
  styleUrl: './view-project.component.scss'
})
export class ViewProjectComponent {
  @Input() project: Project | null = null;
  @Input() isVisible: boolean = false;
  @Output() close = new EventEmitter<void>();

  constructor(private router: Router) {}

  closeModal() {
    this.close.emit();
  }

  goToProjectSites() {
    if (this.project) {
      this.closeModal();
      this.router.navigate(['/blasting-engineer/project-management', this.project.id, 'sites']);
    }
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'status-active';
      case 'inactive':
        return 'status-inactive';
      case 'completed':
        return 'status-completed';
      case 'on hold':
        return 'status-hold';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return 'status-default';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    }).format(date);
  }
}
