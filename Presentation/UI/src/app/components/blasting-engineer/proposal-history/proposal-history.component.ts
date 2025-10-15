import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProposalHistoryService, ProposalHistoryItem, ExplosiveApprovalStatus } from '../../../core/services/proposal-history.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { ProjectService } from '../../../core/services/project.service';
import { ProposalDetailsComponent } from '../proposal-details/proposal-details.component';
import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

interface DisplayProposalItem {
  id: number;
  projectName: string;
  siteName: string;
  proposalType: string;
  status: 'pending' | 'approved' | 'rejected' | 'cancelled';
  submittedDate: Date;
  blastingDate?: string;
  blastTiming?: string;
}

@Component({
  selector: 'app-proposal-history',
  standalone: true,
  imports: [CommonModule, FormsModule, ProposalDetailsComponent],
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

  // Delete modal properties
  showDeleteModal: boolean = false;
  deleteProposalData: { id: number; projectName: string; siteName: string } | null = null;
  isDeleting: boolean = false;

  // Details modal properties
  showDetailsModal: boolean = false;
  selectedProposalId: number | null = null;

  // Timing modal properties
  showTimingModal: boolean = false;
  timingForm = {
    proposalId: 0,
    blastingDate: '',
    blastTiming: ''
  };
  isSavingTiming: boolean = false;

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
    
    console.log('📋 Loading proposal history...');
    
    this.proposalHistoryService.getUserProposals().subscribe({
      next: (proposals: ProposalHistoryItem[]) => {
        // Create an array of observables to fetch site details for each proposal
        const siteRequests = proposals.map(proposal => 
          this.siteService.getSite(proposal.projectSiteId).pipe(
            catchError(error => {
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
                
                console.log('✅ Proposal history loaded successfully:', {
                  count: this.proposalHistory.length,
                  items: this.proposalHistory.map(item => ({
                    id: item.id,
                    projectName: item.projectName,
                    siteName: item.siteName,
                    status: item.status,
                    submittedDate: item.submittedDate
                  }))
                });
                
                this.filteredHistory = [...this.proposalHistory];
                this.isLoading = false;
              },
              error: (error) => {
                console.error('❌ Error loading project details:', error);
                console.log('🔄 Falling back to mapping with site data only...');
                // Fallback to mapping with site data only
                this.proposalHistory = proposals.map((proposal, index) => 
                  this.mapToDisplayItem(proposal, sites[index])
                );
                console.log('⚠️ Fallback proposal history loaded (no projects):', {
                  count: this.proposalHistory.length,
                  items: this.proposalHistory.map(item => ({ id: item.id, projectName: item.projectName, siteName: item.siteName, status: item.status }))
                });
                this.filteredHistory = [...this.proposalHistory];
                this.isLoading = false;
              }
            });
          },
          error: (error) => {
            console.error('❌ Error loading site details:', error);
            console.log('🔄 Falling back to original mapping without site/project data...');
            // Fallback to original mapping without site/project data
            this.proposalHistory = proposals.map(proposal => this.mapToDisplayItem(proposal));
            console.log('⚠️ Fallback proposal history loaded (no sites/projects):', {
              count: this.proposalHistory.length,
              items: this.proposalHistory.map(item => ({ id: item.id, projectName: item.projectName, siteName: item.siteName, status: item.status }))
            });
            this.filteredHistory = [...this.proposalHistory];
            this.isLoading = false;
          }
        });
      },
      error: (error) => {
        console.error('❌ Error loading proposal history:', error);
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
      blastingDate: (proposal as any).blastingDate,
      blastTiming: (proposal as any).blastTiming
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
        item.siteName.toLowerCase().includes(this.searchTerm.toLowerCase());
      
      return matchesStatus && matchesSearch;
    });
  }

  /**
   * Check if a proposal can be deleted (only pending proposals can be deleted)
   */
  canDeleteProposal(status: string): boolean {
    return status === 'pending';
  }

  /**
   * Show delete confirmation modal
   */
  confirmDeleteProposal(id: number, projectName: string, siteName: string): void {
    this.deleteProposalData = { id, projectName, siteName };
    this.showDeleteModal = true;
  }

  /**
   * Cancel delete operation
   */
  cancelDelete(): void {
    this.showDeleteModal = false;
    this.deleteProposalData = null;
    this.isDeleting = false;
  }

  /**
   * Delete the proposal
   */
  deleteProposal(): void {
    if (!this.deleteProposalData) return;

    this.isDeleting = true;
    const proposalId = this.deleteProposalData.id;

    this.proposalHistoryService.deleteProposal(proposalId).subscribe({
      next: () => {
        // Remove the deleted proposal from the arrays
        this.proposalHistory = this.proposalHistory.filter(p => p.id !== proposalId);
        this.filteredHistory = this.filteredHistory.filter(p => p.id !== proposalId);
        
        // Close the modal
        this.cancelDelete();
        
        // Show success message (you could implement a toast notification here)
        console.log('Proposal deleted successfully');
      },
      error: (error) => {
        console.error('Error deleting proposal:', error);
        this.isDeleting = false;
        
        // Handle specific error cases
        let errorMessage = 'Failed to delete proposal. Please try again.';
        if (error.status === 404) {
          errorMessage = 'Proposal not found. It may have already been deleted.';
        } else if (error.status === 403) {
          errorMessage = 'You are not authorized to delete this proposal.';
        } else if (error.message) {
          errorMessage = error.message;
        }
        
        // You could implement a toast notification here instead of alert
        alert(errorMessage);
      }
    });
  }

  // Details modal methods
  viewProposalDetails(proposalId: number): void {
    this.selectedProposalId = proposalId;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedProposalId = null;
  }

  // Timing modal methods
  openTimingModal(proposal: DisplayProposalItem): void {
    this.timingForm = {
      proposalId: proposal.id,
      blastingDate: proposal.blastingDate || '',
      blastTiming: proposal.blastTiming || ''
    };
    this.showTimingModal = true;
  }

  closeTimingModal(): void {
    this.showTimingModal = false;
    this.timingForm = {
      proposalId: 0,
      blastingDate: '',
      blastTiming: ''
    };
    this.isSavingTiming = false;
  }

  saveTiming(): void {
    if (!this.timingForm.blastingDate || !this.timingForm.blastTiming) {
      alert('Please fill in both blasting date and timing');
      return;
    }

    this.isSavingTiming = true;

    // Note: This is a frontend-only implementation
    // When backend is ready, use: this.proposalHistoryService.updateTiming(...)
    console.log('Saving timing (frontend only):', this.timingForm);

    // Simulate API call with timeout
    setTimeout(() => {
      // Update the proposal in the local arrays
      const updateProposal = (p: DisplayProposalItem) => {
        if (p.id === this.timingForm.proposalId) {
          return {
            ...p,
            blastingDate: this.timingForm.blastingDate,
            blastTiming: this.timingForm.blastTiming
          };
        }
        return p;
      };

      this.proposalHistory = this.proposalHistory.map(updateProposal);
      this.filteredHistory = this.filteredHistory.map(updateProposal);

      console.log('✅ Timing updated successfully (frontend only)');
      this.closeTimingModal();
    }, 500);
  }

  isTimingFormValid(): boolean {
    return !!(this.timingForm.blastingDate && this.timingForm.blastTiming);
  }

  hasTimingSet(proposal: DisplayProposalItem): boolean {
    return !!(proposal.blastingDate && proposal.blastTiming);
  }

  formatTimingDisplay(date?: string, time?: string): string {
    if (!date || !time) {
      return 'Not Set';
    }
    try {
      const dateObj = new Date(date);
      const formattedDate = dateObj.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
      });
      return `${formattedDate} at ${time}`;
    } catch {
      return 'Invalid Date/Time';
    }
  }

}
