namespace Application.Interfaces.Infrastructure
{
    public interface IPerformanceMonitor
    {
        IDisposable Measure(string operationName, object? context = null);
    }
} 