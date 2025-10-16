import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../../core/services/site.service';
import { BlastSequenceDataService } from '../../shared/services/blast-sequence-data.service';
import { SiteBlastingService } from '../../../../core/services/site-blasting.service';
import { UnifiedDrillDataService } from '../../../../core/services/unified-drill-data.service';
import { AuthService } from '../../../../core/services/auth.service';
import { StateService } from '../../../../core/services/state.service';
import { ExplosiveCalculationsService, ExplosiveCalculationResultDto } from '../../../../core/services/explosive-calculations.service';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogData } from '../../../../shared/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '../../../../core/services/notification.service';

interface WorkflowStep {
  id: string;
  name: string;
  description: string;
  icon: string;
  route: string;
  completed: boolean;
  enabled: boolean;
  progress?: number;
}

@Component({
  selector: 'app-site-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './site-dashboard.component.html',
  styleUrls: ['./site-dashboard.component.scss']
})
export class SiteDashboardComponent implements OnInit {
  project: Project | null = null;
  site: ProjectSite | null = null;
  
  state$: Observable<any>;

  loading = false;
  error: string | null = null;
  showApproveModal = false;
  showExplosiveApprovalModal: boolean = false;
  
  explosiveApprovalForm = {
    expectedUsageDate: '',
    blastingDate: '',
    blastTiming: '',
    comments: ''
  };
  
  // Explosive calculations data
  explosiveCalculations: ExplosiveCalculationResultDto | null = null;
  totalAnfo: number = 0;
  totalEmulsion: number = 0;
  
  minDate: string = new Date().toISOString().split('T')[0];

  workflowSteps: WorkflowStep[] = [
    {
      id: 'pattern-creator',
      name: 'Drilling Pattern Creator',
      description: 'Create and design drilling patterns for the site',
      icon: 'fas fa-crosshairs',
      route: 'pattern-creator',
      completed: false,
      enabled: true,
      progress: 0
    },
    {
      id: 'sequence-designer',
      name: 'Blast Sequence Designer',
      description: 'Design the blast sequence and connections',
      icon: 'fas fa-project-diagram',
      route: 'sequence-designer',
      completed: false,
      enabled: false,
      progress: 0
    },
    {
      id: 'explosive-calculations',
      name: 'Explosive Calculations',
      description: 'Calculate explosive requirements and parameters',
      icon: 'fas fa-calculator',
      route: 'explosive-calculations',
      completed: false,
      enabled: false,
      progress: 0
    },
    {
      id: 'simulator',
      name: 'Blast Sequence Simulator',
      description: 'Simulate and validate the blast sequence',
      icon: 'fas fa-play-circle',
      route: 'simulator',
      completed: false,
      enabled: false,
      progress: 0
    }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private siteService: SiteService,
    private blastSequenceDataService: BlastSequenceDataService,
    private siteBlastingService: SiteBlastingService,
    private unifiedDrillDataService: UnifiedDrillDataService,
    public authService: AuthService, // Made public for template access
    private stateService: StateService,
    private dialog: MatDialog,
    private notification: NotificationService,
    private explosiveCalculationsService: ExplosiveCalculationsService
  ) {
    this.state$ = this.stateService.state$;
  }

  ngOnInit() {
    const projectId = +(this.route.snapshot.paramMap.get('projectId') || '0');
    const siteId = +(this.route.snapshot.paramMap.get('siteId') || '0');
    
    this.stateService.setProjectId(projectId);
    this.stateService.setSiteId(siteId);



    if (projectId && siteId) {
      this.loadProject();
      this.loadSite();
      this.loadExplosiveCalculations();
      // loadWorkflowProgress() is now called from loadSite() after data is loaded
    }
  }

