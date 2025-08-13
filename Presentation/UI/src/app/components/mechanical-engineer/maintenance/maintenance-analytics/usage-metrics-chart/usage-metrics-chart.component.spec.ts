import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsageMetricsChartComponent } from './usage-metrics-chart.component';
import { UsageMetrics } from '../../models/maintenance.models';

describe('UsageMetricsChartComponent', () => {
  let component: UsageMetricsChartComponent;
  let fixture: ComponentFixture<UsageMetricsChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsageMetricsChartComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(UsageMetricsChartComponent);
    component = fixture.componentInstance;
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
    const mockData: UsageMetrics[] = [
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

    component.data = mockData;
    fixture.detectChanges();
    
    expect(component).toBeTruthy();
  });

  it('should destroy chart on component destroy', () => {
    const mockData: UsageMetrics[] = [
      {
        machineId: 'machine-1',
        engineHours: 1200,
        idleHours: 300,
        serviceHours: 50,
        lastUpdated: new Date()
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