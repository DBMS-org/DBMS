import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { BlastSequenceDataService, WorkflowStep } from '../blast-sequence-data.service';

export enum WorkflowStepId {
  PATTERN = 'pattern',
  SEQUENCE = 'sequence',
  SIMULATE = 'simulate'
}

export interface NavigationState {
  currentStep: WorkflowStepId;
  canNavigateBack: boolean;
  canNavigateForward: boolean;
  nextStepEnabled: boolean;
  previousStepName?: string;
  nextStepName?: string;
}

export interface RouteConfig {
  stepId: WorkflowStepId;
  route: string;
  component: string;
  title: string;
}

@Injectable({
  providedIn: 'root'
})
export class NavigationController {
  
  private readonly routes: RouteConfig[] = [
    {
      stepId: WorkflowStepId.PATTERN,
      route: '/blasting-engineer/drilling-pattern-creator',
      component: 'DrillingPatternCreatorComponent',
      title: 'Create Drilling Pattern'
    },
    {
      stepId: WorkflowStepId.SEQUENCE,
      route: '/blasting-engineer/blast-sequence-designer',
      component: 'BlastSequenceDesignerComponent',
      title: 'Design Blast Sequence'
    },
    {
      stepId: WorkflowStepId.SIMULATE,
      route: '/blasting-engineer/blast-sequence-simulator',
      component: 'BlastSequenceSimulatorComponent',
      title: 'Simulate & Validate'
    }
  ];

  // Site-aware routes for context-specific navigation
  private readonly contextRoutes: RouteConfig[] = [
    {
      stepId: WorkflowStepId.PATTERN,
      route: '/blasting-engineer/project-management/:projectId/sites/:siteId/pattern-creator',
      component: 'DrillingPatternCreatorComponent',
      title: 'Create Drilling Pattern'
    },
    {
      stepId: WorkflowStepId.SEQUENCE,
      route: '/blasting-engineer/project-management/:projectId/sites/:siteId/sequence-designer',
      component: 'BlastSequenceDesignerComponent',
      title: 'Design Blast Sequence'
    },
    {
      stepId: WorkflowStepId.SIMULATE,
      route: '/blasting-engineer/project-management/:projectId/sites/:siteId/simulator',
      component: 'BlastSequenceSimulatorComponent',
      title: 'Simulate & Validate'
    }
  ];

  private navigationStateSubject = new BehaviorSubject<NavigationState>({
    currentStep: WorkflowStepId.PATTERN,
    canNavigateBack: false,
    canNavigateForward: false,
    nextStepEnabled: false
  });

  public navigationState$ = this.navigationStateSubject.asObservable();

  constructor(
    private router: Router,
    private dataService: BlastSequenceDataService
  ) {
    // Subscribe to workflow steps to update navigation state
    this.dataService.workflowSteps$.subscribe(steps => {
      this.updateNavigationState(steps);
    });
  }

  // Workaround to get site context
  private getSiteContext(): { projectId: number; siteId: number } | null {
    // Access the private property directly as a workaround
    return (this.dataService as any).currentSiteContext || null;
  }

  // Site-context-aware navigation methods
  navigateToStepWithContext(stepId: WorkflowStepId, projectId?: number, siteId?: number, force: boolean = false): Promise<boolean> {
    // Get context from parameters or current site context
    const context = projectId && siteId ? { projectId, siteId } : this.getSiteContext();
    
    if (!context) {
      console.warn('No site context available for navigation. Falling back to basic navigation.');
      return this.navigateToStep(stepId, force);
    }

    const routeConfig = this.contextRoutes.find(r => r.stepId === stepId);
    if (!routeConfig) {
      console.error(`No context route found for step: ${stepId}`);
      return Promise.resolve(false);
    }

    // Check if navigation is allowed
    if (!force && !this.canNavigateToStep(stepId)) {
      console.warn(`Navigation to step ${stepId} is not allowed`);
      return Promise.resolve(false);
    }

    // Build the route with actual IDs
    const route = routeConfig.route
      .replace(':projectId', context.projectId.toString())
      .replace(':siteId', context.siteId.toString());

    // Update current workflow step
    this.dataService.setCurrentWorkflowStep(stepId);

    console.log(`Navigating with context: ${route}`);
    // Navigate to the context-aware route
    return this.router.navigate([route]);
  }

