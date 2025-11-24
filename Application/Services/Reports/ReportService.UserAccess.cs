using Application.DTOs.Reports;
using Domain.Entities.UserManagement;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// User & Access Management Report implementation - PHASE 5
    /// Simplified implementation using available repository methods
    /// Note: Uses IUserRepository only, role/permission data loaded via navigation properties
    /// </summary>
    public partial class ReportService
    {
        public async Task<UserAccessReportDto> GetUserAccessReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating User & Access Management Report");

                var report = new UserAccessReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate,
                    EndDate = filter?.EndDate
                };

                // Get all users (8 users according to plan)
                var usersEnumerable = await _userRepository.GetAllAsync();
                var users = usersEnumerable.ToList();

                // Since we don't have separate role/permission repositories, we'll work with what's available
                // Extract role information from users
                var allRoles = users
                    .SelectMany(u => u.UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role))
                    .Distinct()
                    .ToList();

                var allPermissions = allRoles
                    .SelectMany(r => r.RolePermissions.Where(rp => rp.IsActive).Select(rp => rp.Permission))
                    .Distinct()
                    .ToList();

                // Calculate Statistics
                var activeUsers = users.Count(u => u.Status == UserStatus.Active);
                var adminUsers = users.Count(u => u.UserRoles.Any(ur => ur.IsActive && ur.Role.Name.Contains("Admin")));
                var usersWithMultipleRoles = users.Count(u => u.UserRoles.Count(ur => ur.IsActive) > 1);

                report.Statistics = new UserAccessStatisticsDto
                {
                    TotalUsers = users.Count,
                    ActiveUsers = activeUsers,
                    InactiveUsers = users.Count - activeUsers,
                    TotalRoles = allRoles.Count,
                    TotalPermissions = allPermissions.Count,
                    UsersWithMultipleRoles = usersWithMultipleRoles,
                    AveragePermissionsPerUser = users.Any() ?
                        (decimal)users.Average(u => u.UserRoles
                            .Where(ur => ur.IsActive)
                            .SelectMany(ur => ur.Role.RolePermissions.Where(rp => rp.IsActive))
                            .Distinct()
                            .Count()) : 0,
                    AdminUsers = adminUsers,
                    RegularUsers = users.Count - adminUsers
                };

                // User Details
                report.Users = users.Select(u =>
                {
                    var userRoles = u.UserRoles.Where(ur => ur.IsActive).ToList();
                    var userPermissions = userRoles
                        .SelectMany(ur => ur.Role.RolePermissions.Where(rp => rp.IsActive).Select(rp => rp.Permission))
                        .Distinct()
                        .ToList();

                    return new UserAccessDetailDto
                    {
                        UserId = u.Id,
                        Username = u.Name,
                        Name = u.Name,
                        Email = u.Email.Value,
                        PhoneNumber = u.OmanPhone,
                        Region = u.Region,
                        Roles = userRoles.Select(ur => ur.Role.Name).ToList(),
                        Permissions = userPermissions.Select(p => p.Name).ToList(),
                        IsActive = u.Status == UserStatus.Active,
                        CreatedAt = u.CreatedAt,
                        LastLoginDate = u.LastLoginAt,
                        TotalLogins = 0 // Not tracked in current schema
                    };
                }).OrderBy(u => u.Username).ToList();

                // Role Distribution
                report.RoleDistribution = allRoles.Select(r =>
                {
                    var usersWithRole = users.Count(u => u.UserRoles.Any(ur => ur.IsActive && ur.RoleId == r.Id));
                    var rolePermissions = r.RolePermissions.Where(rp => rp.IsActive).Select(rp => rp.Permission).ToList();

                    return new RoleDistributionDto
                    {
                        RoleName = r.Name,
                        Description = r.Description,
                        UserCount = usersWithRole,
                        Percentage = users.Any() ? (decimal)usersWithRole / users.Count * 100 : 0,
                        Permissions = rolePermissions.Select(p => p.Name).ToList(),
                        PermissionCount = rolePermissions.Count
                    };
                }).OrderByDescending(r => r.UserCount).ToList();

                // Permission Matrix
                report.PermissionMatrix = allPermissions.Select(p =>
                {
                    var rolesWithPermission = allRoles
                        .Where(r => r.RolePermissions.Any(rp => rp.IsActive && rp.PermissionId == p.Id))
                        .ToList();

                    var usersWithPermission = users.Count(u => u.UserRoles
                        .Any(ur => ur.IsActive && ur.Role.RolePermissions
                            .Any(rp => rp.IsActive && rp.PermissionId == p.Id)));

                    return new PermissionMatrixDto
                    {
                        PermissionName = p.Name,
                        Description = p.Description,
                        RolesWithPermission = rolesWithPermission.Select(r => r.Name).ToList(),
                        RoleCount = rolesWithPermission.Count,
                        UserCount = usersWithPermission
                    };
                }).OrderByDescending(p => p.UserCount).ToList();

                // User Activity Summary
                report.UserActivity = users.Select(u =>
                {
                    var primaryRole = u.UserRoles
                        .Where(ur => ur.IsActive)
                        .OrderBy(ur => ur.AssignedAt)
                        .FirstOrDefault()?.Role.Name ?? "No Role";

                    var daysSinceLastLogin = u.LastLoginAt.HasValue ?
                        (DateTime.UtcNow - u.LastLoginAt.Value).Days : int.MaxValue;

                    string activityStatus;
                    if (u.Status != UserStatus.Active)
                        activityStatus = "Inactive";
                    else if (!u.LastLoginAt.HasValue || (DateTime.UtcNow - u.CreatedAt).Days < 7)
                        activityStatus = "New";
                    else if (daysSinceLastLogin <= 7)
                        activityStatus = "Active";
                    else if (daysSinceLastLogin <= 30)
                        activityStatus = "Recently Active";
                    else
                        activityStatus = "Dormant";

                    return new UserActivitySummaryDto
                    {
                        UserId = u.Id,
                        Username = u.Name,
                        Name = u.Name,
                        PrimaryRole = primaryRole,
                        LastLoginDate = u.LastLoginAt,
                        TotalLogins = 0, // Not tracked in current schema
                        DaysSinceLastLogin = daysSinceLastLogin == int.MaxValue ? 0 : daysSinceLastLogin,
                        IsActive = u.Status == UserStatus.Active,
                        ActivityStatus = activityStatus
                    };
                }).OrderByDescending(u => u.LastLoginDate ?? DateTime.MinValue).ToList();

                _logger.LogInformation("User & Access Management Report generated successfully with {UserCount} users, {RoleCount} roles, and {PermissionCount} permissions",
                    users.Count, allRoles.Count, allPermissions.Count);

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating User & Access Management Report");
                throw;
            }
        }
    }
}
