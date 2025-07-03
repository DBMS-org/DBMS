using Application.Interfaces.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Middleware
{
    public class AuditPerformanceMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditPerformanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPerformanceMonitor performanceMonitor)
        {
            var request = context.Request;
            var auditContext = new
            {
                Path = request.Path,
                Method = request.Method,
                Query = request.QueryString.ToString(),
                User = context.User.Identity?.Name ?? "Anonymous"
            };

            using (performanceMonitor.Measure("HttpRequest", auditContext))
            {
                await _next(context);
            }
        }
    }
} 