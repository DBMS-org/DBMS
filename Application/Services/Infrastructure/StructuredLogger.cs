using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Services.Infrastructure
{
    /// <summary>
    /// Structured logger implementation with correlation tracking and operation context
    /// </summary>
    public class StructuredLogger<T> : IStructuredLogger<T>
    {
        private readonly ILogger<T> _logger;
        private string _correlationId;

        public StructuredLogger(ILogger<T> logger)
        {
            _logger = logger;
            _correlationId = Guid.NewGuid().ToString("N")[..8]; // Short correlation ID
        }

        public string CorrelationId => _correlationId;

        public void SetCorrelationId(string correlationId)
        {
            _correlationId = correlationId ?? Guid.NewGuid().ToString("N")[..8];
        }

        public IDisposable BeginOperation(string operationName, object? context = null)
        {
            return new OperationScope(_logger, operationName, _correlationId, context);
        }

        public void LogBusinessWarning(string message, object? context = null)
        {
            _logger.LogWarning("[{CorrelationId}] Business Warning: {Message} {@Context}",
                _correlationId, message, context);
        }

        public void LogValidationFailure(string entityType, string[] errors, object? context = null)
        {
            _logger.LogWarning("[{CorrelationId}] Validation failed for {EntityType}: {Errors} {@Context}",
                _correlationId, entityType, string.Join("; ", errors), context);
        }

        public void LogDataAccessError(string operation, Exception exception, object? context = null)
        {
            _logger.LogError(exception, "[{CorrelationId}] Database error during {Operation} {@Context}",
                _correlationId, operation, context);
        }

        public void LogExternalServiceError(string serviceName, Exception exception, object? context = null)
        {
            _logger.LogError(exception, "[{CorrelationId}] External service error for {ServiceName} {@Context}",
                _correlationId, serviceName, context);
        }

        public void LogOperationSuccess(string operationName, object? result = null, long? durationMs = null)
        {
            if (durationMs.HasValue)
            {
                _logger.LogInformation("[{CorrelationId}] Operation {OperationName} completed successfully in {DurationMs}ms {@Result}",
                    _correlationId, operationName, durationMs.Value, result);
            }
            else
            {
                _logger.LogInformation("[{CorrelationId}] Operation {OperationName} completed successfully {@Result}",
                    _correlationId, operationName, result);
            }
        }

        public void LogUnexpectedError(string operationName, Exception exception, object? context = null)
        {
            _logger.LogError(exception, "[{CorrelationId}] Unexpected error during {Operation} {@Context}",
                _correlationId, operationName, context);
        }

        public void LogOperationStart(string operationName, object? context = null)
        {
            _logger.LogInformation("[{CorrelationId}] Starting operation {Operation} {@Context}",
                _correlationId, operationName, context);
        }

        public void LogInformation(string message, object? context = null)
        {
            _logger.LogInformation("[{CorrelationId}] {Message} {@Context}",
                _correlationId, message, context);
        }

        public void LogDebug(string message, object? context = null)
        {
            _logger.LogDebug("[{CorrelationId}] {Message} {@Context}",
                _correlationId, message, context);
        }

        public void LogPerformanceMetric(string operationName, long durationMs, object? metrics = null)
        {
            _logger.LogInformation("[{CorrelationId}] PERF: {OperationName} took {DurationMs}ms {@Metrics}",
                _correlationId, operationName, durationMs, metrics);
        }

        /// <summary>
        /// Operation scope that tracks timing and logs start/end of operations
        /// </summary>
        private class OperationScope : IDisposable
        {
            private readonly ILogger _logger;
            private readonly string _operationName;
            private readonly string _correlationId;
            private readonly Stopwatch _stopwatch;
            private bool _disposed;

            public OperationScope(ILogger logger, string operationName, string correlationId, object? context)
            {
                _logger = logger;
                _operationName = operationName;
                _correlationId = correlationId;
                _stopwatch = Stopwatch.StartNew();

                _logger.LogDebug("[{CorrelationId}] Starting operation {OperationName} {@Context}",
                    _correlationId, _operationName, context);
            }

            public void Dispose()
            {
                if (_disposed) return;

                _stopwatch.Stop();
                var durationMs = _stopwatch.ElapsedMilliseconds;

                var logLevel = durationMs switch
                {
                    > 5000 => LogLevel.Warning, // Operations taking >5s
                    > 1000 => LogLevel.Information, // Operations taking >1s
                    _ => LogLevel.Debug // Fast operations
                };

                _logger.Log(logLevel, "[{CorrelationId}] Completed operation {OperationName} in {DurationMs}ms",
                    _correlationId, _operationName, durationMs);

                _disposed = true;
            }
        }
    }
} 