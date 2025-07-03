using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Authorization
{
    public class OwnershipAuthorizationHandler : AuthorizationHandler<OwnershipRequirement, object>
    {
        private readonly ApplicationDbContext _context;

        public OwnershipAuthorizationHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnershipRequirement requirement, object resource)
        {
            if (resource is IEntityOwnedByUser ownedEntity)
            {
                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    if (ownedEntity.OwningUserId == userId)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
            
            // A more complex scenario might involve checking the resource ID from the route against the database
            // This is a placeholder for a more robust implementation
            if (context.Resource is HttpContext httpContext)
            {
                var resourceIdString = httpContext.Request.RouteValues["id"]?.ToString();
                if (int.TryParse(resourceIdString, out var resourceId))
                {
                    // This is a simplified example. You would need a way to map the resource type 
                    // from the controller to an entity type to query the database.
                    // For example, you could have a convention where the controller name matches the entity name.
                }
            }
            
            await Task.CompletedTask;
        }
    }
} 