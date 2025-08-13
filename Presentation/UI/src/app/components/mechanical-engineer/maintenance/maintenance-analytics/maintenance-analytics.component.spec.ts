import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { MaintenanceAnalyticsComponent } from './maintenance-analytics.component';
import { MaintenanceService } from '../services/maintenance.service';
import { ServiceComplianceData, MTBFMetrics, PartsUsageData, UsageMetrics } from '../models/maintenance.models';

describe('MaintenanceAnalyticsComponent', () => {
  let component: MaintenanceAnalyticsComponent;
  let fixture: ComponentFixture<MaintenanceAnalyticsComponent>;
  let mockMaintenanceService: jasmine.SpyObj<MaintenanceService>;

  const mockServiceComplianceData: ServiceComplianceData = {
    onTime: 85,
    overdue: 15,
    percentage: 85
  };

  const mockMTBFMetrics: MTBFMetrics[] = [
    {
      machineType: 'Excavator',
      mtbfHours: 250,
      periodMonths: 6,
      failureCount: 3
    },
    {
      machineType: 'Bulldozer',
      mtbfHours: 180,
      periodMonths: 6,
      failureCount: 5
    }
  ];

  const mockPartsUsageData: PartsUsageData[] = [
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

  const mockUsageMetrics: UsageMetrics[] = [
    {
      machineId: 'machine-1',
      engineHours: 1200,
      idleHours: 300,
      serviceHours: 50,
      lastUpdated: new Date()
    },
    {
      machineId: 'machine-2',
      engineHours: 800,
      idleHours: 200,
      serviceHours: 30,
      lastUpdated: new Date()
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('MaintenanceService', [
      'getServiceComplianceData',
      'getMTBFMetrics',
      'getPartsUsageData',
      'getUsageMetrics'
    ]);

    await TestBed.configureTestingModule({
      imports: [MaintenanceAnalyticsComponent, NoopAnimationsModule],
      providers: [
        { provide: MaintenanceService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(MaintenanceAnalyticsComponent);
    component = fixture.componentInstance;
    mockMaintenanceService = TestBed.inject(MaintenanceService) as jasmine.SpyObj<MaintenanceService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load analytics data on init', async () => {
    mockMaintenanceService.getServiceComplianceData.and.returnValue(of(mockServiceComplianceData));
    mockMaintenanceService.getMTBFMetrics.and.returnValue(of(mockMTBFMetrics));
    mockMaintenanceService.getPartsUsageData.and.returnValue(of(mockPartsUsageData));
    mockMaintenanceService.getUsageMetrics.and.returnValue(of(mockUsageMetrics));

    component.ngOnInit();
    await fixture.whenStable();

    expect(component.isLoading()).toBeFalse();
    expect(component.serviceComplianceData()).toEqual(mockServiceComplianceData);
    expect(component.mtbfMetrics()).toEqual(mockMTBFMetrics);
    expect(component.partsUsageData()).toEqual(mockPartsUsageData);
    expect(component.usageMetrics()).toEqual(mockUsageMetrics);
  });

  it('should handle loading state correctly', () => {
    mockMaintenanceService.getServiceComplianceData.and.returnValue(of(mockServiceComplianceData));
    mockMaintenanceService.getMTBFMetrics.and.returnValue(of(mockMTBFMetrics));
    mockMaintenanceService.getPartsUsageData.and.returnValue(of(mockPartsUsageData));
    mockMaintenanceService.getUsageMetrics.and.returnValue(of(mockUsageMetrics));

    expect(component.isLoading()).toBeTrue();
    
    component.ngOnInit();
    
    expect(component.isLoading()).toBeTrue();
  });

  it('should handle errors gracefully', async () => {
    mockMaintenanceService.getServiceComplianceData.and.returnValue(throwError(() => new Error('Service error')));
    mockMaintenanceService.getMTBFMetrics.and.returnValue(throwError(() => new Error('Service error')));
    mockMaintenanceService.getPartsUsageData.and.returnValue(throwError(() => new Error('Service error')));
    mockMaintenanceService.getUsageMetrics.and.returnValue(throwError(() => new Error('Service error')));

    component.ngOnInit();
    await fixture.whenStable();

    expect(component.isLoading()).toBeFalse();
    expect(component.hasError()).toBeTrue();
    expect(component.errorMessage()).toContain('Failed to load analytics data');
  });

  it('should calculate total hours correctly', () => {
    component['usageMetrics'].set(mockUsageMetrics);

    expect(component.totalEngineHours()).toBe(2000); // 1200 + 800
    expect(component.totalServiceHours()).toBe(80);  // 50 + 30
    expect(component.totalIdleHours()).toBe(500);    // 300 + 200
  });

  it('should get top parts usage correctly', () => {
    component['partsUsageData'].set(mockPartsUsageData);

    const topParts = component.topPartsUsage();
    expect(topParts.length).toBe(2);
    expect(topParts[0].partName).toBe('Oil Filter');
    expect(topParts[1].partName).toBe('Air Filter');
  });

  it('should refresh data when refreshData is called', () => {
    mockMaintenanceService.getServiceComplianceData.and.returnValue(of(mockServiceComplianceData));
    mockMaintenanceService.getMTBFMetrics.and.returnValue(of(mockMTBFMetrics));
    mockMaintenanceService.getPartsUsageData.and.returnValue(of(mockPartsUsageData));
    mockMaintenanceService.getUsageMetrics.and.returnValue(of(mockUsageMetrics));

    component.refreshData();

    expect(mockMaintenanceService.getServiceComplianceData).toHaveBeenCalled();
    expect(mockMaintenanceService.getMTBFMetrics).toHaveBeenCalled();
    expect(mockMaintenanceService.getPartsUsageData).toHaveBeenCalled();
    expect(mockMaintenanceService.getUsageMetrics).toHaveBeenCalled();
  });

  it('should handle partial data loading', async () => {
    mockMaintenanceService.getServiceComplianceData.and.returnValue(of(mockServiceComplianceData));
    mockMaintenanceService.getMTBFMetrics.and.returnValue(throwError(() => new Error('MTBF error')));
    mockMaintenanceService.getPartsUsageData.and.returnValue(of(mockPartsUsageData));
    mockMaintenanceService.getUsageMetrics.and.returnValue(of(mockUsageMetrics));

    component.ngOnInit();
    await fixture.whenStable();

    expect(component.isLoading()).toBeFalse();
    expect(component.hasError()).toBeFalse();
    expect(component.serviceComplianceData()).toEqual(mockServiceComplianceData);
    expect(component.mtbfMetrics()).toBeNull();
    expect(component.partsUsageData()).toEqual(mockPartsUsageData);
    expect(component.usageMetrics()).toEqual(mockUsageMetrics);
  });
});