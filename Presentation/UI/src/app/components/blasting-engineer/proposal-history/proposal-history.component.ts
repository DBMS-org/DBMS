import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProposalHistoryService, ProposalHistoryItem, ExplosiveApprovalStatus } from '../../../core/services/proposal-history.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { ProjectService } from '../../../core/services/project.service';
import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

interface DisplayProposalItem {
  id: number;
  projectName: string;
  siteName: string;
  proposalType: string;
  status: 'pending' | 'approved' | 'rejected' | 'cancelled';
  submittedDate: Date;
  submittedBy: string;
  reviewedDate?: Date;
  reviewedBy?: string;
  comments?: string;
}

@Component({
  selector: 'app-proposal-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './proposal-history.component.html',
  styleUrl: './proposal-history.component.scss'
})
export class ProposalHistoryComponent implements OnInit {
  proposalHistory: DisplayProposalItem[] = [];
  filteredHistory: DisplayProposalItem[] = [];
  selectedStatus: string = 'all';
  searchTerm: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private proposalHistoryService: ProposalHistoryService,
    private siteService: SiteService,
    private projectService: ProjectService
  ) {}

  ngOnInit() {
    this.loadProposalHistory();
  }

  loadProposalHistory() {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.proposalHistoryService.getUserProposals().subscribe({
      next: (proposals: ProposalHistoryItem[]) => {
        // Create an array of observables to fetch site details for each proposal
        const siteRequests = proposals.map(proposal => 
          this.siteService.getSite(proposal.projectSiteId).pipe(
            catchError(error => {
              console.error(`Error fetching site ${proposal.projectSiteId}:`, error);
              // Return a fallback site object if fetch fails
              return of({
                id: proposal.projectSiteId,
                projectId: 0,
                name: proposal.projectSiteName || `Site ${proposal.projectSiteId}`,
                location: '',
                status: '',
                description: '',
                isPatternApproved: false,
                isSimulationConfirmed: false,
                isOperatorCompleted: false,
                isExplosiveApprovalRequested: false,
                createdAt: new Date(),
                updatedAt: new Date()
              } as ProjectSite);
            })
          )
        );

        // Execute all site requests in parallel
        forkJoin(siteRequests).subscribe({
          next: (sites: ProjectSite[]) => {
            // Create an array of observables to fetch project details
            const projectRequests = sites.map(site => 
              site.projectId > 0 ? 
                this.projectService.getProject(site.projectId).pipe(
                  catchError(error => {
                    console.error(`Error fetching project ${site.projectId}:`, error);
                    return of({ id: site.projectId, name: `Project ${site.projectId}` });
                  })
                ) : 
                of({ id: 0, name: 'Unknown Project' })
            );

            // Execute all project requests in parallel
            forkJoin(projectRequests).subscribe({
              next: (projects: any[]) => {
                // Map proposals with their corresponding site and project data
                this.proposalHistory = proposals.map((proposal, index) => 
                  this.mapToDisplayItem(proposal, sites[index], projects[index])
                );
                this.filteredHistory = [...this.proposalHistory];
                this.isLoading = false;
              },
              error: (error) => {
                console.error('Error loading project details:', error);
                // Fallback to mapping with site data only
                this.proposalHistory = proposals.map((proposal, index) => 
                  this.mapToDisplayItem(proposal, sites[index])
                );
                this.filteredHistory = [...this.proposalHistory];
                this.isLoading = false;
              }
            });
          },
          error: (error) => {
            console.error('Error loading site details:', error);
            // Fallback to original mapping without site/project data
            this.proposalHistory = proposals.map(proposal => this.mapToDisplayItem(proposal));
            this.filteredHistory = [...this.proposalHistory];
            this.isLoading = false;
          }
        });
      },
      error: (error) => {
        console.error('Error loading proposal history:', error);
        this.errorMessage = 'Failed to load proposal history. Please try again.';
        this.isLoading = false;
        // Fallback to empty array on error
        this.proposalHistory = [];
        this.filteredHistory = [];
      }
    });
  }

  private mapToDisplayItem(proposal: ProposalHistoryItem, site?: ProjectSite, project?: any): DisplayProposalItem {
    return {
      id: proposal.id,
      projectName: project?.name || `Project ${site?.projectId || 'Unknown'}`,
      siteName: site?.name || proposal.projectSiteName || `Site ${proposal.projectSiteId}`,
      proposalType: 'Explosive Approval Request',
      status: this.mapStatus(proposal.status),
      submittedDate: proposal.createdAt,
      submittedBy: proposal.requestedByUserName || `User ${proposal.requestedByUserId}`,
      reviewedDate: proposal.approvedAt || proposal.rejectedAt,
      reviewedBy: proposal.approvedByUserName || proposal.rejectedByUserName,
      comments: proposal.approvalComments || proposal.rejectionReason || proposal.comments
    };
  }

  private mapStatus(status: ExplosiveApprovalStatus): 'pending' | 'approved' | 'rejected' | 'cancelled' {
    switch (status) {
      case ExplosiveApprovalStatus.Pending:
        return 'pending';
      case ExplosiveApprovalStatus.Approved:
        return 'approved';
      case ExplosiveApprovalStatus.Rejected:
        return 'rejected';
      case ExplosiveApprovalStatus.Cancelled:
        return 'cancelled';
      default:
        return 'pending';
    }
  }

  filterByStatus(status: string) {
    this.selectedStatus = status;
    this.applyFilters();
  }

  onSearchChange(event: any) {
    this.searchTerm = event.target.value;
    this.applyFilters();
  }

  applyFilters() {
    this.filteredHistory = this.proposalHistory.filter(item => {
      const matchesStatus = this.selectedStatus === 'all' || item.status === this.selectedStatus;
      const matchesSearch = this.searchTerm === '' || 
        item.projectName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        item.siteName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        item.submittedBy.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      return matchesStatus && matchesSearch;
    });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'approved':
        return 'status-approved';
      case 'rejected':
        return 'status-rejected';
      case 'pending':
        return 'status-pending';
      case 'cancelled':
        return 'status-cancelled';
      default:
        return 'status-pending';
    }
  }

}
