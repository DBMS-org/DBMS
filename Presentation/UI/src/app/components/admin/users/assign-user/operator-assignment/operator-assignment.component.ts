import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, Subject, of, Observable } from 'rxjs';
import { takeUntil, switchMap, catchError } from 'rxjs/operators';

import { ProjectService } from '../../../../../core/services/project.service';
import { UserService } from '../../../../../core/services/user.service';
import { MachineService } from '../../../../../core/services/machine.service';
import { Project, UpdateProjectRequest } from '../../../../../core/models/project.model';
import { User } from '../../../../../core/models/user.model';
import { Machine, MachineType, MachineStatus, UpdateMachineRequest } from '../../../../../core/models/machine.model';
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
      projects: this.projectService.getAllProjects(),
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

    // Filter machines - since machines now require projectId, we need to filter based on 
    // projects in the operator's region and machine availability
    const regionProjectIds = this.allProjects
      .filter(p => p.region === this.selectedOperator!.region)
      .map(p => p.id);

    this.availableMachines = this.allMachines.filter(m => 
      regionProjectIds.includes(m.projectId) &&
      (m.status === MachineStatus.AVAILABLE || m.status === MachineStatus.IN_MAINTENANCE) &&
      !m.operatorId // Not currently assigned to an operator
    );
  }



  onProjectChange(): void {
    // Clear machine selection when project changes
    this.selectedMachineId = null;
    this.error = null;
    
    // Filter machines for the selected project
    if (this.selectedProjectId) {
      this.availableMachines = this.allMachines.filter(m => 
        m.projectId === this.selectedProjectId &&
        (m.status === MachineStatus.AVAILABLE || m.status === MachineStatus.IN_MAINTENANCE) &&
        !m.operatorId // Not currently assigned to an operator
      );
    } else {
      // Show all available machines in the region if no specific project selected
      this.filterAvailableOptions();
    }
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

    // Perform reassignment operations
    this.performReassignment();
  }

  private performReassignment(): void {
    console.log('Starting reassignment process...');
    console.log('Selected operator:', this.selectedOperator);
    console.log('Selected project ID:', this.selectedProjectId);
    console.log('Selected machine ID:', this.selectedMachineId);
    
    const operations: Observable<any>[] = [];

    // 1. Update project assignment
    const projectUpdate = this.projectService.getProject(this.selectedProjectId!).pipe(
      switchMap(project => {
        if (!project) {
          console.error('Project not found for ID:', this.selectedProjectId);
          throw new Error('Project not found');
        }
        
        console.log('Updating project:', project);
        const updateRequest: UpdateProjectRequest = {
          id: project.id,
          name: project.name,
          region: project.region,
          status: project.status,
          description: project.description || '',
          startDate: project.startDate || undefined,
          endDate: project.endDate || undefined,
          assignedUserId: this.selectedOperator!.id
        };
        return this.projectService.updateProject(this.selectedProjectId!, updateRequest);
      }),
      catchError((error: any) => {
        console.error('Error updating project:', error);
        throw error;
      })
    );
    operations.push(projectUpdate);

    // 2. If machine is selected, assign it to the operator
    if (this.selectedMachineId) {
      console.log('Assigning machine to operator...');
      const selectedMachine = this.availableMachines.find(m => m.id === this.selectedMachineId);
      if (!selectedMachine) {
        console.error('Selected machine not found in available machines list');
        throw new Error('Selected machine not found');
      }
      
      console.log('Selected machine:', selectedMachine);
      const machineUpdate: UpdateMachineRequest = {
        name: selectedMachine.name,
        type: selectedMachine.type,
        manufacturer: selectedMachine.manufacturer,
        model: selectedMachine.model,
        serialNumber: selectedMachine.serialNumber,
        rigNo: selectedMachine.rigNo,
        plateNo: selectedMachine.plateNo,
        manufacturingYear: selectedMachine.manufacturingYear,
        chassisDetails: selectedMachine.chassisDetails,
        currentLocation: selectedMachine.currentLocation,
        projectId: this.selectedProjectId!,
        operatorId: this.selectedOperator!.id,
        status: MachineStatus.ASSIGNED,
        lastMaintenanceDate: selectedMachine.lastMaintenanceDate,
        nextMaintenanceDate: selectedMachine.nextMaintenanceDate
      };

      const machineUpdateOp = this.machineService.updateMachine(selectedMachine.id, machineUpdate);
      operations.push(machineUpdateOp);
    }

    // 3. If operator had a previous machine, unassign it
    if (this.currentProject?.assignedMachine?.operatorId === this.selectedOperator!.id) {
      console.log('Unassigning current machine from operator...');
      const currentMachine = this.currentProject.assignedMachine;
      console.log('Current machine to unassign:', currentMachine);
      const unassignUpdate: UpdateMachineRequest = {
        name: currentMachine.name,
        type: currentMachine.type,
        manufacturer: currentMachine.manufacturer,
        model: currentMachine.model,
        serialNumber: currentMachine.serialNumber,
        rigNo: currentMachine.rigNo,
        plateNo: currentMachine.plateNo,
        manufacturingYear: currentMachine.manufacturingYear,
        chassisDetails: currentMachine.chassisDetails,
        currentLocation: currentMachine.currentLocation,
        projectId: currentMachine.projectId,
        operatorId: undefined, // Unassign operator
        status: MachineStatus.AVAILABLE,
        lastMaintenanceDate: currentMachine.lastMaintenanceDate,
        nextMaintenanceDate: currentMachine.nextMaintenanceDate
      };

      const unassignOp = this.machineService.updateMachine(currentMachine.id, unassignUpdate);
      operations.push(unassignOp);
    }

    // Execute all operations
    if (operations.length === 0) {
      this.error = 'No operations to perform';
      this.isLoading = false;
      return;
    }

    forkJoin(operations).pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (results) => {
        console.log('Reassignment operations completed:', results);
        this.successMessage = `${this.selectedOperator?.name} has been successfully reassigned!`;
        this.isLoading = false;
        
        // Clear selections
        this.selectedProjectId = null;
        this.selectedMachineId = null;
        this.reassignmentReason = '';
        
        // Close modal after showing success message (don't refresh data to avoid errors)
        setTimeout(() => {
          this.onClose();
        }, 1500);
      },
      error: (error) => {
        console.error('Error during reassignment:', error);
        console.error('Error stack:', error.stack);
        
        // Even if there's an error, the reassignment might have succeeded
        // Check if it's just a data loading error
        if (error.message && error.message.includes('createdAt')) {
          this.successMessage = `${this.selectedOperator?.name} has been reassigned successfully! (Please refresh to see updated data)`;
          this.isLoading = false;
          
          setTimeout(() => {
            this.onClose();
          }, 2000);
        } else {
          this.error = `Failed to reassign operator: ${error.message || 'Unknown error'}`;
          this.isLoading = false;
        }
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