  // Navigation Methods
  navigateToStep(stepId: WorkflowStepId, force: boolean = false): Promise<boolean> {
    const route = this.getRouteForStep(stepId);
    if (!route) {
      console.error(`No route found for step: ${stepId}`);
      return Promise.resolve(false);
    }

    // Check if navigation is allowed
    if (!force && !this.canNavigateToStep(stepId)) {
      console.warn(`Navigation to step ${stepId} is not allowed`);
      return Promise.resolve(false);
    }

    // Update current workflow step
    this.dataService.setCurrentWorkflowStep(stepId);

    // Navigate to the route
    return this.router.navigate([route.route]);
  }

  // Context-aware convenience methods
  navigateToPatternCreator(projectId?: number, siteId?: number): Promise<boolean> {
    return this.navigateToStepWithContext(WorkflowStepId.PATTERN, projectId, siteId);
  }

  navigateToSequenceDesigner(projectId?: number, siteId?: number): Promise<boolean> {
    return this.navigateToStepWithContext(WorkflowStepId.SEQUENCE, projectId, siteId);
  }

  navigateToSimulator(projectId?: number, siteId?: number): Promise<boolean> {
    return this.navigateToStepWithContext(WorkflowStepId.SIMULATE, projectId, siteId);
  }

  navigateNext(): Promise<boolean> {
    const currentState = this.navigationStateSubject.value;
    if (!currentState.canNavigateForward) {
      return Promise.resolve(false);
    }

    const nextStep = this.getNextStep(currentState.currentStep);
    if (nextStep) {
      // Try context-aware navigation first
      const context = this.getSiteContext();
      if (context) {
        return this.navigateToStepWithContext(nextStep, context.projectId, context.siteId);
      } else {
        return this.navigateToStep(nextStep);
      }
    }

    return Promise.resolve(false);
  }

  navigatePrevious(): Promise<boolean> {
    const currentState = this.navigationStateSubject.value;
    if (!currentState.canNavigateBack) {
      return Promise.resolve(false);
    }

    const previousStep = this.getPreviousStep(currentState.currentStep);
    if (previousStep) {
      // Try context-aware navigation first
      const context = this.getSiteContext();
      if (context) {
        return this.navigateToStepWithContext(previousStep, context.projectId, context.siteId);
      } else {
        return this.navigateToStep(previousStep);
      }
    }

    return Promise.resolve(false);
  }

  // Navigation Guards
  canNavigateToStep(stepId: WorkflowStepId): boolean {
    const steps = this.dataService.getWorkflowSteps();
    const targetStep = steps.find((s: WorkflowStep) => s.id === stepId);
    return targetStep ? targetStep.enabled : false;
  }

  canLeaveCurrentStep(): boolean {
    // Add validation logic here if needed
    // For now, allow navigation but could add unsaved changes checks
    return true;
  }

  // Step Information
  getCurrentStep(): WorkflowStepId {
    return this.navigationStateSubject.value.currentStep;
  }

  getCurrentStepInfo(): RouteConfig | null {
    const currentStep = this.getCurrentStep();
    return this.getRouteForStep(currentStep);
  }

  getStepProgress(): { current: number; total: number; percentage: number } {
    const steps = this.dataService.getWorkflowSteps();
    const currentIndex = this.getStepIndex(this.getCurrentStep());
    const completedSteps = steps.filter((s: WorkflowStep) => s.completed).length;
    
    return {
      current: currentIndex + 1,
      total: steps.length,
      percentage: Math.round(((currentIndex + 1) / steps.length) * 100)
    };
  }

  // Breadcrumb Navigation
  getBreadcrumbs(): Array<{ stepId: WorkflowStepId; title: string; active: boolean; enabled: boolean }> {
    const steps = this.dataService.getWorkflowSteps();
    const currentStep = this.getCurrentStep();

    return this.routes.map(route => {
      const step = steps.find((s: WorkflowStep) => s.id === route.stepId);
      return {
        stepId: route.stepId,
        title: route.title,
        active: route.stepId === currentStep,
        enabled: step ? step.enabled : false
      };
    });
  }

  // Data Validation for Navigation
  validateCurrentStepData(): { isValid: boolean; message?: string } {
    const currentStep = this.getCurrentStep();

    switch (currentStep) {
      case WorkflowStepId.PATTERN:
        const patternData = this.dataService.getPatternData();
        if (!patternData || patternData.drillPoints.length === 0) {
          return { isValid: false, message: 'Please create a drilling pattern with at least one hole' };
        }
        return { isValid: true };

      case WorkflowStepId.SEQUENCE:
        const connections = this.dataService.getConnections();
        if (connections.length === 0) {
          return { isValid: false, message: 'Please create at least one blast connection' };
        }
        return { isValid: true };

      case WorkflowStepId.SIMULATE:
        // Simulation step doesn't require additional validation
        return { isValid: true };

      default:
        return { isValid: true };
    }
  }

