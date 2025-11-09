using Application.Interfaces.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.UserManagement;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, DateTime> _blacklistedTokens = new();
        
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
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Replace(" ", "")),
                new Claim("region", user.Region ?? string.Empty)
            };
            
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            try
            {
                if (IsTokenBlacklisted(token))
                {
                    return false;
                }

                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"] ?? "your-very-long-secret-key-here-make-it-at-least-32-characters";
                var issuer = jwtSettings["Issuer"] ?? "DBMS-API";
                var audience = jwtSettings["Audience"] ?? "DBMS-UI";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.ContainsKey(token);
        }

        public async Task BlacklistTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var expiration = jwtToken.ValidTo;

                _blacklistedTokens.TryAdd(token, expiration);

                await CleanupExpiredTokensAsync();
            }
            catch
            {
                _blacklistedTokens.TryAdd(token, DateTime.UtcNow.AddYears(1));
            }
        }

        public async Task CleanupExpiredTokensAsync()
        {
            var now = DateTime.UtcNow;
            var expiredTokens = _blacklistedTokens
                .Where(kvp => kvp.Value < now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var expiredToken in expiredTokens)
            {
                _blacklistedTokens.TryRemove(expiredToken, out _);
            }

            await Task.CompletedTask;
        }
    }
}