  loadProject() {
    this.loading = true;
    const projectId = this.stateService.currentState.activeProjectId;
    if (!projectId) return;

    this.projectService.getProject(projectId).subscribe({
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

  loadSite() {
    const siteId = this.stateService.currentState.activeSiteId;
    if (!siteId) return;

    this.siteService.getSite(siteId).subscribe({
      next: (site) => {
        this.site = site;
        // Initialize site-specific data service
        this.blastSequenceDataService.setSiteContext(this.stateService.currentState.activeProjectId!, this.stateService.currentState.activeSiteId!);
        
        // Wait a moment for backend data to load before checking progress
        setTimeout(() => {
          this.loadWorkflowProgress();
        }, 500);
      },
      error: (error) => {
        console.error('Error loading site:', error);
        this.error = 'Failed to load site information.';
      }
    });
  }

  loadWorkflowProgress() {
    // Load progress for each workflow step
    // This would integrate with your data service to check completion status
    this.blastSequenceDataService.getSiteWorkflowProgress(this.stateService.currentState.activeSiteId!).subscribe({
      next: (progress: any) => {
        this.updateWorkflowSteps(progress);
      },
      error: (error: any) => {
        console.warn('Could not load workflow progress:', error);
      }
    });
  }

  updateWorkflowSteps(progress: any) {
    // Update workflow steps based on saved progress
    this.workflowSteps.forEach(step => {
      const stepProgress = progress[step.id];
      if (stepProgress) {
        step.completed = stepProgress.completed;
        step.progress = stepProgress.progress || 0;
      }
    });

    // Enable next steps based on completion
    for (let i = 0; i < this.workflowSteps.length; i++) {
      if (i === 0) {
        this.workflowSteps[i].enabled = true;
      } else {
        this.workflowSteps[i].enabled = this.workflowSteps[i - 1].completed;
      }
    }
  }

  navigateToStep(step: WorkflowStep) {
    if (!step.enabled) return;
    
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    const route = `/blasting-engineer/project-management/${projectId}/sites/${siteId}/${step.route}`;
    this.router.navigate([route]);
  }

  goBack() {
    const projectId = this.stateService.currentState.activeProjectId;
    this.router.navigate(['/blasting-engineer/project-management', projectId, 'sites']);
  }

  getStepStatusClass(step: WorkflowStep): string {
    if (step.completed) return 'completed';
    if (step.enabled) return 'available';
    return 'disabled';
  }

  getOverallProgress(): number {
    const completedSteps = this.workflowSteps.filter(step => step.completed).length;
    return Math.round((completedSteps / this.workflowSteps.length) * 100);
  }

  formatDate(date: Date | string | null | undefined): string {
    if (!date) {
      return 'N/A';
    }
    
    try {
      // Convert to Date object if it's a string
      const dateObj = typeof date === 'string' ? new Date(date) : date;
      
      // Check if the date is valid
      if (isNaN(dateObj.getTime())) {
        return 'Invalid Date';
      }
      
      return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
      }).format(dateObj);
    } catch (error) {
      console.warn('Error formatting date:', error);
      return 'Invalid Date';
    }
  }

  cleanupSiteData(): void {
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;

    if (projectId && siteId) {
      console.log('🧹 Starting complete cleanup for site:', projectId, siteId);
      
      // Disable cleanup button to prevent multiple clicks
      const cleanupButton = document.querySelector('.cleanup-btn') as HTMLButtonElement;
      if (cleanupButton) {
        cleanupButton.disabled = true;
        cleanupButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i> <span>Cleaning...</span>';
      }
      
      // Clear all site data using Promise-based method
      this.blastSequenceDataService.cleanupSiteData(projectId, siteId).then((success) => {
        // Reset workflow steps to initial state
        this.workflowSteps.forEach(step => {
          step.completed = false;
          step.progress = 0;
          step.enabled = step.id === 'pattern-creator'; // Only first step enabled
        });
        
        // Show result to user
        if (success) {
          console.log('🎉 ✅ COMPLETE SUCCESS: All data deleted from database and frontend');
          this.notification.showSuccess('All site data successfully deleted from database');
        } else {
          console.error('⚠️ ❌ PARTIAL FAILURE: Some database operations failed');
          this.notification.showWarning('Some data may not have been deleted from database. Check console for details.');
        }
        
        // Reload progress after cleanup to confirm everything is reset
        setTimeout(() => {
          this.loadWorkflowProgress();
          console.log('✅ Site cleanup completed - all progress reset to 0%');
        }, 500);
        
        // Re-enable cleanup button
        if (cleanupButton) {
          cleanupButton.disabled = false;
          cleanupButton.innerHTML = '<i class="fas fa-broom"></i> <span>Cleanup Data</span>';
        }
      }).catch((error) => {
        console.error('💥 CLEANUP ERROR:', error);
        this.notification.showError('Failed to cleanup site data: ' + error.message);
        
        // Re-enable cleanup button
        if (cleanupButton) {
          cleanupButton.disabled = false;
          cleanupButton.innerHTML = '<i class="fas fa-broom"></i> <span>Cleanup Data</span>';
        }
      });
    } else {
      console.error('❌ Cannot cleanup: Missing project ID or site ID');
      this.notification.showError('Cannot cleanup: Missing project or site information');
    }
  }

  // Individual step cleanup methods
  cleanupStepData(stepId: string): void {
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;

    if (!projectId || !siteId) return;

    console.log('🧹 Cleaning up step:', stepId);

    switch (stepId) {
      case 'pattern-creator':
        this.blastSequenceDataService.cleanupPatternData(projectId, siteId);
        break;
      case 'sequence-designer':
        this.blastSequenceDataService.cleanupSequenceData(projectId, siteId);
        break;
      case 'explosive-calculations':
        // Cleanup explosive calculations data
        this.blastSequenceDataService.cleanupExplosiveCalculationsData(projectId, siteId);
        break;
      case 'simulator':
        this.blastSequenceDataService.cleanupSimulationData(projectId, siteId);
        break;
    }

    // Update local workflow state immediately
    const stepIndex = this.workflowSteps.findIndex(s => s.id === stepId);
    if (stepIndex !== -1) {
      this.workflowSteps[stepIndex].completed = false;
      this.workflowSteps[stepIndex].progress = 0;

      // Disable subsequent steps if this step is cleared
      for (let i = stepIndex + 1; i < this.workflowSteps.length; i++) {
        this.workflowSteps[i].enabled = false;
        this.workflowSteps[i].completed = false;
        this.workflowSteps[i].progress = 0;
      }

      // Re-enable workflow progression based on what's still completed
      this.updateWorkflowEnabling();
    }

    // Reload progress to reflect changes
    setTimeout(() => {
      this.loadWorkflowProgress();
      console.log(`✅ Step "${stepId}" cleanup completed`);
    }, 300);
  }

  private updateWorkflowEnabling(): void {
    // Always enable first step
    this.workflowSteps[0].enabled = true;

    // Enable subsequent steps based on previous completion
    for (let i = 1; i < this.workflowSteps.length; i++) {
      this.workflowSteps[i].enabled = this.workflowSteps[i - 1].completed;
    }
  }

  canCleanupStep(stepId: string): boolean {
    const step = this.workflowSteps.find(s => s.id === stepId);
    return step ? step.completed || (step.progress !== undefined && step.progress > 0) : false;
  }

  get isBlastingEngineer(): boolean {
    return this.authService.isBlastingEngineer();
  }

  private getApprovalKey(): string {
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    return `patternApproved_${projectId}_${siteId}`;
  }

  get isPatternApproved(): boolean {
    return this.site?.isPatternApproved || false;
  }

  approvePatternForOperator(): void {
    this.showApproveModal = true;
  }

  confirmApprove(): void {
    this.siteService.approvePattern(this.stateService.currentState.activeSiteId!).subscribe({
      next: () => {
        if (this.site) this.site.isPatternApproved = true;
        this.showApproveModal = false;
      }
    });
  }

  cancelApprove(): void {
    this.showApproveModal = false;
  }

  revokeApproval(): void {
    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmDialogData, boolean>(ConfirmDialogComponent, {
      width: '320px',
      data: {
        title: 'Revoke Approval',
        message: 'Revoke pattern approval for the operator? They will lose access until you approve again.',
        confirmText: 'Revoke',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (!confirmed) return;

      this.siteService.revokePattern(this.stateService.currentState.activeSiteId!).subscribe(() => {
        if (this.site) this.site.isPatternApproved = false;
        this.notification.showSuccess('Pattern approval revoked.');
      });
    });
  }

  // Simulation confirmation for admin
  private simulationConfirmKey(): string {
    const siteId = this.stateService.currentState.activeSiteId;
    return `simulation_confirmed_${siteId}`;
  }

  get isSimulationConfirmed(): boolean {
    return this.site?.isSimulationConfirmed || false;
  }

  get isExplosiveApprovalRequested(): boolean {
    return this.site?.isExplosiveApprovalRequested || false;
  }

  confirmSimulationForAdmin() {
    this.siteService.confirmSimulation(this.stateService.currentState.activeSiteId!).subscribe(() => {
      if(this.site) this.site.isSimulationConfirmed = true;
    });
  }

  revokeSimulationConfirmation() {
    this.siteService.revokeSimulation(this.stateService.currentState.activeSiteId!).subscribe(() => {
      if(this.site) this.site.isSimulationConfirmed = false;
    });
  }

  requestExplosiveApproval(): void {
    if (this.site) {
      // First check if there's already a pending request
      this.siteService.hasPendingExplosiveApprovalRequest(this.site.id).subscribe({
        next: (response) => {
          if (response.hasPendingRequest) {
            alert('There is already a pending explosive approval request for this project site. Please wait for the current request to be processed or cancel it first.');
          } else {
            // No pending request, show the modal
            this.explosiveApprovalForm = {
              expectedUsageDate: '',
              blastingDate: '',
              blastTiming: '',
              comments: ''
            };
            this.showExplosiveApprovalModal = true;
          }
        },
        error: (error) => {
          console.error('Error checking for pending explosive approval request:', error);
          // If check fails, still allow the user to try (they'll get proper error handling in confirmExplosiveApprovalRequest)
          this.explosiveApprovalForm = {
            expectedUsageDate: '',
            blastingDate: '',
            blastTiming: '',
            comments: ''
          };
          this.showExplosiveApprovalModal = true;
        }
      });
    }
  }

  confirmExplosiveApprovalRequest(): void {
    if (this.site && this.explosiveApprovalForm.expectedUsageDate) {
      this.siteService.requestExplosiveApproval(
        this.site.id,
        this.explosiveApprovalForm.expectedUsageDate,
        this.explosiveApprovalForm.comments
      ).subscribe({
        next: () => {
           console.log('Explosive approval request sent successfully');
           this.loadSite();
           this.showExplosiveApprovalModal = false;
         },
        error: (error) => {
          console.error('Error sending explosive approval request:', error);
          
          // Handle specific error cases
          if (error.status === 409) {
            alert('There is already a pending explosive approval request for this project site. Please wait for the current request to be processed or cancel it first.');
          } else if (error.status === 400) {
            alert('Invalid request data. Please check your input and try again.');
          } else if (error.status === 401) {
            alert('You are not authorized to make this request. Please log in again.');
          } else {
            alert('An error occurred while sending the explosive approval request. Please try again later.');
          }
        }
      });
    }
  }

  cancelExplosiveApprovalRequest(): void {
    this.showExplosiveApprovalModal = false;
    this.explosiveApprovalForm = {
      expectedUsageDate: '',
      blastingDate: '',
      blastTiming: '',
      comments: ''
    };
  }

  revokeExplosiveApprovalRequest(): void {
    if (this.site) {
      this.siteService.revokeExplosiveApprovalRequest(this.site.id).subscribe({
        next: () => {
           console.log('Explosive approval request revoked successfully');
           this.loadSite();
         },
        error: (error) => {
          console.error('Error revoking explosive approval request:', error);
          
          // Handle specific error cases
          if (error.status === 404) {
            alert('No explosive approval request found to revoke for this project site.');
          } else if (error.status === 401) {
            alert('You are not authorized to revoke this request. Please log in again.');
          } else if (error.message && error.message.includes('No explosive approval request found')) {
            alert('No explosive approval request found to revoke for this project site.');
          } else {
            alert('An error occurred while revoking the explosive approval request. Please try again later.');
          }
        }
      });
    }
  }

  /**
   * Test all backend connectivity and CRUD operations
   * Call this method from browser console to verify backend connections
   */
  testBackendConnectivity(): void {
    console.log('🔍 TESTING BACKEND CONNECTIVITY FOR SITE DASHBOARD');
    console.log('API Base URL:', 'http://localhost:5019');
    
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    
    if (!projectId || !siteId) {
      console.error('❌ No project or site ID available for testing');
      return;
    }

    console.log(`Testing with Project ID: ${projectId}, Site ID: ${siteId}`);
    
    // Test 1: Project Service - Read Operations
    console.log('\n1️⃣ Testing Project Service READ operations...');
    this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        console.log('✅ ProjectService.getProject() - SUCCESS:', project);
      },
      error: (error) => {
        console.error('❌ ProjectService.getProject() - FAILED:', error);
      }
    });

