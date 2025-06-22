import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, Subject, of } from 'rxjs';
import { takeUntil, switchMap } from 'rxjs/operators';

import { ProjectService } from '../../../../../core/services/project.service';
import { UserService } from '../../../../../core/services/user.service';
import { MachineService } from '../../../../../core/services/machine.service';
import { Project } from '../../../../../core/models/project.model';
import { User } from '../../../../../core/models/user.model';
import { Machine, MachineType, MachineStatus } from '../../../../../core/models/machine.model';
import { REGIONS } from '../../../../../core/constants/regions';

interface UserAssignment {
  id: number;
  name: string;
  email: string;
  role: string;
  region: string;
  status: string;
}

interface OperatorProject {
  id: number;
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedMachine?: Machine;
}

@Component({
  selector: 'app-operator-assignment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './operator-assignment.component.html',
  styleUrl: './operator-assignment.component.scss'
})
export class OperatorAssignmentComponent implements OnInit, OnDestroy {
  @Input() selectedOperator: UserAssignment | null = null;
  @Output() closeModal = new EventEmitter<void>();

  private destroy$ = new Subject<void>();

  // Data
  currentProject: OperatorProject | null = null;
  availableProjects: Project[] = [];
  availableMachines: Machine[] = [];
  allProjects: Project[] = [];
  allMachines: Machine[] = [];

  // Form state
  selectedProjectId: number | null = null;
  selectedMachineId: number | null = null;
  reassignmentReason: string = '';

  // UI state
  isLoading = false;
  error: string | null = null;
  successMessage: string | null = null;
  showConfirmDialog = false;

  // Constants
  regionOptions = REGIONS;
  statusOptions = ['Active', 'Planning', 'In Progress', 'Completed', 'On Hold', 'Cancelled'];

  constructor(
    private projectService: ProjectService,
    private userService: UserService,
    private machineService: MachineService
  ) {}

  ngOnInit(): void {
    if (this.selectedOperator) {
      this.loadOperatorData();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadOperatorData(): void {
    if (!this.selectedOperator) return;

    this.isLoading = true;
    this.error = null;

    // Load all required data in parallel
    forkJoin({
      projects: this.projectService.getProjects(),
      machines: this.machineService.getAllMachines()
    }).pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: ({ projects, machines }) => {
        this.allProjects = projects;
        this.allMachines = machines;
        
        // Find operator's current project
        this.findCurrentProject();
        
        // Filter available projects and machines
        this.filterAvailableOptions();
        
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading operator data:', error);
        this.error = 'Failed to load operator assignment data';
        this.loadMockData(); // Fallback to mock data
        this.isLoading = false;
      }
    });
  }

  private findCurrentProject(): void {
    if (!this.selectedOperator) return;

    // Find project assigned to this operator
    const assignedProject = this.allProjects.find(p => p.assignedUserId === this.selectedOperator!.id);
    
    if (assignedProject) {
      // Find machine assigned to this project (if any)
      const assignedMachine = this.allMachines.find(m => 
        m.projectId === assignedProject.id || 
        m.operatorId === this.selectedOperator!.id
      );

      this.currentProject = {
        id: assignedProject.id,
        name: assignedProject.name,
        region: assignedProject.region,
        status: assignedProject.status,
        description: assignedProject.description,
        startDate: assignedProject.startDate,
        endDate: assignedProject.endDate,
        assignedMachine: assignedMachine
      };
    }
  }

  private filterAvailableOptions(): void {
    if (!this.selectedOperator) return;

    // Filter projects by operator's region (excluding current project)
    this.availableProjects = this.allProjects.filter(p => 
      p.region === this.selectedOperator!.region && 
      p.id !== this.currentProject?.id &&
      (p.status === 'Active' || p.status === 'Planning' || p.status === 'In Progress')
    );

    // Filter machines by operator's region and availability
    this.availableMachines = this.allMachines.filter(m => 
      m.currentLocation === this.selectedOperator!.region && 
      (m.status === MachineStatus.AVAILABLE || m.status === MachineStatus.IN_MAINTENANCE)
    );
  }

