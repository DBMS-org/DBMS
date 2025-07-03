namespace Application.DTOs.UserManagement
{
    public class UserAssignmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<UserProjectAssignmentDto> AssignedProjects { get; set; } = new List<UserProjectAssignmentDto>();
    }

    public class UserProjectAssignmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }

    public class AssignProjectRequest
    {
        public int ProjectId { get; set; }
    }

    public class UserAssignmentStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int UsersWithProjects { get; set; }
        public int TotalProjects { get; set; }
        public int AssignedProjects { get; set; }
        public int UnassignedProjects { get; set; }
    }
} 