    // Test 2: Site Service - Read Operations
    console.log('\n2️⃣ Testing Site Service READ operations...');
    this.siteService.getSite(siteId).subscribe({
      next: (site) => {
        console.log('✅ SiteService.getSite() - SUCCESS:', site);
      },
      error: (error) => {
        console.error('❌ SiteService.getSite() - FAILED:', error);
      }
    });

    // Test 3: Site Service - Approval Operations
    console.log('\n3️⃣ Testing Site Service APPROVAL operations...');
    // Note: These will make actual changes, so we just test the HTTP call structure
    console.log('Approval endpoints available:');
    console.log('- POST /api/projectsites/{siteId}/approve');
    console.log('- POST /api/projectsites/{siteId}/revoke');
    console.log('- POST /api/projectsites/{siteId}/confirm-simulation');
    console.log('- POST /api/projectsites/{siteId}/revoke-simulation');

    // Test 4: BlastSequenceDataService - Site Context
    console.log('\n4️⃣ Testing BlastSequenceDataService...');
    this.blastSequenceDataService.setSiteContext(projectId, siteId);
    const context = this.blastSequenceDataService.getCurrentSiteContext();
    console.log('✅ Site context set:', context);

    // Test 5: Workflow Progress
    console.log('\n5️⃣ Testing Workflow Progress...');
    this.blastSequenceDataService.getSiteWorkflowProgress(siteId).subscribe({
      next: (progress) => {
        console.log('✅ Workflow progress loaded:', progress);
      },
      error: (error) => {
        console.error('❌ Workflow progress failed:', error);
      }
    });

