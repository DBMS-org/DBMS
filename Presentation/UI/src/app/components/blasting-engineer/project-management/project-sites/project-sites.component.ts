import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../../core/services/site.service';
import { ExplosiveApprovalRequestService } from '../../../../core/services/explosive-approval-request.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface ExtendedProjectSite extends ProjectSite {
  explosiveApprovalStatus?: 'Pending' | 'Approved' | 'Rejected' | 'Cancelled' | 'Expired' | null;
}

@Component({
  selector: 'app-project-sites',
  imports: [CommonModule, FormsModule, ToastModule, ConfirmDialogModule],
  providers: [ConfirmationService, MessageService],
  templateUrl: './project-sites.component.html',
  styleUrl: './project-sites.component.scss'
})
export class ProjectSitesComponent implements OnInit {
  project: Project | null = null;
  projectId: number = 0;
  sites: ExtendedProjectSite[] = [];
  loading = false;
  error: string | null = null;
  showDeleteModal = false;
  siteToDelete: ExtendedProjectSite | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private siteService: SiteService,
    private explosiveApprovalRequestService: ExplosiveApprovalRequestService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.projectId = +(this.route.snapshot.paramMap.get('id') || '0');
    if (this.projectId) {
      this.loadProject();
      this.loadProjectSites();
    }
  }

  loadProject() {
    this.loading = true;
    this.error = null;

    this.projectService.getProject(this.projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading project:', error);
        this.error = 'Failed to load project information.';
        this.loading = false;
      }
    });
  }

  loadProjectSites() {
    if (!this.projectId) return;

    this.loading = true;
    this.error = null;

    this.siteService.getProjectSites(this.projectId).subscribe({
      next: (sites) => {
        // Create an array of observables to fetch explosive approval status for each site
        const statusRequests = sites.map(site =>
          this.explosiveApprovalRequestService.getExplosiveApprovalRequestsByProjectSite(site.id).pipe(
            catchError(error => {
              console.log(`No explosive approval requests found for site ${site.id}`);
              return of([]);
            })
          )
        );

        // Execute all requests in parallel
        forkJoin(statusRequests).subscribe({
          next: (approvalRequestsArray) => {
            // Map sites with their explosive approval status
            this.sites = sites.map((site, index) => {
              const approvalRequests = approvalRequestsArray[index];
              // Get the latest approval request (most recent)
              const latestRequest = approvalRequests.length > 0
                ? approvalRequests.reduce((latest, current) =>
                    new Date(current.createdAt) > new Date(latest.createdAt) ? current : latest
                  )
                : null;

              return {
                ...site,
                createdAt: new Date(site.createdAt),
                updatedAt: new Date(site.updatedAt),
                explosiveApprovalStatus: latestRequest?.status || null
              };
            });

            // DEBUG: Log loaded sites data
            console.log('📋 DEBUG Loaded project sites:', this.sites.length);
            this.sites.forEach(site => {
              console.log(`  Site: ${site.name}`);
              console.log(`    - isPatternApproved: ${site.isPatternApproved}`);
              console.log(`    - isExplosiveApprovalRequested: ${site.isExplosiveApprovalRequested}`);
              console.log(`    - explosiveApprovalStatus: ${site.explosiveApprovalStatus}`);
              console.log(`    - isOperatorCompleted: ${site.isOperatorCompleted}`);
            });

            this.loading = false;
          },
          error: (error) => {
            console.error('Error loading explosive approval statuses:', error);
            // Fallback to basic site data without approval status
            this.sites = sites.map(site => ({
              ...site,
              createdAt: new Date(site.createdAt),
              updatedAt: new Date(site.updatedAt),
              explosiveApprovalStatus: null
            }));
            this.loading = false;
          }
        });
      },
      error: (error) => {
        console.error('Error loading project sites:', error);
        this.error = 'Failed to load project sites.';
        this.loading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management']);
  }

  addNewSite() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', 'new']);
  }

  addSite() {
    this.addNewSite();
  }

  uploadSurvey(site: ExtendedProjectSite) {
    // Navigate to CSV upload page for survey data
    this.router.navigate(['/blasting-engineer/csv-upload'], {
      queryParams: {
        siteId: site.id,
        siteName: site.name,
        projectId: this.projectId
      }
    });
  }

  createPattern(site: ExtendedProjectSite) {
    // Navigate to site dashboard
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', site.id, 'dashboard']);
  }

  viewDrillVisualization(site: ExtendedProjectSite) {
    // Navigate to drill visualization with proper route parameters
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites', site.id, 'drill-visualization']);
  }

  openDeleteModal(site: ExtendedProjectSite) {
    this.siteToDelete = site;
    this.showDeleteModal = true;
  }

  closeDeleteModal() {
    this.showDeleteModal = false;
    this.siteToDelete = null;
  }

  confirmDelete() {
    if (this.siteToDelete) {
      this.siteService.deleteSite(this.siteToDelete.id).subscribe({
        next: () => {
          this.sites = this.sites.filter(s => s.id !== this.siteToDelete!.id);
          this.closeDeleteModal();
        },
        error: (error) => {
          console.error('Error deleting site:', error);
          this.error = 'Failed to delete site. Please try again.';
          this.closeDeleteModal();
        }
      });
    }
  }

  deleteSite(site: ExtendedProjectSite) {
    this.openDeleteModal(site);
  }

  formatDate(date: Date): string {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    }).format(date);
  }

  // Completion statistics methods
  getCompletedSitesCount(): number {
    return this.sites.filter(site => site.isOperatorCompleted).length;
  }

  getPendingSitesCount(): number {
    return this.sites.filter(site => !site.isOperatorCompleted).length;
  }

  getCompletionPercentage(): number {
    if (this.sites.length === 0) return 0;
    return Math.round((this.getCompletedSitesCount() / this.sites.length) * 100);
  }

  // Site completion functionality
  canCompleteSite(site: ExtendedProjectSite): boolean {
    // A site can be completed if:
    // 1. Pattern is approved (isPatternApproved = true)
    // 2. Explosive approval status is 'Approved'

    // DEBUG: Log the site state
    console.log('🔍 DEBUG canCompleteSite for site:', site.name);
    console.log('  - isPatternApproved:', site.isPatternApproved);
    console.log('  - explosiveApprovalStatus:', site.explosiveApprovalStatus);

    const canComplete = site.isPatternApproved &&
                        site.explosiveApprovalStatus === 'Approved';

    console.log('  ➡️ Result:', canComplete ? '✅ CAN COMPLETE' : '❌ CANNOT COMPLETE');

    return canComplete;
  }

  getCompleteButtonTooltip(site: ExtendedProjectSite): string {
    const missingRequirements: string[] = [];

    if (!site.isPatternApproved) {
      missingRequirements.push('Pattern approval');
    }

    if (site.explosiveApprovalStatus !== 'Approved') {
      if (!site.explosiveApprovalStatus) {
        missingRequirements.push('Explosive approval request');
      } else if (site.explosiveApprovalStatus === 'Pending') {
        missingRequirements.push('Explosive approval (currently pending)');
      } else if (site.explosiveApprovalStatus === 'Rejected') {
        missingRequirements.push('Explosive approval (request was rejected)');
      } else if (site.explosiveApprovalStatus === 'Cancelled') {
        missingRequirements.push('Explosive approval (request was cancelled)');
      } else if (site.explosiveApprovalStatus === 'Expired') {
        missingRequirements.push('Explosive approval (request expired)');
      }
    }

    if (missingRequirements.length > 0) {
      return `Missing requirements: ${missingRequirements.join(', ')}`;
    }

    return 'Mark site as complete';
  }

  completeSite(site: ExtendedProjectSite) {
    if (!this.canCompleteSite(site)) {
      return;
    }

    // Show PrimeNG confirmation dialog before completing
    this.confirmationService.confirm({
      message: `Are you sure you want to mark "${site.name}" as complete? This action will mark the site as completed by the operator and update the completion status. This action cannot be undone.`,
      header: 'Complete Site',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Yes, Complete',
      rejectLabel: 'Cancel',
      acceptButtonStyleClass: 'p-button-success',
      rejectButtonStyleClass: 'p-button-secondary',
      accept: () => {
        this.loading = true;
        this.error = null;

        this.siteService.completeSite(site.id).subscribe({
          next: () => {
            // Update the local site data
            const siteIndex = this.sites.findIndex(s => s.id === site.id);
            if (siteIndex !== -1) {
              this.sites[siteIndex] = {
                ...this.sites[siteIndex],
                isOperatorCompleted: true,
                updatedAt: new Date()
              };
            }
            this.loading = false;

            // Show success toast notification
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: `Site "${site.name}" marked as complete successfully!`,
              life: 5000
            });
          },
          error: (error) => {
            console.error('Error completing site:', error);
            this.error = 'Failed to complete site. Please try again.';
            this.loading = false;

            // Show error toast notification
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Failed to complete site. Please try again.',
              life: 5000
            });
          }
        });
      }
    });
  }
}
