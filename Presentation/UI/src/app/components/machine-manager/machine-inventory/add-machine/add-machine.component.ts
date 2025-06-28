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

@Component({
  selector: 'app-add-machine',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './add-machine.component.html',
  styleUrl: './add-machine.component.scss'
})
export class AddMachineComponent implements OnInit {
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
      projectId: ['', Validators.required],
      status: [MachineStatus.AVAILABLE, Validators.required]
    });
  }

  ngOnInit(): void {
    // Watch for region changes to load projects
    this.machineForm.get('region')?.valueChanges.subscribe(regionValue => {
      this.onRegionChange(regionValue);
    });
  }

  onRegionChange(region: string): void {
    // Reset project selection when region changes
    this.machineForm.get('projectId')?.setValue('');
    this.availableProjects = [];

    if (region) {
      this.loadProjectsByRegion(region);
    }
  }

  private loadProjectsByRegion(region: string): void {
    this.isLoadingProjects = true;
    
    this.projectService.getAllProjects().subscribe({
      next: (projects) => {
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
    // Since we're using region names as strings, we need to map them to IDs
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
        projectId: parseInt(formValue.projectId),
        regionId: this.getRegionId(),
        status: formValue.status
      };
      
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
      this.markFormGroupTouched();
    }
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