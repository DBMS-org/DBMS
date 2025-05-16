using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.API.Middleware
{
    public class AutoLogoutMiddleware
    {
        private readonly RequestDelegate _next;
        
        public AutoLogoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // TODO: Implement direct user lookup and update logic here
                    var user = new User { Id = userId, LastActivityTime = DateTime.UtcNow };
                }
            }
            
            await _next(context);
        }
    }
    
    public static class AutoLogoutMiddlewareExtensions
    {
        public static IApplicationBuilder UseAutoLogout(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AutoLogoutMiddleware>();
        }
    }
}
