import { Component, EventEmitter, Output, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MachineService } from '../../../../core/services/machine.service';
import { ProjectService } from '../../../../core/services/project.service';
import { 
  Machine, 
  CreateMachineRequest,
  MachineType, 
  MachineStatus
} from '../../../../core/models/machine.model';
import { Project } from '../../../../core/models/project.model';
import { REGIONS } from '../../../../core/constants/regions';

/**
 * Add Machine Component
 * 
 * Modal component for creating new drilling machines in the inventory system.
 * Features include:
 * - Comprehensive form validation for all machine properties
 * - Dynamic region-based project loading and assignment
 * - Real-time location preview based on region and project selection
 * - Automatic region ID mapping from region names
 * - Form state management with loading and error handling
 * - Integration with machine and project services for data persistence
 */
@Component({
  selector: 'app-add-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-machine.component.html',
  styleUrl: './add-machine.component.scss'
})
export class AddMachineComponent implements OnInit {
  // Event emitters for parent component communication
  @Output() machineSaved = new EventEmitter<Machine>();
  @Output() close = new EventEmitter<void>();

  // Form management and state
  machineForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  // Data arrays for dropdowns and selections
  regions = REGIONS;
  availableProjects: Project[] = [];
  isLoadingProjects = false;

  // Enum references for template usage
  MachineType = MachineType;
  MachineStatus = MachineStatus;

