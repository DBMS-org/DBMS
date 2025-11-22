import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MaintenanceJobService, MaintenanceJobDto } from '../maintenance-job.service';

@Component({
  selector: 'app-job-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss']
})
export class JobDetailsComponent implements OnInit {
  job: MaintenanceJobDto | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private maintenanceService: MaintenanceJobService
  ) {}

  ngOnInit(): void {
    const jobId = this.route.snapshot.paramMap.get('id');
    if (jobId) {
      this.loadJob(+jobId);
    } else {
      this.errorMessage = 'Invalid job ID';
      this.isLoading = false;
    }
  }

  loadJob(id: number): void {
    this.maintenanceService.getJobById(id).subscribe({
      next: (job) => {
        this.job = job;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load job details';
        console.error('Error loading job:', err);
        this.isLoading = false;
      }
    });
  }

  goBack(): void {
    // Detect if we're in admin or machine-manager context
    const currentUrl = this.router.url;
    if (currentUrl.includes('/admin/')) {
      this.router.navigate(['/admin/maintenance-management']);
    } else {
      this.router.navigate(['/machine-manager/maintenance-management']);
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Scheduled': return 'status-scheduled';
      case 'InProgress': return 'status-in-progress';
      case 'Completed': return 'status-completed';
      case 'Overdue': return 'status-overdue';
      case 'Cancelled': return 'status-cancelled';
      default: return '';
    }
  }

  getTypeClass(type: string): string {
    return 'type-' + type.toLowerCase();
  }
}
