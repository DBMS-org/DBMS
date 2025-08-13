import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

import { JobFiltersComponent } from './job-filters.component';
import { MaintenanceJob, MaintenanceStatus, MaintenanceType, JobFilters } from '../../models/maintenance.models';

describe('JobFiltersComponent', () => {
  let component: JobFiltersComponent;
  let fixture: ComponentFixture<JobFiltersComponent>;

  const mockJobs: MaintenanceJob[] = [
    {
      id: '1',
      machineId: 'M001',
      machineName: 'Excavator 1',
      serialNumber: 'EX001',
      project: 'Project A',
      scheduledDate: new Date('2024-01-15T10:00:00'),
      type: MaintenanceType.PREVENTIVE,
      status: MaintenanceStatus.SCHEDULED,
      assignedTo: ['John Doe'],
      estimatedHours: 4,
      reason: 'Regular maintenance',
      createdAt: new Date('2024-01-01'),
      updatedAt: new Date('2024-01-01')
    },
    {
      id: '2',
      machineId: 'M002',
      machineName: 'Bulldozer 1',
      serialNumber: 'BD001',
      project: 'Project B',
      scheduledDate: new Date('2024-01-16T14:00:00'),
      type: MaintenanceType.CORRECTIVE,
      status: MaintenanceStatus.IN_PROGRESS,
      assignedTo: ['Jane Smith', 'Bob Johnson'],
      estimatedHours: 6,
      reason: 'Engine repair',
      createdAt: new Date('2024-01-02'),
      updatedAt: new Date('2024-01-02')
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobFiltersComponent, NoopAnimationsModule, ReactiveFormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(JobFiltersComponent);
    component = fixture.componentInstance;
    
    // Set input signal
    fixture.componentRef.setInput('jobs', mockJobs);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty filters', () => {
    fixture.detectChanges();
    expect(component.activeFiltersCount()).toBe(0);
    expect(component.filterForm.get('searchTerm')?.value).toBe('');
  });

  it('should initialize with provided initial filters', () => {
    const initialFilters: JobFilters = {
      searchTerm: 'test',
      status: [MaintenanceStatus.SCHEDULED]
    };
    
    fixture.componentRef.setInput('initialFilters', initialFilters);
    fixture.detectChanges();

    expect(component.filterForm.get('searchTerm')?.value).toBe('test');
    expect(component.filterForm.get('status')?.value).toEqual([MaintenanceStatus.SCHEDULED]);
  });

  it('should update active filters count when form changes', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      searchTerm: 'test',
      status: [MaintenanceStatus.SCHEDULED]
    });

    expect(component.activeFiltersCount()).toBe(2);
  });

  it('should extract unique machine types from jobs', () => {
    fixture.detectChanges();
    
    const machineTypes = component.availableMachineTypes();
    expect(machineTypes).toContain('Excavator');
    expect(machineTypes).toContain('Bulldozer');
  });

  it('should extract unique projects from jobs', () => {
    fixture.detectChanges();
    
    const projects = component.availableProjects();
    expect(projects).toContain('Project A');
    expect(projects).toContain('Project B');
  });

  it('should extract unique technicians from jobs', () => {
    fixture.detectChanges();
    
    const technicians = component.availableTechnicians();
    expect(technicians).toContain('John Doe');
    expect(technicians).toContain('Jane Smith');
    expect(technicians).toContain('Bob Johnson');
  });

  it('should emit searchChanged when search input changes', (done) => {
    fixture.detectChanges();
    
    component.searchChanged.subscribe(searchTerm => {
      expect(searchTerm).toBe('test search');
      done();
    });

    component.filterForm.patchValue({ searchTerm: 'test search' });
  });

  it('should emit filtersChanged when applyFilters is called', (done) => {
    fixture.detectChanges();
    
    component.filtersChanged.subscribe(filters => {
      expect(filters.searchTerm).toBe('test');
      expect(filters.status).toEqual([MaintenanceStatus.SCHEDULED]);
      done();
    });

    component.filterForm.patchValue({
      searchTerm: 'test',
      status: [MaintenanceStatus.SCHEDULED]
    });
    
    component.applyFilters();
  });

  it('should clear search when clearSearch is called', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({ searchTerm: 'test' });
    component.clearSearch();
    
    expect(component.filterForm.get('searchTerm')?.value).toBe('');
  });

  it('should remove specific filter when removeFilter is called', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      searchTerm: 'test',
      status: [MaintenanceStatus.SCHEDULED]
    });
    
    component.removeFilter('searchTerm');
    
    expect(component.filterForm.get('searchTerm')?.value).toBe('');
    expect(component.filterForm.get('status')?.value).toEqual([MaintenanceStatus.SCHEDULED]);
  });

  it('should reset all filters when resetFilters is called', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      searchTerm: 'test',
      status: [MaintenanceStatus.SCHEDULED],
      machineType: ['Excavator']
    });
    
    component.resetFilters();
    
    expect(component.filterForm.get('searchTerm')?.value).toBe('');
    expect(component.filterForm.get('status')?.value).toEqual([]);
    expect(component.filterForm.get('machineType')?.value).toEqual([]);
  });

  it('should return correct filter description', () => {
    fixture.detectChanges();
    
    expect(component.getFilterDescription()).toBe('No filters applied');
    
    component.filterForm.patchValue({ searchTerm: 'test' });
    component['updateActiveFiltersCount']();
    
    expect(component.getFilterDescription()).toBe('1 filter applied');
    
    component.filterForm.patchValue({ status: [MaintenanceStatus.SCHEDULED] });
    component['updateActiveFiltersCount']();
    
    expect(component.getFilterDescription()).toBe('2 filters applied');
  });

  it('should return correct status icon', () => {
    expect(component.getStatusIcon(MaintenanceStatus.SCHEDULED)).toBe('schedule');
    expect(component.getStatusIcon(MaintenanceStatus.IN_PROGRESS)).toBe('play_circle');
    expect(component.getStatusIcon(MaintenanceStatus.COMPLETED)).toBe('check_circle');
  });

  it('should return correct status icon class', () => {
    expect(component.getStatusIconClass(MaintenanceStatus.IN_PROGRESS)).toBe('icon-in-progress');
    expect(component.getStatusIconClass(MaintenanceStatus.OVERDUE)).toBe('icon-overdue');
  });

  it('should format date range text correctly', () => {
    fixture.detectChanges();
    
    const startDate = new Date('2024-01-01');
    const endDate = new Date('2024-01-31');
    
    component.filterForm.patchValue({
      startDate: startDate,
      endDate: endDate
    });
    
    const dateRangeText = component.getDateRangeText();
    expect(dateRangeText).toContain('1/1/2024');
    expect(dateRangeText).toContain('1/31/2024');
  });

  it('should format status text correctly', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      status: [MaintenanceStatus.SCHEDULED, MaintenanceStatus.IN_PROGRESS]
    });
    
    const statusText = component.getStatusText();
    expect(statusText).toBe('Scheduled, In Progress');
  });

  it('should format machine type text correctly', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      machineType: ['Excavator', 'Bulldozer']
    });
    
    const machineTypeText = component.getMachineTypeText();
    expect(machineTypeText).toBe('Excavator, Bulldozer');
  });

  it('should format project text correctly', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      project: ['Project A']
    });
    
    const projectText = component.getProjectText();
    expect(projectText).toBe('Project A');
  });

  it('should format assigned to text correctly', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      assignedTo: ['John Doe', 'Jane Smith']
    });
    
    const assignedToText = component.getAssignedToText();
    expect(assignedToText).toBe('John Doe, Jane Smith');
  });

  it('should handle multiple items in filter text formatting', () => {
    fixture.detectChanges();
    
    component.filterForm.patchValue({
      status: [MaintenanceStatus.SCHEDULED, MaintenanceStatus.IN_PROGRESS, MaintenanceStatus.COMPLETED]
    });
    
    const statusText = component.getStatusText();
    expect(statusText).toBe('3 statuses');
  });
});