  private loadMockData(): void {
    // Mock data as fallback
    this.currentProject = {
      id: 1,
      name: 'Muttrah Construction Project',
      region: this.selectedOperator?.region || 'Muscat',
      status: 'In Progress',
      description: 'Major construction project in Muttrah area',
      startDate: new Date('2024-01-15'),
      endDate: new Date('2024-12-31'),
              assignedMachine: {
          id: 101,
          name: 'Excavator CAT-320',
          type: MachineType.EXCAVATOR,
          model: 'CAT-320',
          manufacturer: 'Caterpillar',
          serialNumber: 'CAT-320-001',
          status: MachineStatus.ASSIGNED,
          currentLocation: this.selectedOperator?.region || 'Muscat',
          operatorId: this.selectedOperator?.id,
          projectId: 1,
          createdAt: new Date(),
          updatedAt: new Date()
        }
    };

    this.availableProjects = [
      {
        id: 2,
        name: 'Al Khuwair Development',
        region: this.selectedOperator?.region || 'Muscat',
        status: 'Planning',
        description: 'New development project in Al Khuwair',
        startDate: new Date('2024-06-01'),
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 3,
        name: 'Seeb Infrastructure',
        region: this.selectedOperator?.region || 'Muscat',
        status: 'Active',
        description: 'Infrastructure upgrade in Seeb',
        startDate: new Date('2024-03-01'),
        createdAt: new Date(),
        updatedAt: new Date()
      }
    ];

    this.availableMachines = [
      {
        id: 102,
        name: 'Bulldozer D6T',
        type: MachineType.BULLDOZER,
        model: 'CAT-D6T',
        manufacturer: 'Caterpillar',
        serialNumber: 'CAT-D6T-001',
        status: MachineStatus.AVAILABLE,
        currentLocation: this.selectedOperator?.region || 'Muscat',
        createdAt: new Date(),
        updatedAt: new Date()
      },
      {
        id: 103,
        name: 'Loader 950M',
        type: MachineType.LOADER,
        model: 'CAT-950M',
        manufacturer: 'Caterpillar',
        serialNumber: 'CAT-950M-001',
        status: MachineStatus.AVAILABLE,
        currentLocation: this.selectedOperator?.region || 'Muscat',
        createdAt: new Date(),
        updatedAt: new Date()
      }
    ];
  }

  onProjectChange(): void {
    // Clear machine selection when project changes
    this.selectedMachineId = null;
    this.error = null;
  }

  onMachineChange(): void {
    this.error = null;
  }

  reassignOperator(): void {
    if (!this.selectedProjectId) {
      this.error = 'Please select a project to assign the operator to';
      return;
    }

    if (!this.reassignmentReason.trim()) {
      this.error = 'Please provide a reason for reassignment';
      return;
    }

    this.showConfirmDialog = true;
  }

  confirmReassignment(): void {
    this.showConfirmDialog = false;
    this.isLoading = true;
    this.error = null;

    if (!this.selectedOperator || !this.selectedProjectId) {
      this.error = 'Missing required information for reassignment';
      this.isLoading = false;
      return;
    }

    // Get the selected project details first to preserve other data
    this.projectService.getProject(this.selectedProjectId).pipe(
      switchMap(project => {
        // Create full update request with existing project data + new operator assignment
        const updateRequest = {
          id: project.id,
          name: project.name,
          region: project.region,
          status: project.status,
          description: project.description,
          startDate: project.startDate,
          endDate: project.endDate,
          assignedUserId: this.selectedOperator!.id
        };
        
        // Update the project with new operator assignment
        return this.projectService.updateProject(this.selectedProjectId!, updateRequest);
      }),
      takeUntil(this.destroy$)
    ).subscribe({
      next: () => {
        this.successMessage = `${this.selectedOperator?.name} has been successfully reassigned to the selected project!`;
        this.isLoading = false;
        
        // Refresh the data to show updated assignments
        this.loadOperatorData();
        
        // Close modal after showing success message
        setTimeout(() => {
          this.onClose();
        }, 2000);
      },
      error: (error) => {
        console.error('Error during reassignment:', error);
        this.error = `Failed to reassign operator: ${error.message}`;
        this.isLoading = false;
      }
    });
  }

  cancelReassignment(): void {
    this.showConfirmDialog = false;
  }

  onClose(): void {
    this.closeModal.emit();
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active': return 'status-active';
      case 'in progress': return 'status-in-progress';
      case 'completed': return 'status-completed';
      case 'on hold': return 'status-on-hold';
      case 'cancelled': return 'status-cancelled';
      case 'planning': return 'status-planning';
      default: return 'status-default';
    }
  }

  getMachineStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'active': return 'machine-active';
      case 'available': return 'machine-available';
      case 'maintenance': return 'machine-maintenance';
      case 'out of service': return 'machine-out-of-service';
      default: return 'machine-default';
    }
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'Not specified';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }

  getUserInitials(name: string): string {
    return name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2);
  }

  // Getter methods for template expressions to avoid complex bindings
  get selectedProjectName(): string {
    if (!this.selectedProjectId) return '';
    const project = this.availableProjects.find(p => p.id === this.selectedProjectId);
    return project?.name || '';
  }

  get selectedMachineName(): string {
    if (!this.selectedMachineId) return '';
    const machine = this.availableMachines.find(m => m.id === this.selectedMachineId);
    return machine?.name || '';
  }
}
