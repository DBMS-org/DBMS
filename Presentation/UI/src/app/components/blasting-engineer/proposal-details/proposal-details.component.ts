import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProposalHistoryService, ProposalHistoryItem, ExplosiveApprovalStatus } from '../../../core/services/proposal-history.service';
import { SiteService, ProjectSite } from '../../../core/services/site.service';
import { ProjectService } from '../../../core/services/project.service';
import { UserService } from '../../../core/services/user.service';
import { Subscription } from 'rxjs';
import { forkJoin, of, Observable } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

export interface ProposalDetailsData {
  id: number;
  status: string;
  expectedUsageDate: Date;
  comments: string;
  requesterName: string;
  requesterEmail: string;
  requesterId: number;
  requesterRegion: string;
  projectSite: string;
  projectName?: string;
  projectRegion?: string;
  processedBy?: string;
  processorEmail?: string;
  processedAt?: Date;
  rejectionReason?: string;
  createdAt: Date;
  updatedAt: Date;
}

@Component({
  selector: 'app-proposal-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './proposal-details.component.html',
  styleUrls: ['./proposal-details.component.scss']
})
export class ProposalDetailsComponent implements OnInit, OnDestroy {
  @Input() proposalId: number | null = null;
  @Output() closeEvent = new EventEmitter<void>();

  proposalData: ProposalDetailsData | null = null;
  isLoading = false;
  hasError = false;
  private subscription: Subscription = new Subscription();