    // Test 6: SiteBlastingService - Backend API calls
    console.log('\n6️⃣ Testing SiteBlastingService backend APIs...');
    
    // Test getting all site data
    this.siteBlastingService.getAllSiteData(projectId, siteId).subscribe({
      next: (data) => {
        console.log('✅ SiteBlastingService.getAllSiteData() - SUCCESS:', data);
      },
      error: (error) => {
        console.log('ℹ️ SiteBlastingService.getAllSiteData() - No data or connection issue:', error.status === 404 ? 'No data found (normal)' : error);
      }
    });

    // Test getting blast connections
    this.siteBlastingService.getBlastConnections(projectId, siteId).subscribe({
      next: (connections) => {
        console.log('✅ SiteBlastingService.getBlastConnections() - SUCCESS:', connections);
      },
      error: (error) => {
        console.log('ℹ️ SiteBlastingService.getBlastConnections() - No data or connection issue:', error.status === 404 ? 'No connections found (normal)' : error);
      }
    });

    console.log('\n🔍 Backend connectivity test completed. Check the results above.');
    console.log('✅ = Success, ❌ = Error, ℹ️ = Info/Expected behavior');
  }

  /**
   * Test specific CRUD operations with safe test data
   */
  testCrudOperations(): void {
    console.log('🔍 TESTING CRUD OPERATIONS');
    
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    
    if (!projectId || !siteId) {
      console.error('❌ No project or site ID available for CRUD testing');
      return;
    }

    // Test CREATE operation - Save test workflow data
    console.log('\n📝 Testing CREATE operation...');
    const testWorkflowData = {
      stepId: 'test-step',
      stepData: {
        testField: 'test-value',
        timestamp: new Date().toISOString(),
        description: 'Test data for CRUD verification'
      }
    };

    this.siteBlastingService.updateWorkflowStep(projectId, siteId, 'test-step', testWorkflowData).subscribe({
      next: (result) => {
        console.log('✅ CREATE operation - SUCCESS:', result);
        
        // Test READ operation
        console.log('\n📖 Testing READ operation...');
        this.siteBlastingService.getSiteData(projectId, siteId, 'test-step').subscribe({
          next: (data) => {
            console.log('✅ READ operation - SUCCESS:', data);
            
            // Test UPDATE operation
            console.log('\n📝 Testing UPDATE operation...');
            const updatedData = {
              ...testWorkflowData.stepData,
              testField: 'updated-test-value',
              updatedAt: new Date().toISOString()
            };

            this.siteBlastingService.updateSiteData(projectId, siteId, 'test-step', {
              jsonData: updatedData
            }).subscribe({
              next: (updateResult) => {
                console.log('✅ UPDATE operation - SUCCESS:', updateResult);
                
                // Test DELETE operation
                console.log('\n🗑️ Testing DELETE operation...');
                this.siteBlastingService.deleteSiteData(projectId, siteId, 'test-step').subscribe({
                  next: (deleteResult) => {
                    console.log('✅ DELETE operation - SUCCESS:', deleteResult);
                    console.log('\n🎉 All CRUD operations completed successfully!');
                  },
                  error: (error) => {
                    console.error('❌ DELETE operation - FAILED:', error);
                  }
                });
              },
              error: (error) => {
                console.error('❌ UPDATE operation - FAILED:', error);
              }
            });
          },
          error: (error) => {
            console.error('❌ READ operation - FAILED:', error);
          }
        });
      },
      error: (error) => {
        console.error('❌ CREATE operation - FAILED:', error);
      }
    });
  }

  /**
   * Quick method to test if backend is reachable
   */
  testBackendHealth(): void {
    console.log('🏥 Testing backend health...');
    
    const projectId = this.stateService.currentState.activeProjectId;
    
    if (!projectId) {
      console.error('❌ No project ID available');
      return;
    }

    // Simple health check using project service
    this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        console.log('✅ Backend is HEALTHY - API responding correctly');
        console.log('🔗 Connected to:', 'http://localhost:5019');
        console.log('📊 Sample response:', { id: project.id, name: project.name });
      },
      error: (error) => {
        console.error('❌ Backend health check FAILED');
        console.error('🔗 Attempted connection to:', 'http://localhost:5019');
        console.error('📋 Error details:', {
          status: error.status,
          message: error.message,
          url: error.url
        });
        
        if (error.status === 0) {
          console.error('💡 Suggestion: Check if backend server is running on http://localhost:5019');
        }
      }
    });
  }

  /**
   * Test specifically if drill points are deleted
   */
  testDrillPointDeletion(): void {
    console.log('🎯 TESTING DRILL POINT DELETION SPECIFICALLY');
    
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    
    if (!projectId || !siteId) {
      console.error('❌ No project or site ID available');
      return;
    }

    console.log(`🎯 Testing drill point deletion for Project ${projectId}, Site ${siteId}`);
    
    // Step 1: Check current drill points
    console.log('\n📋 Step 1: Checking current drill points...');
    this.unifiedDrillDataService.getDrillPoints(projectId, siteId).subscribe({
      next: (points) => {
        console.log(`📊 Found ${points.length} drill points before deletion`);
        
        if (points.length === 0) {
          console.log('ℹ️ No drill points to delete');
          return;
        }
        
        // Step 2: Delete drill points
        console.log('\n🗑️ Step 2: Deleting drill points...');
        this.unifiedDrillDataService.clearAllDrillPoints(projectId, siteId).subscribe({
          next: (success) => {
            console.log('🔍 Deletion result:', success);
            
            // Step 3: Verify deletion
            console.log('\n🔍 Step 3: Verifying deletion...');
            setTimeout(() => {
              this.unifiedDrillDataService.getDrillPoints(projectId, siteId).subscribe({
                next: (remainingPoints) => {
                  console.log(`📊 Found ${remainingPoints.length} drill points after deletion`);
                  
                  if (remainingPoints.length === 0) {
                    console.log('✅ SUCCESS: All drill points were deleted from database');
                  } else {
                    console.log('❌ FAILED: Some drill points still exist in database');
                    console.log('🔍 Remaining points:', remainingPoints);
                  }
                },
                error: (error) => {
                  if (error.status === 404) {
                    console.log('✅ SUCCESS: No drill points found (404 = properly deleted)');
                  } else {
                    console.log('❌ ERROR: Failed to verify deletion:', error);
                  }
                }
              });
            }, 1000);
          },
          error: (error) => {
            console.error('❌ DELETION FAILED:', error);
            console.error('🔗 Failed endpoint: DELETE /api/DrillPointPattern/drill-points');
          }
        });
      },
      error: (error) => {
        console.log('ℹ️ No drill points found initially:', error.status);
      }
    });
  }

  /**
   * Test the cleanup functionality by verifying data is actually deleted
   */
  testCleanupFunctionality(): void {
    console.log('🧪 TESTING CLEANUP FUNCTIONALITY');
    
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    
    if (!projectId || !siteId) {
      console.error('❌ No project or site ID available for cleanup testing');
      return;
    }

    console.log(`🎯 Testing cleanup for Project ${projectId}, Site ${siteId}`);
    
    // Step 1: Check what data exists before cleanup
    console.log('\n📋 Step 1: Checking existing data...');
    
    Promise.all([
      // Check workflow data
      new Promise((resolve) => {
        this.siteBlastingService.getAllSiteData(projectId, siteId).subscribe({
          next: (data) => resolve({ type: 'workflow', exists: true, count: data.length }),
          error: () => resolve({ type: 'workflow', exists: false, count: 0 })
        });
      }),
      
      // Check drill points
      new Promise((resolve) => {
        this.unifiedDrillDataService.getDrillPoints(projectId, siteId).subscribe({
          next: (points) => resolve({ type: 'drill-points', exists: true, count: points.length }),
          error: () => resolve({ type: 'drill-points', exists: false, count: 0 })
        });
      }),
      
      // Check blast connections
      new Promise((resolve) => {
        this.siteBlastingService.getBlastConnections(projectId, siteId).subscribe({
          next: (connections) => resolve({ type: 'connections', exists: true, count: connections.length }),
          error: () => resolve({ type: 'connections', exists: false, count: 0 })
        });
      })
    ]).then((beforeResults: any[]) => {
      console.log('📊 Data before cleanup:');
      beforeResults.forEach(result => {
        console.log(`   ${result.type}: ${result.exists ? `${result.count} items` : 'No data'}`);
      });
      
      // Step 2: Perform cleanup
      console.log('\n🧹 Step 2: Performing cleanup...');
      this.blastSequenceDataService.cleanupSiteData(projectId, siteId).then((cleanupSuccess) => {
        
        console.log('🔍 Cleanup result:', cleanupSuccess ? 'SUCCESS' : 'FAILED');
        
        // Step 3: Verify data is actually deleted
        console.log('\n🔍 Step 3: Verifying data deletion...');
        
        setTimeout(() => {
          Promise.all([
            // Check workflow data again
            new Promise((resolve) => {
              this.siteBlastingService.getAllSiteData(projectId, siteId).subscribe({
                next: (data) => resolve({ type: 'workflow', exists: true, count: data.length }),
                error: () => resolve({ type: 'workflow', exists: false, count: 0 })
              });
            }),
            
            // Check drill points again
            new Promise((resolve) => {
              this.unifiedDrillDataService.getDrillPoints(projectId, siteId).subscribe({
                next: (points) => resolve({ type: 'drill-points', exists: true, count: points.length }),
                error: () => resolve({ type: 'drill-points', exists: false, count: 0 })
              });
            }),
            
            // Check blast connections again
            new Promise((resolve) => {
              this.siteBlastingService.getBlastConnections(projectId, siteId).subscribe({
                next: (connections) => resolve({ type: 'connections', exists: true, count: connections.length }),
                error: () => resolve({ type: 'connections', exists: false, count: 0 })
              });
            })
          ]).then((afterResults: any[]) => {
            console.log('📊 Data after cleanup:');
            afterResults.forEach(result => {
              console.log(`   ${result.type}: ${result.exists ? `${result.count} items` : 'No data'}`);
            });
            
            // Step 4: Compare results
            console.log('\n📈 Step 4: Cleanup verification:');
            let allDataDeleted = true;
            
            afterResults.forEach((after, index) => {
              const before = beforeResults[index];
              const wasDeleted = (before.count > 0 && after.count === 0) || (before.count === 0 && after.count === 0);
              
              console.log(`   ${after.type}: ${before.count} → ${after.count} ${wasDeleted ? '✅' : '❌'}`);
              
              if (!wasDeleted && before.count > 0) {
                allDataDeleted = false;
              }
            });
            
            console.log('\n🎯 ======= CLEANUP TEST RESULT =======');
            if (allDataDeleted) {
              console.log('✅ SUCCESS: All data was properly deleted from database');
            } else {
              console.log('❌ FAILED: Some data still exists in database');
            }
            console.log('====================================');
          });
        }, 1000); // Wait 1 second for cleanup to complete
      });
    });
  }

  loadExplosiveCalculations() {
    const projectId = this.stateService.currentState.activeProjectId;
    const siteId = this.stateService.currentState.activeSiteId;
    
    if (!projectId || !siteId) return;

    this.explosiveCalculationsService.getByProjectAndSite(projectId, siteId).subscribe({
      next: (calculations) => {
        if (calculations && calculations.length > 0) {
          // Get the latest calculation result
          this.explosiveCalculations = calculations[calculations.length - 1];
          this.totalAnfo = this.explosiveCalculations.totalAnfo || 0;
          this.totalEmulsion = this.explosiveCalculations.totalEmulsion || 0;
        } else {
          // No calculations found, set defaults
          this.totalAnfo = 0;
          this.totalEmulsion = 0;
        }
      },
      error: (error) => {
        console.error('Error loading explosive calculations:', error);
        // Set defaults on error
        this.totalAnfo = 0;
        this.totalEmulsion = 0;
      }
    });
  }
}