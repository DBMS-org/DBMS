import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Project } from '../../../../core/models/project.model';
import { ProjectService } from '../../../../core/services/project.service';
import { SiteService, ProjectSite } from '../../../../core/services/site.service';
import { BlastSequenceDataService } from '../../shared/services/blast-sequence-data.service';

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
  imports: [CommonModule],
  templateUrl: './site-dashboard.component.html',
  styleUrls: ['./site-dashboard.component.scss']
})
export class SiteDashboardComponent implements OnInit {
  project: Project | null = null;
  site: ProjectSite | null = null;
  projectId: number = 0;
  siteId: number = 0;
  loading = false;
  error: string | null = null;

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
    private blastSequenceDataService: BlastSequenceDataService
  ) {}

  ngOnInit() {
    this.projectId = +(this.route.snapshot.paramMap.get('projectId') || '0');
    this.siteId = +(this.route.snapshot.paramMap.get('siteId') || '0');
    
    if (this.projectId && this.siteId) {
      this.loadProject();
      this.loadSite();
      // loadWorkflowProgress() is now called from loadSite() after data is loaded
    }
  }

  loadProject() {
    this.loading = true;
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

  loadSite() {
    this.siteService.getSite(this.siteId).subscribe({
      next: (site) => {
        this.site = site;
        // Initialize site-specific data service
        this.blastSequenceDataService.setSiteContext(this.projectId, this.siteId);
        
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
    this.blastSequenceDataService.getSiteWorkflowProgress(this.siteId).subscribe({
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
    
    const route = `/blasting-engineer/project-management/${this.projectId}/sites/${this.siteId}/${step.route}`;
    this.router.navigate([route]);
  }

  goBack() {
    this.router.navigate(['/blasting-engineer/project-management', this.projectId, 'sites']);
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
    if (this.projectId && this.siteId) {
      console.log('🧹 Starting complete cleanup for site:', this.projectId, this.siteId);
      
      // Clear all site data
      this.blastSequenceDataService.cleanupSiteData(this.projectId, this.siteId);
      
      // Reset workflow steps to initial state
      this.workflowSteps.forEach(step => {
        step.completed = false;
        step.progress = 0;
        step.enabled = step.id === 'pattern-creator'; // Only first step enabled
      });
      
      // Reload progress after cleanup to confirm everything is reset
      setTimeout(() => {
        this.loadWorkflowProgress();
        console.log('✅ Site cleanup completed - all progress reset to 0%');
      }, 300);
    }
  }

  // Individual step cleanup methods
  cleanupStepData(stepId: string): void {
    if (!this.projectId || !this.siteId) return;

    console.log('🧹 Cleaning up step:', stepId);

    switch (stepId) {
      case 'pattern-creator':
        this.blastSequenceDataService.cleanupPatternData(this.projectId, this.siteId);
        break;
      case 'sequence-designer':
        this.blastSequenceDataService.cleanupSequenceData(this.projectId, this.siteId);
        break;
      case 'simulator':
        this.blastSequenceDataService.cleanupSimulationData(this.projectId, this.siteId);
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
} 