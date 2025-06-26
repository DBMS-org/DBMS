using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "your-very-long-secret-key-here-make-it-at-least-32-characters";
            var issuer = jwtSettings["Issuer"] ?? "DBMS-API";
            var audience = jwtSettings["Audience"] ?? "DBMS-UI";
            var expireHours = int.Parse(jwtSettings["ExpireHours"] ?? "24");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };
            
            // Add role claims from UserRoles navigation property
            var activeRoles = user.GetActiveRoles();
            foreach (var role in activeRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            // If no roles found, add a default role (for backwards compatibility)
            if (!activeRoles.Any())
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }
            
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 