  constructor(
    private proposalHistoryService: ProposalHistoryService,
    private siteService: SiteService,
    private projectService: ProjectService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    if (this.proposalId) {
      this.loadProposalDetails();
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  loadProposalDetails(): void {
    if (!this.proposalId) {
      console.error('❌ No proposal ID provided');
      this.hasError = true;
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    this.hasError = false;
    
    console.log('📋 Loading proposal details for ID:', this.proposalId);

    // Get the proposal data first
    const loadSub = this.proposalHistoryService.getUserProposals().subscribe({
      next: (proposals: ProposalHistoryItem[]) => {
        console.log('📦 All proposals received:', proposals);
        
        const proposal = proposals.find((p: ProposalHistoryItem) => p.id === this.proposalId);
        if (proposal) {
          console.log('✅ Found proposal:', proposal);
          console.log('🔍 Proposal status:', proposal.status, 'Type:', typeof proposal.status);
          // Enrich the proposal data with site, project, and user information
          this.enrichProposalData(proposal);
        } else {
          console.error('❌ Proposal not found with ID:', this.proposalId);
          this.hasError = true;
          this.isLoading = false;
        }
      },
      error: (error: any) => {
        console.error('❌ Error loading proposals:', error);
        this.hasError = true;
        this.isLoading = false;
      }
    });

    this.subscription.add(loadSub);
  }

  private enrichProposalData(proposal: ProposalHistoryItem): void {
    console.log('🔄 Enriching proposal data:', proposal);
    
    // Create observables for each piece of data we need to fetch
    const siteRequest = proposal.projectSiteId ? 
      this.siteService.getSite(proposal.projectSiteId).pipe(
        catchError(() => of(null))
      ) : of(null);

    const requesterRequest = proposal.requestedByUserId ? 
      this.userService.getUser(proposal.requestedByUserId).pipe(
        catchError(() => of(null))
      ) : of(null);

    // Use available processor user ID properties - prioritize processedByUserId
    const processorUserId = proposal.processedByUserId || proposal.approvedByUserId || proposal.rejectedByUserId;
    console.log('🔍 Processor User ID:', processorUserId);
    
    const processorRequest = processorUserId ? 
      this.userService.getUser(processorUserId).pipe(
        catchError((error) => {
          console.error('❌ Error fetching processor user:', error);
          return of(null);
        })
      ) : of(null);

    const enrichSub = siteRequest.pipe(
      switchMap(site => {
        const projectRequest = site?.projectId ? 
          this.projectService.getProject(site.projectId).pipe(
            catchError(() => of(null))
          ) : of(null);

        return forkJoin({
          site: of(site),
          project: projectRequest,
          requester: requesterRequest,
          processor: processorRequest
        });
      }),
      map(({ site, project, requester, processor }) => {
        console.log('📊 Enriched data:', { site, project, requester, processor });
        return this.mapToEnrichedProposalDetailsData(proposal, site, project, requester, processor);
      }),
      catchError(() => {
        // If enrichment fails, fall back to basic mapping
        return of(this.mapToProposalDetailsData(proposal));
      })
    ).subscribe({
      next: (proposalData) => {
        this.proposalData = proposalData;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error enriching proposal data:', error);
        this.proposalData = this.mapToProposalDetailsData(proposal);
        this.isLoading = false;
      }
    });

    this.subscription.add(enrichSub);
  }

  private mapToEnrichedProposalDetailsData(
    proposal: ProposalHistoryItem, 
    site: ProjectSite | null, 
    project: any, 
    requester: any, 
    processor: any
  ): ProposalDetailsData {
    return {
      id: proposal.id,
      status: this.getStatusText(proposal.status),
      expectedUsageDate: proposal.expectedUsageDate,
      comments: proposal.comments || '',
      
      // Enriched requester information
      requesterName: requester?.name || proposal.requestedByUserName || 'Unknown',
      requesterEmail: requester?.email || 'unknown@example.com',
      requesterId: proposal.requestedByUserId,
      requesterRegion: requester?.region || 'Unknown',
      
      // Enriched project and site information
      projectSite: site?.name || proposal.projectSiteName || `Site ${proposal.projectSiteId}`,
      projectName: project?.name || 'Unknown Project',
      projectRegion: project?.region || 'Unknown',
      
      // Enriched processing information
      processedBy: processor?.name || proposal.processedByUser?.name || (proposal.approvedByUserName || proposal.rejectedByUserName) || 'Not processed yet',
      processorEmail: processor?.email?.value || processor?.email || proposal.processedByUser?.email || 'N/A',
      processedAt: proposal.processedAt || proposal.approvedAt || proposal.rejectedAt,
      rejectionReason: proposal.rejectionReason,
      
      createdAt: proposal.createdAt,
      updatedAt: proposal.updatedAt
    };
  }

  private getStatusText(status: number | ExplosiveApprovalStatus | string): string {
    console.log('🔍 Status value received:', status, 'Type:', typeof status);
    
    // Handle string status values directly
    if (typeof status === 'string') {
      switch (status.toLowerCase()) {
        case 'pending': return 'Pending';
        case 'approved': return 'Approved';
        case 'rejected': return 'Rejected';
        case 'cancelled': return 'Cancelled';
        default: 
          console.warn('⚠️ Unknown string status value:', status);
          return status; // Return the original string if it's not recognized
      }
    }
    
    // Handle numeric enum values
    const statusValue = typeof status === 'number' ? status : Number(status);
    
    switch (statusValue) {
      case 0: return 'Pending';
      case 1: return 'Approved';
      case 2: return 'Rejected';
      case 3: return 'Cancelled';
      default: 
        console.warn('⚠️ Unknown numeric status value:', status, 'Defaulting to Unknown');
        return 'Unknown';
    }
  }

  private mapToProposalDetailsData(proposal: ProposalHistoryItem): ProposalDetailsData {
    return {
      id: proposal.id,
      status: this.getStatusText(proposal.status),
      expectedUsageDate: proposal.expectedUsageDate,
      comments: proposal.comments || '',
      requesterName: proposal.requestedByUserName || 'Unknown',
      requesterEmail: 'unknown@example.com',
      requesterId: proposal.requestedByUserId || 0,
      requesterRegion: 'Unknown',
      projectSite: proposal.projectSiteName || `Site ${proposal.projectSiteId}`,
      projectName: 'Unknown Project',
      projectRegion: 'Unknown',
      processedBy: proposal.approvedByUserName || proposal.rejectedByUserName || 'Not processed yet',
      processorEmail: 'N/A',
      processedAt: proposal.approvedAt || proposal.rejectedAt,
      rejectionReason: proposal.rejectionReason,
      createdAt: proposal.createdAt,
      updatedAt: proposal.updatedAt
    };
  }

  closeDetails(): void {
    this.closeEvent.emit();
  }

  retryLoad(): void {
    this.loadProposalDetails();
  }
}
