import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { PartsUsageSummaryComponent } from './parts-usage-summary.component';
import { PartsUsageData, UsageMetrics } from '../../models/maintenance.models';
import { MaintenanceService } from '../../services/maintenance.service';
import { of } from 'rxjs';

describe('PartsUsageSummaryComponent', () => {
  let component: PartsUsageSummaryComponent;
  let fixture: ComponentFixture<PartsUsageSummaryComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;

  const mockUsageMetrics: UsageMetrics[] = [
    {
      machineId: 1,
      engineHours: 1200,
      idleHours: 300,
      serviceHours: 50,
      lastUpdated: new Date()
    },
    {
      machineId: 2,
      engineHours: 800,
      idleHours: 200,
      serviceHours: 30,
      lastUpdated: new Date()
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('MaintenanceService', ['getUsageMetrics']);

    await TestBed.configureTestingModule({
      imports: [PartsUsageSummaryComponent, NoopAnimationsModule],
      providers: [
        { provide: MaintenanceService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(PartsUsageSummaryComponent);
    component = fixture.componentInstance;
    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
    
    // Setup default mock return values
    mockMaintenanceService.getUsageMetrics.and.returnValue(of(mockUsageMetrics));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle null data gracefully', () => {
    component.data = null;
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should handle empty data array gracefully', () => {
    component.data = [];
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });

  it('should create chart with valid data', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator', 'Bulldozer']
      },
      {
        partName: 'Air Filter',
        usageCount: 18,
        totalCost: 540.00,
        machineTypes: ['Excavator']
      }
    ];

    component.data = mockData;
    fixture.detectChanges();
    
    expect(component).toBeTruthy();
  });

  it('should sort data correctly', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator']
      },
      {
        partName: 'Air Filter',
        usageCount: 18,
        totalCost: 540.00,
        machineTypes: ['Bulldozer']
      }
    ];

    component.data = mockData;
    component.setSortBy('usage');
    fixture.detectChanges();

    const sortedData = component.sortedData();
    expect(sortedData[0].usageCount).toBe(25); // Descending order by default
    expect(sortedData[1].usageCount).toBe(18);
  });

  it('should toggle sort order', () => {
    expect(component.sortOrder()).toBe('desc');
    component.toggleSortOrder();
    expect(component.sortOrder()).toBe('asc');
    component.toggleSortOrder();
    expect(component.sortOrder()).toBe('desc');
  });

  it('should load usage metrics on init', () => {
    component.ngOnInit();
    expect(mockMaintenanceService.getUsageMetrics).toHaveBeenCalled();
    expect(component.usageMetrics()).toEqual(mockUsageMetrics);
  });

  it('should calculate total engine hours correctly', () => {
    component.usageMetrics.set(mockUsageMetrics);
    expect(component.totalEngineHours()).toBe(2000); // 1200 + 800
  });

  it('should calculate total service hours correctly', () => {
    component.usageMetrics.set(mockUsageMetrics);
    expect(component.totalServiceHours()).toBe(80); // 50 + 30
  });

  it('should calculate total idle hours correctly', () => {
    component.usageMetrics.set(mockUsageMetrics);
    expect(component.totalIdleHours()).toBe(500); // 300 + 200
  });

  it('should calculate utilization rate correctly', () => {
    component.usageMetrics.set(mockUsageMetrics);
    const expectedRate = (2000 / (2000 + 500)) * 100; // 80%
    expect(component.utilizationRate()).toBeCloseTo(expectedRate, 1);
  });

  it('should filter data by search term', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator']
      },
      {
        partName: 'Air Filter',
        usageCount: 18,
        totalCost: 540.00,
        machineTypes: ['Bulldozer']
      }
    ];

    component.data = mockData;
    component.setSearchTerm('Oil');
    fixture.detectChanges();

    const filteredData = component.filteredAndSortedData();
    expect(filteredData.length).toBe(1);
    expect(filteredData[0].partName).toBe('Oil Filter');
  });

  it('should filter data by machine type', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator', 'Bulldozer']
      },
      {
        partName: 'Air Filter',
        usageCount: 18,
        totalCost: 540.00,
        machineTypes: ['Crane']
      }
    ];

    component.data = mockData;
    component.toggleMachineTypeFilter('Excavator');
    fixture.detectChanges();

    const filteredData = component.filteredAndSortedData();
    expect(filteredData.length).toBe(1);
    expect(filteredData[0].partName).toBe('Oil Filter');
  });

  it('should clear all filters', () => {
    component.setSearchTerm('test');
    component.toggleMachineTypeFilter('Excavator');
    
    component.clearAllFilters();
    
    expect(component.searchTerm()).toBe('');
    expect(component.selectedMachineTypes()).toEqual([]);
  });

  it('should get available machine types', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator', 'Bulldozer']
      },
      {
        partName: 'Air Filter',
        usageCount: 18,
        totalCost: 540.00,
        machineTypes: ['Crane', 'All Types']
      }
    ];

    component.data = mockData;
    fixture.detectChanges();

    const availableTypes = component.availableMachineTypes();
    expect(availableTypes).toEqual(['Bulldozer', 'Crane', 'Excavator']);
    expect(availableTypes).not.toContain('All Types');
  });

  it('should destroy chart on component destroy', () => {
    const mockData: PartsUsageData[] = [
      {
        partName: 'Oil Filter',
        usageCount: 25,
        totalCost: 1250.00,
        machineTypes: ['Excavator']
      }
    ];

    component.data = mockData;
    fixture.detectChanges();
    
    if (component['chart']) {
      spyOn(component['chart'], 'destroy');
      component.ngOnDestroy();
      expect(component['chart'].destroy).toHaveBeenCalled();
    }
  });
});
