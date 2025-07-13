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

@Component({
  selector: 'app-edit-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './edit-machine.component.html',
  styleUrl: './edit-machine.component.scss'
})
export class EditMachineComponent implements OnInit {
  @Input() machine!: Machine;
  @Output() machineSaved = new EventEmitter<Machine>();
  @Output() close = new EventEmitter<void>();

  machineForm: FormGroup;
  isLoading = false;
  error: string | null = null;

  // Data arrays
  regions = REGIONS;
  availableProjects: Project[] = [];
  isLoadingProjects = false;

  // Enums for template
  MachineType = MachineType;
  MachineStatus = MachineStatus;

  constructor(
    private formBuilder: FormBuilder,
    private machineService: MachineService,
    private projectService: ProjectService
  ) {
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
    if (this.machine) {
      this.initializeForm();
    }

    // Watch for region changes to load projects
    this.machineForm.get('region')?.valueChanges.subscribe(regionValue => {
      this.onRegionChange(regionValue);
    });
  }

  private initializeForm(): void {
    // Parse current location to extract region and project
    const locationParts = this.parseLocation(this.machine.currentLocation);
    
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

    // If machine has a projectId, use it directly to set the project
    if (this.machine.projectId) {
      // First, we need to determine which region this project belongs to
      this.projectService.getAllProjects().subscribe({
        next: (projects) => {
          const machineProject = projects.find(p => p.id === this.machine.projectId);
          if (machineProject) {
            // Set the region based on the project
            this.machineForm.patchValue({ 
              region: machineProject.region,
              projectId: this.machine.projectId?.toString() || ''
            });
            
            // Load all projects for this region
            this.loadProjectsByRegion(machineProject.region);
          }
        },
        error: (error) => {
          console.error('Error loading project for machine:', error);
          // Fallback to location parsing
          this.initializeFromLocation(locationParts);
        }
      });
    } else {
      // Fallback to location parsing if no projectId
      this.initializeFromLocation(locationParts);
    }
  }

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

  onRegionChange(region: string): void {
    // Reset project selection when region changes
    this.machineForm.get('projectId')?.setValue('');
    this.availableProjects = [];

    if (region) {
      this.loadProjectsByRegion(region);
    }
  }

  private loadProjectsByRegion(region: string): Promise<void> {
    return new Promise((resolve) => {
      this.isLoadingProjects = true;
      
      this.projectService.getAllProjects().subscribe({
        next: (projects) => {
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

  private getLocationValue(): string {
    return this.locationPreview;
  }

  private getRegionId(): number | undefined {
    const regionName = this.machineForm.get('region')?.value;
    if (!regionName) return undefined;
    
    // Find the region ID based on the region name
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

  onSubmit(): void {
    if (this.machineForm.valid) {
      this.isLoading = true;
      this.error = null;

      const formValue = this.machineForm.value;
      
      // Check if project has changed
      const originalProjectId = this.machine.projectId;
      const newProjectId = formValue.projectId ? parseInt(formValue.projectId) : undefined;
      const projectChanged = originalProjectId !== newProjectId;
      
      if (projectChanged) {
        // If project changed, delete and recreate machine
        this.deleteAndRecreate(formValue);
      } else {
        // If project is the same, do normal update
        this.updateMachine(formValue);
      }
    } else {
      this.markFormGroupTouched();
    }
  }

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

  onCancel(): void {
    this.close.emit();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.machineForm.controls).forEach(key => {
      const control = this.machineForm.get(key);
      control?.markAsTouched();
    });
  }

  get machineTypeOptions() {
    return Object.values(MachineType);
  }

  get machineStatusOptions() {
    return Object.values(MachineStatus);
  }
} 