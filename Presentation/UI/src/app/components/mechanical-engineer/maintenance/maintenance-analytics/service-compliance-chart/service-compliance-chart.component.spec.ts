import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServiceComplianceChartComponent } from './service-compliance-chart.component';
import { ServiceComplianceData } from '../../models/maintenance.models';
import { MatIconModule } from '@angular/material/icon';
import { DebugElement } from '@angular/core';
import { By } from '@angular/platform-browser';

describe('ServiceComplianceChartComponent', () => {
  let component: ServiceComplianceChartComponent;
  let fixture: ComponentFixture<ServiceComplianceChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceComplianceChartComponent, MatIconModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceComplianceChartComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Insufficient Data Scenarios', () => {
    it('should show insufficient data message when data is null', () => {
      component.data = null;
      fixture.detectChanges();
      
      const insufficientDataElement = fixture.debugElement.query(By.css('.insufficient-data'));
      expect(insufficientDataElement).toBeTruthy();
      
      const messageElement = insufficientDataElement.query(By.css('p'));
      expect(messageElement.nativeElement.textContent).toContain('No service compliance data available');
    });

    it('should show insufficient data message when total jobs < 5', () => {
      const mockData: ServiceComplianceData = {
        onTime: 2,
        overdue: 1,
        percentage: 67
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const insufficientDataElement = fixture.debugElement.query(By.css('.insufficient-data'));
      expect(insufficientDataElement).toBeTruthy();
      
      const messageElement = insufficientDataElement.query(By.css('p'));
      expect(messageElement.nativeElement.textContent).toContain('Only 3 maintenance jobs found');
    });

    it('should show insufficient data message when no jobs exist', () => {
      const mockData: ServiceComplianceData = {
        onTime: 0,
        overdue: 0,
        percentage: 0
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const insufficientDataElement = fixture.debugElement.query(By.css('.insufficient-data'));
      expect(insufficientDataElement).toBeTruthy();
      
      const messageElement = insufficientDataElement.query(By.css('p'));
      expect(messageElement.nativeElement.textContent).toContain('No completed or overdue maintenance jobs found');
    });

    it('should display data requirements when insufficient data', () => {
      component.data = null;
      fixture.detectChanges();
      
      const requirementsElement = fixture.debugElement.query(By.css('.data-requirements'));
      expect(requirementsElement).toBeTruthy();
      
      const listItems = requirementsElement.queryAll(By.css('li'));
      expect(listItems.length).toBe(3);
      expect(listItems[0].nativeElement.textContent).toContain('At least 5 completed or overdue maintenance jobs');
    });
  });

  describe('Sufficient Data Scenarios', () => {
    it('should create chart with sufficient valid data', () => {
      const mockData: ServiceComplianceData = {
        onTime: 8,
        overdue: 2,
        percentage: 80
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const chartContainer = fixture.debugElement.query(By.css('.chart-container'));
      expect(chartContainer).toBeTruthy();
      
      const insufficientDataElement = fixture.debugElement.query(By.css('.insufficient-data'));
      expect(insufficientDataElement).toBeFalsy();
    });

    it('should display compliance summary with correct values', () => {
      const mockData: ServiceComplianceData = {
        onTime: 15,
        overdue: 5,
        percentage: 75
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const percentageValue = fixture.debugElement.query(By.css('.percentage-value'));
      expect(percentageValue.nativeElement.textContent.trim()).toBe('75%');
      
      const onTimeCount = fixture.debugElement.query(By.css('.breakdown-item.on-time .count'));
      expect(onTimeCount.nativeElement.textContent.trim()).toBe('15');
      
      const overdueCount = fixture.debugElement.query(By.css('.breakdown-item.overdue .count'));
      expect(overdueCount.nativeElement.textContent.trim()).toBe('5');
    });

    it('should apply good compliance styling for high percentage', () => {
      const mockData: ServiceComplianceData = {
        onTime: 85,
        overdue: 15,
        percentage: 85
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const percentageValue = fixture.debugElement.query(By.css('.percentage-value'));
      expect(percentageValue.nativeElement.classList).toContain('good');
    });

    it('should apply poor compliance styling for low percentage', () => {
      const mockData: ServiceComplianceData = {
        onTime: 5,
        overdue: 10,
        percentage: 33
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const percentageValue = fixture.debugElement.query(By.css('.percentage-value'));
      expect(percentageValue.nativeElement.classList).toContain('poor');
    });
  });

  describe('Compliance Level Analysis', () => {
    it('should return excellent level for 95%+ compliance', () => {
      component.data = { onTime: 19, overdue: 1, percentage: 95 };
      expect(component.getComplianceLevel()).toBe('excellent');
      expect(component.getComplianceIcon()).toBe('sentiment_very_satisfied');
    });

    it('should return good level for 80-94% compliance', () => {
      component.data = { onTime: 8, overdue: 2, percentage: 80 };
      expect(component.getComplianceLevel()).toBe('good');
      expect(component.getComplianceIcon()).toBe('sentiment_satisfied');
    });

    it('should return fair level for 60-79% compliance', () => {
      component.data = { onTime: 6, overdue: 4, percentage: 60 };
      expect(component.getComplianceLevel()).toBe('fair');
      expect(component.getComplianceIcon()).toBe('sentiment_neutral');
    });

    it('should return poor level for <60% compliance', () => {
      component.data = { onTime: 3, overdue: 7, percentage: 30 };
      expect(component.getComplianceLevel()).toBe('poor');
      expect(component.getComplianceIcon()).toBe('sentiment_dissatisfied');
    });

    it('should display appropriate compliance message for excellent performance', () => {
      component.data = { onTime: 19, overdue: 1, percentage: 95 };
      const message = component.getComplianceMessage();
      expect(message).toContain('Excellent compliance rate!');
      expect(message).toContain('19 of 20 services completed on time');
    });

    it('should display appropriate compliance message for poor performance', () => {
      component.data = { onTime: 3, overdue: 7, percentage: 30 };
      const message = component.getComplianceMessage();
      expect(message).toContain('Poor compliance rate');
      expect(message).toContain('Immediate action needed for 7 overdue maintenances');
    });
  });

  describe('Chart Management', () => {
    it('should create chart canvas when data is sufficient', () => {
      const mockData: ServiceComplianceData = {
        onTime: 8,
        overdue: 2,
        percentage: 80
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const canvas = fixture.debugElement.query(By.css('canvas'));
      expect(canvas).toBeTruthy();
    });

    it('should not create chart canvas when data is insufficient', () => {
      const mockData: ServiceComplianceData = {
        onTime: 2,
        overdue: 1,
        percentage: 67
      };

      component.data = mockData;
      fixture.detectChanges();
      
      const canvas = fixture.debugElement.query(By.css('canvas'));
      expect(canvas).toBeFalsy();
    });

    it('should destroy chart on component destroy when chart exists', () => {
      const mockData: ServiceComplianceData = {
        onTime: 8,
        overdue: 2,
        percentage: 80
      };

      component.data = mockData;
      fixture.detectChanges();
      
      // Mock chart object
      const mockChart = { destroy: jasmine.createSpy('destroy') };
      component['chart'] = mockChart as any;
      
      component.ngOnDestroy();
      expect(mockChart.destroy).toHaveBeenCalled();
    });

    it('should handle chart destruction gracefully when no chart exists', () => {
      component['chart'] = null;
      expect(() => component.ngOnDestroy()).not.toThrow();
    });
  });

  describe('Data Processing Logic', () => {
    it('should correctly identify insufficient data', () => {
      // Test with null data
      component.data = null;
      expect(component.hasInsufficientData()).toBe(true);

      // Test with insufficient total
      component.data = { onTime: 2, overdue: 1, percentage: 67 };
      expect(component.hasInsufficientData()).toBe(true);

      // Test with sufficient data
      component.data = { onTime: 8, overdue: 2, percentage: 80 };
      expect(component.hasInsufficientData()).toBe(false);
    });

    it('should generate appropriate insufficient data messages', () => {
      // Test null data message
      component.data = null;
      expect(component.insufficientDataMessage()).toContain('No service compliance data available');

      // Test zero jobs message
      component.data = { onTime: 0, overdue: 0, percentage: 0 };
      expect(component.insufficientDataMessage()).toContain('No completed or overdue maintenance jobs found');

      // Test insufficient jobs message
      component.data = { onTime: 2, overdue: 1, percentage: 67 };
      expect(component.insufficientDataMessage()).toContain('Only 3 maintenance jobs found');
    });

    it('should correctly identify good and poor compliance', () => {
      // Test good compliance (>=80%)
      component.data = { onTime: 8, overdue: 2, percentage: 80 };
      expect(component.isGoodCompliance()).toBe(true);
      expect(component.isPoorCompliance()).toBe(false);

      // Test poor compliance (<60%)
      component.data = { onTime: 3, overdue: 7, percentage: 30 };
      expect(component.isGoodCompliance()).toBe(false);
      expect(component.isPoorCompliance()).toBe(true);

      // Test medium compliance (60-79%)
      component.data = { onTime: 7, overdue: 3, percentage: 70 };
      expect(component.isGoodCompliance()).toBe(false);
      expect(component.isPoorCompliance()).toBe(false);
    });
  });

  describe('Component Lifecycle', () => {
    it('should handle ngOnChanges with sufficient data', () => {
      const mockData: ServiceComplianceData = {
        onTime: 8,
        overdue: 2,
        percentage: 80
      };

      component.data = mockData;
      spyOn(component as any, 'createChart');
      
      component.ngOnChanges();
      expect(component['createChart']).toHaveBeenCalled();
    });

    it('should handle ngOnChanges with insufficient data', () => {
      const mockData: ServiceComplianceData = {
        onTime: 2,
        overdue: 1,
        percentage: 67
      };

      // Set up existing chart
      const mockChart = { destroy: jasmine.createSpy('destroy') };
      component['chart'] = mockChart as any;
      
      component.data = mockData;
      component.ngOnChanges();
      
      expect(mockChart.destroy).toHaveBeenCalled();
      expect(component['chart']).toBeNull();
    });
  });
});