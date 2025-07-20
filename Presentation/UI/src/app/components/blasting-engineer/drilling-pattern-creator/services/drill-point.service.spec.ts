import { TestBed } from '@angular/core/testing';
import { DrillPointService } from './drill-point.service';
import { DrillPoint, PatternSettings } from '../models/drill-point.model';

describe('DrillPointService - Depth Management', () => {
  let service: DrillPointService;
  let mockDrillPoints: DrillPoint[];
  let mockSettings: PatternSettings;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DrillPointService);
    
    // Setup mock data
    mockSettings = {
      spacing: 3.0,
      burden: 2.5,
      depth: 10.0
    };

    mockDrillPoints = [
      {
        id: 'DH1',
        x: 0,
        y: 0,
        depth: 10.0, // Global depth
        spacing: 3.0,
        burden: 2.5
      },
      {
        id: 'DH2',
        x: 3,
        y: 0,
        depth: 15.0, // Custom depth
        spacing: 3.0,
        burden: 2.5
      },
      {
        id: 'DH3',
        x: 6,
        y: 0,
        depth: 10.0, // Global depth
        spacing: 3.0,
        burden: 2.5
      }
    ];
  });

  describe('hasCustomDepth', () => {
    it('should return false for points with global depth', () => {
      const result = service.hasCustomDepth(mockDrillPoints[0], 10.0);
      expect(result).toBeFalse();
    });

    it('should return true for points with custom depth', () => {
      const result = service.hasCustomDepth(mockDrillPoints[1], 10.0);
      expect(result).toBeTrue();
    });

    it('should handle small floating point differences', () => {
      const point = { ...mockDrillPoints[0], depth: 10.005 };
      const result = service.hasCustomDepth(point, 10.0);
      expect(result).toBeFalse(); // Should be within tolerance
    });
  });

  describe('getPointsWithCustomDepths', () => {
    it('should return only points with custom depths', () => {
      const result = service.getPointsWithCustomDepths(mockDrillPoints, 10.0);
      expect(result.length).toBe(1);
      expect(result[0].id).toBe('DH2');
      expect(result[0].depth).toBe(15.0);
    });

    it('should return empty array when all points have global depth', () => {
      const allGlobalPoints = mockDrillPoints.map(p => ({ ...p, depth: 10.0 }));
      const result = service.getPointsWithCustomDepths(allGlobalPoints, 10.0);
      expect(result.length).toBe(0);
    });
  });

  describe('applyGlobalDepthToAll', () => {
    it('should update all points to global depth', () => {
      const testPoints = [...mockDrillPoints];
      service.applyGlobalDepthToAll(testPoints, 12.0);
      
      testPoints.forEach(point => {
        expect(point.depth).toBe(12.0);
      });
    });
  });

  describe('validateDepthRange', () => {
    it('should return true for valid depth values', () => {
      expect(service.validateDepthRange(1.0)).toBeTrue();
      expect(service.validateDepthRange(25.0)).toBeTrue();
      expect(service.validateDepthRange(50.0)).toBeTrue();
    });

    it('should return false for invalid depth values', () => {
      expect(service.validateDepthRange(0.5)).toBeFalse();
      expect(service.validateDepthRange(51.0)).toBeFalse();
      expect(service.validateDepthRange(-1.0)).toBeFalse();
    });
  });

  describe('formatDepth', () => {
    it('should round depth to 2 decimal places', () => {
      expect(service.formatDepth(10.123456)).toBe(10.12);
      expect(service.formatDepth(10.999)).toBe(11.0);
      expect(service.formatDepth(10.0)).toBe(10.0);
    });
  });
});