using System.Diagnostics;

namespace Application.Utilities
{
    /// <summary>
    /// Utility class for resource management patterns across application services
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// Default timeout for operations that don't specify one
        /// </summary>
        public const int DEFAULT_OPERATION_TIMEOUT_MINUTES = 15;

        /// <summary>
        /// Creates a combined cancellation token with timeout and optional parent token
        /// </summary>
        /// <param name="timeoutMinutes">Timeout in minutes</param>
        /// <param name="parentToken">Optional parent cancellation token</param>
        /// <returns>Combined cancellation token source (must be disposed)</returns>
        public static CancellationTokenSource CreateTimeoutToken(int timeoutMinutes = DEFAULT_OPERATION_TIMEOUT_MINUTES, CancellationToken parentToken = default)
        {
            var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(timeoutMinutes));
            
            if (parentToken == default)
            {
                return timeoutCts;
            }
            
            var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(parentToken, timeoutCts.Token);
            timeoutCts.Dispose(); // Dispose the timeout source since we're using the combined one
            
            return combinedCts;
        }

        /// <summary>
        /// Executes an operation with timeout and cancellation support
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operation">Operation to execute</param>
        /// <param name="operationName">Name for logging</param>
        /// <param name="timeoutMinutes">Timeout in minutes</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Result of the operation</returns>
        public static async Task<T> ExecuteWithTimeoutAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            string operationName,
            int timeoutMinutes = DEFAULT_OPERATION_TIMEOUT_MINUTES,
            CancellationToken cancellationToken = default)
        {
            using var timeoutCts = CreateTimeoutToken(timeoutMinutes, cancellationToken);
            
            try
            {
                return await operation(timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                throw new TimeoutException($"Operation '{operationName}' timed out after {timeoutMinutes} minutes");
            }
        }

        /// <summary>
        /// Executes an operation with timeout and cancellation support (void return)
        /// </summary>
        /// <param name="operation">Operation to execute</param>
        /// <param name="operationName">Name for logging</param>
        /// <param name="timeoutMinutes">Timeout in minutes</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        public static async Task ExecuteWithTimeoutAsync(
            Func<CancellationToken, Task> operation,
            string operationName,
            int timeoutMinutes = DEFAULT_OPERATION_TIMEOUT_MINUTES,
            CancellationToken cancellationToken = default)
        {
            using var timeoutCts = CreateTimeoutToken(timeoutMinutes, cancellationToken);
            
            try
            {
                await operation(timeoutCts.Token);
            }
            catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
            {
                throw new TimeoutException($"Operation '{operationName}' timed out after {timeoutMinutes} minutes");
            }
        }

        /// <summary>
        /// Safely disposes multiple disposable resources
        /// </summary>
        /// <param name="disposables">Resources to dispose</param>
        public static void SafeDispose(params IDisposable?[] disposables)
        {
            foreach (var disposable in disposables)
            {
                try
                {
                    disposable?.Dispose();
                }
                catch
                {
                    // Swallow disposal exceptions to prevent masking original exceptions
                }
            }
        }

        /// <summary>
        /// Safely disposes multiple async disposable resources
        /// </summary>
        /// <param name="disposables">Resources to dispose</param>
        public static async Task SafeDisposeAsync(params IAsyncDisposable?[] disposables)
        {
            foreach (var disposable in disposables)
            {
                try
                {
                    if (disposable != null)
                    {
                        await disposable.DisposeAsync();
                    }
                }
                catch
                {
                    // Swallow disposal exceptions to prevent masking original exceptions
                }
            }
        }

        /// <summary>
        /// Creates a performance tracking scope that measures operation duration
        /// </summary>
        /// <param name="operationName">Name of the operation</param>
        /// <param name="onComplete">Optional callback when operation completes</param>
        /// <returns>Disposable scope that tracks timing</returns>
        public static IDisposable TrackPerformance(string operationName, Action<string, TimeSpan>? onComplete = null)
        {
            return new PerformanceTracker(operationName, onComplete);
        }

        /// <summary>
        /// Batches items for processing to manage memory usage
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="items">Items to batch</param>
        /// <param name="batchSize">Size of each batch</param>
        /// <returns>Batched items</returns>
        public static IEnumerable<IEnumerable<T>> BatchItems<T>(IEnumerable<T> items, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentException("Batch size must be greater than 0", nameof(batchSize));

            var batch = new List<T>(batchSize);
            
            foreach (var item in items)
            {
                batch.Add(item);
                
                if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<T>(batchSize);
                }
            }
            
            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        /// <summary>
        /// Processes items in batches with cancellation support
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="items">Items to process</param>
        /// <param name="batchSize">Size of each batch</param>
        /// <param name="processor">Batch processor function</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Total number of items processed</returns>
        public static async Task<int> ProcessInBatchesAsync<T>(
            IEnumerable<T> items,
            int batchSize,
            Func<IEnumerable<T>, CancellationToken, Task> processor,
            CancellationToken cancellationToken = default)
        {
            var totalProcessed = 0;
            
            foreach (var batch in BatchItems(items, batchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                await processor(batch, cancellationToken);
                totalProcessed += batch.Count();
            }
            
            return totalProcessed;
        }

        /// <summary>
        /// Performance tracking scope implementation
        /// </summary>
        private class PerformanceTracker : IDisposable
        {
            private readonly string _operationName;
            private readonly Action<string, TimeSpan>? _onComplete;
            private readonly Stopwatch _stopwatch;
            private bool _disposed;

            public PerformanceTracker(string operationName, Action<string, TimeSpan>? onComplete)
            {
                _operationName = operationName;
                _onComplete = onComplete;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                if (_disposed) return;
                
                _stopwatch.Stop();
                var elapsed = _stopwatch.Elapsed;
                
                try
                {
                    _onComplete?.Invoke(_operationName, elapsed);
                }
                catch
                {
                    // Swallow callback exceptions
                }
                
                _disposed = true;
            }
        }
    }
} 