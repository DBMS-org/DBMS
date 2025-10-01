using Domain.Common;
using Domain.Entities.MachineManagement;

namespace Domain.Entities.ProjectManagement
{
    public class Region : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public string Country { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();
        public virtual ICollection<StoreManagement.Store> Stores { get; set; } = new List<StoreManagement.Store>();
    }
}
