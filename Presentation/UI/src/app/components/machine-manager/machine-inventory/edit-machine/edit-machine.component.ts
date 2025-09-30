import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MachineService } from '../../../../core/services/machine.service';
import { ProjectService } from '../../../../core/services/project.service';
import { 
  Machine, 
  UpdateMachineRequest,
  CreateMachineRequest,
  MachineType, 
  MachineStatus
} from '../../../../core/models/machine.model';
import { Project } from '../../../../core/models/project.model';
import { REGIONS } from '../../../../core/constants/regions';

/**
 * Edit Machine Component
 * 
 * Modal component for editing existing drilling machine records with comprehensive features:
 * - Pre-populated form with current machine data
 * - Dynamic region-project relationship management
 * - Location parsing and reconstruction from current machine location
 * - Smart project assignment handling (delete/recreate when project changes)
 * - Real-time form validation with visual feedback
 * - Cascading dropdowns for region and project selection
 * 
 * Handles complex scenarios like project reassignment which requires machine recreation
 * due to database constraints and maintains data integrity throughout the process.
 */
@Component({
  selector: 'app-edit-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './edit-machine.component.html',
  styleUrl: './edit-machine.component.scss'
})
export class EditMachineComponent implements OnInit {
  // Input/Output properties for parent-child communication
  @Input() machine!: Machine; // Machine data to edit
  @Output() machineSaved = new EventEmitter<Machine>(); // Emits updated machine data
  @Output() close = new EventEmitter<void>(); // Emits close modal request

  // Form management and loading states
  machineForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  // Dynamic data collections for dropdowns
  regions = REGIONS; // Static regions list from constants
  availableProjects: Project[] = []; // Projects filtered by selected region
  isLoadingProjects = false; // Loading state for project dropdown

  // Enum references for template usage
  MachineType = MachineType;
  MachineStatus = MachineStatus;

