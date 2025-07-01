import { KonvaTargetWithPointId } from '../models/drill-point.model';
import Konva from 'konva';

export class KonvaHelpers {
  /**
   * Safely extracts pointId from Konva event target with proper type checking
   */
  static getPointIdFromTarget(target: any): string | null {
    // Try direct attrs first
    if (target?.attrs?.pointId) {
      return target.attrs.pointId;
    }
    
    // Try custom property on target
    if (target?.pointId) {
      return target.pointId;
    }
    
    // Try parent attrs
    if (target?.parent?.attrs?.pointId) {
      return target.parent.attrs.pointId;
    }
    
    // Try custom property on parent
    if (target?.parent?.pointId) {
      return target.parent.pointId;
    }
    
    return null;
  }

  /**
   * Type-safe way to set pointId on Konva objects
   */
  static setPointId(target: Konva.Node, pointId: string): void {
    target.setAttr('pointId', pointId);
    (target as any).pointId = pointId; // Fallback for direct access
  }

  /**
   * Safely sets data on Konva objects
   */
  static setNodeData<T>(target: Konva.Node, data: T): void {
    (target as any).data = data;
  }

  /**
   * Safely gets data from Konva objects
   */
  static getNodeData<T>(target: Konva.Node): T | null {
    return (target as any).data || null;
  }

  /**
   * Type-safe cursor setting
   */
  static setCursor(stage: Konva.Stage, cursor: string): void {
    if (stage.container()) {
      stage.container().style.cursor = cursor;
    }
  }
} 