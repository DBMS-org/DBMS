/**
 * Custom error classes for the drilling pattern creator component
 * These errors provide specific context for different types of failures
 */

/**
 * Base error class for all pattern-related errors
 */
export abstract class PatternError extends Error {
  public readonly timestamp: Date;
  public readonly context: string;
  public readonly recoverable: boolean;

  constructor(
    message: string,
    context: string = 'Unknown',
    recoverable: boolean = false
  ) {
    super(message);
    this.name = this.constructor.name;
    this.context = context;
    this.recoverable = recoverable;
    this.timestamp = new Date();
    
    // Maintains proper stack trace for where our error was thrown (only available on V8)
    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, this.constructor);
    }
  }

  /**
   * Returns a user-friendly error message
   */
  abstract getUserMessage(): string;

  /**
   * Returns error metadata for logging
   */
  getMetadata(): Record<string, any> {
    return {
      name: this.name,
      context: this.context,
      recoverable: this.recoverable,
      timestamp: this.timestamp.toISOString(),
      stack: this.stack
    };
  }
}

/**
 * Error thrown when drill point validation fails
 */
export class PatternValidationError extends PatternError {
  public readonly field: string;
  public readonly value: any;
  public readonly validationRule: string;

  constructor(
    message: string,
    field: string,
    value: any,
    validationRule: string,
    context: string = 'Validation'
  ) {
    super(message, context, true);
    this.field = field;
    this.value = value;
    this.validationRule = validationRule;
  }

  getUserMessage(): string {
    switch (this.validationRule) {
      case 'MINIMUM_SPACING':
        return `Drill points must be at least ${this.value}m apart for safety.`;
      case 'MAXIMUM_DEPTH':
        return `Drill depth cannot exceed ${this.value}m for this site.`;
      case 'DUPLICATE_POINT':
        return 'A drill point already exists at this location.';
      case 'INVALID_COORDINATES':
        return 'The drill point coordinates are outside the valid site boundaries.';
      case 'REQUIRED_FIELD':
        return `${this.field} is required and cannot be empty.`;
      default:
        return `Invalid ${this.field}: ${this.message}`;
    }
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      field: this.field,
      value: this.value,
      validationRule: this.validationRule
    };
  }
}

/**
 * Error thrown when canvas operations fail
 */
export class CanvasOperationError extends PatternError {
  public readonly operation: string;
  public readonly canvasState?: any;

  constructor(
    message: string,
    operation: string,
    canvasState?: any,
    context: string = 'Canvas'
  ) {
    super(message, context, true);
    this.operation = operation;
    this.canvasState = canvasState;
  }

  getUserMessage(): string {
    switch (this.operation) {
      case 'INITIALIZE':
        return 'Failed to initialize the drilling pattern canvas. Please refresh the page.';
      case 'RENDER':
        return 'Unable to display the drilling pattern. The canvas may be corrupted.';
      case 'ZOOM':
        return 'Zoom operation failed. Try resetting the view.';
      case 'PAN':
        return 'Pan operation failed. Try resetting the view.';
      case 'POINT_PLACEMENT':
        return 'Unable to place drill point. Please try again.';
      case 'POINT_SELECTION':
        return 'Unable to select drill point. Please try clicking again.';
      case 'POINT_DRAG':
        return 'Unable to move drill point. Please try again.';
      default:
        return `Canvas operation "${this.operation}" failed. Please try again.`;
    }
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      operation: this.operation,
      canvasState: this.canvasState
    };
  }
}

/**
 * Error thrown when state management operations fail
 */
export class StateManagementError extends PatternError {
  public readonly stateType: string;
  public readonly action: string;
  public readonly previousState?: any;

  constructor(
    message: string,
    stateType: string,
    action: string,
    previousState?: any,
    context: string = 'StateManagement'
  ) {
    super(message, context, true);
    this.stateType = stateType;
    this.action = action;
    this.previousState = previousState;
  }

  getUserMessage(): string {
    switch (this.action) {
      case 'LOAD':
        return 'Failed to load drilling pattern data. Please check your connection and try again.';
      case 'SAVE':
        return 'Failed to save drilling pattern. Your changes may be lost. Please try saving again.';
      case 'UPDATE':
        return 'Failed to update pattern settings. Please try again.';
      case 'RESET':
        return 'Failed to reset pattern state. Please refresh the page.';
      default:
        return `State operation "${this.action}" failed. Please try again.`;
    }
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      stateType: this.stateType,
      action: this.action,
      previousState: this.previousState
    };
  }
}

/**
 * Error thrown when data persistence operations fail
 */
export class DataPersistenceError extends PatternError {
  public readonly operation: string;
  public readonly dataType: string;
  public readonly httpStatus?: number;