  constructor(
    private formBuilder: FormBuilder,
    private machineService: MachineService,
    private projectService: ProjectService
  ) {
    // Initialize reactive form with validation rules
    this.machineForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      type: [MachineType.DRILL_RIG, Validators.required],
      manufacturer: ['', [Validators.required, Validators.minLength(2)]],
      model: ['', [Validators.required, Validators.minLength(2)]],
      serialNumber: ['', [Validators.required, Validators.minLength(3)]],
      rigNo: [''],
      plateNo: [''],
      manufacturingYear: ['', [Validators.pattern(/^\d{4}$/)]],
      chassisDetails: [''],
      region: [''],
      projectId: [''],
      status: [MachineStatus.AVAILABLE, Validators.required]
    });
  }

  /**
   * Component initialization
   * Sets up form with machine data and establishes region-project relationship
   */
  ngOnInit(): void {
    if (this.machine) {
      this.initializeForm();
    }

    // Watch for region changes to dynamically load corresponding projects
    this.machineForm.get('region')?.valueChanges.subscribe(regionValue => {
      this.onRegionChange(regionValue);
    });
  }

  /**
   * Initialize form with existing machine data
   * Handles complex location parsing and project assignment logic
   */
  private initializeForm(): void {
    // Parse current location to extract region and project information
    const locationParts = this.parseLocation(this.machine.currentLocation);
    
    // Populate form with basic machine data
    this.machineForm.patchValue({
      name: this.machine.name,
      type: this.machine.type,
      manufacturer: this.machine.manufacturer,
      model: this.machine.model,
      serialNumber: this.machine.serialNumber,
      rigNo: this.machine.rigNo || '',
      plateNo: this.machine.plateNo || '',
      manufacturingYear: this.machine.manufacturingYear?.toString() || '',
      chassisDetails: this.machine.chassisDetails || '',
      region: locationParts.region || '',
      status: this.machine.status
    });

    // Handle project assignment - prefer projectId over location parsing
    if (this.machine.projectId) {
      // Load all projects to find the machine's current project and determine its region
      this.projectService.getAllProjects().subscribe({
        next: (projects) => {
          const machineProject = projects.find(p => p.id === this.machine.projectId);
          if (machineProject) {
            // Set region and project based on current assignment
            this.machineForm.patchValue({ 
              region: machineProject.region,
              projectId: this.machine.projectId?.toString() || ''
            });
            
            // Load all projects for this region to populate dropdown
            this.loadProjectsByRegion(machineProject.region);
          }
        },
        error: (error) => {
          console.error('Error loading project for machine:', error);
          // Fallback to location parsing if project loading fails
          this.initializeFromLocation(locationParts);
        }
      });
    } else {
      // Use location parsing if no direct project assignment
      this.initializeFromLocation(locationParts);
    }
  }

  /**
   * Initialize form using parsed location data as fallback
   * Used when machine has no direct project assignment
   */
  private initializeFromLocation(locationParts: { region?: string; projectName?: string }): void {
    // Load projects for the current region if it exists
    if (locationParts.region) {
      this.loadProjectsByRegion(locationParts.region).then(() => {
        // Set project after projects are loaded
        if (locationParts.projectName) {
          const project = this.availableProjects.find(p => 
            p.name.toLowerCase() === locationParts.projectName?.toLowerCase()
          );
          if (project) {
            this.machineForm.patchValue({ projectId: project.id.toString() });
          }
        }
      });
    }
  }

  /**
   * Parse machine location string to extract region and project information
   * Handles various location formats: "Region - Project", "Region", or fallback
   */
  private parseLocation(location: string | undefined): { region?: string; projectName?: string } {
    if (!location || location === 'Default Location') {
      return {};
    }

    // Check if location contains " - " separator (region - project format)
    if (location.includes(' - ')) {
      const parts = location.split(' - ');
      return {
        region: parts[0].trim(),
        projectName: parts[1].trim()
      };
    }

    // Check if location matches any region exactly
    const matchingRegion = this.regions.find(r => 
      r.toLowerCase() === location.toLowerCase()
    );
    
    if (matchingRegion) {
      return { region: matchingRegion };
    }

    return {};
  }

  /**
   * Handle region selection change
   * Resets project selection and loads projects for the new region
   */
  onRegionChange(region: string): void {
    // Reset project selection when region changes
    this.machineForm.get('projectId')?.setValue('');
    this.availableProjects = [];

    if (region) {
      this.loadProjectsByRegion(region);
    }
  }

  /**
   * Load projects filtered by region
   * Returns promise for synchronization with form initialization
   */
  private loadProjectsByRegion(region: string): Promise<void> {
    return new Promise((resolve) => {
      this.isLoadingProjects = true;
      
      this.projectService.getAllProjects().subscribe({
        next: (projects) => {
          // Filter projects by selected region (case-insensitive)
          this.availableProjects = projects.filter(project => 
            project.region && project.region.toLowerCase() === region.toLowerCase()
          );
          this.isLoadingProjects = false;
          resolve();
        },
        error: (error) => {
          console.error('Error loading projects:', error);
          this.availableProjects = [];
          this.isLoadingProjects = false;
          resolve();
        }
      });
    });
  }

  /**
   * Generate location preview string for display
   * Combines region and project name in standardized format
   */
  get locationPreview(): string {
    const region = this.machineForm.get('region')?.value;
    const projectId = this.machineForm.get('projectId')?.value;
    
    if (!region) {
      return 'Default Location';
    }
    
    if (projectId) {
      const selectedProject = this.availableProjects.find(p => p.id == projectId);
      return selectedProject ? `${region} - ${selectedProject.name}` : region;
    }
    
    return region;
  }

  /**
   * Get the final location value for machine record
   */
  private getLocationValue(): string {
    return this.locationPreview;
  }

  /**
   * Map region name to region ID for database storage
   * Uses predefined mapping for Oman regions
   */
  private getRegionId(): number | undefined {
    const regionName = this.machineForm.get('region')?.value;
    if (!regionName) return undefined;
    
    // Static mapping of region names to IDs
    const regionMapping: { [key: string]: number } = {
      'Muscat': 1,
      'Dhofar': 2,
      'Musandam': 3,
      'Al Buraimi': 4,
      'Al Dakhiliyah': 5,
      'Al Dhahirah': 6,
      'Al Wusta': 7,
      'Al Batinah North': 8,
      'Al Batinah South': 9,
      'Ash Sharqiyah North': 10,
      'Ash Sharqiyah South': 11
    };
    
    return regionMapping[regionName];
  }

  /**
   * Handle form submission
   * Determines whether to update or delete/recreate based on project changes
   */
  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.machineForm.value;
      
      // Check if project assignment has changed
      const originalProjectId = this.machine.projectId;
      const newProjectId = formValue.projectId ? parseInt(formValue.projectId) : undefined;
      const projectChanged = originalProjectId !== newProjectId;
      
      if (projectChanged) {
        // Project change requires delete/recreate due to database constraints
        this.deleteAndRecreate(formValue);
      } else {
        // Standard update for same project
        this.updateMachine(formValue);
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  /**
   * Perform standard machine update
   * Used when project assignment hasn't changed
   */
  private updateMachine(formValue: any): void {
    const request: UpdateMachineRequest = {
      name: formValue.name,
      type: formValue.type,
      manufacturer: formValue.manufacturer,
      model: formValue.model,
      serialNumber: formValue.serialNumber,
      rigNo: formValue.rigNo || undefined,
      plateNo: formValue.plateNo || undefined,
      manufacturingYear: formValue.manufacturingYear ? parseInt(formValue.manufacturingYear) : undefined,
      chassisDetails: formValue.chassisDetails || undefined,
      currentLocation: this.getLocationValue(),
      projectId: formValue.projectId ? parseInt(formValue.projectId) : undefined,
      regionId: this.getRegionId(),
      status: formValue.status,
      lastMaintenanceDate: this.machine.lastMaintenanceDate,
      nextMaintenanceDate: this.machine.nextMaintenanceDate
    };
    
    this.machineService.updateMachine(this.machine.id, request).subscribe({
      next: (machine: Machine) => {
        this.machineSaved.emit(machine);
        this.isLoading = false;
      },
      error: (error: any) => {
        this.error = 'Failed to update machine. Please try again.';
        this.isLoading = false;
        console.error('Error updating machine:', error);
      }
    });
  }

  /**
   * Delete existing machine and recreate with new data
   * Required when project assignment changes due to database constraints
   */
  private deleteAndRecreate(formValue: any): void {
    // First delete the existing machine
    this.machineService.deleteMachine(this.machine.id).subscribe({
      next: () => {
        // Then create a new machine with updated information
        const createRequest: CreateMachineRequest = {
          name: formValue.name,
          type: formValue.type,
          manufacturer: formValue.manufacturer,
          model: formValue.model,
          serialNumber: formValue.serialNumber,
          rigNo: formValue.rigNo || undefined,
          plateNo: formValue.plateNo || undefined,
          manufacturingYear: formValue.manufacturingYear ? parseInt(formValue.manufacturingYear) : undefined,
          chassisDetails: formValue.chassisDetails || undefined,
          currentLocation: this.getLocationValue(),
          projectId: formValue.projectId ? parseInt(formValue.projectId) : undefined,
          regionId: this.getRegionId(),
          status: formValue.status
        };

        this.machineService.addMachine(createRequest).subscribe({
          next: (newMachine: Machine) => {
            this.machineSaved.emit(newMachine);
            this.isLoading = false;
          },
          error: (error: any) => {
            this.error = 'Failed to update machine. Please try again.';
            this.isLoading = false;
            console.error('Error recreating machine:', error);
          }
        });
      },
      error: (error: any) => {
        this.error = 'Failed to update machine. Please try again.';
        this.isLoading = false;
        console.error('Error deleting machine:', error);
      }
    });
  }

  /**
   * Handle modal close request
   */
  onCancel(): void {
    this.close.emit();
  }

  /**
   * Mark all form controls as touched to trigger validation display
   */
  private markFormGroupTouched(): void {
    Object.keys(this.machineForm.controls).forEach(key => {
      const control = this.machineForm.get(key);
      control?.markAsTouched();
    });
  }

  // Getter methods for template dropdown options
  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
}