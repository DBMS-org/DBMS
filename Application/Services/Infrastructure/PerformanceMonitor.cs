using Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Services.Infrastructure
{
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private readonly IStructuredLogger<PerformanceMonitor> _logger;

        public PerformanceMonitor(IStructuredLogger<PerformanceMonitor> logger)
        {
            _logger = logger;
        }

        public IDisposable Measure(string operationName, object? context = null)
        {
            return new PerformanceScope(_logger, operationName, context);
        }

        private class PerformanceScope : IDisposable
        {
            private readonly IStructuredLogger<PerformanceMonitor> _logger;
            private readonly string _operationName;
            private readonly object? _context;
            private readonly Stopwatch _stopwatch;

            public PerformanceScope(IStructuredLogger<PerformanceMonitor> logger, string operationName, object? context)
            {
                _logger = logger;
                _operationName = operationName;
                _context = context;
                _stopwatch = Stopwatch.StartNew();
                _logger.LogOperationStart(_operationName, _context);
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _logger.LogPerformanceMetric(_operationName, _stopwatch.ElapsedMilliseconds, _context);
            }
        }
    }
} 