  constructor(
    message: string,
    operation: string,
    dataType: string,
    httpStatus?: number,
    context: string = 'DataPersistence'
  ) {
    super(message, context, false);
    this.operation = operation;
    this.dataType = dataType;
    this.httpStatus = httpStatus;
  }

  getUserMessage(): string {
    if (this.httpStatus) {
      switch (this.httpStatus) {
        case 401:
          return 'Your session has expired. Please log in again.';
        case 403:
          return 'You do not have permission to perform this action.';
        case 404:
          return 'The drilling pattern data could not be found.';
        case 409:
          return 'The drilling pattern has been modified by another user. Please refresh and try again.';
        case 500:
          return 'Server error occurred. Please try again later or contact support.';
        default:
          return `Network error (${this.httpStatus}). Please check your connection and try again.`;
      }
    }

    switch (this.operation) {
      case 'LOAD':
        return 'Failed to load drilling pattern from server. Please check your connection.';
      case 'SAVE':
        return 'Failed to save drilling pattern to server. Please try again.';
      case 'DELETE':
        return 'Failed to delete drilling pattern. Please try again.';
      default:
        return `Data operation "${this.operation}" failed. Please try again.`;
    }
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      operation: this.operation,
      dataType: this.dataType,
      httpStatus: this.httpStatus
    };
  }
}

/**
 * Error thrown when component initialization fails
 */
export class ComponentInitializationError extends PatternError {
  public readonly componentName: string;
  public readonly phase: string;

  constructor(
    message: string,
    componentName: string,
    phase: string,
    context: string = 'ComponentInitialization'
  ) {
    super(message, context, false);
    this.componentName = componentName;
    this.phase = phase;
  }

  getUserMessage(): string {
    return `Failed to initialize ${this.componentName}. Please refresh the page. If the problem persists, contact support.`;
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      componentName: this.componentName,
      phase: this.phase
    };
  }
}

/**
 * Error thrown when performance thresholds are exceeded
 */
export class PerformanceError extends PatternError {
  public readonly metric: string;
  public readonly threshold: number;
  public readonly actualValue: number;

  constructor(
    message: string,
    metric: string,
    threshold: number,
    actualValue: number,
    context: string = 'Performance'
  ) {
    super(message, context, true);
    this.metric = metric;
    this.threshold = threshold;
    this.actualValue = actualValue;
  }

  getUserMessage(): string {
    switch (this.metric) {
      case 'RENDER_TIME':
        return 'The drilling pattern is taking longer than expected to render. Consider reducing the number of drill points.';
      case 'MEMORY_USAGE':
        return 'High memory usage detected. The application may become slow. Consider refreshing the page.';
      case 'POINT_COUNT':
        return `Too many drill points (${this.actualValue}). Maximum recommended is ${this.threshold} for optimal performance.`;
      default:
        return `Performance issue detected: ${this.metric} exceeded threshold.`;
    }
  }

  getMetadata(): Record<string, any> {
    return {
      ...super.getMetadata(),
      metric: this.metric,
      threshold: this.threshold,
      actualValue: this.actualValue
    };
  }
}

/**
 * Type guard functions for error identification
 */
export function isPatternValidationError(error: any): error is PatternValidationError {
  return error instanceof PatternValidationError;
}

export function isCanvasOperationError(error: any): error is CanvasOperationError {
  return error instanceof CanvasOperationError;
}

export function isStateManagementError(error: any): error is StateManagementError {
  return error instanceof StateManagementError;
}

export function isDataPersistenceError(error: any): error is DataPersistenceError {
  return error instanceof DataPersistenceError;
}

export function isComponentInitializationError(error: any): error is ComponentInitializationError {
  return error instanceof ComponentInitializationError;
}

export function isPerformanceError(error: any): error is PerformanceError {
  return error instanceof PerformanceError;
}

export function isPatternError(error: any): error is PatternError {
  return error instanceof PatternError;
}

/**
 * Error severity levels
 */
export enum ErrorSeverity {
  LOW = 'low',
  MEDIUM = 'medium',
  HIGH = 'high',
  CRITICAL = 'critical'
}

/**
 * Get error severity based on error type
 */
export function getErrorSeverity(error: Error): ErrorSeverity {
  if (isComponentInitializationError(error) || isDataPersistenceError(error)) {
    return ErrorSeverity.CRITICAL;
  }
  
  if (isStateManagementError(error) || isCanvasOperationError(error)) {
    return ErrorSeverity.HIGH;
  }
  
  if (isPerformanceError(error)) {
    return ErrorSeverity.MEDIUM;
  }
  
  if (isPatternValidationError(error)) {
    return ErrorSeverity.LOW;
  }
  
  return ErrorSeverity.MEDIUM;
}