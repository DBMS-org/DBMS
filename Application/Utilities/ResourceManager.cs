using System.Diagnostics;

namespace Application.Utilities
{
    public static class ResourceManager
    {
        public const int DEFAULT_OPERATION_TIMEOUT_MINUTES = 15;

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
                }
            }
        }

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
                }
            }
        }

        public static IDisposable TrackPerformance(string operationName, Action<string, TimeSpan>? onComplete = null)
        {
            return new PerformanceTracker(operationName, onComplete);
        }

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
                }
                
                _disposed = true;
            }
        }
    }
} 