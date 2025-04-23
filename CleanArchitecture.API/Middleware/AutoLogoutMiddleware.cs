using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Domain.Interfaces.Repositories;

namespace CleanArchitecture.API.Middleware
{
    public class AutoLogoutMiddleware
    {
        private readonly RequestDelegate _next;
        
        public AutoLogoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var user = await userRepository.GetByIdAsync(userId);
                    
                    if (user != null)
                    {
                        if ((DateTime.UtcNow - user.LastActivityTime).TotalMinutes > 30)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsJsonAsync(new { message = "Session expired due to inactivity. Please log in again." });
                            return;
                        }
                        
                        user.LastActivityTime = DateTime.UtcNow;
                        await userRepository.UpdateAsync(user);
                    }
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
