using Microsoft.Extensions.Logging;

namespace Application.Interfaces.Infrastructure
{
    /// <summary>
    /// Structured logging service with correlation tracking and operation context
    /// </summary>
    public interface IStructuredLogger<T>
    {
        /// <summary>
        /// Current correlation ID for the operation
        /// </summary>
        string CorrelationId { get; }

        /// <summary>
        /// Sets the correlation ID for the current operation
        /// </summary>
        void SetCorrelationId(string correlationId);

        /// <summary>
        /// Logs the start of an operation with context
        /// </summary>
        IDisposable BeginOperation(string operationName, object? context = null);

        /// <summary>
        /// Logs a business logic warning with structured data
        /// </summary>
        void LogBusinessWarning(string message, object? context = null);

        /// <summary>
        /// Logs a validation failure with detailed information
        /// </summary>
        void LogValidationFailure(string entityType, string[] errors, object? context = null);

        /// <summary>
        /// Logs a database operation error
        /// </summary>
        void LogDataAccessError(string operation, Exception exception, object? context = null);

        /// <summary>
        /// Logs an external service error
        /// </summary>
        void LogExternalServiceError(string serviceName, Exception exception, object? context = null);

        /// <summary>
        /// Logs successful completion of an operation
        /// </summary>
        void LogOperationSuccess(string operationName, object? result = null, long? durationMs = null);

        /// <summary>
        /// Logs an unexpected error with full context
        /// </summary>
        void LogUnexpectedError(string operationName, Exception exception, object? context = null);

        /// <summary>
        /// Logs an information message with structured data
        /// </summary>
        void LogInformation(string message, object? context = null);

        /// <summary>
        /// Logs a debug message with structured data
        /// </summary>
        void LogDebug(string message, object? context = null);

        /// <summary>
        /// Logs performance metrics for an operation
        /// </summary>
        void LogPerformanceMetric(string operationName, long durationMs, object? metrics = null);
    }
} 