using System.Collections.Generic;
using System.Security.Claims;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user, IEnumerable<Role> roles);
        bool ValidateToken(string token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
} 