  // Quick Actions
  jumpToSimulation(): Promise<boolean> {
    // Validate that we have all required data
    const patternData = this.dataService.getPatternData();
    const connections = this.dataService.getConnections();

    if (!patternData) {
      console.warn('Cannot jump to simulation: No pattern data');
      return Promise.resolve(false);
    }

    if (connections.length === 0) {
      console.warn('Cannot jump to simulation: No blast connections');
      return Promise.resolve(false);
    }

    return this.navigateToStep(WorkflowStepId.SIMULATE, true);
  }

  startNewWorkflow(): Promise<boolean> {
    // Reset all data
    this.dataService.clearPatternData();
    
    // Navigate to first step
    return this.navigateToStep(WorkflowStepId.PATTERN, true);
  }

  // URL Sync
  syncWithCurrentRoute(currentUrl: string): void {
    const stepId = this.getStepIdFromUrl(currentUrl);
    if (stepId && stepId !== this.getCurrentStep()) {
      this.dataService.setCurrentWorkflowStep(stepId);
    }
  }

  // Private Helper Methods
  private updateNavigationState(steps: WorkflowStep[]): void {
    const currentStep = steps.find(s => s.current)?.id as WorkflowStepId || WorkflowStepId.PATTERN;
    const currentIndex = this.getStepIndex(currentStep);
    
    const previousStep = currentIndex > 0 ? this.routes[currentIndex - 1] : null;
    const nextStep = currentIndex < this.routes.length - 1 ? this.routes[currentIndex + 1] : null;
    const nextStepData = nextStep ? steps.find(s => s.id === nextStep.stepId) : null;

    const navigationState: NavigationState = {
      currentStep,
      canNavigateBack: currentIndex > 0,
      canNavigateForward: nextStepData ? nextStepData.enabled : false,
      nextStepEnabled: nextStepData ? nextStepData.enabled : false,
      previousStepName: previousStep?.title,
      nextStepName: nextStep?.title
    };

    this.navigationStateSubject.next(navigationState);
  }

  private getRouteForStep(stepId: WorkflowStepId): RouteConfig | null {
    return this.routes.find(r => r.stepId === stepId) || null;
  }

  private getStepIndex(stepId: WorkflowStepId): number {
    return this.routes.findIndex(r => r.stepId === stepId);
  }

  private getNextStep(currentStep: WorkflowStepId): WorkflowStepId | null {
    const currentIndex = this.getStepIndex(currentStep);
    return currentIndex < this.routes.length - 1 ? this.routes[currentIndex + 1].stepId : null;
  }

  private getPreviousStep(currentStep: WorkflowStepId): WorkflowStepId | null {
    const currentIndex = this.getStepIndex(currentStep);
    return currentIndex > 0 ? this.routes[currentIndex - 1].stepId : null;
  }

  private getStepIdFromUrl(url: string): WorkflowStepId | null {
    const route = this.routes.find(r => url.includes(r.route));
    return route ? route.stepId : null;
  }

  // Export/Import Navigation
  exportWorkflowState(): any {
    return {
      currentStep: this.getCurrentStep(),
      patternData: this.dataService.getPatternData(),
      connections: this.dataService.getConnections(),
      timestamp: new Date().toISOString()
    };
  }

  importWorkflowState(state: any): Promise<boolean> {
    try {
      if (state.patternData) {
        this.dataService.setPatternData(state.patternData);
      }
      
      if (state.connections) {
        this.dataService.setConnections(state.connections);
      }

      if (state.currentStep) {
        return this.navigateToStep(state.currentStep, true);
      }

      return Promise.resolve(true);
    } catch (error) {
      console.error('Failed to import workflow state:', error);
      return Promise.resolve(false);
    }
  }

  // Analytics/Progress Tracking
  getWorkflowAnalytics(): {
    totalTimeSpent: number;
    stepsCompleted: number;
    currentSession: string;
    completionPercentage: number;
  } {
    const steps = this.dataService.getWorkflowSteps();
    const completedSteps = steps.filter((s: WorkflowStep) => s.completed).length;
    const progress = this.getStepProgress();

    return {
      totalTimeSpent: 0, // Would need to track this separately
      stepsCompleted: completedSteps,
      currentSession: new Date().toISOString(),
      completionPercentage: progress.percentage
    };
  }
} 