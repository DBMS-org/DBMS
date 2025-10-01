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
      status: [MachineStatus.AVAILABLE, Validators.required]
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
        projectId: formValue.projectId ? parseInt(formValue.projectId) : undefined,
        regionId: this.getRegionId(),
        status: formValue.status
      };
      
      // Submit machine creation request
      this.machineService.addMachine(request).subscribe({
        next: (machine: Machine) => {
          this.machineSaved.emit(machine);
          this.isLoading = false;
        },
        error: (error: any) => {
          this.error = 'Failed to create machine. Please try again.';
          this.isLoading = false;
          console.error('Error creating machine:', error);
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
   */
  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  /**
   * Provides machine status options for template dropdown
   */
  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
}