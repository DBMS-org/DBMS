namespace Application.Interfaces.Infrastructure;

using System.Security.Claims;

public interface IUserContext
{
    ClaimsPrincipal? User { get; }

    bool IsInRole(string role);

    string? Region { get; }

    int? UserId { get; }
} 