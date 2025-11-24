namespace Application.DTOs.Reports
{
    public class UserAccessReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Statistics
        public UserAccessStatisticsDto Statistics { get; set; } = new();

        // User Details
        public List<UserAccessDetailDto> Users { get; set; } = new();

        // Role Distribution
        public List<RoleDistributionDto> RoleDistribution { get; set; } = new();

        // Permission Matrix
        public List<PermissionMatrixDto> PermissionMatrix { get; set; } = new();

        // Activity Summary
        public List<UserActivitySummaryDto> UserActivity { get; set; } = new();
    }

    public class UserAccessStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int TotalRoles { get; set; }
        public int TotalPermissions { get; set; }
        public int UsersWithMultipleRoles { get; set; }
        public decimal AveragePermissionsPerUser { get; set; }
        public int AdminUsers { get; set; }
        public int RegularUsers { get; set; }
    }

    public class UserAccessDetailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int TotalLogins { get; set; }
    }

    public class RoleDistributionDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public decimal Percentage { get; set; }
        public List<string> Permissions { get; set; } = new();
        public int PermissionCount { get; set; }
    }

    public class PermissionMatrixDto
    {
        public string PermissionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> RolesWithPermission { get; set; } = new();
        public int RoleCount { get; set; }
        public int UserCount { get; set; }
    }

    public class UserActivitySummaryDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PrimaryRole { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public int TotalLogins { get; set; }
        public int DaysSinceLastLogin { get; set; }
        public bool IsActive { get; set; }
        public string ActivityStatus { get; set; } = string.Empty; // Active, Inactive, New
    }
}
