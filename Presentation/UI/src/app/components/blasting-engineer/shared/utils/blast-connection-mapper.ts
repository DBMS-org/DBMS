import { BlastConnection as ApiBlastConnection } from '../../../../core/models/site-blasting.model';
import { BlastConnection as ModelBlastConnection, ConnectorType } from '../../drilling-pattern-creator/models/drill-point.model';

/**
 * Utility class to map between different BlastConnection formats
 * This helps handle the mismatch between the API model and the UI model
 */
export class BlastConnectionMapper {
  /**
   * Maps from API model to UI model
   */
  static toModelFormat(apiConnection: ApiBlastConnection): ModelBlastConnection {
    return {
      id: apiConnection.id,
      fromHoleId: apiConnection.point1DrillPointId || apiConnection.fromHoleId,
      toHoleId: apiConnection.point2DrillPointId || apiConnection.toHoleId,
      connectorType: this.mapConnectorType(apiConnection.connectorType),
      delay: apiConnection.delay,
      sequence: apiConnection.sequence,
      // Create default start/end points if not available
      startPoint: apiConnection.startPoint || {
        id: `${apiConnection.id}-start`,
        label: '1',
        x: 0,
        y: 0,
        isHidden: true
      },
      endPoint: apiConnection.endPoint || {
        id: `${apiConnection.id}-end`,
        label: '2',
        x: 0,
        y: 0,
        isHidden: true
      }
    };
  }

  /**
   * Maps from UI model to API model
   */
  static toApiFormat(modelConnection: ModelBlastConnection): ApiBlastConnection {
    return {
      id: modelConnection.id,
      point1DrillPointId: modelConnection.fromHoleId,
      point2DrillPointId: modelConnection.toHoleId,
      fromHoleId: modelConnection.fromHoleId,
      toHoleId: modelConnection.toHoleId,
      connectorType: this.mapBackConnectorType(modelConnection.connectorType),
      delay: modelConnection.delay,
      sequence: modelConnection.sequence,
      projectId: 0, // These would need to be set by the caller
      siteId: 0,    // These would need to be set by the caller
      startPoint: modelConnection.startPoint,
      endPoint: modelConnection.endPoint
    };
  }

  /**
   * Maps a batch of connections from API to UI format
   */
  static toModelFormatBatch(apiConnections: ApiBlastConnection[]): ModelBlastConnection[] {
    return apiConnections.map(conn => this.toModelFormat(conn));
  }

  /**
   * Maps a batch of connections from UI to API format
   */
  static toApiFormatBatch(modelConnections: ModelBlastConnection[]): ApiBlastConnection[] {
    return modelConnections.map(conn => this.toApiFormat(conn));
  }

  /**
   * Maps connector types between formats
   */
  private static mapConnectorType(apiConnectorType: any): ConnectorType {
    if (typeof apiConnectorType === 'string') {
      return apiConnectorType as ConnectorType;
    }
    
    // Numeric enum mapping
    return apiConnectorType === 0 
      ? ConnectorType.DETONATING_CORD 
      : ConnectorType.CONNECTORS;
  }

  /**
   * Maps connector types from UI format back to API format
   */
  private static mapBackConnectorType(modelConnectorType: ConnectorType): any {
    if (modelConnectorType === ConnectorType.DETONATING_CORD) {
      return 0; // DetonatingCord in API enum
    } else {
      return 1; // Connectors in API enum
    }
  }
}










