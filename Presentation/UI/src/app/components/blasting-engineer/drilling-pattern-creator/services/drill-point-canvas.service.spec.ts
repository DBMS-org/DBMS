import { TestBed } from '@angular/core/testing';
import { DrillPointCanvasService } from './drill-point-canvas.service';
import { DrillPoint } from '../models/drill-point.model';

describe('DrillPointCanvasService', () => {
  let service: DrillPointCanvasService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DrillPointCanvasService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('hasCustomDepth', () => {
    it('should return true when point depth differs from global depth', () => {
      const point: DrillPoint = {
        id: 'DH1',
        x: 0,
        y: 0,
        depth: 15.0,
        spacing: 3,
        burden: 2.5
      };
      const globalDepth = 10.0;

      const result = service.hasCustomDepth(point, globalDepth);

      expect(result).toBe(true);
    });

    it('should return false when point depth matches global depth', () => {
      const point: DrillPoint = {
        id: 'DH1',
        x: 0,
        y: 0,
        depth: 10.0,
        spacing: 3,
        burden: 2.5
      };
      const globalDepth = 10.0;

      const result = service.hasCustomDepth(point, globalDepth);

      expect(result).toBe(false);
    });

    it('should return false when point depth is within tolerance of global depth', () => {
      const point: DrillPoint = {
        id: 'DH1',
        x: 0,
        y: 0,
        depth: 10.005,
        spacing: 3,
        burden: 2.5
      };
      const globalDepth = 10.0;

      const result = service.hasCustomDepth(point, globalDepth);

      expect(result).toBe(false);
    });
  });

  describe('getPointsWithCustomDepths', () => {
    it('should return only points with custom depths', () => {
      const points: DrillPoint[] = [
        {
          id: 'DH1',
          x: 0,
          y: 0,
          depth: 10.0, // Global depth
          spacing: 3,
          burden: 2.5
        },
        {
          id: 'DH2',
          x: 3,
          y: 0,
          depth: 15.0, // Custom depth
          spacing: 3,
          burden: 2.5
        },
        {
          id: 'DH3',
          x: 6,
          y: 0,
          depth: 8.5, // Custom depth
          spacing: 3,
          burden: 2.5
        }
      ];
      const globalDepth = 10.0;

      const result = service.getPointsWithCustomDepths(points, globalDepth);

      expect(result.length).toBe(2);
      expect(result[0].id).toBe('DH2');
      expect(result[1].id).toBe('DH3');
    });

    it('should return empty array when no points have custom depths', () => {
      const points: DrillPoint[] = [
        {
          id: 'DH1',
          x: 0,
          y: 0,
          depth: 10.0,
          spacing: 3,
          burden: 2.5
        },
        {
          id: 'DH2',
          x: 3,
          y: 0,
          depth: 10.0,
          spacing: 3,
          burden: 2.5
        }
      ];
      const globalDepth = 10.0;

      const result = service.getPointsWithCustomDepths(points, globalDepth);

      expect(result.length).toBe(0);
    });
  });
});