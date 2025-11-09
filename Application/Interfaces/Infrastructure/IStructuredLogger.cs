using Microsoft.Extensions.Logging;

namespace Application.Interfaces.Infrastructure
{
    public interface IStructuredLogger<T>
    {
        string CorrelationId { get; }

        void SetCorrelationId(string correlationId);

        IDisposable BeginOperation(string operationName, object? context = null);

        void LogBusinessWarning(string message, object? context = null);

        void LogValidationFailure(string entityType, string[] errors, object? context = null);

        void LogDataAccessError(string operation, Exception exception, object? context = null);

        void LogExternalServiceError(string serviceName, Exception exception, object? context = null);

        void LogOperationSuccess(string operationName, object? result = null, long? durationMs = null);

        void LogUnexpectedError(string operationName, Exception exception, object? context = null);

        void LogOperationStart(string operationName, object? context = null);

        void LogInformation(string message, object? context = null);

        void LogDebug(string message, object? context = null);

        void LogPerformanceMetric(string operationName, long durationMs, object? metrics = null);
    }
} 