  constructor(
    private formBuilder: FormBuilder,
    private machineService: MachineService,
    private projectService: ProjectService
  ) {
    // Initialize reactive form with comprehensive validation rules
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
      // Service Configuration
      engineServiceInterval: [500, [Validators.required, Validators.min(1)]],
      currentEngineServiceHours: [0, [Validators.required, Validators.min(0)]],
      lastEngineServiceDate: [''],
      drifterServiceInterval: [300, [Validators.min(1)]],
      currentDrifterServiceHours: [0, [Validators.min(0)]],
      lastDrifterServiceDate: ['']
    });
  }

  ngOnInit(): void {
    // Set up reactive form subscriptions for dynamic behavior
    this.machineForm.get('region')?.valueChanges.subscribe(regionValue => {
      this.onRegionChange(regionValue);
    });
  }

  /**
   * Handles region selection changes and loads associated projects
   * Resets project selection when region changes to maintain data consistency
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
   * Loads projects filtered by the selected region
   * Provides dynamic project options based on regional assignments
   */
  private loadProjectsByRegion(region: string): void {
    this.isLoadingProjects = true;
    
    this.projectService.getAllProjects().subscribe({
      next: (projects) => {
        // Filter projects by region with case-insensitive matching
        this.availableProjects = projects.filter(project => 
          project.region && project.region.toLowerCase() === region.toLowerCase()
        );
        this.isLoadingProjects = false;
      },
      error: (error) => {
        console.error('Error loading projects:', error);
        this.availableProjects = [];
        this.isLoadingProjects = false;
      }
    });
  }

  /**
   * Generates a human-readable location preview string
   * Combines region and project information for user clarity
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
   * Returns the formatted location string for database storage
   */
  private getLocationValue(): string {
    return this.locationPreview;
  }

  /**
   * Maps region names to their corresponding database IDs
   * Ensures proper foreign key relationships in the database
   */
  private getRegionId(): number | undefined {
    const regionName = this.machineForm.get('region')?.value;
    if (!regionName) return undefined;
    
    // Static mapping of region names to database IDs
    // This should ideally come from a service or configuration
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
   * Handles form submission and machine creation
   * Validates form data, transforms it to API format, and calls machine service
   */
  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.machineForm.value;
      const projectId = formValue.projectId ? parseInt(formValue.projectId) : undefined;

      // Automatically determine status based on project assignment
      // If project is selected → ASSIGNED, otherwise → AVAILABLE
      const status = projectId ? MachineStatus.ASSIGNED : MachineStatus.AVAILABLE;

      // Transform form data to API request format
      const request: CreateMachineRequest = {
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
        projectId: projectId,
        regionId: this.getRegionId(),
        status: status,
        // Service Configuration
        engineServiceInterval: formValue.engineServiceInterval,
        currentEngineServiceHours: formValue.currentEngineServiceHours,
        lastEngineServiceDate: formValue.lastEngineServiceDate || undefined,
        drifterServiceInterval: formValue.drifterServiceInterval || undefined,
        currentDrifterServiceHours: formValue.currentDrifterServiceHours || undefined,
        lastDrifterServiceDate: formValue.lastDrifterServiceDate || undefined
      };
      
      // Submit machine creation request
      this.machineService.addMachine(request).subscribe({
        next: (machine: Machine) => {
          this.machineSaved.emit(machine);
          this.isLoading = false;
        },
        error: (error: any) => {
          // Handle different error types
          console.error('Error creating machine:', error);

          // Extract message from ApiResponse format
          // Backend returns { success: false, message: "...", statusCode: 409 }
          let message = 'An error occurred';

          if (error.error) {
            if (typeof error.error === 'string') {
              message = error.error;
            } else if (error.error.message) {
              // ApiResponse format has a 'message' property (capital M)
              message = error.error.message;
            } else if (error.error.Message) {
              // Try capital M in case of different casing
              message = error.error.Message;
            }
          } else if (error.message) {
            message = error.message;
          }

          // Set specific messages based on status code
          if (error.status === 409) {
            this.error = message;
          } else if (error.status === 400) {
            this.error = message;
          } else {
            this.error = message;
          }

          this.isLoading = false;
        }
      });
    } else {
      // Mark all fields as touched to show validation errors
      this.markFormGroupTouched();
    }
  }

  /**
   * Handles modal cancellation and cleanup
   */
  onCancel(): void {
    this.close.emit();
  }

  /**
   * Marks all form controls as touched to trigger validation display
   * Used when form is submitted with invalid data
   */
  private markFormGroupTouched(): void {
    Object.keys(this.machineForm.controls).forEach(key => {
      const control = this.machineForm.get(key);
      control?.markAsTouched();
    });
  }

  /**
   * Provides machine type options for template dropdown
   * Restricted to Drill Rig only for machine manager inventory
   */
  get machineTypeOptions() {
    return [MachineType.DRILL_RIG];
  }

  /**
   * Returns true if the selected machine type is a Drill Rig
   */
  get isDrillRig(): boolean {
    return this.machineForm.get('type')?.value === MachineType.DRILL_RIG;
  }

  /**
   * Calculates hours remaining until next engine service
   */
  get engineHoursToNextService(): number {
    const interval = this.machineForm.get('engineServiceInterval')?.value || 500;
    const current = this.machineForm.get('currentEngineServiceHours')?.value || 0;
    return Math.max(0, interval - current);
  }

  /**
   * Calculates hours remaining until next drifter service
   */
  get drifterHoursToNextService(): number {
    const interval = this.machineForm.get('drifterServiceInterval')?.value || 300;
    const current = this.machineForm.get('currentDrifterServiceHours')?.value || 0;
    return Math.max(0, interval - current);
  }

  /**
   * Returns CSS class for engine service urgency color coding
   */
  get engineServiceUrgencyClass(): string {
    const remaining = this.engineHoursToNextService;
    if (remaining < 0) return 'overdue';
    if (remaining < 20) return 'critical';
    if (remaining < 50) return 'warning';
    if (remaining < 100) return 'caution';
    return 'good';
  }

  /**
   * Returns CSS class for drifter service urgency color coding
   */
  get drifterServiceUrgencyClass(): string {
    const remaining = this.drifterHoursToNextService;
    if (remaining < 0) return 'overdue';
    if (remaining < 20) return 'critical';
    if (remaining < 50) return 'warning';
    if (remaining < 100) return 'caution';
    return 'good';
  }

  /**
   * Provides machine status options for template dropdown
   */
  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
}