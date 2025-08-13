import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComponentRef } from '@angular/core';
import { MTBFMetricsComponent } from './mtbf-metrics.component';
import { MTBFMetrics } from '../../models/maintenance.models';

describe('MTBFMetricsComponent', () => {
  let component: MTBFMetricsComponent;
  let fixture: ComponentFixture<MTBFMetricsComponent>;

  const mockMTBFData: MTBFMetrics[] = [
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
    },
    {
      machineType: 'Drill Rig',
      mtbfHours: 320,
      periodMonths: 6,
      failureCount: 2
    },
    {
      machineType: 'Loader',
      mtbfHours: 210,
      periodMonths: 6,
      failureCount: 4
    }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MTBFMetricsComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(MTBFMetricsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle null data gracefully', () => {
    fixture.componentRef.setInput('data', null);
    fixture.detectChanges();

    expect(component).toBeTruthy();
    expect(component.sortedData()).toEqual([]);
    expect(component.averageMTBF()).toBe(0);
    expect(component.totalFailures()).toBe(0);
  });

  it('should handle empty data array gracefully', () => {
    fixture.componentRef.setInput('data', []);
    fixture.detectChanges();

    expect(component).toBeTruthy();
    expect(component.sortedData()).toEqual([]);
    expect(component.averageMTBF()).toBe(0);
    expect(component.totalFailures()).toBe(0);
  });

  it('should calculate correct statistics with valid data', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    // Average MTBF: (250 + 180 + 320 + 210) / 4 = 240
    expect(component.averageMTBF()).toBe(240);

    // Total failures: 3 + 5 + 2 + 4 = 14
    expect(component.totalFailures()).toBe(14);

    // Best performer should be Drill Rig with 320 hours
    expect(component.bestPerformer()?.machineType).toBe('Drill Rig');
    expect(component.bestPerformer()?.mtbfHours).toBe(320);

    // Data should be sorted by MTBF hours descending
    const sorted = component.sortedData();
    expect(sorted[0].machineType).toBe('Drill Rig');
    expect(sorted[1].machineType).toBe('Excavator');
    expect(sorted[2].machineType).toBe('Loader');
    expect(sorted[3].machineType).toBe('Bulldozer');
  });

  it('should calculate correct bar width percentages', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    // Max MTBF is 320 hours (Drill Rig)
    expect(component.getBarWidth(320)).toBe(100); // 320/320 * 100 = 100%
    expect(component.getBarWidth(250)).toBe(78.125); // 250/320 * 100 = 78.125%
    expect(component.getBarWidth(210)).toBe(65.625); // 210/320 * 100 = 65.625%
    expect(component.getBarWidth(180)).toBe(56.25); // 180/320 * 100 = 56.25%
  });

  it('should display no data message when data is empty', () => {
    fixture.componentRef.setInput('data', []);
    fixture.detectChanges();

    const noDataElement = fixture.nativeElement.querySelector('.no-data');
    expect(noDataElement).toBeTruthy();
    expect(noDataElement.textContent).toContain('No MTBF data available');
  });

  it('should display summary statistics when data is available', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    const summaryStats = fixture.nativeElement.querySelector('.summary-stats');
    expect(summaryStats).toBeTruthy();

    const statValues = fixture.nativeElement.querySelectorAll('.stat-value');
    expect(statValues.length).toBe(3);
    expect(statValues[0].textContent).toContain('240h'); // Average MTBF
    expect(statValues[1].textContent).toContain('Drill Rig'); // Best performer
    expect(statValues[2].textContent).toContain('14'); // Total failures
  });

  it('should display trend items for each machine type', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    const trendItems = fixture.nativeElement.querySelectorAll('.trend-item');
    expect(trendItems.length).toBe(4);

    // Check that machine types are displayed
    const machineTypes = Array.from(trendItems as NodeListOf<Element>).map(item =>
      item.querySelector('.machine-type')?.textContent?.trim()
    );
    expect(machineTypes).toContain('Excavator');
    expect(machineTypes).toContain('Bulldozer');
    expect(machineTypes).toContain('Drill Rig');
    expect(machineTypes).toContain('Loader');
  });

  it('should apply correct CSS classes based on MTBF performance', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    const trendItems = fixture.nativeElement.querySelectorAll('.trend-item');

    // Drill Rig (320h) and Excavator (250h) should have high-mtbf class (above 240h average)
    // Loader (210h) and Bulldozer (180h) should have low-mtbf class (below 240h average)
    const highMtbfItems = fixture.nativeElement.querySelectorAll('.trend-item.high-mtbf');
    const lowMtbfItems = fixture.nativeElement.querySelectorAll('.trend-item.low-mtbf');

    expect(highMtbfItems.length).toBe(2);
    expect(lowMtbfItems.length).toBe(2);
  });

  it('should destroy chart on component destroy', () => {
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    // Wait for chart creation
    setTimeout(() => {
      if (component['chart']) {
        spyOn(component['chart'], 'destroy');
        component.ngOnDestroy();
        expect(component['chart'].destroy).toHaveBeenCalled();
      }
    }, 100);
  });

  it('should handle single machine type data', () => {
    const singleMachineData: MTBFMetrics[] = [
      {
        machineType: 'Excavator',
        mtbfHours: 250,
        periodMonths: 6,
        failureCount: 3
      }
    ];

    fixture.componentRef.setInput('data', singleMachineData);
    fixture.detectChanges();

    expect(component.averageMTBF()).toBe(250);
    expect(component.totalFailures()).toBe(3);
    expect(component.bestPerformer()?.machineType).toBe('Excavator');
    expect(component.getBarWidth(250)).toBe(100);
  });

  it('should update computed values when data changes', () => {
    // Initial data
    fixture.componentRef.setInput('data', mockMTBFData);
    fixture.detectChanges();

    expect(component.averageMTBF()).toBe(240);
    expect(component.totalFailures()).toBe(14);

    // Updated data
    const updatedData: MTBFMetrics[] = [
      {
        machineType: 'Excavator',
        mtbfHours: 300,
        periodMonths: 6,
        failureCount: 2
      },
      {
        machineType: 'Bulldozer',
        mtbfHours: 200,
        periodMonths: 6,
        failureCount: 4
      }
    ];

    fixture.componentRef.setInput('data', updatedData);
    fixture.detectChanges();

    expect(component.averageMTBF()).toBe(250); // (300 + 200) / 2 = 250
    expect(component.totalFailures()).toBe(6); // 2 + 4 = 6
    expect(component.bestPerformer()?.machineType).toBe('Excavator');
  });
});