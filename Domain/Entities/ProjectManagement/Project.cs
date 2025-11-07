using Domain.Common;
using Domain.Entities.UserManagement;
using Domain.Entities.MachineManagement;

namespace Domain.Entities.ProjectManagement
{
    public class Project : BaseAuditableEntity, IEntityOwnedByUser
    {
        public string Name { get; set; } = string.Empty;
        
        public string Region { get; set; } = string.Empty;
        
        public ProjectStatus Status { get; set; } = ProjectStatus.Planned;
        
        public string Description { get; set; } = string.Empty;
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public int? AssignedUserId { get; set; }
        public int? RegionId { get; set; }
        
        public int OwningUserId => AssignedUserId ?? 0;
        
        // Navigation properties
        public virtual User? AssignedUser { get; set; }
        public virtual Region? RegionNavigation { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; } = new List<ProjectSite>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
        public virtual ICollection<StoreManagement.Store> Stores { get; set; } = new List<StoreManagement.Store>();
    }
}
